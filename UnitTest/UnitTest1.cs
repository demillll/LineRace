using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LineRace;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]


        public void TestFuel()

        {

            Car car = new Car()

            {

                Fuel = 100

            };

            car = new FuelDecorates(car);

            Assert.AreEqual(car.Fuel, 100);


        }

        [TestMethod]

        public void TestPlayer()

        {

            Car car = new Car();

            Assert.IsFalse(car.IsPlayer);

        }
        [TestMethod]
        public void TestCrash()

        {

            Car car = new Car();

            Assert.IsFalse(car.IsCrash);

        }
    }
}
