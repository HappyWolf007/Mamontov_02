using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace Mamontov_02.Pages
{
    public partial class AuthPage : Page
    {
        public AuthPage()
        {
            InitializeComponent();
        }
        private void ButtonEnter_Click(object sender, RoutedEventArgs e)
        {

            if (string.IsNullOrEmpty(TextBoxLogin.Text) || string.IsNullOrEmpty(PasswordBox.Password))
            {
                MessageBox.Show("Введите логин и пароль!");
                return;
            }

            using (var db = new Entities()) 
            {           
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
                    NavigationService?.Navigate(new CompletedAdsPage()); 
                    IsAuth.isAuth=true;
                }
            }
        }

        private void ButtonRegister_Click(object sender, RoutedEventArgs e)
        {
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
    }
}

