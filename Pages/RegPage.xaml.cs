using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace Mamontov_02.Pages
{

    public partial class RegPage : Page
    {
        public RegPage()
        {
            InitializeComponent();
        }

        private void ButtonRegister_Click(object sender, RoutedEventArgs e)
        {

            if (string.IsNullOrEmpty(TextBoxLogin.Text) || string.IsNullOrEmpty(PasswordBox.Password) || string.IsNullOrEmpty(ConfirmPasswordBox.Password))
            {
                MessageBox.Show("Заполните все поля!");
                return;
            }

            if (PasswordBox.Password != ConfirmPasswordBox.Password)
            {
                MessageBox.Show("Пароли не совпадают!");
                return;
            }


            using (var db = new Entities()) 
            {
                var existingUser = db.User.FirstOrDefault(u => u.Username == TextBoxLogin.Text);
                if (existingUser != null)
                {
                    MessageBox.Show("Пользователь с таким логином уже существует.");
                    return;
                }

               
                var newUser = new User
                {
                    Username = TextBoxLogin.Text,
                    Password = PasswordBox.Password,
               
                };

                db.User.Add(newUser);
                db.SaveChanges();

                MessageBox.Show("Регистрация прошла успешно!");
                NavigationService?.Navigate(new AuthPage()); 
            }
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {

            NavigationService?.Navigate(new AuthPage());
        }

        private void PasswordBox_KeyDown(object sender, KeyEventArgs e)
        {
            txtHintPassword.Visibility = Visibility.Hidden;
        }

        private void ConfirmPasswordBox_KeyDown(object sender, KeyEventArgs e)
        {
            txtHintConfirmPassword.Visibility = Visibility.Hidden;
        }

        private void TextBoxLogin_TextChanged(object sender, TextChangedEventArgs e)
        {
            txtHintLogin.Visibility = Visibility.Hidden;
        }

       
    }
}