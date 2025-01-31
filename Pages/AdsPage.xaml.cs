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
            var categories = Entities.GetContext().Category.Select(u => u.Name).ToList();
            var ads = Entities.GetContext().Ads.ToList();
            var adsType = Entities.GetContext().AdsType.Select(u => u.Name).ToList();
            
            categories.Insert(0, "Все категории");
            cities.Insert(0, "Все города");
            adsType.Insert(0, "Все категории");
 
            CategoryComboBox.ItemsSource = categories;
            CategoryComboBox.SelectedIndex = 0;
            CityComboBox.ItemsSource = cities;
            CityComboBox.SelectedIndex = 0;
            AdsTypeComboBox.ItemsSource = adsType;
            AdsTypeComboBox.SelectedIndex = 0;
            StatusComboBox.SelectedIndex = 0;
            UpdateAds();
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

        

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
          
            SearchBox.Text = "";
            CategoryComboBox.SelectedIndex = 0; 
            StatusComboBox.SelectedIndex = 0;
            CityComboBox.SelectedIndex = 0;
            AdsTypeComboBox.SelectedIndex = 0;
            LoadAds();
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateAds();
        }
        private void UpdateAds()
        {
            var currentAds = Entities.GetContext().Ads.ToList();
            currentAds = currentAds.Where(x => x.Name.ToLower().Contains(SearchBox.Text.ToLower())).ToList();

            if (CategoryComboBox.SelectedIndex == 0)
                AdsListView.ItemsSource = currentAds.OrderBy(x => x.Name).ToList();
            else AdsListView.ItemsSource = currentAds.OrderByDescending(x => x.Name).ToList();
        }
        private void AddAdButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsAuth.isAuth)
                NavigationService?.Navigate(new AddAdPage(null));
            else
                MessageBox.Show("Пожалуйста, авторизуйтесь для данной функции через меню 'Личный кабинет'");
        }

        private void ViewCompletedButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new CompletedAdsPage());
        }

       

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedAd = (sender as Button)?.DataContext as Ads;
            if (selectedAd != null)
            {
                var result = MessageBox.Show("Вы уверены, что хотите удалить это объявление?", "Удалить объявление", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
         
                    var adToDelete = Entities.GetContext().Ads.FirstOrDefault(a => a.ID == selectedAd.ID);
                    if (adToDelete != null)
                    {
                        Entities.GetContext().Ads.Remove(adToDelete);
                        Entities.GetContext().SaveChanges();

                  
                        UpdateAds();
                    }
                }
            }
        }

        private void DeleteButton_Loaded(object sender, RoutedEventArgs e)
        {
            if (IsAuth.isAuth || CurrentUser.UserName == null)
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
            else
            {
                var button = sender as Button;
           
                button.Visibility = Visibility.Hidden;
                
            }
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

        private void AdsTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AdsTypeComboBox.SelectedIndex == 0)
            {
                var ads = Entities.GetContext().Ads.ToList();
                AdsListView.ItemsSource = ads;
            }
            else
            {
                var ads = Entities.GetContext().Ads.ToList();
                ads = ads.Where(x => x.AdsTypeID == AdsTypeComboBox.SelectedIndex).ToList();
                AdsListView.ItemsSource = ads;
            }
        }

        private void StatusComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedStatus = (StatusComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            var ads = Entities.GetContext().Ads.ToList();

            if (selectedStatus == "Активно")
            {
                ads = ads.Where(x => x.IsOpen).ToList();
            }
            else if (selectedStatus == "Завершено")
            {
                ads = ads.Where(x => !x.IsOpen).ToList();
            }
            else ads = ads.ToList();

            AdsListView.ItemsSource = ads;
        }

       

        private void AdsListView_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (IsAuth.isAuth)
            {
                var selectedAd = AdsListView.SelectedItem as Ads; 
                if (selectedAd != null)
                {
                    NavigationService?.Navigate(new AddAdPage(selectedAd));
                }
            }
           
            
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            
                Entities.GetContext().ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
                AdsListView.ItemsSource = Entities.GetContext().Ads.ToList();
            
        }
    }
}
