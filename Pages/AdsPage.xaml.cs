using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Mamontov_02;

namespace Mamontov_02.Pages
{
    public partial class AdsPage : Page
    {
        public AdsPage()
        {
            InitializeComponent();
            var cities = Entities.GetContext().City.Select(u => u.Name).ToList();
           
            //CityComboBox.DisplayMemberPath = "Name";
            //CityComboBox.SelectedValuePath = "ID";

            var categories = Entities.GetContext().Category.Select(u => u.Name).ToList();
            var ads = Entities.GetContext().Ads.ToList();

            categories.Insert(0, "Все категории");
            cities.Insert(0, "Все города");

            CategoryComboBox.ItemsSource = categories;
            CategoryComboBox.SelectedIndex = 0;
            CityComboBox.ItemsSource = cities;
            CityComboBox.SelectedIndex = 0;
        }

        private void CategoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CategoryComboBox.SelectedIndex == 0)
            {
                var ads = Entities.GetContext().Ads.ToList();
                AdsListView.ItemsSource = ads;
            }
            else
            {
               

                var ads = Entities.GetContext().Ads.ToList();
                ads = ads.Where(x => x.CategoryID == CategoryComboBox.SelectedIndex).ToList();
                AdsListView.ItemsSource = ads;
            }
        }

        private void LoadAds()
        {
            var ads = Entities.GetContext().Ads.ToList();
            ads = ads.Where(x => x.Name.ToLower().Contains(SearchBox.Text.ToLower())).ToList();

            



        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {  
            if (CategoryComboBox.Text != "Все категории")
            {
                var ads = Entities.GetContext().Ads.ToList();
                ads = ads.Where(x => x.CategoryID == CategoryComboBox.SelectedIndex).ToList();
                AdsListView.ItemsSource = ads;
            }
            else
            {
                var ads = Entities.GetContext().Ads.ToList();
                AdsListView.ItemsSource = ads;
            }
         

        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
          
            SearchBox.Text = "";
            CategoryComboBox.SelectedIndex = 0; 
            LoadAds();
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateAds();
        }
        private void UpdateAds()
        {
            var currentUsers = Entities.GetContext().Ads.ToList();
            currentUsers = currentUsers.Where(x => x.Name.ToLower().Contains(SearchBox.Text.ToLower())).ToList();

            if (CategoryComboBox.SelectedIndex == 0)
                AdsListView.ItemsSource = currentUsers.OrderBy(x => x.Name).ToList();
            else AdsListView.ItemsSource = currentUsers.OrderByDescending(x => x.Name).ToList();
        }
        private void AddAdButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new AddAdPage(null));
        }

        private void ViewCompletedButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new CompletedAdsPage());
        }

        private void EditAd_Click(object sender, RoutedEventArgs e)
        {
            var selectedAd = (sender as Button)?.DataContext as Ads;
            if (selectedAd != null)
            {
                NavigationService?.Navigate(new EditAdPage(selectedAd.ID));
            }
        }

        private void EditAdButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedAd = (sender as Button)?.DataContext as Ads;
            if (selectedAd != null)
            {
                NavigationService?.Navigate(new AddAdPage(selectedAd));  // Передаем объявление в AddAdPage для редактирования
            }
        }

        private void EditAdButton_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsAuth.isAuth)
            {
                var button = sender as Button;
                var ad = button?.DataContext as Ads;

                if (ad != null && ad.Seller == CurrentUser.UserName)
                {
                    button.Visibility = Visibility.Visible;
                }
                else
                {
                    button.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

            //Entities.GetContext().ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
            //AdsListView.ItemsSource = Entities.GetContext().Ads.ToList();
        }

        private void CityComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CityComboBox.SelectedIndex == 0)
            {
                var ads = Entities.GetContext().Ads.ToList();
                AdsListView.ItemsSource = ads;
            }
            else
            {
                var ads = Entities.GetContext().Ads.ToList();
                ads = ads.Where(x => x.CityID == CityComboBox.SelectedIndex).ToList();
                AdsListView.ItemsSource = ads;
            }
        }
    }
}
