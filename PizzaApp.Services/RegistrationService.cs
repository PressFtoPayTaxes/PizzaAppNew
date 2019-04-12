using PizzaApp.DataAccess;
using PizzaApp.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaApp.Services.Abstract
{
    public class RegistrationService
    {
        private User registratedUser = new User();
        public string RepeatedPassword { get; set; }
        private int _verificationKey;

        public IMessageSender Sender { get; set; }
        public static string DifferentPasswordsMessage { get; } = "Entered password are different";
        public static string ThisUserAlreadyExistsMessage { get; } = "User with this login and password already exists";
        public static string EmptyFieldsMessage { get; } = "Some of the fields are empty";
        public static string IncorrectNumberMessage { get; } = "Phone number is incorrect";
        public static string WellDoneMessage { get; } = "All fields are correct";



        public void GetUserParameters(string name, string login, string password, string repeatedPassword, string phoneNumber, string address)
        {
            registratedUser.Name = name;
            registratedUser.Login = login;
            registratedUser.Password = password;
            RepeatedPassword = repeatedPassword;
            registratedUser.PhoneNumber = phoneNumber;
            registratedUser.Address = address;
        }

        public string CheckUserParameters()
        {
            if (RepeatedPassword != registratedUser.Password)
                return DifferentPasswordsMessage;

            UsersTableHandler handler = new UsersTableHandler();
            var users = handler.SelectAllUsers();
            foreach (var user in users)
                if (registratedUser.Password == user.Password && registratedUser.Login == user.Login)
                    return ThisUserAlreadyExistsMessage;

            if (registratedUser.Login == string.Empty || registratedUser.Password == string.Empty || registratedUser.Address == string.Empty || registratedUser.PhoneNumber == string.Empty)
                return EmptyFieldsMessage;

            if (!registratedUser.PhoneNumber.Contains('+'))
                return IncorrectNumberMessage;

            return WellDoneMessage;
        }

        public void SendMessage()
        {
            _verificationKey = Sender.SendMessage(registratedUser.PhoneNumber);
            if(_verificationKey == -1)
            {
                throw new Twilio.Exceptions.ApiException("message is not delivered");
            }
        }

        public bool Registrate(int verificationKey)
        {
            if (_verificationKey == verificationKey)
            {
                if (InsertIntoDatabase())
                    return true;
                return false;
            }
            return false;
        }

        public bool InsertIntoDatabase()
        {
            using (var connection = new SqlConnection())
            {
                connection.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
                var transaction = connection.BeginTransaction();
                try
                {
                    var command = new SqlCommand("insert into Users values(@name, @login, @password, @address, @phoneNumber)");

                    #region CreateParameters
                    command.Parameters.Add(new SqlParameter
                    {
                        ParameterName = "@name",
                        SqlDbType = System.Data.SqlDbType.NVarChar,
                        SqlValue = registratedUser.Name
                    });

                    command.Parameters.Add(new SqlParameter
                    {
                        ParameterName = "@login",
                        SqlDbType = System.Data.SqlDbType.NVarChar,
                        SqlValue = registratedUser.Login
                    });

                    command.Parameters.Add(new SqlParameter
                    {
                        ParameterName = "@password",
                        SqlDbType = System.Data.SqlDbType.NVarChar,
                        SqlValue = registratedUser.Password
                    });

                    command.Parameters.Add(new SqlParameter
                    {
                        ParameterName = "@address",
                        SqlDbType = System.Data.SqlDbType.NVarChar,
                        SqlValue = registratedUser.Address
                    });

                    command.Parameters.Add(new SqlParameter
                    {
                        ParameterName = "@phoneNumber",
                        SqlDbType = System.Data.SqlDbType.NVarChar,
                        SqlValue = registratedUser.PhoneNumber
                    });
                    #endregion

                    int affectedRows = command.ExecuteNonQuery();

                    if (affectedRows < 1)
                    {
                        throw new Exception();
                    }

                    transaction.Commit();
                    return true;
                }
                catch (SqlException exception)
                {
                    transaction.Rollback();
                    return false;
                }
                catch (Exception exception)
                {
                    transaction.Rollback();
                    return false;
                }
            }

        }
    }
}