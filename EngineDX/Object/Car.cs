using LineRace.Engine;
using SharpDX;
using SharpDX.DirectInput;
using System.Diagnostics;
using LineRace.Multiplayer;
using System.Drawing;

namespace LineRace.Object
{
	class Car : GameObject
	{
		/// <summary>
		/// Имя применяемого бонуса
		/// </summary>
		public virtual string BonusName { get; set; }
		/// <summary>
		/// Скорость передвижения
		/// </summary>
		public virtual Vector2 Speed { get; set; }
		/// <summary>
		/// Является ли этот танк правым
		/// </summary>

		public bool IsCrusht { get; set; }

		public PlayerProperities Property { get; set; }
		public bool IsRight { get; set; }
		/// <summary>
		/// Массив клавиш для управления танком назад[0], вперёд[1], угол пушки вверх[2], угол пушки вниз[3], выстрел[4]
		/// </summary>
		private readonly Key[] keys;

		public Packet packet = new Packet(1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 100, 100);

		protected Car() { }
		/// <summary>
		/// Конструктор класса Tank
		/// </summary>
		/// <param name="position">Позиция танка на игровом поле</param>
		/// <param name="size">Размер танка в пикселях</param>
		/// <param name="angle">Угол танка</param>
		/// <param name="sprite">Спрайт</param>
		/// <param name="isRight">Правый или левый танк</param>
		/// <param name="keys">Массив клавиш для управления: назад[0], вперёд[1], угол пушки вверх[2], угол пушки вниз[3], выстрел[4]</param>
		public Car(Vector2 position, Vector2 size, float angle, Sprite sprite, bool isRight, Key[] keys, bool isActive)
			: base(position, size, angle, sprite, isActive)
		{
			IsRight = isRight;
			this.keys = keys;
			Speed = new Vector2(6f, 0);

			Property = new PlayerProperities()
			{
				Force = 20,
				Angle = -45,
			};
		}
		/// <summary>
		/// Управление танком
		/// </summary>
		/// <param name="keyboardState">Состояние клавиатуры</param>
		public virtual void Control(KeyboardState keyboardState)
		{
			if (keyboardState.IsPressed(keys[0]) && (IsRight ? Position.X >= 600 : Position.X >= -200))
			{
				Position -= Speed;
			}
			if (keyboardState.IsPressed(keys[1]) && (IsRight ? Position.X <= 1910 - Size.X : Position.X <= 500))
			{
				Position += Speed;
			}
		}
		/// <summary>
		/// Обновление состояния объекта
		/// </summary>
		public virtual void Update()
		{
		}


		public virtual void ReadPacketData(Packet packet)
		{
			if (packet != null)
			{
				Position = new Vector2(packet.PositionX, packet.PositionY);
			}
		}
	}
}
