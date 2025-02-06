using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;

namespace LineRace
{
    class BonusFuel : Bonus
    {
        /// <summary>
        /// Констурктор бонуса топлива
        /// </summary>
        
        /// /// <param name="sprite">параметр класса Sprite</param>
        /// <param name="position">позиция объекта</param>
        /// <param name="scale">масштаб объекта</param>
        
        public BonusFuel(Sprite sprite, Vector2 position, float scale) : base(sprite, position, scale)
        {
            collider = new Collider(this, new Vector2(0.4f, 0.4f));
        }

		public override void BonusAction(GameObject @object)
		{
			Car car = @object as Car;
			if (car != null)
			{
				@object = new FuelDecorates(car);

				// Синхронизация состояния
				if (NetworkManager.IsServer)
				{
					NetworkManager.SendBonusAction("Fuel", car.Id);
				}
			}
		}
	}
}
