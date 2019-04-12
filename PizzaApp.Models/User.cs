using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaApp.Models
{
    public class User
    {
        public string Name { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Address { get; set; } 
        public string PhoneNumber { get; set; }
        public List<Product> Basket { get; set; }
        public double Money { get; set; }

        public User()
        {
            Basket = new List<Product>();
        }

        public double GetBasketTotal()
        {
            double total = 0;
            foreach(var product in Basket)
                total += product.Cost;
            return total;
        }

        public bool PurchaseProducts()
        {
            if(Money > GetBasketTotal())
            {
                Money -= GetBasketTotal();
                return true;
            }
            return false;
        }
    }
}
