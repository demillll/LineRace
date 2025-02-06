using LineRace.Bonuses.Decorator;
using LineRace.Engine;
using LineRace.Object;
using SharpDX;

namespace LineRace.Bonuses
{
	/// <summary>
	/// Класс замедления 
	/// </summary>
	class SlowingBonus : Bonus
	{
		/// <summary>
		/// Конструктор класса SlowingBonus
		/// </summary>
		/// <param name="position">Позиция</param>
		/// <param name="size">Размер</param>
		/// <param name="angle">Угол</param>
		/// <param name="sprite">Спрайт</param>
		/// <param name="isActive">Активен ли</param>
		public SlowingBonus(Vector2 position, Vector2 size, float angle, Sprite sprite, bool isActive)
			: base(position, size, angle, sprite, isActive)
		{
		}
		public override Car TankDecorate(Car tank)
		{
			return new FuelDecorator(tank, 5000f);
		}
	}
}
