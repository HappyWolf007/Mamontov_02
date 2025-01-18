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

            // Загружаем категории и добавляем сортировку
            var categories = Entities.GetContext().Category.Select(u => u.Name).ToList();
            var ads = Entities.GetContext().Ads.ToList();
            // Добавляем "Все категории" как первый элемент
            categories.Insert(0, "Все категории");

            // Привязываем отсортированные категории
            CategoryComboBox.ItemsSource = categories;
            CategoryComboBox.SelectedIndex = 0; // Устанавливаем по умолчанию "Все категории"
        }

        private void CategoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Загружаем объявления на основе выбранной категории
            LoadAds();
        }

        private void LoadAds()
        {
            var ads = Entities.GetContext().Ads.ToList();
            ads = ads.Where(x => x.Name.ToLower().Contains(SearchBox.Text.ToLower())).ToList();

            // Если выбрана категория, фильтруем объявления по выбранной категории


            //if (CategoryComboBox.Text != "Все категории")
            //{
            //ads = ads.Where(x => x.CategoryID == CategoryComboBox.SelectedIndex).ToList();
            //}



            // Привязка фильтрованных данных к ListView
            //AdsListView.ItemsSource = ads;
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {  
            if (CategoryComboBox.Text != "Все категории")
            {
                var ads = Entities.GetContext().Ads.ToList();
                ads = ads.Where(x => x.CategoryID == CategoryComboBox.SelectedIndex).ToList();
                AdsListView.ItemsSource = ads;
            }
         

        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            // Сбрасываем все фильтры и обновляем список
            SearchBox.Text = "";
            CategoryComboBox.SelectedIndex = 0; // "Все категории"
            LoadAds();
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            LoadAds();
        }

        // Кнопки для навигации и действий с объявлениями
        private void AddAdButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new AddAdPage());
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
                NavigationService?.Navigate(new EditAdPage(selectedAd.ID));
            }
        }

        private void EditAdButton_Loaded(object sender, RoutedEventArgs e)
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

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // Обновление комбобокса при загрузке страницы
            Entities.GetContext().ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
            AdsListView.ItemsSource = Entities.GetContext().Ads.ToList();
        }
    }
}
