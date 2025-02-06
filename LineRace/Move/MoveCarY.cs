using System;
using System.Collections.Generic;
using LineRace;
using SharpDX;
using SharpDX.DirectInput;

namespace LineRace
{
	class MoveCarY : MoveObject
	{
		private float speed;
		private Key MoveUp;
		private Key MoveDown;
		private float PressDown;

		public static BackgroundLoseWin LoseLeft;
		public static BackgroundLoseWin LoseRight;

		private int FuelKoef = 4;
		private string Animation;
		private int Numer;
		private bool IsStop = false;

		public MoveCarY(Key MoveUp, Key MoveDown, string Animation, int Numer)
		{
			this.MoveUp = MoveUp;
			this.MoveDown = MoveDown;
			this.Animation = Animation;
			this.Numer = Numer;
			speed = 0f;
		}

		public override void Update(List<GameObject> gameObjects)
		{
			inputDirectX.UpdateKeyboardState();
			@object.position.center.Y += speed;

			if (inputDirectX.KeyboardUpdated)
			{
				foreach (var obj in gameObjects)
				{
					if (obj is Car && ((Car)obj).IsPlayer && (@object.position.center - obj.position.center).Length() < 0.17f)
					{
						speed = 0;
						((Car)obj).IsCrash = true;
						IsStop = true;
					}
				}

				if ((Numer == 1 || Numer == 2) && @object.Site == false && IsStop)
				{
					speed = 0;
					AddImages.CreateBackgroundLoses();
					LoseLeft = AddImages.CreateBackgroundLoses()[Numer <= 2 ? 0 : 1];
					LoseLeft.IsActiv = true;
				}

				if (!((Car)@object).IsPlayer)
				{
					if (inputDirectX.KeyboardState.IsPressed(MoveUp) && !inputDirectX.KeyboardState.IsPressed(MoveDown))
					{
						if (speed < 0.2f)
							speed += 0.00005f;
						if (((Car)@object).Fuel > 0)
						{
							((Car)@object).Fuel -= speed * FuelKoef;
							if (((Car)@object).Fuel <= 0)
							{
								speed = 0;
								AddImages.CreateBackgroundLoses();
								LoseLeft = AddImages.CreateBackgroundLoses()[Numer <= 2 ? 0 : 1];
								LoseLeft.IsActiv = true;
							}
						}
					}

					if (inputDirectX.KeyboardState.IsPressed(MoveDown) && !inputDirectX.KeyboardState.IsPressed(MoveUp))
					{
						speed -= 0.0001f;
						@object.sprite.SetAnimation(Animation);
						@object.position.center.Y -= PressDown;
					}

					if (!inputDirectX.KeyboardState.IsPressed(MoveUp))
					{
						if (speed > 0)
							speed -= 0.00005f;
						else if (speed < 0)
							speed = 0;
					}
				}

				Random rnd = new Random();
				float zoneX = RandomFloat(rnd, -0.43, 0);
				float zoneYLimit = RandomFloat(rnd, 2, 15);

				if (@object.position.center.Y > zoneYLimit)
				{
					@object.position.center.Y = -0.8f;
					@object.position.center.X = zoneX;
				}
				if (@object.position.center.Y < -0.8f)
				{
					@object.position.center.Y = zoneYLimit;
					@object.position.center.X = zoneX;
				}

				// Отправляем обновления о положении машины на сервер
				if (NetworkManager.IsServer)
				{
					NetworkManager.SendObjectPosition(@object);
				}
			}
		}

		private float RandomFloat(Random random, double min, double max)
		{
			return (float)(min + random.NextDouble() * (max - min));
		}
	}

}
