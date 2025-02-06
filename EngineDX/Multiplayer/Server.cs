using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LineRace.Multiplayer
{

	public class Server
	{
		public TcpListener socket;
		public IPEndPoint serverEndPoint;
		public List<TcpClient> tcpClients = new List<TcpClient>();
		private bool _isFirstPlayerConnected = false;
		private bool _isSecondPlayerConnected = false;

		public Server(string serverIp, int serverPort)
		{
			serverEndPoint = new IPEndPoint(IPAddress.Parse(serverIp), serverPort);
			socket = new TcpListener(serverEndPoint);
		}
		public Server(int serverPort)
		{
			var serverIp = Dns.GetHostEntry(Dns.GetHostName())
				.AddressList.First(adress => adress.AddressFamily == AddressFamily.InterNetwork);
			serverEndPoint = new IPEndPoint(serverIp, serverPort);
			socket = new TcpListener(serverEndPoint);
		}
		private string GetClientData(TcpClient client)
		{
			var data = GetClientByteData(client);
			if (data == null)
			{
				return null;
			}
			else
			{
				return Encoding.UTF8.GetString(data).TrimEnd('\0');
			}
		}
		private byte[] GetClientByteData(TcpClient client)
		{

			NetworkStream stream = client.GetStream();
			if (stream.DataAvailable)
			{
				byte[] data = new byte[client.Available];
				stream.Read(data, 0, data.Length);
				return data;
			}
			else return null;
		}
		private void SendClientData(string message, TcpClient client)
		{
			NetworkStream stream = client.GetStream();
			byte[] data = Encoding.UTF8.GetBytes(message);
			stream.Write(data, 0, data.Length);
		}
		private void SendClientByteData(byte[] data, TcpClient client)
		{
			if (data == null)
			{
				return;
			}
			NetworkStream stream = client.GetStream();
			if (stream.DataAvailable)
			{
				byte[] buff = new byte[client.Available];
				stream.Read(buff, 0, buff.Length);
				Debug.WriteLine("clean");
			}
			stream.Write(data, 0, data.Length);
		}
		public void MainServerLoop()
		{
			while (true)
			{
				SendClientByteData(GetClientByteData(tcpClients[0]), tcpClients[1]);
				SendClientByteData(GetClientByteData(tcpClients[1]), tcpClients[0]);

			}
		}
		public void StartAcceptPlayers()
		{
			socket.Start();
			while (tcpClients.Count < 2)
			{
				var client = socket.AcceptTcpClient();
				tcpClients.Add(client);
			}
			foreach (var client in tcpClients)
			{
				SendClientData("Wecominn ", client);
			}
			while (!(_isFirstPlayerConnected && _isSecondPlayerConnected))
			{
				if (GetClientData(tcpClients[0]) == "Ready to start")
				{
					_isFirstPlayerConnected = true;
				}
				if (GetClientData(tcpClients[1]) == "Ready to start")
				{
					_isSecondPlayerConnected = true;
				}
				Thread.Sleep(500);
			}
			foreach (var client in tcpClients)
			{
				SendClientData("Start ", client);
			}
		}
	}
}
