using SharpDX;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace LineRace
{
	public static class NetworkManager
	{
		private static TcpListener _serverListener;
		private static TcpClient _client;
		private static NetworkStream _stream;
		private static BinaryReader _reader;
		private static BinaryWriter _writer;

		public static bool IsServer { get; set; } = false;

		// Метод для запуска сервера
		public static void StartServer(int port)
		{
			_serverListener = new TcpListener(System.Net.IPAddress.Any, port);
			_serverListener.Start();
			Console.WriteLine("Server started...");
			_client = _serverListener.AcceptTcpClient();
			InitializeStream();
			ListenForData();
		}

		// Отправка данных о позиции объекта
		public static void SendObjectPosition(GameObject gameObject)
		{
			// Пример отправки данных о позиции через сеть
			var positionData = new { x = gameObject.position.center.X, y = gameObject.position.center.Y };

			// Здесь вы можете использовать конкретную сетевую библиотеку для отправки данных
		}

		// Получение данных о позиции объекта
		public static GameObject ReceiveObjectPosition()
		{
			// Пример получения данных о позиции объекта с сервера
			// В реальном приложении данные будут поступать из сокета или другого сетевого протокола
			return new GameObject
			{
				position = new Position { center = new Vector3(0, 0, 0) }, // Пример полученной позиции
				sprite = new Sprite() // Пример полученной спрайт-анимации
			};
		}

		// Метод для подключения клиента к серверу
		public static void StartClient(string serverAddress, int port)
		{
			_client = new TcpClient(serverAddress, port);
			InitializeStream();
		}

		private static void InitializeStream()
		{
			_stream = _client.GetStream();
			_reader = new BinaryReader(_stream);
			_writer = new BinaryWriter(_stream);
		}

		// Метод для отправки данных
		public static void SendData(string message)
		{
			if (_writer != null)
			{
				_writer.Write(message);
				_writer.Flush();
			}
		}

		// Метод для получения данных
		public static string ReceiveData()
		{
			if (_reader != null)
			{
				return _reader.ReadString();
			}
			return string.Empty;
		}

		// Получение данных (для клиента)
		private static void ListenForData()
		{
			new Thread(() =>
			{
				while (true)
				{
					string data = ReceiveData();
					if (!string.IsNullOrEmpty(data))
					{
						string[] parts = data.Split(':');
						if (parts[0] == "Bonus")
						{
							string bonusType = parts[1];
							int carId = int.Parse(parts[2]);
							Car car = GameScene.GetCarById(carId);

							if (bonusType == "Fuel")
							{
								car = new FuelDecorates(car);
							}
							else if (bonusType == "Barrel")
							{
								car = new BarrelDecorates(car);
							}

							// Обновляем машину на сервере (если клиент)
							if (!IsServer)
							{
								GameScene.GetCarById(carId).Fuel = car.Fuel;
								GameScene.GetCarById(carId).MaxFuel = car.MaxFuel;
							}
						}

						if (parts[0] == "CarUpdate")
						{
							int carId = int.Parse(parts[1]);
							float fuel = float.Parse(parts[2]);
							float maxFuel = float.Parse(parts[3]);

							Car car = GameScene.GetCarById(carId);
							car.Fuel = fuel;
							car.MaxFuel = maxFuel;
						}
					}
				}
			}).Start();
		}

		// Отправка действия бонуса
		public static void SendBonusAction(string bonusType, int carId)
		{
			if (IsServer)
			{
				string message = $"Bonus:{bonusType}:{carId}";
				SendData(message);
			}
		}

		// Отправка обновлений о машине
		public static void SendCarUpdate(int carId, float fuel, float maxFuel)
		{
			if (IsServer)
			{
				string message = $"CarUpdate:{carId}:{fuel}:{maxFuel}";
				SendData(message); // отправляем данные на клиент
			}
		}
		public static Car GetCarById(int carId)
		{
			if (carId == Car1.Id) return Car1;
			if (carId == Car2.Id) return Car2;
			return null;
		}

		// Остановка сервера/клиента
		public static void Stop()
		{
			_reader?.Close();
			_writer?.Close();
			_stream?.Close();
			_client?.Close();
			_serverListener?.Stop();
		}
	}
}
