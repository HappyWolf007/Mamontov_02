using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Mamontov_02.Pages
{
    public partial class AddAdPage : Page
    {
        private Ads _adToEdit = new Ads();  // Объявление для редактирования

        // Конструктор для редактирования существующего объявления
        public AddAdPage(Ads ad)
        {
            InitializeComponent();
            if (ad != null)
                _adToEdit = ad;
            DataContext = ad;

            // LoadCities();
            CityComboBox.ItemsSource = Entities.GetContext().City.Select(u => u.Name).ToList();
            CategoryComboBox.ItemsSource = Entities.GetContext().Category.Select(u => u.Name).ToList();
            AdsTypeComboBox.ItemsSource = Entities.GetContext().AdsType.Select(u => u.Name).ToList();
            //LoadCategory();
            //LoadAdsTypes();

            // Заполняем поля на основе данных объявления
            NameTextBox.Text = ad.Name;
            DescriptionTextBox.Text = ad.Description;
            CostTextBox.Text = ad.Cost.ToString();
            PhotoTextBox.Text = ad.Photo;

            // Устанавливаем выбранные значения в ComboBox
            // CityComboBox.SelectedValue = ad.CityID;
            //CategoryComboBox.SelectedValue = ad.CategoryID;
            //AdsTypeComboBox.SelectedValue = ad.AdsType;

            // Если редактируем существующее объявление, показываем ComboBox для статуса
            if (_adToEdit != null)
            {
                StatusComboBox.Visibility = Visibility.Visible;
                StatusTextBlock.Visibility = Visibility.Visible;
                // Устанавливаем текущий статус в ComboBox
                var selectedStatus = ad.IsOpen ? "Открыто" : "Завершено";
                foreach (var item in StatusComboBox.Items)
                {
                    var comboBoxItem = item as ComboBoxItem;
                    if (comboBoxItem?.Content.ToString() == selectedStatus)
                    {
                        StatusComboBox.SelectedItem = comboBoxItem;
                        break;
                    }
                }
            }
        }

        


        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Считываем данные из полей
            string adName = NameTextBox.Text;
            string adDescription = DescriptionTextBox.Text;
            string adCostText = CostTextBox.Text;
            string adPhoto = PhotoTextBox.Text;
            string adCity = CityComboBox.Text;
            string adCategory = CategoryComboBox.Text;
            string adAdsType = AdsTypeComboBox.Text;
            string adStatus = StatusComboBox.Text?.ToString();
            if(PhotoTextBox.Text == null) { adPhoto = "NULL"; }
            // Проверка на пустые поля
            if (string.IsNullOrEmpty(adName) || string.IsNullOrEmpty(adDescription) || string.IsNullOrEmpty(adCostText))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
                return;
            }

            // Попробуем безопасно преобразовать стоимость
            decimal adCost;
            if (!decimal.TryParse(adCostText, out adCost))
            {
                MessageBox.Show("Пожалуйста, введите корректную стоимость.");
                return;
            }

            // Получаем выбранные значения
            //var selectedCategory = Entities.GetContext().Category
            //    .Where(c => c.Name == adCategory)
            //    .FirstOrDefault();

            //if (selectedCategory == null)
            //{
            //    MessageBox.Show("Категория не найдена.");
            //    return;
            //}

            var selectedCityID = ReturnCity(adCity);
            var selectedCategoryID = ReturnCategory(adCategory);
            var selectedAdsTypeID = ReturnAdsType(adAdsType);


            // Проверка на статус
            bool isOpen = adStatus == "Открыто" ? true : false;

            using (var db = new Entities())
            {
                // Если редактируем существующее объявление
                if (_adToEdit != null)
                {
                    var currentAd = db.Ads.Where(x => x.ID == _adToEdit.ID).FirstOrDefault();
                    if (currentAd != null)
                    {
                        currentAd.Name = adName;
                        currentAd.Description = adDescription;
                        currentAd.Cost = adCost;
                        currentAd.Photo = adPhoto;
                        currentAd.CategoryID = selectedCategoryID;
                        currentAd.AdsTypeID = selectedAdsTypeID;
                        currentAd.CityID = selectedCityID;
                        currentAd.IsOpen = isOpen;
                        db.SaveChanges();
                        MessageBox.Show("Объявление обновлено.");
                        NavigationService?.GoBack();  // Возвращаемся на предыдущую страницу
                    }
                    else
                    {
                        MessageBox.Show("Объявление не найдено.");
                    }
                }
                else  // Если создаем новое объявление
                {
                    var newAd = new Ads
                    {
                        Name = adName,
                        Description = adDescription,
                        Cost = adCost,
                        Photo = adPhoto,
                        CategoryID = selectedCategoryID,
                        AdsTypeID = selectedAdsTypeID,
                        CityID = selectedCityID,
                        IsOpen = isOpen,
                        PublicationDate = DateTime.Now
                    };

                    db.Ads.Add(newAd);
                    db.SaveChanges();
                    MessageBox.Show("Объявление добавлено.");
                    NavigationService?.GoBack();  // Возвращаемся на предыдущую страницу
                }
            }
        }
    

        private int ReturnCity(string addCity)
        {
            return Entities.GetContext().City.Where(x => x.Name == addCity).Select(u => u.ID).FirstOrDefault();
        }
        private int ReturnCategory(string addCategory)
        {
            return Entities.GetContext().Category.Where(x => x.Name == addCategory).Select(u => u.ID).FirstOrDefault();
        }
        
        private int ReturnAdsType(string addAdsType)
        {
            return Entities.GetContext().AdsType.Where(x => x.Name == addAdsType).Select(u => u.ID).FirstOrDefault();
        }
    }

}
