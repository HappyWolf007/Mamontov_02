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
    /// Логика взаимодействия для EditAdPage.xaml
    /// </summary>
    public partial class EditAdPage : Page
    {
        private int _adId;

        public EditAdPage(int adId)
        {
            InitializeComponent();
            _adId = adId;
            LoadAdData();  // Загружаем данные объявления
        }

        private void LoadAdData()
        {
            using (var db = new Entities())  // Используем контекст данных
            {
                var ad = db.Ads.FirstOrDefault(a => a.ID == _adId);
                if (ad != null)
                {
                    NameTextBox.Text = ad.Name;
                    DescriptionTextBox.Text = ad.Description;
                    CostTextBox.Text = ad.Cost.ToString();

                    // Исправленный код для выбора категории из ComboBox
                    var categoryItem = CategoryComboBox.Items
                        .Cast<ComboBoxItem>()
                        .FirstOrDefault(c => c.Content.ToString() == ad.Category.Name); // Сравниваем с именем категории

                    if (categoryItem != null)
                    {
                        CategoryComboBox.SelectedItem = categoryItem;
                    }
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Проверка на пустые поля
            if (string.IsNullOrEmpty(NameTextBox.Text) || string.IsNullOrEmpty(DescriptionTextBox.Text) || string.IsNullOrEmpty(CostTextBox.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
                return;
            }

            using (var db = new Entities())  // Используем контекст данных
            {
                var ad = db.Ads.FirstOrDefault(a => a.ID == _adId);
                if (ad != null)
                {
                    ad.Name = NameTextBox.Text;
                    ad.Description = DescriptionTextBox.Text;
                    ad.Cost = decimal.Parse(CostTextBox.Text);

                    // Получаем имя выбранной категории из ComboBox
                    var selectedCategoryName = (CategoryComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

                    // Находим объект категории по имени
                    var category = db.Category.FirstOrDefault(c => c.Name == selectedCategoryName);

                    // Если категория найдена, присваиваем её
                    if (category != null)
                    {
                        ad.Category = category;
                    }
                    else
                    {
                        MessageBox.Show("Категория не найдена.");
                        return;
                    }

                    db.SaveChanges();

                    MessageBox.Show("Объявление обновлено.");
                    NavigationService?.GoBack();  // Возвращаемся на предыдущую страницу
                }
                else
                {
                    MessageBox.Show("Объявление не найдено.");
                }
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}