using LineRace;
using SharpDX.Direct2D1;
using SharpDX.Windows;
using SharpDX;
using System.Collections.Generic;
using System;

public class GameScene : IDisposable
{
	public static float WorldScale;
	public static float CarScale;
	public static float FonScale;

	private static float unitsPerHeight = 80.0f;
	public RenderForm renderForm;

	public static Car Car1;
	public static Car Car2;

	public static Direct2D dx2d;
	public static Background background;

	public static List<GameObject> gameObjects = new List<GameObject>();
	public static RectangleF clientRect;
	public static InputDirectX inputDirectX;

	public GameScene()
	{
		renderForm = new RenderForm("Line Race") { Width = 1920, Height = 1080 };
		WorldScale = renderForm.ClientSize.Height / unitsPerHeight;
		CarScale = renderForm.ClientSize.Height / 40f;
		FonScale = renderForm.ClientSize.Height / 23f;
		dx2d = new Direct2D(renderForm);
		inputDirectX = new InputDirectX(renderForm);
		AddImages.AddAnimations();
		background = new Background(new Sprite("Background"), new Vector2(0, 0), WorldScale);
		AddImages.CreateObjects();

		// Создаем машины
		Car1 = AddImages.CreateCars()[0];
		Car2 = AddImages.CreateCars()[1];

		// Если это сервер, отправляем начальные данные
		if (NetworkManager.IsServer)
		{
			NetworkManager.SendCarUpdate(Car1.Id, Car1.Fuel, Car1.MaxFuel);
			NetworkManager.SendCarUpdate(Car2.Id, Car2.Fuel, Car2.MaxFuel);
		}
	}

	private void RenderCallback()
	{
		TimeHelper.Update();
		inputDirectX.UpdateKeyboardState();

		WindowRenderTarget target = dx2d.RenderTarget;
		Size2F targetSize = target.Size;
		clientRect.Width = targetSize.Width;
		clientRect.Height = targetSize.Height;

		target.BeginDraw();
		target.Clear(Color.AliceBlue);
		background.Draw(1.0f, WorldScale, unitsPerHeight / 980.0f, clientRect.Height, dx2d);

		// Отображаем все игровые объекты
		for (int i = 0; i < GameObject.gameObjects.Count; i++)
		{
			GameObject.gameObjects[i].Draw(1.0f, clientRect.Height, dx2d);
		}

		target.Transform = Matrix3x2.Identity;

		// Отображаем информацию о топливе
		TextRender textRender = new TextRender(dx2d.RenderTarget, 20, SharpDX.DirectWrite.ParagraphAlignment.Near, SharpDX.DirectWrite.TextAlignment.Leading, Color.Red);
		target.DrawText($"\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n" +
						$"Fuel: {(int)Car1.Fuel}%\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t\t     " +
						$"Fuel: {(int)Car2.Fuel}%", textRender.textFormat, clientRect, textRender.Brush);

		target.EndDraw();

		// Если это сервер, отправляем обновления состояния
		if (NetworkManager.IsServer)
		{
			NetworkManager.SendCarUpdate(Car1.Id, Car1.Fuel, Car1.MaxFuel);
			NetworkManager.SendCarUpdate(Car2.Id, Car2.Fuel, Car2.MaxFuel);
		}
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
