using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using LineRace;

namespace LineRaceWindow
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
			// Главное меню по умолчанию видно
			MainGrid.Visibility = Visibility.Visible;
			MainContent.Visibility = Visibility.Collapsed;
		}

        private void PlayOnOneComputerButton_Click(object sender, RoutedEventArgs e)
        {
            GameScene gameScene = new GameScene();
            gameScene.Run();
        }
		// Обработчик для кнопки "Играть по сети"
		private void PlayOnNETButton_Click(object sender, RoutedEventArgs e)
		{
			// Скрыть главное меню
			MainGrid.Visibility = Visibility.Collapsed;

			// Показать сетевую игру
			MainContent.Visibility = Visibility.Visible;
			MainContent.Content = new NetworkGameControl(this); // Загружаем сетевой экран
		}

		public void BackToMainMenu()
		{
			// Скрыть сетевую игру
			MainContent.Visibility = Visibility.Collapsed;

			// Показать главное меню
			MainGrid.Visibility = Visibility.Visible;
		}



		// Обработчик для кнопки "Выход"
		private void ExitButton_Click(object sender, RoutedEventArgs e)
		{
			System.Windows.Application.Current.Shutdown();
		}


	}
}
