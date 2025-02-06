using LineRace.Multiplayer;
using LineRace.Object;
using SharpDX;
using SharpDX.DirectInput;
using System.Diagnostics;

namespace LineRace.Bonuses
{
	/// <summary>
	/// Абстрактный класс декоратор танка
	/// </summary>
	abstract class CarDecorator : Car
	{
		/// <summary>
		/// Декорируемый танк
		/// </summary>
		protected Car tank;
		/// <summary>
		/// Часы
		/// </summary>
		protected Stopwatch watch = new Stopwatch();
		/// <summary>
		/// Задекорирован ли
		/// </summary>
		public bool IsDecorate { get; set; }
		/// <summary>
		/// Длительность декорации
		/// </summary>
		public float Duration { get; set; }
		public override Vector2 Speed { get => tank.Speed; set => tank.Speed = value; }
		public override Vector2 Position { get => tank.Position; set => tank.Position = value; }
		public override Vector2 Scale { get => tank.Scale; set => tank.Scale = value; }
		public override float Angle { get => tank.Angle; set => tank.Angle = value; }
		public override Vector2 CenterRotation { get => tank.CenterRotation; set => tank.CenterRotation = value; }
		public override Vector2 ScaleTranslation { get => tank.ScaleTranslation; set => tank.ScaleTranslation = value; }
		public override string BonusName { get => tank.BonusName; set => tank.BonusName = value; }
		public override Vector2 Size => tank.Size;
		/// <summary>
		/// Конструктор класса CarDecorator
		/// </summary>
		/// <param name="tank">Декорируемый танк</param>
		/// <param name="duration">Длительность декорации</param>
		public CarDecorator(Car tank, float duration)
			: base()
		{
			this.tank = tank;
			Duration = duration;
			IsDecorate = true;
			watch.Start();
		}
		public override void Control(KeyboardState keyboardState)
		{
			tank.Control(keyboardState);
		}
		public override void Update()
		{
			if (watch.ElapsedMilliseconds >= Duration)
			{
				IsDecorate = false;
			}
			tank.Update();
		}

		public override void Draw()
		{
			tank.Draw();
		}

		public override void ReadPacketData(Packet packet)
		{
			if (packet != null)
			{
				Position = new Vector2(packet.PositionX, packet.PositionY);
			}
		}
		/// <summary>
		/// Получение старого объекта
		/// </summary>
		/// <returns>Старый танк</returns>
		public virtual Car GetOldTank()
		{
			tank.BonusName = "Нет";
			return tank;
		}
	}
}
