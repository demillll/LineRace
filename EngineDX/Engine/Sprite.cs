using SharpDX;
using SharpDX.Direct2D1;

namespace LineRace.Engine
{
	public class Sprite
	{

		/// <summary>
		/// Инфаструктурный объект
		/// </summary>
		private Rendering rendering;
		/// <summary>
		/// Индекс кпртинки спрайта в коллекции
		/// </summary>
		private int bitmapIndex;
		/// <summary>
		/// Ширина
		/// </summary>
		public float width;
		/// <summary>
		/// Высота
		/// </summary>
		public float height;

		public Sprite(Rendering rendering, int bitmapIndex)
		{
			this.rendering = rendering;
			this.bitmapIndex = bitmapIndex;
			width = rendering.Bitmaps[bitmapIndex].Size.Width;
			height = rendering.Bitmaps[bitmapIndex].Size.Height;
		}

		/// <summary>
		/// Отрисовка спрайта
		/// </summary>
		/// <param name="translation"></param>
		/// <param name="scaleX"></param>
		/// <param name="scaleY"></param>
		/// <param name="angle"></param>
		/// <param name="scaleTranclation"></param>
		/// <param name="centerRotation"></param>
		/// <param name="opacity"></param>
		public void Draw(Vector2 translation, float scaleX, float scaleY, float angle, Vector2 scaleTranclation, Vector2 centerRotation, float opacity)
		{
			// Получаем из инфраструктурного объекта "цель" отрисовки
			WindowRenderTarget renderTarget = rendering.RenderTarget;
			renderTarget.Transform = Matrix3x2.Scaling(scaleX, scaleY, scaleTranclation) *
				Matrix3x2.Rotation(-angle, centerRotation) *
				Matrix3x2.Translation(translation);
			// Нарисовываемся
			SharpDX.Direct2D1.Bitmap bitmap = rendering.Bitmaps[bitmapIndex];
			renderTarget.DrawBitmap(bitmap, opacity, SharpDX.Direct2D1.BitmapInterpolationMode.Linear);
		}
	}
}

