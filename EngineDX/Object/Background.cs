using LineRace.Engine;
using SharpDX;

namespace LineRace.Object
{
	/// <summary>
	/// Класс фона
	/// </summary>
	class Background : GameObject
	{
		/// <summary>
		/// Конструктор фона 
		/// </summary>
		/// <param name="position">Позиция</param>
		/// <param name="size">Размер</param>
		/// <param name="angle">Угол</param>
		/// <param name="sprite">Спрайт</param>
		/// <param name="isActive">Активен ли</param>
		public Background(Vector2 position, Vector2 size, float angle, Sprite sprite, bool isActive)
			: base(position, size, angle, sprite, isActive)
		{
		}


	}
}
