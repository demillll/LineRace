
namespace LineRace.Object
{
	/// <summary>
	/// Клас для расчёта коллизий
	/// </summary>
	public static class Collision
	{
		/// <summary>
		/// Расчёт коллизии
		/// </summary>
		/// <param name="gameObject1">Объект</param>
		/// <param name="gameObject2">Объект</param>
		/// <returns></returns>
		public static bool IsColission(GameObject gameObject1, GameObject gameObject2)
		{
			float pos1 = gameObject1.Position.X + gameObject1.Size.X;
			float pos2 = gameObject1.Position.X;
			float pos3 = gameObject1.Position.Y + gameObject1.Size.Y;
			float pos4 = gameObject1.Position.Y;
			if (gameObject2.Position.X >= pos2 && gameObject2.Position.X <= pos1 && gameObject2.Position.Y >= pos4 && gameObject2.Position.Y <= pos3 ||
			   (gameObject2.Position.X + gameObject2.Size.X) >= pos2 && (gameObject2.Position.X + gameObject2.Size.X) <= pos1 && (gameObject2.Position.Y + gameObject2.Size.Y) >= pos4)
			{
				return true;
			}
			else return false;
		}
	}
}
