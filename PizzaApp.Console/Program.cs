using PizzaApp.Models;
using PizzaApp.Services;
using PizzaApp.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaApp.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Registration registration = new SmsRegistration();

            User user = new User
            {
                Name = "Игорь",
                Login = "PressFToPayTaxes",
                Password = "123w3DW456",
                RepeatedPassword = "123w3DW456",
                Address = "пр. Республики, 26-5",
                PhoneNumber = "87015286829",
            };

            registration.GetUserParameters(user.Name, user.Login, user.Password, user.RepeatedPassword, user.PhoneNumber, user.Address);

            registration.CheckUserParameters();

            registration.GetMessage(user.PhoneNumber);
        }
    }
}
