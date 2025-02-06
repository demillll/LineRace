using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineRace
{
    public class BarrelDecorates : CarDecorators
    {
        public BarrelDecorates(Car car) : base(car)
        {
            car.maxFuel += 25;

			// Отправка обновления машины
			if (NetworkManager.IsServer)
			{
				NetworkManager.SendCarUpdate(car.Id, car.Fuel, car.MaxFuel);
			}
		}
    }
}
