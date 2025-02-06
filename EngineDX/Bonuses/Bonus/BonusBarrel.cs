using LineRace.Engine;
using SharpDX;
using LineRace.Object;

namespace LineRace.Bonuses
{
	/// <summary>
	/// Класс увеличения скорости
	/// </summary>
	class SpeedBonus : Bonus
	{
		/// <summary>
		/// Конструктор класса SpeedBonus
		/// </summary>
		/// <param name="position">Позиция</param>
		/// <param name="size">Размер</param>
		/// <param name="angle">Угол</param>
		/// <param name="sprite">Спрайт</param>
		/// <param name="isActive">Активен ли</param>
		public SpeedBonus(Vector2 position, Vector2 size, float angle, Sprite sprite, bool isActive)
			: base(position, size, angle, sprite, isActive)
		{
		}
		public override Car TankDecorate(Car tank)
		{
			return new BarrelDecorator(tank, 5000f);
		}
	}
}
