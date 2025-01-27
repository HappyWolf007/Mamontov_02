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

    public partial class EditAdPage : Page
    {
        private int _adId;

        public EditAdPage(int adId)
        {
            InitializeComponent();
            _adId = adId;
            LoadAdData();  
        }

        private void LoadAdData()
        {
            using (var db = new Entities())  
            {
                var ad = db.Ads.FirstOrDefault(a => a.ID == _adId);
                if (ad != null)
                {
                    NameTextBox.Text = ad.Name;
                    DescriptionTextBox.Text = ad.Description;
                    CostTextBox.Text = ad.Cost.ToString();

                    var categoryItem = CategoryComboBox.Items
                        .Cast<ComboBoxItem>()
                        .FirstOrDefault(c => c.Content.ToString() == ad.Category.Name); 

                    if (categoryItem != null)
                    {
                        CategoryComboBox.SelectedItem = categoryItem;
                    }
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {

            if (string.IsNullOrEmpty(NameTextBox.Text) || string.IsNullOrEmpty(DescriptionTextBox.Text) || string.IsNullOrEmpty(CostTextBox.Text))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
                return;
            }

            using (var db = new Entities())  
            {
                var ad = db.Ads.FirstOrDefault(a => a.ID == _adId);
                if (ad != null)
                {
                    ad.Name = NameTextBox.Text;
                    ad.Description = DescriptionTextBox.Text;
                    ad.Cost = decimal.Parse(CostTextBox.Text);

                    var selectedCategoryName = (CategoryComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

                    var category = db.Category.FirstOrDefault(c => c.Name == selectedCategoryName);

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
                    NavigationService?.GoBack(); 
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