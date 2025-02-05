using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;


namespace LineRace
{
    public class TextRender
    {
        
        public TextFormat textFormat { get; set; }

        public Brush Brush { get; private set; }
        
        private SharpDX.DirectWrite.Factory writeFactory = new SharpDX.DirectWrite.Factory();
        
        public TextRender(RenderTarget renderTarget, int size, ParagraphAlignment paragraphAlignment, TextAlignment textAlignment, Color color)
        {
            textFormat = new TextFormat(writeFactory, "Calibri", size);
            textFormat.ParagraphAlignment = paragraphAlignment;
            textFormat.TextAlignment = textAlignment;
            Brush = new SolidColorBrush(renderTarget, color);
        }
    }
}