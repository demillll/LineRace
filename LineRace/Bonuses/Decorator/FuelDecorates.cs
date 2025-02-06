using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineRace
{
    public class FuelDecorates : CarDecorators
    {
		public FuelDecorates(Car car) : base(car)
		{
			car.Fuel += 50;

			if (NetworkManager.IsServer)
			{
				NetworkManager.SendCarUpdate(car.Id, car.Fuel, car.MaxFuel);
			}
		}

	}
}
