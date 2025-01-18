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
        public AddAdPage()
        {
            InitializeComponent();

            // Загружаем список городов в ComboBox
            using (var db = new Entities())
            {
                var cities = db.City.ToList();  // Получаем все города из базы данных
                CityComboBox.ItemsSource = cities;  // Присваиваем список городов
                CityComboBox.DisplayMemberPath = "Name";  // Название города для отображения
                CityComboBox.SelectedValuePath = "ID";   // ID города, который будет сохраняться
            }

            AdsTypeComboBox.SelectedIndex = 0;
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

            using (var db = new Entities())  // Используем контекст данных
            {
                // Получаем имя выбранной категории из ComboBox
                var selectedCategoryName = (CategoryComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

                // Находим объект категории в базе данных по имени
                var category = db.Category.FirstOrDefault(c => c.Name == selectedCategoryName);

                if (category == null)
                {
                    MessageBox.Show("Категория не найдена.");
                    return;
                }

                // Получаем выбранный город из ComboBox
                var selectedCity = CityComboBox.SelectedItem as City;  // Выбираем объект City
                if (selectedCity == null)
                {
                    MessageBox.Show("Город не найден.");
                    return;
                }

                // Получаем выбранный тип объявления
                var selectedAdsType = (AdsTypeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

                if (string.IsNullOrEmpty(selectedAdsType))
                {
                    MessageBox.Show("Пожалуйста, выберите тип объявления.");
                    return;
                }

                // Создаем новое объявление
                var newAd = new Ads
                {
                    Name = NameTextBox.Text,
                    Description = DescriptionTextBox.Text,
                    Cost = cost,  // Присваиваем стоимость
                    Category = category,  // Присваиваем объект Category
                    Seller = CurrentUser.UserName,  // Присваиваем продавца
                    AdsType = selectedAdsType,  // Присваиваем тип объявления
                    City = selectedCity,  // Присваиваем город
                    CityID = selectedCity.ID,  // Устанавливаем ID города
                    Photo = PhotoTextBox.Text,  // Адрес фотографии
                    IsOpen = true,  // Объявление по умолчанию открыто
                    PublicationDate = DateTime.Now  // Устанавливаем дату публикации
                };

                // Добавляем объявление в базу данных
                db.Ads.Add(newAd);
                db.SaveChanges();

                MessageBox.Show("Объявление добавлено.");
                NavigationService?.GoBack();  // Возвращаемся на предыдущую страницу
            }
        }







    }
}