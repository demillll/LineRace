using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineRace
{
    public abstract class MoveObject 
    {
        protected InputDirectX inputDirectX = GameScene.inputDirectX;

        protected GameObject @object;

        public void SignToObject(GameObject @object)
        {
            this.@object = @object;
        }

		// Абстрактный метод для отправки данных о положении объекта (реализация будет в дочернем классе)
		public abstract void SendPositionData();

		// Абстрактный метод для получения данных о положении объекта (реализация будет в дочернем классе)
		public abstract void ReceivePositionData();

		public abstract void Update(List<GameObject> gameObjects);
	}

}

