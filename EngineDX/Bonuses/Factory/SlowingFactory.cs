using LineRace.Engine;
using SharpDX;

namespace LineRace.Bonuses.Factory
{
	/// <summary>
	/// Фабрика замедления
	/// </summary>
	class SlowingFactory : BonusFactory
	{
		/// <summary>
		/// Фабрика скорости выстрела
		/// </summary>
		protected Sprite sprite;
		public SlowingFactory(Sprite sprite)
		{
			this.sprite = sprite;
		}
		public override Bonus Create(Vector2 position, Vector2 size)
		{
			return new SlowingBonus(position, size, 0f, sprite, true);
		}
	}
}
