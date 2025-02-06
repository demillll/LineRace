
using LineRace.Object;
using SharpDX;

namespace LineRace.Bonuses.Decorator
{
	/// <summary>
	/// Класс декрирующий замедление
	/// </summary>
	class FuelDecorator : CarDecorator
	{
		public FuelDecorator(Car tank, float duration)
			: base(tank, duration)
		{
			tank.Speed -= new Vector2(3f, 0);
			tank.BonusName = "Замедление";
		}
		public override Car GetOldTank()
		{
			tank.Speed += new Vector2(3f, 0);
			return base.GetOldTank();
		}
	}
}
