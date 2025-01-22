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
    /// <summary>
    /// Логика взаимодействия для AddAdPage.xaml
    /// </summary>
    public partial class AddAdPage : Page
    {
        private Ads _adToEdit;  // Объявление для редактирования

        // Конструктор для создания нового объявления
      

        // Конструктор для редактирования существующего объявления
        public AddAdPage(Ads ad)
        {
            InitializeComponent();

            _adToEdit = ad;
            LoadCities();
            LoadCategory();
            LoadAdsTypes();

            // Заполняем поля на основе данных объявления
            NameTextBox.Text = ad.Name;
            DescriptionTextBox.Text = ad.Description;
            CostTextBox.Text = ad.Cost.ToString();
            PhotoTextBox.Text = ad.Photo;

            // Устанавливаем выбранные значения в ComboBox
            CityComboBox.SelectedValue = ad.CityID;
            CategoryComboBox.SelectedValue = ad.CategoryID;  // Или можно использовать индексы, если хотите работать с номерами
            AdsTypeComboBox.SelectedValue = ad.AdsType;
        }

        private void LoadCities()
        {
            using (var db = new Entities())
            {
                var cities = db.City.ToList();
                CityComboBox.ItemsSource = cities;
                CityComboBox.DisplayMemberPath = "Name";
                CityComboBox.SelectedValuePath = "ID";
            }
        }
        private void LoadCategory()
        {
            using (var db = new Entities())
            {
                var categories1 = db.Category.ToList();
                CategoryComboBox.ItemsSource = categories1;
                CategoryComboBox.DisplayMemberPath = "Name";
                CategoryComboBox.SelectedValuePath = "ID";
            }
        }
        private void LoadAdsTypes()
        {
            using (var db = new Entities())
            {
                var cities = db.AdsType.ToList();
                AdsTypeComboBox.ItemsSource = cities;
                AdsTypeComboBox.DisplayMemberPath = "Name";
                AdsTypeComboBox.SelectedValuePath = "Name";
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Проверка на пустые поля
            if (string.IsNullOrEmpty(NameTextBox.Text) || string.IsNullOrEmpty(DescriptionTextBox.Text) ||
                string.IsNullOrEmpty(CostTextBox.Text)) // Фото должно быть обязательным полем
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
                return;
            }

            // Попробуем безопасно преобразовать стоимость
            decimal cost;
            if (!decimal.TryParse(CostTextBox.Text, out cost))
            {
                MessageBox.Show("Пожалуйста, введите корректную стоимость.");
                return;
            }

            using (var db = new Entities())
            {
                var selectedCategoryName = (CategoryComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
                var category = db.Category.FirstOrDefault(c => c.Name == selectedCategoryName);

                if (category == null)
                {
                    MessageBox.Show("Категория не найдена.");
                    return;
                }

                var selectedCity = CityComboBox.SelectedItem as City;
                if (selectedCity == null)
                {
                    MessageBox.Show("Город не найден.");
                    return;
                }

                var selectedAdsType = (AdsTypeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
                if (string.IsNullOrEmpty(selectedAdsType))
                {
                    MessageBox.Show("Пожалуйста, выберите тип объявления.");
                    return;
                }

                if (_adToEdit == null)  // Если редактируем, то обновляем существующее объявление
                {
                    // Создаем новое объявление
                    var newAd = new Ads
                    {
                        Name = NameTextBox.Text,
                        Description = DescriptionTextBox.Text,
                        Cost = cost,
                        Category = category,
                        Seller = CurrentUser.UserName,
                        AdsType = selectedAdsType,
                        City = selectedCity,
                        CityID = selectedCity.ID,
                        Photo = PhotoTextBox.Text,
                        IsOpen = true,
                        PublicationDate = DateTime.Now
                    };

                    db.Ads.Add(newAd);
                }
                else  // Если редактируем существующее, то просто обновляем
                {
                    // Обновляем поля существующего объявления
                    _adToEdit.Name = NameTextBox.Text;
                    _adToEdit.Description = DescriptionTextBox.Text;
                    _adToEdit.Cost = cost;
                    _adToEdit.Category = category;
                    _adToEdit.City = selectedCity;
                    _adToEdit.AdsType = selectedAdsType;
                    _adToEdit.Photo = PhotoTextBox.Text;

                    // Так как объект уже отслеживается, дополнительных изменений состояния делать не нужно
                }

                db.SaveChanges();  // Сохраняем изменения
                MessageBox.Show("Объявление сохранено.");
                NavigationService?.GoBack();  // Возвращаемся на предыдущую страницу
            }
        }
    }

}