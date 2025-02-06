using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LineRace.Object
{
	public class PlayerProperities
	{
		/// <summary>
		/// Угол отскока
		/// </summary>
		public virtual float Angle { get; set; }
		/// <summary>
		/// Сила удара
		/// </summary>
		public virtual float Force { get; set; }
	}
}
