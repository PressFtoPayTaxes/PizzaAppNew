using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaApp.Models
{
    public abstract class Product
    {
        public string Name { get; set; }
        public int Cost { get; set; }
    }
}
