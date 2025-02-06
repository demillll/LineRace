using LineRace.Engine;
using LineRace.Object;
using SharpDX;

namespace LineRace.Bonuses
{
	/// <summary>
	/// Абстрактный класс бонусов
	/// </summary>
	abstract class Bonus : GameObject
	{
		/// <summary>
		/// Конструктор класса Bonus
		/// </summary>
		/// <param name="position"></param>
		/// <param name="size"></param>
		/// <param name="angle"></param>
		/// <param name="sprite"></param>
		/// <param name="isActive"></param>
		public Bonus(Vector2 position, Vector2 size, float angle, Sprite sprite, bool isActive)
			: base(position, size, angle, sprite, isActive)
		{
		}
		/// <summary>
		/// Декорация танка
		/// </summary>
		/// <param name="tank">Декорируемый танк</param>
		/// <returns></returns>
		public abstract Car TankDecorate(Car tank);
	}
}
