using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.DirectInput;


namespace LineRace
{

	class MoveBonus : MoveObject
	{
		public float speed;

		private Random random;
		private Key MoveUp;
		private Key MoveDown;
		private string Animation;

		public MoveBonus(Key MoveUp, Key MoveDown, string Animation)
		{
			this.MoveUp = MoveUp;
			this.MoveDown = MoveDown;
			this.Animation = Animation;

			random = new Random();
			speed = 0f;
		}

		public override void Update(List<GameObject> gameObjects)
		{
			inputDirectX.UpdateKeyboardState();
			@object.position.center.Y += speed;

			// Обработка бонусов и их передача на сервер
			for (int i = 0; i < gameObjects.Count; i++)
			{
				if (gameObjects[i] is Car && ((Car)gameObjects[i]).IsPlayer)
				{
					// Коллизия с бонусом
					if (@object.position.center.DistanceTo(gameObjects[i].position.center) < 0.17f)
					{
						if (@object is BonusFuel)
						{
							HandleBonusFuel(gameObjects[i]);
						}
						else if (@object is BonusBarrel)
						{
							HandleBonusBarrel(gameObjects[i]);
						}
					}
				}
			}

			if (inputDirectX.KeyboardUpdated)
			{
				if (inputDirectX.KeyboardState.IsPressed(MoveUp) && !inputDirectX.KeyboardState.IsPressed(MoveDown))
				{
					if (speed < 0.2f)
						speed += 0.00005f;
				}

				if (!inputDirectX.KeyboardState.IsPressed(MoveUp) && speed > 0)
				{
					speed -= 0.00005f;
				}

				if (inputDirectX.KeyboardState.IsPressed(MoveDown) && !inputDirectX.KeyboardState.IsPressed(MoveUp))
				{
					speed -= 0.0001f;
					@object.sprite.SetAnimation(Animation);
				}
			}

			// Отправляем обновление позиции бонуса на сервер
			if (NetworkManager.IsServer)
			{
				NetworkManager.SendObjectPosition(@object);
			}
		}

		private void HandleBonusFuel(GameObject car)
		{
			FuelDecorates decorator;
			if (MoveUp == Key.W)
			{
				@object.position.center = new Vector2(random.NextFloat(-0.43f, 0.54f), random.NextFloat(-20f, 20f));
				decorator = new FuelDecorates(GameScene.Car1);
			}
			else
			{
				@object.position.center = new Vector2(random.NextFloat(0.83f, 1.82f), random.NextFloat(-20f, 20f));
				decorator = new FuelDecorates(GameScene.Car2);
			}
			NetworkManager.SendBonusCollision(@object, car);  // Отправка на сервер
		}

		private void HandleBonusBarrel(GameObject car)
		{
			BarrelDecorates decorator;
			if (MoveUp == Key.W)
			{
				@object.position.center = new Vector2(random.NextFloat(-0.43f, 0.54f), random.NextFloat(-40f, -30f));
				decorator = new BarrelDecorates(GameScene.Car1);
			}
			else
			{
				@object.position.center = new Vector2(random.NextFloat(0.83f, 1.82f), random.NextFloat(-40f, -30f));
				decorator = new BarrelDecorates(GameScene.Car2);
			}
			NetworkManager.SendBonusCollision(@object, car);  // Отправка на сервер
		}
	}
}
