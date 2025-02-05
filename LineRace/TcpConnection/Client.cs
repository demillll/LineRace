using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LineRace
{
	public class Client : ITcpNetworkConnection, IDisposable
	{
		public event Action<object> OnGetNetworkData;
		public event Action<string> OnConnectionLost;

		public Socket ClientSocket { get; private set; }
		private readonly string _serverIP;
		private readonly int _port;

		public Client(string serverIP, int port = 8000)
		{
			// Инициализируем сокет
			ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			_serverIP = serverIP;
			_port = port;
			Debug.WriteLine("Клиент инициализирован для IP: " + _serverIP);
		}

		public async Task Connect()
		{
			try
			{
				await Task.Run(() =>
				{
					ClientSocket.Connect(_serverIP, _port);
					Debug.WriteLine("Соединение с сервером установлено");
				});
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Ошибка в Connect: {ex.Message}");
			}
		}

		public async Task GetNetworkData<T>()
		{
			try
			{
				Debug.WriteLine("Начало получения данных");

				// Отправляем запрос (в данном случае объект по умолчанию)
				var requestObj = default(T);
				var requestJson = JsonConvert.SerializeObject(requestObj);
				var requestData = Encoding.UTF8.GetBytes(requestJson);

				await Task.Run(() =>
				{
					ClientSocket.Send(requestData);
				});

				// Получаем ответ
				byte[] buffer = new byte[65535];
				int bytesReceived = await Task.Run(() => ClientSocket.Receive(buffer));

				if (bytesReceived == 0)
				{
					Debug.WriteLine("Данные не получены");
					return;
				}

				var resultText = Encoding.UTF8.GetString(buffer, 0, bytesReceived);
				var result = JsonConvert.DeserializeObject<T>(resultText);
				OnGetNetworkData?.Invoke(result);
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Ошибка в GetNetworkData: {ex.Message}");
			}
		}

		public async Task UpdateNetworkData<T>(T obj)
		{
			try
			{
				var json = JsonConvert.SerializeObject(obj);
				Debug.WriteLine($"Отправка JSON: {json}");

				var data = Encoding.UTF8.GetBytes(json);

				await Task.Run(() =>
				{
					ClientSocket.Send(data);
				});

				if (ClientSocket != null && ClientSocket.Connected)
				{
					try
					{
						var buffer = new byte[65535];
						int bytesReceived = await Task.Run(() => ClientSocket.Receive(buffer));
						var resultText = Encoding.UTF8.GetString(buffer, 0, bytesReceived);
						var result = JsonConvert.DeserializeObject<T>(resultText);

						OnGetNetworkData?.Invoke(result);
					}
					catch (SocketException ex)
					{
						OnConnectionLost?.Invoke("Потеряно соединение с сервером");
						Debug.WriteLine($"Socket error: {ex.Message}");
					}
				}
				else
				{
					OnConnectionLost?.Invoke("Потеряно соединение с сервером");
					Debug.WriteLine("Сокет не подключён или равен null");
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Ошибка в UpdateNetworkData: {ex.Message}");
			}
		}

		public void CheckConnection()
		{
			if (ClientSocket == null || !ClientSocket.Connected)
			{
				OnConnectionLost?.Invoke("Соединение с сервером разорвано.");
				Dispose();
			}
		}

		public void Dispose()
		{
			try
			{
				if (ClientSocket != null)
				{
					if (ClientSocket.Connected)
					{
						ClientSocket.Shutdown(SocketShutdown.Both);
					}
					ClientSocket.Close();
					ClientSocket.Dispose();
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Ошибка при Dispose: {ex.Message}");
			}
		}

		public void UnsubscribeActions()
		{
			OnGetNetworkData = null;
		}
	}
}
