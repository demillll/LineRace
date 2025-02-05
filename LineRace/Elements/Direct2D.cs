using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX.WIC;
using SharpDX.Windows;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;

namespace LineRace
{
    public class Direct2D : IDisposable
    {
        // Фабрика для создания 2D объектов
        private SharpDX.Direct2D1.Factory factory;
        public WindowRenderTarget RenderTarget { get; private set; }

        // Фабрика (уже третяя?) для работы с изображениями (WIC = Windows Imaging Component)
        private ImagingFactory imagingFactory;

        // В конструкторе создаем все объекты
        public Direct2D(RenderForm form)
        {
            // Создание фабрик для 2D объектов и текста
            factory = new SharpDX.Direct2D1.Factory();

            // Инициализация "прямой рисовалки":
            //   Свойства отрисовщика
            RenderTargetProperties renderProp = new RenderTargetProperties()
            {
                DpiX = 0,
                DpiY = 0,
                MinLevel = FeatureLevel.Level_10,
                PixelFormat = new SharpDX.Direct2D1.PixelFormat(SharpDX.DXGI.Format.B8G8R8A8_UNorm, AlphaMode.Premultiplied),
                Type = RenderTargetType.Hardware,
                Usage = RenderTargetUsage.None
            };
            //   Свойства отрисовщика, связанные с окном (дискриптор окна, размер в пикселях и способ представления результирующего изображения)
            HwndRenderTargetProperties winProp = new HwndRenderTargetProperties()
            {
                Hwnd = form.Handle,
                PixelSize = new Size2(form.ClientSize.Width, form.ClientSize.Height),
                PresentOptions = PresentOptions.None
                // Immediately // None - vSync
            };
            //   Создание "цели" и задание свойств сглаживания
            RenderTarget = new WindowRenderTarget(factory, renderProp, winProp);
            RenderTarget.AntialiasMode = AntialiasMode.PerPrimitive;
            RenderTarget.TextAntialiasMode = SharpDX.Direct2D1.TextAntialiasMode.Cleartype;

            // Создание "картиночной" фабрики
            imagingFactory = new ImagingFactory();
        }
        // Чтение изображения из bmp
       
        public List<SharpDX.Direct2D1.Bitmap> LoadBitmap(params string[] paths)
        {
            var bitmaps = new List<SharpDX.Direct2D1.Bitmap>();
            for (int i = 0; i < paths.Length; i++)
            {
                // Декодер формата
                BitmapDecoder decoder = new BitmapDecoder(imagingFactory, paths[i], DecodeOptions.CacheOnDemand);
                // Берем первый фрейм
                BitmapFrameDecode frame = decoder.GetFrame(0);
                // Также нужен конвертер формата 
                FormatConverter converter = new FormatConverter(imagingFactory);
                converter.Initialize(frame, SharpDX.WIC.PixelFormat.Format32bppPRGBA, BitmapDitherType.Ordered4x4, null, 0.0, BitmapPaletteType.Custom);
                // Вот теперь можно и bitmap
                SharpDX.Direct2D1.Bitmap bitmap = SharpDX.Direct2D1.Bitmap.FromWicBitmap(RenderTarget, converter);

                // Освобождаем неуправляемые ресурсы
                Utilities.Dispose(ref converter);
                Utilities.Dispose(ref frame);
                Utilities.Dispose(ref decoder);
                bitmaps.Add(bitmap);
            }
            return bitmaps;
        }

        public void Dispose()
        {
            Utilities.Dispose(ref imagingFactory);
            //Utilities.Dispose(ref RenderTarget);
            Utilities.Dispose(ref factory);
        }
    }
}
