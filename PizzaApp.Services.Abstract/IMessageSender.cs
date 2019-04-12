using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaApp.Services.Abstract
{
    public interface IMessageSender
    {
        int SendMessage(string phoneNumber);
    }
}
