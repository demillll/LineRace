using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.Windows;

namespace LineRace
{
    public class GameScene : IDisposable
    {
        public static float WorldScale;
        public static float CarScale;
        public static float FonScale;
       // public static Car car;
        //public float fuel1;
        //public float fuel2;

        private static float unitsPerHeight = 80.0f;
        // Окно программы
        public RenderForm renderForm;

        public static Car Car1;
        public static Car Car2;


        // Инфраструктурные объекты
        public static Direct2D dx2d;

        public static Background background;


        public static List<GameObject> gameObjects = new List<GameObject>();




        // Клиетская область порта отрисовки в устройство-независимых пикселях
        public static RectangleF clientRect;

        public static InputDirectX inputDirectX;

        // В конструкторе создаем форму, инфраструктурные объекты, подгружаем спрайты, создаем помощник для работы со временем
        // В конце дергаем ресайsзинг формы для вычисления масштаба и установки пределов по горизонтали и вертикали
        public GameScene()
        {
            renderForm = new RenderForm("Line Race") {Width = 1920, Height = 1080 };
            WorldScale = renderForm.ClientSize.Height / unitsPerHeight;
            CarScale = renderForm.ClientSize.Height / 40f;

            FonScale = renderForm.ClientSize.Height / 23f;
            dx2d = new Direct2D(renderForm);
            inputDirectX = new InputDirectX(renderForm);
            AddImages.AddAnimations();
            background = new Background(new Sprite("Background"), new Vector2(0, 0), WorldScale);
            AddImages.CreateObjects();

            Car1 = AddImages.CreateCars()[0];
            Car2 = AddImages.CreateCars()[1];
        }
        private void RenderCallback()
        {

            // Дергаем обновление состояния "временного" помощника и объектов ввода
            TimeHelper.Update();
            //Обновляем время и счетчик кадров
            

            inputDirectX.UpdateKeyboardState();
            // Область просмотра в "прямом иксе 2 измерения" считается в "попугаях", т.е. в устройство-независимых пикселях несмотря на "dpiAware" в манифесте приложения
            // Поэтому для расчетов масштаба берем не клиентскую область формы, которая в честных пикселях, а RenderTarget-а
            WindowRenderTarget target = dx2d.RenderTarget;
            Size2F targetSize = target.Size;
            clientRect.Width = targetSize.Width;
            clientRect.Height = targetSize.Height;

            // Начинаем вывод графики
            target.BeginDraw();
            // Перво-наперво - очистить область отображения
            target.Clear(Color.AliceBlue);
            background.Draw(1.0f, WorldScale, unitsPerHeight / 980.0f, clientRect.Height, dx2d);


            for (int i = 0; i < GameObject.gameObjects.Count; i++)
            {

                GameObject.gameObjects[i].Draw(1.0f, clientRect.Height, dx2d);

            }



            target.Transform = Matrix3x2.Identity;
            target.Transform = Matrix3x2.Identity;
            TextRender textRender = new TextRender(dx2d.RenderTarget, 20, SharpDX.DirectWrite.ParagraphAlignment.Near, SharpDX.DirectWrite.TextAlignment.Leading, Color.Red);
            target.DrawText($"\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n" +
            $" " +
            $" Fuel: {(int)Car1.Fuel}%"+ $"\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t     " + $"Fuel: {(int)Car2.Fuel}%", textRender.textFormat, clientRect, textRender.Brush);

            target.EndDraw();

        }

        public void Run()
        {
            RenderLoop.Run(renderForm, RenderCallback);
        }

        public void Dispose()
        {
            
            dx2d.Dispose();
            renderForm.Dispose();
        }


    }
}
