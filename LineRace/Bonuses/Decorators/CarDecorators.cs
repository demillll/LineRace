using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;

namespace LineRace
{
    public abstract class CarDecorators : Car
    {
        public Car car;

        public CarDecorators(Car car) : base()
        {
            this.car = car;
        }
		// Метод для отправки обновлений о состоянии машины
		public void SendCarUpdate()
		{
			NetworkManager.SendCarUpdate(car.Id, car.Fuel, car.MaxFuel);
		}
	}
}
