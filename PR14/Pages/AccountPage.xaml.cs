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
    /// Логика взаимодействия для AccountPage.xaml
    /// </summary>
    public partial class AccountPage : Page
    {
        public AccountPage()
        {
            InitializeComponent();
            LoadUserData();
        }

        private void LoadUserData()
        {
            if (Core.CurrentUser != null)
            {
                FullNameText.Text = Core.CurrentUser.FullName;
                LoginText.Text = Core.CurrentUser.Login;
            }
        }

        private void Orders_Click(object sender, RoutedEventArgs e)
        {
            var userOrders = Core.Context.Orders
                .Where(o => o.UserId == Core.CurrentUser.Id)
                .ToList();

            if (userOrders.Count == 0)
            {
                MessageBox.Show("У вас пока нет заказов.");
            }
            else
            {
                MessageBox.Show($"Количество заказов: {userOrders.Count}");
            }
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            Core.CurrentUser = null;
            NavigationService.Navigate(new LoginPage());
        }
    }
}
