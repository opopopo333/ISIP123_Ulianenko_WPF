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
    /// Логика взаимодействия для ProductsPage.xaml
    /// </summary>
    public partial class ProductsPage : Page
    {
        public ProductsPage()
        {
            InitializeComponent();
            LoadProducts();
        }

        private void LoadProducts()
        {
            // Используем Core.Context вместо создания нового объекта
            ProductsList.ItemsSource = Core.Context.Products.ToList();
        }

        private void AddToCart_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var product = button.DataContext as Products;

            var existing = Core.Cart.Find(x => x.Product.Id == product.Id);
            if (existing != null)
                existing.Quantity++;
            else
                Core.Cart.Add(new CartItem { Product = product, Quantity = 1 });

            MessageBox.Show($"{product.Name} добавлен в корзину!");
        }
    }
}
