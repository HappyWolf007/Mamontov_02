using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
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
using Mamontov_02.Pages;

namespace Mamontov_02
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Closing += MainWindow_Closing;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (MainFrame.CanGoBack) MainFrame.GoBack();
        }

        private void MainFrame_OnNavigated(object sender, NavigationEventArgs e)
        {
            string username = CurrentUser.UserName;
            if (username == null) username = "Пользователь";
            if (!(e.Content is Page page)) return;
            this.Title = $"{username} - {page.Title}";
            if (page is AdsPage)
            {
                BackButton.Visibility = Visibility.Collapsed;
                MainPageButton.Visibility = Visibility.Collapsed;
            }
            if (page is AuthPage)
            {
                PersonalAccountButton.Visibility = Visibility.Collapsed;
                
            }

            else
            {
                PersonalAccountButton.Visibility = Visibility.Visible;
                BackButton.Visibility = Visibility.Visible;
                MainPageButton.Visibility = Visibility.Visible;
            }
        }
      
        private void PersonalAccountButton_Click(object sender, RoutedEventArgs e)
        {
            if (!IsAuth.isAuth)
                MainFrame.NavigationService.Navigate(new AuthPage());
            else
                MainFrame.NavigationService.Navigate(new CompletedAdsPage());
        }

        private void MainPageButton_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(new AdsPage());
        } 
        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы действительно хотите выйти из приложения?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.No)
            {
                e.Cancel = true;
            }
            else if (result == MessageBoxResult.Yes) 
            {
                CurrentUser.UserName = null;
                IsAuth.isAuth = false;
            }
        }
    }
}
       