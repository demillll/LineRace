using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Mathematics.Interop;

namespace LineRace
{
    class Line : GameObject
    {
        /// <summary>
        /// Конструктор разметки
        /// </summary>
        /// <param name="sprite">параметр класса Sprite</param>
        /// <param name="startPos">стартовая позиция объекта</param>
        /// <param name="scale">масштаб объекта</param>
        /// <param name="site">Сторона игрока</param>
        public Line(Sprite sprite, Vector2 startPos, float scale, bool site) : base(sprite, startPos, scale, site)
        {
            collider = new Collider(this, new Vector2(1f, 1f));

        }
        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="sprite">Параметр класса Sprite</param>
        /// <param name="startPos">Стартовая позиция</param>
        /// <param name="scale">Маштаб</param>
        /// <param name="site">Сторона игрока</param>
        public override void Draw(float opacity, float height, Direct2D dx2d)
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
    }
}
