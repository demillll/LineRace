using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using Newtonsoft.Json;

namespace LineRace
{
    public class Position
    {
        private static readonly float Pi = (float)Math.PI;

        private static readonly float _2Pi = 2.0f * (float)Math.PI;
        //  "center" - вектор, который содержит координаты центра объекта;
        public Vector2 center;
        //"scale" - коэффициент масштабирования объекта;
        public float scale = 1.0f;

        public float angle;
        public float Angle
        {
            get => angle;
            set
            {
                angle = value;
                if (angle > Pi) angle -= _2Pi;
                else if (angle < -Pi) angle += _2Pi;
            }
        }

        public Position(float centerX, float centerY, float angle, float scale)
        {
            this.angle = angle;
            this.center.X = centerX;
            this.center.Y = centerY;
            this.scale = scale;
        }
		public string Serialize()
		{
			return JsonConvert.SerializeObject(this);
		}

		// Метод для десериализации
		public static Position Deserialize(string json)
		{
			return JsonConvert.DeserializeObject<Position>(json);
		}

	}

}
//Данный код содержит определение класса "Position", который представляет позицию игрового объекта. Класс имеет следующие поля:

//"scale" - коэффициент масштабирования объекта;
//"angle" - угол поворота объекта.*\

