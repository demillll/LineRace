using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LineRace.Engine;
using LineRace.Multiplayer;

namespace WpfApp_Game
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			Client client;
			//try
			//{
			if ((bool)CheckBox.IsChecked)
			{
				var server = new Server(5555);
				var th1 = new Thread(() =>
				{
					server.StartAcceptPlayers();
					server.MainServerLoop();
				});
				th1.Start();
				this.Title = server.serverEndPoint.ToString();
				client = new Client(server.serverEndPoint);
				StartApplication(client);
			}
			else
			{
				client = new Client(TextBox.Text, 5555);
				StartApplication(client);
			}
			//}
			//catch (Exception ex)
			//{
			//    MessageBox.Show(ex.Message);
			//}
		}
		private void StartApplication(Client client)
		{
			while (true)
			{
				string d = client.GetDataFromServer();
				if (d != null && d == "Wecominn ")
				{
					Debug.WriteLine("Connected");
					break;
				}
				Thread.Sleep(50);
			}
			client.SendData1("Ready to start");
			while (true)
			{
				if (client.GetDataFromServer() == "Start ")
				{
					Debug.WriteLine("Start game");
					break;
				}
				Thread.Sleep(50);
			}
			using (RenderingApp application = new RenderingApp())
			{
				application.Run(client, (bool)CheckBox.IsChecked);
			}
		}
	}
}
