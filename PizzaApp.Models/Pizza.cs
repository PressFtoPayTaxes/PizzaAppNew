using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaApp.Models
{
    public class Pizza : Product
    {
        public Ingridients[] Filling { get; set; }
        public int Size { get; set; }
    }
}
