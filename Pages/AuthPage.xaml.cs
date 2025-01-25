using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        public AuthPage()
        {
            InitializeComponent();
        }

        private void ButtonEnter_Click(object sender, RoutedEventArgs e)
        {
            // Проверка на пустые поля
            if (string.IsNullOrEmpty(TextBoxLogin.Text) || string.IsNullOrEmpty(PasswordBox.Password))
            {
                MessageBox.Show("Введите логин и пароль!");
                return;
            }

            
            //string PasswordHash = GetHash(PasswordBox.Password);

            using (var db = new Entities()) // Убедитесь, что это ваш контекст базы данных
            {
                // Поиск пользователя с указанным логином и паролем
                var user = db.User
                             .FirstOrDefault(u => u.Username == TextBoxLogin.Text && u.Password == PasswordBox.Password);

                if (user == null)
                {
                    MessageBox.Show("Пользователь с такими данными не найден");
                    return;
                }

                else
                {
                    CurrentUser.UserName = user.Username;
                    NavigationService?.Navigate(new AdsPage()); 
                    IsAuth.isAuth=true;
                }
            }
        }

        private void ButtonRegister_Click(object sender, RoutedEventArgs e)
        {
            // Навигация на страницу регистрации
            NavigationService?.Navigate(new RegPage());
        }

        private void TextBoxLogin_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(TextBoxLogin.Text.Length > 0) txtHintLogin.Visibility = Visibility.Hidden;
            else txtHintLogin.Visibility = Visibility.Visible;
        }

        private void PasswordBox_KeyDown(object sender, KeyEventArgs e)
        {   if(PasswordBox.Password.Length > 0) 
                txtHintPassword.Visibility = Visibility.Hidden;
        }

        //public static string GetHash(string password)
        //{
        //    using (var hash = SHA1.Create())
        //    {
        //        return string.Concat(hash.ComputeHash(Encoding.UTF8.GetBytes(password)).Select(x => x.ToString("X2")));
        //    }
        //}
    }
}

