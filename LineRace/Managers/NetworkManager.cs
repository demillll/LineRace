﻿using AmmunitionLibrary;
using GameLibrary;
using GameLibrary.Dirigible;
using LineRace;
using OpenTK;
using PrizesLibrary.Factories;
using PrizesLibrary.Prizes;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TcpConnectionLibrary;

namespace LineRace
{
	public class NetworkManager
	{
		public event Action<string> OnNetworkConnectionLost;

		public List<Bullet> _firstPlayerBulletList = new List<Bullet>();
		public List<Bullet> _secondPlayerBulletList = new List<Bullet>();

		public AbstractDirigible CurrentPlayer { get; set; }
		public AbstractDirigible NetworkPlayer { get; set; }
		public Prize CurrentPrize { get; set; }

		public List<Prize> CurrentPrizeList;

		private ITcpNetworkConnection _networkConnection;

		public Client Client { get; private set; }
		public Server Server { get; private set; }

		private NetworkData _currentNetworkData = new NetworkData();
		public BulletData BulletData;

		public PrizeFactory PrizeFactory { get; set; }

		private GameManager _gameManager;
		private UIManager _uiManager;
		private TimeManager _timeManager;
		private PlayerManager _playerManager;

		private Random random;

		public NetworkManager(GameManager gameManager, UIManager uiManager, TimeManager timeManager, PlayerManager playerManager)
		{
			_gameManager = gameManager;
			_uiManager = uiManager;
			_timeManager = timeManager;
			_playerManager = playerManager;
			_uiManager.SetGameMaangers(_gameManager);
		}

		public void SetNetworkStartData(ITcpNetworkConnection networkConnection, bool isLeftPlayer, int seed)
		{
			_networkConnection = networkConnection;

			if (isLeftPlayer)
			{
				CurrentPlayer = _gameManager.FirstPlayer;
				NetworkPlayer = _gameManager.SecondPlayer;
			}
			else
			{
				CurrentPlayer = _gameManager.SecondPlayer;
				NetworkPlayer = _gameManager.FirstPlayer;
			}
			CurrentPrizeList = _gameManager.PrizeList;

			random = new Random(seed);
			PrizeFactory = new PrizeFactory(random);

			_networkConnection.OnGetNetworkData += OnGetNetworkData;
		}

		private void OnGetNetworkData(object obj)
		{
			try
			{

				NetworkData networkData = (NetworkData)obj;

				if (networkData is null)
					return;

				// Обновляем данные для сетевого игрока
				NetworkPlayer.PositionCenter = new Vector2(networkData.PositionX, networkData.PositionY);

				NetworkPlayer.Health = networkData.Health;
				NetworkPlayer.Armor = networkData.Armor;
				NetworkPlayer.Fuel = networkData.Fuel;
				NetworkPlayer.Ammo = networkData.Ammo;
				NetworkPlayer.Speed = networkData.Speed;
				NetworkPlayer.NumberOfPrizesReceived = networkData.NumberOfPrizesReceived;
				NetworkPlayer.IsTurnedLeft = networkData.IsTurningLeft;

				// Обновляем текстуру сетевого игрока в зависимости от поворота

				if (CurrentPlayer == _gameManager.FirstPlayer)
				{
					_playerManager.UpdatePlayerTexture();

				}
				else
				{
					_playerManager.UpdatePlayerTexture();
				}


				var bulletData = networkData.BulletData;

				if (bulletData == null)
				{
					return;
				}

				// Создание и добавление пули, если это другая сторона
				if (bulletData.ShooterID != CurrentPlayer.DirigibleID)
				{
					if (CurrentPlayer == _gameManager.FirstPlayer)
					{
						_secondPlayerBulletList.Add(_gameManager.CreateNewAmmo(bulletData));
					}
					else
					{
						_firstPlayerBulletList.Add(_gameManager.CreateNewAmmo(bulletData));
					}
				}
				else
				{
					Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
				}

			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error in OnGetNetworkData: {ex.Message}");
			}
		}



		public async Task UpdateNetworkData()
		{

			var positionCenter = CurrentPlayer.PositionCenter;
			_currentNetworkData.PositionX = positionCenter.X;
			_currentNetworkData.PositionY = positionCenter.Y;

			_currentNetworkData.BulletData = BulletData;

			_currentNetworkData.Health = CurrentPlayer.Health;
			_currentNetworkData.Armor = CurrentPlayer.Armor;
			_currentNetworkData.Fuel = CurrentPlayer.Fuel;
			_currentNetworkData.Ammo = CurrentPlayer.Ammo;
			_currentNetworkData.Speed = CurrentPlayer.Speed;
			_currentNetworkData.NumberOfPrizesReceived = CurrentPlayer.NumberOfPrizesReceived;
			_currentNetworkData.IsTurningLeft = CurrentPlayer.IsTurnedLeft;

			await _networkConnection.UpdateNetworkData(_currentNetworkData);

			BulletData = null;
		}


		public async void StartServer()
		{
			Server = new Server();
			int seed = new Random().Next();

			Server.OnGetNetworkData += (_) => StartGame(seed, Server, true);
			Server.OnConnectionLost += message => OnNetworkConnectionLost?.Invoke(message);

			await Server.Start();
			Console.WriteLine("Server started. Client connected.");

			await Server.UpdateNetworkData<int>(seed);
		}
		public async void StartClient(TextBox ipAddressInput)
		{
			Client = new Client(ipAddressInput.Text);

			Client.OnGetNetworkData += (obj) =>
			{
				Console.WriteLine("Event OnGetData triggered");
				StartGame((int)obj, Client, false);
			};
			Client.OnConnectionLost += message => OnNetworkConnectionLost?.Invoke(message);

			await Client.Connect();
			Console.WriteLine("Client connected successfully.");

			await Client.GetNetworkData<int>();
		}

		private void StartGame(int seed, ITcpNetworkConnection networkConnection, bool isServer)
		{
			try
			{
				Console.WriteLine("StartGame is called");
				networkConnection.UnsubscribeActions();

				Console.WriteLine("Calling SetNetworkStartData");
				SetNetworkStartData(networkConnection, isServer, seed);
				Console.WriteLine("SetNetworkStartData completed");

				// Проверяем, вызывается ли метод HideRoleSelection
				Console.WriteLine("Calling HideRoleSelection");
				if (Application.Current != null)
				{
					Application.Current.Dispatcher.Invoke(() =>
					{
						_uiManager.HideRoleSelection();
					});
				}
				else
				{
					Console.WriteLine("Application.Current is null. Cannot access Dispatcher.");
				}

				Console.WriteLine("HideRoleSelection completed");

				Console.WriteLine("Role Selection is Hided");
				Application.Current.Dispatcher.Invoke(() =>
				{
					_timeManager.LaunchTimers(this);
					Console.WriteLine("Timers started in UI thread");
				});
				Console.WriteLine("Timers started");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error in StartGame: {ex.Message}");
				Console.WriteLine(ex.StackTrace);
			}
		}
	}
}
