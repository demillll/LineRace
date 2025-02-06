using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Direct2D1;

namespace LineRace
{
    public class GameObject
    {
        /// <summary>
        /// Поле класса Sprite
        /// </summary>
        protected internal Sprite sprite;
        /// <summary>
        /// Поле класса Position
        /// </summary>
        protected internal Position position;

		public Vector2 Position { get; set; }

		/// <summary>
		/// Список объектов класса MoveObject
		/// </summary>
		protected internal List<MoveObject> moveObject = new List<MoveObject>();

        /// <summary>
        /// Переменная для маштаба
        /// </summary>
        public float scale;

        /// <summary>
        /// Поле класса Collider
        /// </summary>
        protected internal Collider collider;

        /// <summary>
        /// Список объектов класса GameObject
        /// </summary>
        public static List<GameObject> gameObjects = new List<GameObject>();
        /// <summary>
        /// Переменная стороны игрока
        /// </summary>
        public bool Site;
        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="sprite">Параметр класса Sprite</param>
        /// <param name="startPos">Стартовая позиция</param>
        /// <param name="scale">Маштаб</param>
        /// <param name="site">Сторона игрока</param>
        public GameObject(Sprite sprite, Vector2 startPos, float scale, bool site)
        {
            this.sprite = sprite;
            this.position = new Position(startPos.X, startPos.Y, 0.0f, scale);
            this.scale = scale;
            gameObjects.Add(this);
            if (collider == null)
            {
                collider = new Collider(this, new Vector2(1, 1));
            }
            Site = site;


        }
        /// <summary>
        /// Метод отисовки
        /// </summary>
        /// <param name="opacity">непрозрачность</param>
        /// <param name="height">ширина</param>
        /// <param name="dx2d">параметр класса dx2d</param>
        public virtual void Draw(float opacity, float height, Direct2D dx2d)
        {
            Matrix3x2 matrix;
            if (moveObject != null)
            {
                foreach (var script in moveObject)
                {
                    script.Update(gameObjects);
                }
            }
			Bitmap bitmap = sprite.animation.GetCurrentSprite(this.sprite);
			Vector2 translation = new Vector2();
			translation.X = sprite.PositionOfCenter.X / bitmap.Size.Width + position.center.X * position.scale;
			translation.Y = sprite.PositionOfCenter.Y / bitmap.Size.Height + position.center.Y * position.scale;

			matrix = Matrix3x2.Rotation(-position.angle, translation) *
					 Matrix3x2.Scaling(position.scale * scale / bitmap.Size.Width, position.scale * scale / bitmap.Size.Height, translation) *
					 Matrix3x2.Translation(translation * scale);

			WindowRenderTarget r = dx2d.RenderTarget;
			r.Transform = matrix;
			r.DrawBitmap(bitmap, opacity, BitmapInterpolationMode.NearestNeighbor);
		}

		// Метод для обновления позиции объекта и отправки данных о его положении
		public void UpdatePositionAndSend()
		{
			// Отправляем обновление состояния позиции объекта
			if (NetworkManager.IsServer)
			{
				NetworkManager.SendObjectPosition(this);
			}
		}

		public virtual void AddMove(params MoveObject[] moveAdd)
		{
			for (int i = 0; i < moveAdd.Length; i++)
			{
				moveAdd[i].SignToObject(this);
				moveObject.Add(moveAdd[i]);
			}
		}

    }
}
