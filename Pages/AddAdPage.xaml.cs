using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Mamontov_02.Pages
{
    public partial class AddAdPage : Page
    {
        private Ads _adToEdit = new Ads();  

        public AddAdPage(Ads ad)
        {
            InitializeComponent();
            if (ad != null) 
                _adToEdit = ad;
            DataContext = ad;

            CityComboBox.ItemsSource = Entities.GetContext().City.Select(u => u.Name).ToList();
            CategoryComboBox.ItemsSource = Entities.GetContext().Category.Select(u => u.Name).ToList();
            AdsTypeComboBox.ItemsSource = Entities.GetContext().AdsType.Select(u => u.Name).ToList();
            if (ad != null)
            {
                NameTextBox.Text = ad.Name;
                DescriptionTextBox.Text = ad.Description;
                CostTextBox.Text = ad.Cost.ToString();
                PhotoTextBox.Text = ad.Photo;
            }

            if (ad != null)
            {
                StatusComboBox.Visibility = Visibility.Visible;
                StatusTextBlock.Visibility = Visibility.Visible;
       
                var selectedStatus = ad.IsOpen ? "Активно" : "Завершено";
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
            string adName = NameTextBox.Text;
            string adDescription = DescriptionTextBox.Text;
            string adCostText = CostTextBox.Text;
            string adPhoto = PhotoTextBox.Text;
            string adCity = CityComboBox.Text;
            string adCategory = CategoryComboBox.Text;
            string adAdsType = AdsTypeComboBox.Text;
            string adStatus = StatusComboBox.Text?.ToString();

            if (string.IsNullOrEmpty(adName) || string.IsNullOrEmpty(adDescription) || string.IsNullOrEmpty(adCostText) || string.IsNullOrEmpty(adCity) || string.IsNullOrEmpty(adCategory) || string.IsNullOrEmpty(adAdsType))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
                return;
            }

            int adCost;
            if (!int.TryParse(adCostText, out adCost) || adCost < 0)
            {
                MessageBox.Show("Пожалуйста, введите корректную неотрицательную целую стоимость.");
                return;
            }

            var selectedCityID = ReturnCity(adCity);
            var selectedCategoryID = ReturnCategory(adCategory);
            var selectedAdsTypeID = ReturnAdsType(adAdsType);
            bool isOpen = adStatus == "Активно" ? true : false;


            using (var db = new Entities())
            {
                if (_adToEdit != null)
                {
                    var currentAd = db.Ads.Where(x => x.ID == _adToEdit.ID).FirstOrDefault();
                    if (currentAd != null)
                    {
                        MessageBox.Show($"444");
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
                        NavigationService?.GoBack();
                    }


                    else
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
                            Seller = CurrentUser.UserName,
                            PublicationDate = DateTime.Now
                        };
                        MessageBox.Show($"Saving Ad: {adName}, {adDescription}, {adCost}, {adPhoto}, {adCity}, {adCategory}, {adAdsType}, {isOpen}");
                        db.Ads.Add(newAd);
                        db.SaveChanges();
                        MessageBox.Show("Объявление добавлено.");
                        NavigationService?.GoBack();
                    }
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
