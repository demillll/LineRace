using SharpDX;

namespace LineRace.Bonuses
{
	/// <summary>
	/// Абстрактный класс фабрики бонусов
	/// </summary>
	abstract class BonusFactory
	{
		/// <summary>
		/// Конструктор класса BonusFactory
		/// </summary>
		public BonusFactory() { }
		/// <summary>
		/// Создание бонуса
		/// </summary>
		/// <param name="position">Позиция</param>
		/// <param name="size">Размер</param>
		/// <returns></returns>
		public abstract Bonus Create(Vector2 position, Vector2 size);
	}
}
