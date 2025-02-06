using LineRace.Object;
using SharpDX;
using System.Threading.Tasks;

namespace LineRace.Bonuses
{
	/// <summary>
	/// Класс декорирующий скорость 
	/// </summary>
	class BarrelDecorator : CarDecorator
	{
		public BarrelDecorator(Car tank, float duration)
			: base(tank, duration)
		{
			tank.Speed += new Vector2(3f, 0);
			tank.BonusName = "Скорость";
		}
		public override Car GetOldTank()
		{
			tank.Speed -= new Vector2(3f, 0);
			return base.GetOldTank();
		}
	}
}
