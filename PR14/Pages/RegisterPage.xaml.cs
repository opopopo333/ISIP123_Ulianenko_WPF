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

namespace PR14.Pages
{
    /// <summary>
    /// Логика взаимодействия для RegisterPage.xaml
    /// </summary>
    public partial class RegisterPage : Page
    {
        public RegisterPage()
        {
            InitializeComponent();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            string fullName = FullNameBox.Text.Trim();
            string login = LoginBox.Text.Trim();
            string password = PasswordBox.Password.Trim();

            if (string.IsNullOrEmpty(fullName) ||
                string.IsNullOrEmpty(login) ||
                string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Заполните все поля.");
                return;
            }

            bool loginExists = Core.Context.Users
                .Any(u => u.Login == login);

            if (loginExists)
            {
                MessageBox.Show("Пользователь с таким логином уже существует.");
                return;
            }

            var newUser = new User
            {
                FullName = fullName,
                Login = login,
                Password = password
            };

            Core.Context.Users.Add(newUser);
            Core.Context.SaveChanges();

            MessageBox.Show("Регистрация успешна.");

            bool success = RegisterUser(FullNameBox.Text.Trim(), LoginBox.Text.Trim(), PasswordBox.Password.Trim());

            if (success)
            {
                NavigationService.Navigate(new LoginPage());
            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new LoginPage());
        }


        public bool RegisterUser(string fullName, string login, string password)
        {
            if (string.IsNullOrEmpty(fullName) || string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                return false;
            }

            using (var db = new pr14Entities1())
            {
                bool loginExists = db.Users.Any(u => u.Login == login);

                if (loginExists)
                {
                    return false;
                }

                try
                {
                    var newUser = new User
                    {
                        FullName = fullName,
                        Login = login,
                        Password = password
                    };

                    db.Users.Add(newUser);
                    db.SaveChanges();

                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

    }
}
