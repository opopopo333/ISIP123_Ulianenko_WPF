using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PR13
{
    public class Core
    {
        public static PR13Entities Context = new PR13Entities();
        public static List<CartItem> Cart = new List<CartItem>(); // корзина
    }

    public class CartItem
    {
        public Products Product { get; set; }  // сам товар
        public int Quantity { get; set; }     // количество

        // Свойство для отображения общей суммы позиции
        public decimal Total => Product.Price * Quantity;
    }
}
