using PizzaApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaApp.Services.Abstract
{
    public abstract class Registration : IRegistration
    {
        private bool passwordChecked = false;
        User registratedUser = new User();

        public void GetUserParameters(string name, string login, string password, string repeatedPassword, string phoneNumber, string address)
        {
            registratedUser.Name = name;
            registratedUser.Login = login;
            registratedUser.Password = password;
            registratedUser.PhoneNumber = phoneNumber;
            registratedUser.Address = address;
        }

        public void CheckUserParameters()
        {
            foreach (char symbol in registratedUser.Password)
            {
                if (Char.IsNumber(symbol))
                {
                    passwordChecked = true;
                }
                else if (Char.IsUpper(symbol))
                {
                    passwordChecked = true;
                }
            }
            if (registratedUser.Name != string.Empty && registratedUser.Login != string.Empty && passwordChecked == true && registratedUser.Password.Equals(registratedUser.RepeatedPassword))
            {
                GetMessage(registratedUser.PhoneNumber);
                throw new Exception("Successful registration");
            }
            else
            {
                throw new ArgumentException("error filling registration fields!");
            }
        }

        public virtual void GetMessage(string phoneNumber)
        {

        }
    }
}