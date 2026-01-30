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

namespace PR13
{
    /// <summary>
    /// Логика взаимодействия для CartPage.xaml
    /// </summary>
    public partial class CartPage : Page
    {
        public CartPage()
        {
            InitializeComponent();
            LoadCart();
        }

        private void LoadCart()
        {
            CartList.ItemsSource = Core.Cart;
            TotalText.Text = $"Общая сумма: {Core.Cart.Sum(i => i.Product.Price * i.Quantity):C}";
        }

        private void Checkout_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CheckoutPage());
        }
    }
}
