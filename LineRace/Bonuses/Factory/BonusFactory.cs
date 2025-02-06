using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.DirectInput;


namespace LineRace
{
    public static class BonusFactory
    {
		public static Bonus CreateBonusLeft<T>() where T : Bonus
		{
			Bonus bonus = null;

			if (typeof(T) == typeof(BonusFuel))
			{
				BonusFuel bonusFuel = new BonusFuel(new Sprite("FuelBones"), new Vector2(0.2f, -5f), GameScene.CarScale);
				bonusFuel.AddMove(new MoveBonus(Key.W, Key.S, "FuelBones"));
				bonus = bonusFuel;
			}
			else if (typeof(T) == typeof(BonusBarrel))
			{
				BonusBarrel bonusBarrel = new BonusBarrel(new Sprite("BarrelBonus"), new Vector2(0.2f, -10f), GameScene.CarScale);
				bonusBarrel.AddMove(new MoveBonus(Key.W, Key.S, "BarrelBonus"));
				bonus = bonusBarrel;
			}

			// Отправляем данные бонуса на сервер
			if (bonus != null)
			{
				string serializedBonus = bonus.Serialize();
				NetworkManager.SendDataToServer(serializedBonus); // Отправка данных на сервер
			}

			return bonus;
		}

		public static Bonus CreateBonusRight<T>() where T : Bonus
		{
			Bonus bonus = null;

			if (typeof(T) == typeof(BonusFuel))
			{
				BonusFuel bonusFuel = new BonusFuel(new Sprite("FuelBones"), new Vector2(1.3f, -5f), GameScene.CarScale);
				bonusFuel.AddMove(new MoveBonus(Key.Up, Key.Down, "FuelBones"));
				bonus = bonusFuel;
			}
			else if (typeof(T) == typeof(BonusBarrel))
			{
				BonusBarrel bonusBarrel = new BonusBarrel(new Sprite("BarrelBonus"), new Vector2(1.4f, -10f), GameScene.CarScale);
				bonusBarrel.AddMove(new MoveBonus(Key.Up, Key.Down, "BarrelBonus"));
				bonus = bonusBarrel;
			}

			// Отправляем данные бонуса на сервер
			if (bonus != null)
			{
				string serializedBonus = bonus.Serialize();
				NetworkManager.SendDataToServer(serializedBonus); // Отправка данных на сервер
			}

			return bonus;
		}

	}
}
