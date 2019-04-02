using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaApp.Services.Abstract
{
    public interface IRegistration
    {
        void GetUserParameters(string name, string login, string password, string repeatedPassword, string phoneNumber, string address);

        bool CheckUserParameters();

        void InsertIntoDatabase();

        void GetMessage(string phoneNumber);
    }
}
