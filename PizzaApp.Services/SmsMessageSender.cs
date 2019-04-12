using PizzaApp.Models;
using PizzaApp.Services.Abstract;
using System;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace PizzaApp.Services
{
    public class SmsMessageSender : IMessageSender
    {
        public int SendMessage(string phoneNumber)
        {
            const string accountSid = "ACdcc801525e5a94284004100706d3f503";
            const string authToken = "4429b063acb6af1f7907a8a9ffa15052";

            TwilioClient.Init(accountSid, authToken);

            var randomNumber = new Random();
            var verificationKey = randomNumber.Next(1000, 9999);

            try
            {
                var message = MessageResource.Create(
                    from: new Twilio.Types.PhoneNumber("+16504926746"),
                    body: verificationKey.ToString(),
                    to: new Twilio.Types.PhoneNumber(phoneNumber)
                );
            }
            catch(Exception)
            {
                return -1;
            }
            return verificationKey;
        }
    }
}
