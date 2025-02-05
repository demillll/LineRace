using SharpDX;
using LineRaceWindow;         // Пространство имён для DxWpfControl
       // Пространство имён для бонусов не нужно так как они реализованы в текущем проекте (Bonus, BonusBarrel, BonusFuel)
using System;
using System.Collections.Generic;
using DirigibleBattle.Managers;

namespace LineRace.Managers
{
	public class GameManager
	{
		public AbstractDirigible FirstPlayer;
		public AbstractDirigible SecondPlayer;

		// Заменяем PrizeList на BonusList (тип Bonus, который может представлять BonusBarrel и BonusFuel)
		public List<Bonus> BonusList;

		private UIManager _uiManager;
		private PlayerManager _playerManager;
		private FactoryBonus _factoryBonus; // Вместо PrizeManager теперь менеджер бонусов
		private MainWindow _mainWindow;

		// Конструктор теперь принимает DxWpfControl вместо GLWpfControl
		public GameManager(DxWpfControl dxControl, MainWindow mainWindow, UIManager uiManager, PlayerManager playerManager, FactoryBonus _factoryBonus)
		{
			_playerManager = playerManager;
			_factoryBonus = _factoryBonus;
			_mainWindow = mainWindow;
			_uiManager = uiManager;

			GameSettings(dxControl);

			TextureManager.SetupTexture();
			ColliderManager.SetupColliders();

			SetupGameObjects();
		}

		private bool UpdateResult = true;

		public async void GameTimer_Tick(NetworkManager networkManager, object sender, EventArgs e)
		{
			if (!UpdateResult)
			{
				return;
			}
			UpdateResult = false;

			_uiManager.GameStateCheck(_mainWindow);

			_playerManager.CheckPlayerDamage(networkManager._firstPlayerBulletList, ref SecondPlayer);
			_playerManager.CheckPlayerDamage(networkManager._secondPlayerBulletList, ref FirstPlayer);

			// Вызываем метод для применения бонусов вместо призов
			_factoryBonus.ApplyBonus(BonusList, ref FirstPlayer);
			_factoryBonus.ApplyBonus(BonusList, ref SecondPlayer);

			_playerManager.PlayerShoot();
			_uiManager.UpdatePlayerStats();

			networkManager.CurrentPlayer.Idle();

			if (networkManager.CurrentPlayer == FirstPlayer)
			{
				_playerManager.PlayerControl(TextureManager.firstDirigibleTextureLeft, TextureManager.firstDirigibleTextureRight);
				_playerManager.UpdatePlayerTexture();
			}
			else
			{
				_playerManager.PlayerControl(TextureManager.secondDirigibleTextureLeft, TextureManager.secondDirigibleTextureLeft);
				_playerManager.UpdatePlayerTexture();
			}

			await networkManager.UpdateNetworkData();
			UpdateResult = true;
		}

		// Метод настройки DirectX-контрола (DxWpfControl)
		private void GameSettings(DxWpfControl dxControl)
		{
			var settings = new DxWpfControlSettings
			{
				// Здесь задайте необходимые настройки для DirectX (например, версия, параметры устройства и т.д.)
			};
			dxControl.Start(settings);
			dxControl.InvalidateVisual();

			// Здесь можно добавить инициализацию, специфичную для DirectX, вместо вызовов OpenGL (GL.Enable, GL.BlendFunc и т.д.)
		}

		private void SetupGameObjects()
		{
			// Используем SharpDX.Vector2 вместо OpenTK.Vector2
			FirstPlayer = new BasicDirigible(new Vector2(-0.6f, -0.4f), TextureManager.firstDirigibleTextureRight);
			SecondPlayer = new BasicDirigible(new Vector2(0.5f, 0f), TextureManager.secondDirigibleTextureLeft);

			BonusList = new List<Bonus>(); // Инициализируем список бонусов
		}


	}
}
