using PizzaApp.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaApp.Services.Abstract
{
    public abstract class Registration : IRegistration
    {
        private bool passwordChecked = false;
        protected User registratedUser = new User();
        protected int _verificationKey;

        public void GetUserParameters(string name, string login, string password, string repeatedPassword, string phoneNumber, string address)
        {
            registratedUser.Name = name;
            registratedUser.Login = login;
            registratedUser.Password = password;
            registratedUser.RepeatedPassword = repeatedPassword;
            registratedUser.PhoneNumber = phoneNumber;
            registratedUser.Address = address;
        }

        public bool CheckUserParameters()
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
                return true;
            }
            return false;
        }



        public virtual void GetMessage(string phoneNumber)
        {

        }

        public void InsertIntoDatabase()
        {
            using (var connection = new SqlConnection())
            {
                connection.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\СкоропадИг.CORP\source\repos\PizzaApp.Console\PizzaApp.DataAccess\PizzaDatabase.mdf;Integrated Security=True";
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
                }
                catch(SqlException exception)
                {
                    transaction.Rollback();
                }
                catch(Exception exception)
                {
                    transaction.Rollback();
                }
            }

        }
    }
}