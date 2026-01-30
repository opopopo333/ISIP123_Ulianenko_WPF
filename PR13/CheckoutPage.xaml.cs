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
    /// Логика взаимодействия для CheckoutPage.xaml
    /// </summary>
    public partial class CheckoutPage : Page
    {
        public CheckoutPage()
        {
            InitializeComponent();
            CartList.ItemsSource = Core.Cart;
            TotalText.Text = $"Общая сумма: {Core.Cart.Sum(i => i.Product.Price * i.Quantity):C}";
        }

        private void PlaceOrder_Click(object sender, RoutedEventArgs e)
        {
            // Создание заказа
            var order = new Orders
            {
                FullName = FullNameText.Text,
                Email = EmailText.Text,
                Address = AddressText.Text,
                TotalAmount = Core.Cart.Sum(i => i.Product.Price * i.Quantity),
                OrderDate = DateTime.Now
            };

            Core.Context.Orders.Add(order);
            Core.Context.SaveChanges();

            // Добавляем товары заказа
            foreach (var item in Core.Cart)
            {
                Core.Context.OrderItems.Add(new OrderItems
                {
                    OrderId = order.Id,
                    ProductId = item.Product.Id,
                    Quantity = item.Quantity,
                    Price = item.Product.Price
                });
            }

            Core.Context.SaveChanges();

            Core.Cart.Clear();
            MessageBox.Show("Заказ оформлен!");
            NavigationService.Navigate(new ProductsPage());
        }
    }
}
