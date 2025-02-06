using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineRace.Multiplayer
{
	public class Packet
	{
		public float PositionX { get; set; }
		public float PositionY { get; set; }
		public int IsMoving { get; set; }

		public float LinePositionX_1 { get; set; }
		public float LinePositionY_1 { get; set; }

		public float LinePositionX_2 { get; set; }
		public float LinePositionY_2 { get; set; }

		public float LinePositionX_3 { get; set; }
		public float LinePositionY_3 { get; set; }

		public float LinePositionX_4 { get; set; }
		public float LinePositionY_4 { get; set; }

		public float EnemyX { get; set; }
		public float EnemyY { get; set; }

		public Packet(float positionX, float positionY, int isMoving,
			float linePositionX_1, float linePositionY_1,
			float linePositionX_2, float linePositionY_2,
			float linePositionX_3, float linePositionY_3,
			float linePositionX_4, float linePositionY_4,
			float enemyX, float enemyY)

		{
			PositionX = positionX;
			PositionY = positionY;
			IsMoving = isMoving;

			LinePositionX_1 = linePositionX_1;
			LinePositionX_2 = linePositionX_2;
			LinePositionX_3 = linePositionX_3;
			LinePositionX_4 = linePositionX_4;
			LinePositionY_1 = linePositionY_1;
			LinePositionY_2 = linePositionY_2;
			LinePositionY_3 = linePositionY_3;
			LinePositionY_4 = linePositionY_4;
			EnemyX = enemyX;
			EnemyY = enemyY;
		}

		public Packet SetNewData(float positionX, float positionY, int isMoving
			//, float linePositionX_5, float linePositionY_5, float linePositionX_6, float linePositionY_6, float linePositionX_7, float linePositionY_7,
			//float linePositionX_8, float linePositionY_8
			)
		{
			PositionX = positionX;
			PositionY = positionY;

			IsMoving = isMoving;
			//LinePositionX_5 = linePositionX_5;
			//LinePositionY_5 = linePositionY_5;
			//LinePositionX_6 = linePositionX_6;
			//LinePositionY_6 = linePositionY_6;
			//LinePositionX_7 = linePositionY_7;
			//LinePositionX_8 = linePositionX_8;
			//LinePositionY_8 = linePositionY_8;

			return this;
		}
	}
}
