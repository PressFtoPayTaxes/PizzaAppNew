using PizzaApp.Models;
using PizzaApp.Services.Abstract;
using System;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace PizzaApp.Services
{
    public class SmsRegistration : Registration
    {
        public override void GetMessage(string phoneNumber)
        {
            const string accountSid = "AC6d85d3c48e34dab66c0b3d5bbfbedaa2";
            const string authToken = "a013a3232c874252d30faaca24c4ef70";

            TwilioClient.Init(accountSid, authToken);

            var randomNumber = new Random();
            var verificationKey = randomNumber.Next(1000, 9999);

            var message = MessageResource.Create(
                from: new Twilio.Types.PhoneNumber("+13303668494"),
                body: verificationKey.ToString(),
                to: new Twilio.Types.PhoneNumber("+77015286829")
            );

            _verificationKey = verificationKey;

            Console.WriteLine(message.Sid);
            Console.WriteLine("Сообщение отправлено");
            
        }
    }
}
