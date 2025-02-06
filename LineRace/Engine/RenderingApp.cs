using System;
using System.Drawing;
using SharpDX.Windows;
using SharpDX.Direct2D1;
using SharpDX.DirectInput;
using SharpDX;
using System.Collections.Generic;
using LineRace.Object;
using LineRace.Bonuses;
using LineRace.Bonuses.Factory;
using SharpDX.Mathematics.Interop;
using System.Windows.Forms;
using LineRace.Multiplayer;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace LineRace.Engine
{
	/// <summary>
	/// Класс отрисовки и обновление всех процессов
	/// </summary>
	public class RenderingApp : IDisposable
	{
		/// <summary>
		/// Для расчета коэффициента масштабирования принимаем, что вся область окна по высоте вмещает 20 единиц длинны виртуального игрового пространства
		/// </summary>
		public static float UnitsPerHeight { get; } = 20.0f;
		/// <summary>
		/// Коофициент масштабирования
		/// </summary>
		public float Scale { get; private set; }
		private SharpDX.RectangleF clientRect;
		private RenderForm renderForm;
		private WindowRenderTarget renderTarget;
		private Rendering rendering;
		private InputHandler input;
		private static Random random = new Random(123);
		private bool isPlayerHost = true;
		private Packet packet;
		/// <summary>
		/// Массив клавиш для управление правым танком назад[0], вперёд[1], угол пушки вверх[2], угол пушки вниз[3], выстрел[4]
		/// </summary>
		private readonly Key[] keysForP1 = { Key.A, Key.D };
		/// <summary>
		/// Массив клавиш для управление правым танком назад[0], вперёд[1], угол пушки вверх[2], угол пушки вниз[3], выстрел[4]
		/// </summary>
		private readonly Key[] keysForP2 = { Key.Left, Key.Right };

		private readonly Key[] keysForP3 = { Key.W, Key.S };
		private readonly Key[] keysForP4 = { Key.Up, Key.Down };
		/// <summary>
		/// Список игровых объектов
		/// </summary>
		public static List<GameObject> gameObjects;
		private BonusFactory bonusFactory;
		private Client client;

		/// <summary>
		/// Конструктор класса\инициализатор
		/// </summary>
		public RenderingApp()
		{
			renderForm = new RenderForm("ГОНКИ")
			{
				IsFullscreen = true,
				ClientSize = new Size(1920, 1080),
				AllowUserResizing = true
			};
			rendering = new Rendering(renderForm);
			renderTarget = rendering.RenderTarget;
			input = new InputHandler(renderForm);

		}
		/// <summary>
		/// Use for multiplayer game
		/// </summary>
		/// <param name="isPlayerHost">Player host or not</param>
		public RenderingApp(bool isPlayerHost) : this()
		{
			isPlayerHost = this.isPlayerHost;
		}
		/// <summary>
		/// Запуск приложения
		/// </summary>
		public void Run(Client client, bool isHost)
		{
			Start();
			this.client = client;
			isPlayerHost = isHost;

			renderForm.Resize += RenderForm_Resize;
			RenderLoop.Run(renderForm, RenderCallback);
		}
		/// <summary>
		/// Формирование каждого кадра, обновление состояний объектов
		/// </summary>
		private void RenderCallback()
		{
			var t = client.GetPacketDataFromServer();
			packet = t;
			input.UpdateKeyboardState();
			Time.UpdateTime();
			Size2F targetSize = renderTarget.Size;
			clientRect.Width = targetSize.Width;
			clientRect.Height = targetSize.Height;

			if (isPlayerHost)
			{
				((Car)gameObjects[1]).Control(input.KeyboardState);

				((Line)gameObjects[3]).Control(input.KeyboardState);
				((Line)gameObjects[4]).Control(input.KeyboardState);
				((Line)gameObjects[5]).Control(input.KeyboardState);
				((Line)gameObjects[6]).Control(input.KeyboardState);

				((EnimesCar)gameObjects[12]).Control(input.KeyboardState);


				var pack = client.GetPacketDataFromServer();


				if (pack != null)
				{
					((Car)gameObjects[2]).Position = new Vector2(pack.PositionX, pack.PositionY);

					((Line)gameObjects[7]).Position = new Vector2(pack.LinePositionX_1, pack.LinePositionY_1);
					((Line)gameObjects[8]).Position = new Vector2(pack.LinePositionX_2, pack.LinePositionY_2);
					((Line)gameObjects[9]).Position = new Vector2(pack.LinePositionX_3, pack.LinePositionY_3);
					((Line)gameObjects[10]).Position = new Vector2(pack.LinePositionX_4, pack.LinePositionY_4);

					((EnimesCar)gameObjects[11]).Position = new Vector2(pack.EnemyX, pack.EnemyY);
				}

			}
			else
			{

				((Car)gameObjects[2]).Control(input.KeyboardState);

				((Line)gameObjects[7]).Control(input.KeyboardState);
				((Line)gameObjects[8]).Control(input.KeyboardState);
				((Line)gameObjects[9]).Control(input.KeyboardState);
				((Line)gameObjects[10]).Control(input.KeyboardState);

				((EnimesCar)gameObjects[11]).Control(input.KeyboardState);


				var pack = client.GetPacketDataFromServer();

				if (pack != null)
				{
					((Car)gameObjects[1]).Position = new Vector2(pack.PositionX, pack.PositionY);

					((Line)gameObjects[3]).Position = new Vector2(pack.LinePositionX_1, pack.LinePositionY_1);
					((Line)gameObjects[4]).Position = new Vector2(pack.LinePositionX_2, pack.LinePositionY_2);
					((Line)gameObjects[5]).Position = new Vector2(pack.LinePositionX_3, pack.LinePositionY_3);
					((Line)gameObjects[6]).Position = new Vector2(pack.LinePositionX_4, pack.LinePositionY_4);

					((EnimesCar)gameObjects[12]).Position = new Vector2(pack.EnemyX, pack.EnemyY);

				}

			}
			((Car)gameObjects[1]).Update();
			((Car)gameObjects[2]).Update();


			((Line)gameObjects[3]).Update();
			((Line)gameObjects[4]).Update();
			((Line)gameObjects[5]).Update();
			((Line)gameObjects[6]).Update();

			((Line)gameObjects[7]).Update();
			((Line)gameObjects[8]).Update();
			((Line)gameObjects[9]).Update();
			((Line)gameObjects[10]).Update();

			((EnimesCar)gameObjects[11]).Update(gameObjects);
			((EnimesCar)gameObjects[12]).Update(gameObjects);



			if (gameObjects[1] is CarDecorator && !((CarDecorator)gameObjects[1]).IsDecorate)
			{
				gameObjects[1] = ((CarDecorator)gameObjects[1]).GetOldTank();
			}
			if (gameObjects[2] is CarDecorator && !((CarDecorator)gameObjects[2]).IsDecorate)
			{
				gameObjects[2] = ((CarDecorator)gameObjects[2]).GetOldTank();
			}

			foreach (GameObject gameObject in gameObjects)
			{
				if (gameObject is Bonus)
				{
					if (Object.Collision.IsColission(gameObject, gameObjects[2]))
					{
						gameObjects[2] = ((Bonus)gameObject).TankDecorate((Car)gameObjects[2]);
						gameObjects.Remove(gameObject);
						break;
					}
					if (Object.Collision.IsColission(gameObject, gameObjects[1]))
					{
						gameObjects[1] = ((Bonus)gameObject).TankDecorate((Car)gameObjects[1]);
						gameObjects.Remove(gameObject);
						break;
					}
				}
			}
			if (Time.CurrentTime > 10)
			{
				CreateEnimes();
				//CreateBonuse();
				Time.Reset();
			}

			if (isPlayerHost)
			{
				client.SendData(new Packet(((Car)gameObjects[1]).Position.X, ((Car)gameObjects[1]).Position.Y, ((Line)gameObjects[3]).IsMoving,
					gameObjects[3].Position.X, gameObjects[3].Position.Y,
					gameObjects[4].Position.X, gameObjects[4].Position.Y,
					gameObjects[5].Position.X, gameObjects[5].Position.Y,
					gameObjects[6].Position.X, gameObjects[6].Position.Y,
					gameObjects[12].Position.X, gameObjects[12].Position.Y));
			}
			else
			{
				client.SendData(new Packet(((Car)gameObjects[2]).Position.X, ((Car)gameObjects[2]).Position.Y, ((Line)gameObjects[7]).IsMoving,
					gameObjects[7].Position.X, gameObjects[7].Position.Y,
					gameObjects[8].Position.X, gameObjects[8].Position.Y,
					gameObjects[9].Position.X, gameObjects[9].Position.Y,
					gameObjects[10].Position.X, gameObjects[10].Position.Y,
					gameObjects[11].Position.X, gameObjects[11].Position.Y));

			}

			if (((Car)gameObjects[1]).IsCrusht == true || ((Car)gameObjects[2]).IsCrusht == true)
			{
				gameObjects[0].ChangeSprite(new Sprite(rendering, rendering.LoadBitmap(@"Sprites\image2.png")));

				foreach (var o in gameObjects)
				{
					o.IsActive = false;

				}

				gameObjects[0].IsActive = true;
			}

			renderTarget.BeginDraw();
			DrawText(targetSize);
			renderTarget.EndDraw();
		}


		public void CreateEnimes()
		{

		}


		/// <summary>
		/// Создание бонуса
		/// </summary>
		//private void CreateBonuse()
		//{

		//    bonusFactory = ChoiseFactory(random.Next(0, 2));
		//    GameObject gameObject = bonusFactory.Create(new Vector2((random.Next(0, 10) >= 5) ? (float)random.NextDouble(10, 720) : (float)random.NextDouble(1160, 1870), 855), new Vector2(60, 60));
		//    gameObjects.Add(gameObject);
		//}
		/// <summary>
		/// Выбор фабрики
		/// </summary>
		/// <param name="factory">Номер фабрики 0 - 6</param>
		/// <returns>Фабрика бонусов</returns>
		//private BonusFactory ChoiseFactory(int factory)
		//{
		//    Sprite sprite;
		//    switch (factory)
		//    {
		//        case 0:
		//            sprite = new Sprite(rendering, rendering.LoadBitmap(@"Sprites\SpeedBonus.png"));
		//            return new SpeedBonusFactory(sprite);
		//        case 1:
		//            sprite = new Sprite(rendering, rendering.LoadBitmap(@"Sprites\SlowingBonus.png"));
		//            return new SlowingFactory(sprite);

		//    }
		//    return null;
		//}
		/// <summary>
		/// Изменение размера окна приложения
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void RenderForm_Resize(object sender, EventArgs e)
		{
			int width = renderForm.ClientSize.Width;
			int height = renderForm.ClientSize.Height;
			rendering.RenderTarget.Resize(new Size2(width, height));
			clientRect.Width = rendering.RenderTarget.Size.Width;
			clientRect.Height = rendering.RenderTarget.Size.Height;
			Scale = height / UnitsPerHeight;
		}
		/// <summary>
		/// Определение игровых объектов, а также их параметров и спрайтов
		/// </summary>
		private void Start()
		{
			gameObjects = new List<GameObject>();
			Sprite sprite = new Sprite(rendering, rendering.LoadBitmap(@"Sprites\Background.png"));
			Background background = new Background(new Vector2(0, 0), new Vector2(renderForm.ClientSize.Width, renderForm.ClientSize.Height), 0f, sprite, true);
			sprite = new Sprite(rendering, rendering.LoadBitmap(@"Sprites\Car1.png"));
			Sprite sprite2 = new Sprite(rendering, rendering.LoadBitmap(@"Sprites\Car2.png"));
			Sprite sprite3 = new Sprite(rendering, rendering.LoadBitmap(@"Sprites\Line1.png"));
			Car tank = new Car(new Vector2(400, 300), new Vector2(500, 500), 0f, sprite, false, keysForP1, true);
			Car tank1 = new Car(new Vector2(1400, 300), new Vector2(500, 500), 0f, sprite2, true, keysForP2, true);
			//
			Line line1 = new Line(new Vector2(400, 300), new Vector2(500, 500), 0f, sprite3, false, keysForP3, true);
			Line line2 = new Line(new Vector2(100, 300), new Vector2(500, 500), 0f, sprite3, false, keysForP3, true);
			Line line3 = new Line(new Vector2(400, 200), new Vector2(500, 500), 0f, sprite3, false, keysForP3, true);
			Line line4 = new Line(new Vector2(100, 200), new Vector2(500, 500), 0f, sprite3, false, keysForP3, true);
			//
			Line line5 = new Line(new Vector2(1000, 300), new Vector2(500, 500), 0f, sprite3, true, keysForP4, true);
			Line line6 = new Line(new Vector2(1400, 300), new Vector2(500, 500), 0f, sprite3, true, keysForP4, true);
			Line line7 = new Line(new Vector2(1000, 200), new Vector2(500, 500), 0f, sprite3, true, keysForP4, true);
			Line line8 = new Line(new Vector2(1400, 200), new Vector2(500, 500), 0f, sprite3, true, keysForP4, true);

			Sprite sprite9 = new Sprite(rendering, rendering.LoadBitmap(@"Sprites\Car3.png"));
			EnimesCar enimes_right_1 = new EnimesCar(new Vector2((float)random.NextDouble(950, 1100), -400), new Vector2(500, 500), 0f, sprite9, true, keysForP4, true);
			EnimesCar enimes_left_1 = new EnimesCar(new Vector2((float)random.NextDouble(100, 200), -400), new Vector2(500, 500), 0f, sprite9, false, keysForP3, true);

			gameObjects.Add(background); //0
			gameObjects.Add(tank);//1
			gameObjects.Add(tank1);//2

			gameObjects.Add(line1);//3
			gameObjects.Add(line2);//4
			gameObjects.Add(line3);//5
			gameObjects.Add(line4);//6

			gameObjects.Add(line5);//7
			gameObjects.Add(line6);//8
			gameObjects.Add(line7);//9
			gameObjects.Add(line8);//10


			gameObjects.Add(enimes_right_1);//11
			gameObjects.Add(enimes_left_1);//12



			//gameObjects.Add(background2);

			Time.Reset();
		}

		public void ShowBackground()
		{


		}
		private void DrawText(Size2F targetSize)
		{
			renderTarget.Clear(SharpDX.Color.Gray);
			foreach (GameObject gameObject in gameObjects)
			{
				gameObject.Draw();
			}
			renderTarget.Transform = Matrix3x2.Identity;
		}

		/// <summary>
		/// Освобождение ресурсов
		/// </summary>
		public void Dispose()
		{
			renderForm.Dispose();
			rendering.Dispose();
			input.Dispose();
		}
	}
}
