using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LineRace.Engine;
using SharpDX;
using SharpDX.DirectInput;
using System.Diagnostics;
using LineRace.Multiplayer;
using System.Drawing;

namespace LineRace.Object
{
	public class EnimesCar : GameObject
	{
		Random random = new Random();
		RenderingApp renderingApp;
		/// <summary>
		/// Массив клавиш для управления танком назад[0], вперёд[1], угол пушки вверх[2], угол пушки вниз[3], выстрел[4]
		/// </summary>
		public PlayerProperities Property { get; set; }
		public bool IsRight { get; set; }
		/// <summary>
		/// Массив клавиш для управления машиной назад[0], вперёд[1], угол пушки вверх[2], угол пушки вниз[3], выстрел[4]
		/// </summary>
		private readonly Key[] keys;

		// public Packet packet = new Packet(1, 1, false);
		public virtual Vector2 Speed { get; set; }

		public int IsMoving { get; set; }

		public virtual Vector2 MAX_Line { get; set; }
		public virtual Vector2 MIN_Line { get; set; }
		protected EnimesCar()
		{
		}
		/// <summary>
		/// Конструктор класса Tank
		/// </summary>
		/// <param name="position">Позиция танка на игровом поле</param>
		/// <param name="size">Размер танка в пикселях</param>
		/// <param name="angle">Угол танка</param>
		/// <param name="sprite">Спрайт</param>
		/// <param name="gun">Пушка</param>
		/// <param name="isRight">Правый или левый танк</param>
		/// <param name="keys">Массив клавиш для управления: назад[0], вперёд[1], угол пушки вверх[2], угол пушки вниз[3], выстрел[4]</param>
		public EnimesCar(Vector2 position, Vector2 size, float angle, Sprite sprite, bool isRight, Key[] keys, bool isActive)
			: base(position, size, angle, sprite, isActive)
		{

			this.keys = keys;
			Speed = new Vector2(0, 6f);
			MAX_Line = new Vector2(400, 500);
			MIN_Line = new Vector2(400, 0);
			IsRight = isRight;

			IsMoving = 0;

			Property = new PlayerProperities()
			{

			};


		}
		/// <summary>
		/// Управление танком
		/// </summary>
		/// <param name="keyboardState">Состояние клавиатуры</param>
		public virtual void Control(KeyboardState keyboardState)
		{
			//LinePosition();
			if ((keyboardState.IsPressed(keys[0]) && (IsRight ? Position.X >= 940 : Position.X >= 0)) || (IsMoving == 1))
			{

				Position += Speed;
				IsMoving = 1;
			}

			if ((keyboardState.IsPressed(keys[1]) && (IsRight ? Position.X <= 1910 - Size.X : Position.X <= 800)) || (IsMoving == 2))
			{
				Position -= Speed;
				IsMoving = 2;
			}

			if (!keyboardState.IsPressed(keys[0]) && !keyboardState.IsPressed(keys[1]))
			{
				IsMoving = 0;
			}

			if (Time.CurrentTime > 4)
			{

				Time.Reset();
			}

			if (Position.Y < 100)
			{
				if (Time.CurrentTime > 4)
				{
					Position = new Vector2(Position.X, 900);
					Time.Reset();
				}

			}
			if (Position.Y > 900)
			{
				if (Time.CurrentTime > 4)
				{
					Position = new Vector2(IsRight ? random.Next(500, 1410) : random.Next(-200, 600), -400);
					Time.Reset();
				}

			}



		}



		/// <summary>
		/// Обновление состояния объекта
		/// </summary>
		public virtual void Update(List<GameObject> gameObjects)
		{
			foreach (var obj in gameObjects)
			{
				if (obj is Car && (Position - obj.Position).Length() < 50)
				{
					Speed = new Vector2(0, 0);

					((Car)obj).IsCrusht = true;

				}

			}
		}


	}
}
