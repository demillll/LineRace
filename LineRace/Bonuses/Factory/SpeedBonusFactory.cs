using LineRace.Engine;
using SharpDX;

namespace LineRace.Bonuses
{
	/// <summary>
	/// Фабрика бонуса скорости
	/// </summary>
	class SpeedBonusFactory : BonusFactory
	{
		/// <summary>
		/// Спрайт бонуса
		/// </summary>
		protected Sprite sprite;
		public SpeedBonusFactory(Sprite sprite)
		{
			this.sprite = sprite;
		}
		public override Bonus Create(Vector2 position, Vector2 size)
		{
			return new SpeedBonus(position, size, 0f, sprite, true);
		}
	}
}
