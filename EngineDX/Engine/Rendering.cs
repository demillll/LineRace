using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Windows;
using SharpDX.Direct2D1;
using SharpDX.WIC;
using SharpDX.DirectWrite;
using Direct2D1 = SharpDX.Direct2D1;
using WIC = SharpDX.WIC;
using RenderTargetFactory = SharpDX.Direct2D1.Factory;


namespace LineRace.Engine
{
	public class Rendering : IDisposable
	{
		/// <summary>
		/// Фабрика для создания окна и изображения
		/// </summary>
		private RenderTargetFactory renderTargetFactory;
		/// <summary>
		/// Фабрика для создания окна и изображения
		/// </summary>
		private WindowRenderTarget renderTarget;
		public WindowRenderTarget RenderTarget { get => renderTarget; }
		/// <summary>
		/// Фабрика изображений
		/// </summary>
		private static ImagingFactory imageFactory;
		private static WindowRenderTarget staticRenderTarget;
		/// <summary>
		/// Фабрика текста
		/// </summary>
		private static SharpDX.DirectWrite.Factory writeFactory;

		private List<SharpDX.Direct2D1.Bitmap> bitmaps;
		public List<SharpDX.Direct2D1.Bitmap> Bitmaps { get => bitmaps; }

		private static Brush redBrush;
		/// <summary>
		/// Красная кить. 
		/// </summary>
		public static Brush RedBrush { get => redBrush; }

		private static Brush greanBrush;
		/// <summary>
		/// Зелёная кисть.
		/// </summary>
		public static Brush GreanBrush { get => greanBrush; }


		private static Brush whiteBrush;
		/// <summary>
		/// Белая кисть.
		/// </summary>
		public static Brush WhiteBrush { get => whiteBrush; }

		private static TextFormat textFormatStats;
		/// <summary>
		/// Стандартный формат текста: "Calibri", 18
		/// </summary>
		public static TextFormat TextFormatStats { get => textFormatStats; }

		private static TextFormat textFormatPlayerStats;
		/// <summary>
		///  Формат текста, информация о игроке: "Calibri", 25
		/// </summary>
		public static TextFormat TextFormatPlayerStats { get => textFormatPlayerStats; }

		private static TextFormat textFormatGameOver;
		/// <summary>
		/// Формат текста, для окончания игры: "Calibri", 58
		/// </summary>
		public static TextFormat TextFormatGameOver { get => textFormatGameOver; }

		private static TextFormat textFormatMenu;
		/// <summary>
		/// Формат тукста, для стартого меню: "Calibri", 80
		/// </summary>
		public static TextFormat TextFormatMenu { get => textFormatMenu; }

		public Rendering(RenderForm form)
		{
			// Создание фабрики
			renderTargetFactory = new RenderTargetFactory();
			imageFactory = new ImagingFactory();
			writeFactory = new SharpDX.DirectWrite.Factory();

			//   Свойства отрисовщика
			RenderTargetProperties renderProperties = new RenderTargetProperties()
			{
				DpiX = 0,
				DpiY = 0,
				MinLevel = FeatureLevel.Level_10,
				PixelFormat = new Direct2D1.PixelFormat(SharpDX.DXGI.Format.B8G8R8A8_UNorm, AlphaMode.Premultiplied),
				Type = RenderTargetType.Hardware,
				Usage = RenderTargetUsage.None
			};

			//   Свойства отрисовщика, связанные с окном (дискриптор окна, размер в пикселях и способ представления результирующего изображения)
			HwndRenderTargetProperties winProperties = new HwndRenderTargetProperties()
			{
				Hwnd = form.Handle,
				PixelSize = new Size2(form.ClientSize.Width, form.ClientSize.Height),
				PresentOptions = PresentOptions.None
			};

			renderTarget = new WindowRenderTarget(renderTargetFactory, renderProperties, winProperties);
			staticRenderTarget = renderTarget;
			renderTarget.AntialiasMode = AntialiasMode.PerPrimitive;
			renderTarget.TextAntialiasMode = SharpDX.Direct2D1.TextAntialiasMode.Cleartype;

			textFormatStats = new TextFormat(writeFactory, "Calibri", 18)
			{
				ParagraphAlignment = ParagraphAlignment.Near,
				TextAlignment = TextAlignment.Leading
			};

			textFormatPlayerStats = new TextFormat(writeFactory, "Calibri", 28)
			{
				ParagraphAlignment = ParagraphAlignment.Near,
				TextAlignment = TextAlignment.Leading
			};

			textFormatGameOver = new TextFormat(writeFactory, "Calibri", 52)
			{
				ParagraphAlignment = ParagraphAlignment.Center,
				TextAlignment = TextAlignment.Center
			};

			textFormatMenu = new TextFormat(writeFactory, "Calibri", 80)
			{
				ParagraphAlignment = ParagraphAlignment.Center,
				TextAlignment = TextAlignment.Center
			};

			whiteBrush = new SolidColorBrush(renderTarget, Color.White);
			greanBrush = new SolidColorBrush(renderTarget, Color.Green);
			redBrush = new SolidColorBrush(renderTarget, Color.Red);
		}

		/// <summary>
		/// Чтение из изображений
		/// </summary>
		/// <param name="imageFileName">Расположение изображения</param>
		/// <returns></returns>
		public int LoadBitmap(string imageFileName)
		{
			// Декодер формата
			BitmapDecoder decoder = new BitmapDecoder(imageFactory, imageFileName, DecodeOptions.CacheOnDemand);
			// Берем первый фрейм
			BitmapFrameDecode frame = decoder.GetFrame(0);
			// Также нужен конвертер формата 
			FormatConverter converter = new FormatConverter(imageFactory);
			converter.Initialize(frame, WIC.PixelFormat.Format32bppPRGBA, BitmapDitherType.Ordered4x4, null, 0.0, BitmapPaletteType.Custom);

			Direct2D1.Bitmap bitmap = Direct2D1.Bitmap.FromWicBitmap(staticRenderTarget, converter);

			Utilities.Dispose(ref converter);
			Utilities.Dispose(ref frame);
			Utilities.Dispose(ref decoder);

			if (bitmaps == null) bitmaps = new List<SharpDX.Direct2D1.Bitmap>(4);
			bitmaps.Add(bitmap);
			return bitmaps.Count - 1;
		}
		/// <summary>
		/// Освобождение ресурсов
		/// </summary>
		public void Dispose()
		{
			Utilities.Dispose(ref renderTarget);
			Utilities.Dispose(ref imageFactory);
			Utilities.Dispose(ref renderTargetFactory);
			Utilities.Dispose(ref whiteBrush);
			Utilities.Dispose(ref greanBrush);
			Utilities.Dispose(ref redBrush);
			Utilities.Dispose(ref textFormatStats);
			Utilities.Dispose(ref textFormatPlayerStats);
			Utilities.Dispose(ref textFormatMenu);
			Utilities.Dispose(ref textFormatGameOver);
		}
	}
}