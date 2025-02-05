using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LineRaceWindow
{
	public partial class NetworkGameControl : UserControl
	{
		private MainWindow _mainWindow;

		public NetworkGameControl(MainWindow mainWindow)
		{
			InitializeComponent();
			_mainWindow = mainWindow;
		}

		private void BackButton_Click(object sender, RoutedEventArgs e)
		{
			// Вызываем метод возврата в главное меню
			_mainWindow.BackToMainMenu();
		}




		private void CreateServerButton_Click(object sender, RoutedEventArgs e)
		{
			StatusText.Text = "Сервер запущен, ожидайте подключение клиента";
		}

		private void ConnectServerButton_Click(object sender, RoutedEventArgs e)
		{
			IpInputPanel.Visibility = Visibility.Visible;
		}

		private void ConfirmIpButton_Click(object sender, RoutedEventArgs e)
		{
			string ip = IpAddressBox.Text;
			StatusText.Text = $"Подключение к {ip}...";
		}
	}
}
