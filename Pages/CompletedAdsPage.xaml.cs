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

namespace Mamontov_02.Pages
{

    public partial class CompletedAdsPage : Page
    {
        public CompletedAdsPage()
        {
            InitializeComponent();
            UpdateAds();
        }

        private void EditAdButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedAd = (sender as Button)?.DataContext as Ads;
            if (selectedAd != null)
            {
                NavigationService?.Navigate(new AddAdPage(selectedAd));
            }
        }

        private void UpdateAds()
        {
            var ads = Entities.GetContext().Ads.ToList();

            if (CurrentUser.UserName != null)
                ads = ads.Where(x => x.Seller == CurrentUser.UserName).ToList();

           
            if (ShowCompletedAdsCheckBox.IsChecked == true)
            {
                ads = ads.Where(x => !x.IsOpen).ToList();
            }

            AdsListView.ItemsSource = ads;

            decimal totalProfit = ads.Where(x => !x.IsOpen).Sum(x => x.Cost);
            TotalProfit.Text = $"Общая прибыль: {totalProfit:C}";
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
          

            Entities.GetContext().ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
            AdsListView.ItemsSource = Entities.GetContext().Ads.ToList();

            
        }

        private void ShowCompletedAdsCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            UpdateAds();
        }

        private void ShowCompletedAdsCheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            UpdateAds();
        }

        

        private void AdsListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
          
                var selectedAd = AdsListView.SelectedItem as Ads;
                if (selectedAd != null)
                {
                    NavigationService?.Navigate(new AddAdPage(selectedAd));
                }
            
        }
    }

}
