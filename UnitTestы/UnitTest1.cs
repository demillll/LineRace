using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LineRace;
using LineRace.Engine;
using LineRace.Object;
using SharpDX;
using LineRace.Multiplayer;
namespace UnitTestы
{
	[TestClass]
	public class UnitTest1 : GameObject
	{
		static Random random = new Random();
		[TestMethod]
		public void TestPointPosition()
		{
			float x1;
			float x2;
			float y1;
			float y2;
			float posX;
			float posY;
			float expexted;
			float actual;
			int i = 0;
			while (i < 1000)
			{
				x1 = random.Next(0, 1000);
				x2 = random.Next(0, 1000);
				y1 = random.Next(0, 1000);
				y2 = random.Next(0, 1000);
				posX = random.Next(0, 1000);
				posY = random.Next(0, 1000);
				expexted = ((posX - x1) * (y2 - y1)) - ((posY - y1) * (x2 - x1));
				actual = GetPositionRelativeToTheVector(x1, y1, x2, y2, posX, posY);
				Assert.AreEqual(expexted, actual);
				i++;
			}
		}
		[TestMethod]
		public void TestCollision()
		{
			bool expected;
			bool actual;
			float pos1;
			float pos2;
			float pos3;
			float pos4;
			GameObject gameObject1;
			GameObject gameObject2;
			int i = 0;
			while (i < 1000)
			{
				gameObject1 = new GameObject(new Vector2(random.Next(0, 1000), random.Next(0, 1000)), new Vector2(random.Next(0, 100), random.Next(0, 100)), 0f, sprite);
				gameObject2 = new GameObject(new Vector2(random.Next(0, 1000), random.Next(0, 1000)), Vector2.One, 0f, sprite);
				expected = false;
				pos1 = gameObject1.Position.X + gameObject1.Size.X;
				pos2 = gameObject1.Position.X;
				pos3 = gameObject1.Position.Y + gameObject1.Size.Y;
				pos4 = gameObject1.Position.Y;
				if (gameObject2.Position.X >= pos2 && gameObject2.Position.X <= pos1 && gameObject2.Position.Y >= pos4 && gameObject2.Position.Y <= pos3 ||
			   (gameObject2.Position.X + gameObject2.Size.X) >= pos2 && (gameObject2.Position.X + gameObject2.Size.X) <= pos1 && (gameObject2.Position.Y + gameObject2.Size.Y) >= pos4)
				{
					expected = true;
				}
				actual = LineRace.Object.Collision.IsColission(gameObject1, gameObject2);
				Assert.AreEqual(expected, actual);
				i++;
			}
		}
		[TestMethod]
		public void TestVectorRotate()
		{
			Vector2 vector;
			Vector2 expected;
			int i = 0;
			while (i < 1000)
			{
				vector = new Vector2(random.Next(0, 10000), random.Next(0, 10000));
				expected = new Vector2((float)(vector.X * Math.Cos(-Angle) - vector.Y * Math.Sin(-Angle)), (float)(vector.X * Math.Sin(-Angle) + vector.Y * Math.Cos(-Angle)));
				Assert.AreEqual(expected, Rotate(vector));
				i++;
			}
		}

		[TestMethod]
		public void TestParse()
		{
			var vector = new Vector2(134.5f, 234);


			Assert.AreEqual(vector, Client.Parse(vector.ToString()));
		}
	}
}
