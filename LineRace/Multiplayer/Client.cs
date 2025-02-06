using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using SharpDX;
using Newtonsoft.Json;

namespace LineRace.Multiplayer
{
	public class Client
	{

		public TcpClient socket;
		public IPEndPoint serverEndPoint;
		Packet packet = new Packet(1, 1, 0, 1, 1, 1, 1, 1, 1, 1, 1, 100, 100);

		public Client(string serverIp, int serverPort)
		{
			serverEndPoint = new IPEndPoint(IPAddress.Parse(serverIp), serverPort);
			socket = new TcpClient();
			Connect();
		}
		public Client(IPEndPoint serverEndPoint)
		{
			this.serverEndPoint = serverEndPoint;
			socket = new TcpClient();
			Connect();
		}
		public void Connect()
		{
			socket.Connect(serverEndPoint);
		}
		public string GetDataFromServer()
		{
			if (socket.Available > 0)
			{
				NetworkStream stream = socket.GetStream();
				byte[] data = new byte[socket.ReceiveBufferSize];
				int bytes = stream.Read(data, 0, data.Length);
				//Debug.WriteLine(Encoding.UTF8.GetString(data, 0, bytes));
				return Encoding.UTF8.GetString(data, 0, bytes);
			}
			return null;
		}
		public Packet GetPacketDataFromServer()
		{
			var data = GetDataFromServer();
			if (data != null)
			{
				var datas = data.Split(' ');
				var packets = new List<Packet>();
				foreach (var d in datas)
				{
					if (d != "")
					{
						try
						{
							packets.Add(JsonSerializer.Deserialize<Packet>(d));
						}
						catch
						{

						}
					}
				}
				foreach (var pack in packets)
				{
					packet = pack;
				}

			}

			return packet;
		}
		public void SendData(Packet packet)
		{
			NetworkStream stream = socket.GetStream();
			byte[] data = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(packet) + " ");
			stream.WriteAsync(data, 0, data.Length);
		}
		public void SendData1(string s)
		{
			NetworkStream stream = socket.GetStream();
			byte[] data = Encoding.UTF8.GetBytes(s);
			stream.Write(data, 0, data.Length);
		}
		public static SharpDX.Vector2 Parse(string data)
		{
			if (data == "Wecominn" || data == null)
			{
				return new SharpDX.Vector2(0f);
			}
			//X: 1398,5 Y: 670X: 1398,5 Y: 670
			data = data.Replace(':', ' ');
			var result = data.Split(' ');

			return new SharpDX.Vector2(float.Parse(result[1]), float.Parse(result[3].Replace("X", "")));
		}
	}
}
