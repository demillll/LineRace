using System;
using LineRace.Engine;
using SharpDX;

namespace LineRace.Object
{
	/// <summary>
	/// Игровой объект
	/// </summary>
	public class GameObject
	{
		/// <summary>
		/// Коофициент масштабирования
		/// </summary>
		protected Vector2 scale;
		/// <summary>
		/// Размер в пикселях
		/// </summary>
		protected Vector2 size;
		/// <summary>
		/// Спрайт
		/// </summary>
		protected Sprite sprite;
		/// <summary>
		/// Позиция объекта
		/// </summary>
		public virtual Vector2 Position { get; set; }
		/// <summary>
		/// Локальный коофициент масштабирования
		/// </summary>
		public virtual Vector2 Scale { get; set; }
		/// <summary>
		/// Угол объекта
		/// </summary>
		public virtual float Angle { get; set; }
		/// <summary>
		/// Центр вращения объекта
		/// </summary>
		public virtual Vector2 CenterRotation { get; set; }
		public virtual Vector2 ScaleTranslation { get; set; }
		/// <summary>
		/// Прозрачность
		/// </summary>
		public float Opacity { get; set; }
		/// <summary>
		/// Размер
		/// </summary>
		public virtual Vector2 Size { get => size; }
		/// <summary>
		/// Активен ли объект
		/// </summary>
		public virtual bool IsActive { get; set; }
		protected GameObject() { }
		/// <summary>
		/// Конструктор класса GameObject
		/// </summary>
		/// <param name="position">Позиция</param>
		/// <param name="size">Размер</param>
		/// <param name="angle">Угол</param>
		/// <param name="sprite">Спрайт</param>
		/// <param name="isActive">Акливен ли</param>
		public GameObject(Vector2 position, Vector2 size, float angle, Sprite sprite, bool isActive)
		{
			Position = position;
			this.size = size;
			Angle = angle;
			this.sprite = sprite;
			IsActive = true;
			scale = new Vector2(size.X / sprite.width, size.Y / sprite.height);
			CenterRotation = new Vector2(size.X / 2, size.Y / 2);
			ScaleTranslation = Vector2.Zero;
			Scale = Vector2.One;
			Opacity = 1.0f;
		}

		public GameObject(Vector2 position, Vector2 size, float angle, Sprite sprite)
		{
			Position = position;
			this.size = size;
			Angle = angle;
			this.sprite = sprite;
		}
		/// <summary>
		/// Отрисовка
		/// </summary>
		public virtual void Draw()
		{

			if (IsActive)
			{
				sprite.Draw(Position, scale.X * Scale.X, scale.Y * Scale.Y, Angle, ScaleTranslation, CenterRotation, Opacity);
			}
		}
		/// <summary>
		/// Поворот вектора
		/// </summary>
		/// <param name="vector">Поворачиваемый вектор</param>
		/// <returns></returns>
		protected Vector2 Rotate(Vector2 vector)
		{
			return new Vector2((float)(vector.X * Math.Cos(-Angle) - vector.Y * Math.Sin(-Angle)), (float)(vector.X * Math.Sin(-Angle) + vector.Y * Math.Cos(-Angle)));
		}
		/// <summary>
		/// Проверка подожения точки отностительно вектора
		/// </summary>
		/// <param name="x1">Х вектора</param>
		/// <param name="y1">У вектора</param>
		/// <param name="x2">Х1 вектора</param>
		/// <param name="y2">У1 вектора</param>
		/// <param name="posX">Х точки</param>
		/// <param name="posY">У точки</param>
		/// <returns></returns>
		protected float GetPositionRelativeToTheVector(float x1, float y1, float x2, float y2, float posX, float posY)
		{
			return ((posX - x1) * (y2 - y1)) - ((posY - y1) * (x2 - x1));
		}

		public void ChangeSprite(Sprite sprite)
		{
			this.sprite = sprite;
		}
	}
}

