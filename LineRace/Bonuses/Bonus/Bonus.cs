using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;

namespace LineRace
{
    public abstract class Bonus : GameObject
    {
		/// <summary>
		/// констурктор бонуса
		/// </summary>
		/// <param name="sprite">параметр класса Sprite</param>
		/// <param name="startPos">стартовая позиция объекта</param>
		/// <param name="scale">масштаб объекта</param>
		public bool Collected { get; private set; } = false;
		public Bonus(Sprite sprite, Vector2 startPos, float scale) : base(sprite, startPos, scale, true) { }

		public abstract void BonusAction(GameObject @object);
		public void CollectBonus()
		{
			if (!Collected)
			{
				Collected = true;
				// Логика удаления или отправки события серверу
			}
		}
	}
}
