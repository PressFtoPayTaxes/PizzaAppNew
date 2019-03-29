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
            var chars = "0123456789";
            var stringMessage = new char[4];
            var random = new Random();

            for (int i = 0; i < stringMessage.Length; i++)
            {
                stringMessage[i] = chars[random.Next(chars.Length)];
            }

            var finalString = new String(stringMessage);
            TwilioClient.Init(accountSid, authToken);
            var message = MessageResource.Create(
                body: finalString,
                from: new Twilio.Types.PhoneNumber("+13303668494"),
                to: new Twilio.Types.PhoneNumber(phoneNumber)
            );
        }
    }
}
