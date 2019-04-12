using PizzaApp.Models;
using PizzaApp.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaApp.DataAccess
{
    public class UsersTableHandler
    {
        private readonly string _connectionString;

        public UsersTableHandler()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            //_connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C: \Users\Admin\Documents\Visual Studio 2015\Projects\PizzaApp\PizzaApp.DataAccess\PizzaDatabase.mdf;Integrated Security=True";
        }

        public void InsertUser(User obj)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = connection.CreateCommand())
            {
                try
                {
                    connection.Open();
                    command.CommandText = $"insert into Users(Name, Login, Password, Address, PhoneNumber) values(@name, @login, @password, @address, @phoneNumber)";

                    var nameParameter = command.CreateParameter();
                    nameParameter.ParameterName = "@name";
                    nameParameter.SqlDbType = System.Data.SqlDbType.NVarChar;
                    nameParameter.SqlValue = obj.Name;

                    var loginParameter = command.CreateParameter();
                    nameParameter.ParameterName = "@login";
                    nameParameter.SqlDbType = System.Data.SqlDbType.NVarChar;
                    nameParameter.SqlValue = obj.Login;

                    var passwordParameter = command.CreateParameter();
                    nameParameter.ParameterName = "@password";
                    nameParameter.SqlDbType = System.Data.SqlDbType.NVarChar;
                    nameParameter.SqlValue = obj.Password;

                    var addressParameter = command.CreateParameter();
                    nameParameter.ParameterName = "@address";
                    nameParameter.SqlDbType = System.Data.SqlDbType.NVarChar;
                    nameParameter.SqlValue = obj.Address;

                    var phoneNumberParameter = command.CreateParameter();
                    nameParameter.ParameterName = "@phoneNumber";
                    nameParameter.SqlDbType = System.Data.SqlDbType.NVarChar;
                    nameParameter.SqlValue = obj.PhoneNumber;

                    command.Parameters.Add(nameParameter);
                    command.Parameters.Add(loginParameter);
                    command.Parameters.Add(passwordParameter);
                    command.Parameters.Add(addressParameter);
                    command.Parameters.Add(phoneNumberParameter);

                    int affectedRows = command.ExecuteNonQuery();

                    if (affectedRows < 1)
                    {
                        throw new Exception("Вставка не удалась");
                    }
                }
                catch (SqlException exception)
                {
                    //обработать
                    throw;
                }
                catch (Exception exception)
                {
                    //обработать
                    throw;
                }
            }
        }

        public User SelectUserByLoginAndPassword(string login, string password)
        {
            User user = new User();

            using (var connection = new SqlConnection(_connectionString))
            using (var command = connection.CreateCommand())
            {
                try
                {
                    connection.Open();
                    command.CommandText = "select * from Users where Login = @login and Password = @password";

                    //var loginParameter = command.CreateParameter();
                    //loginParameter.ParameterName = "@login";
                    //loginParameter.SqlDbType = System.Data.SqlDbType.NVarChar;
                    //loginParameter.SqlValue = login;

                    //var passwordParameter = command.CreateParameter();
                    //loginParameter.ParameterName = "@password";
                    //loginParameter.SqlDbType = System.Data.SqlDbType.NVarChar;
                    //loginParameter.SqlValue = password;

                    command.Parameters.Add("@login", System.Data.SqlDbType.NVarChar).Value = login;
                    command.Parameters.Add("@password", System.Data.SqlDbType.NVarChar).Value = password;

                    var sqlDataReader = command.ExecuteReader();

                    if (!sqlDataReader.HasRows)
                        return user;

                    while (sqlDataReader.Read())
                    {
                        user.Name = sqlDataReader["Name"].ToString();
                        user.Login = sqlDataReader["Login"].ToString();
                        user.Password = sqlDataReader["Password"].ToString();
                        user.Address = sqlDataReader["Address"].ToString();
                        user.PhoneNumber = sqlDataReader["PhoneNumber"].ToString();
                        user.Money = double.Parse(sqlDataReader["Money"].ToString());
                    }
                    sqlDataReader.Close();
                }
                catch (SqlException exception)
                {
                    //обработать
                    throw;
                }
                catch (Exception exception)
                {
                    //обработать
                    throw;
                }

                return user;
            }
        }

        public List<User> SelectAllUsers()
        {
            List<User> users = new List<User>();

            using (var connection = new SqlConnection(_connectionString))
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandText = "select * from Users";

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    users.Add(new User
                    {
                        Name = reader["Name"].ToString(),
                        Login = reader["Login"].ToString(),
                        Password = reader["Password"].ToString(),
                        Address = reader["Address"].ToString(),
                        PhoneNumber = reader["PhoneNumber"].ToString(),
                        Money = double.Parse(reader["Money"].ToString())
                    });
                }
                reader.Close();
            }


            return users;
        }

        public void UpdateMoney(User user, double sum)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandText = "update Users set Money = @sum where Login = @login and Password = @password";

                command.Parameters.Add("@sum", System.Data.SqlDbType.Money).Value = sum;
                command.Parameters.Add("@login", System.Data.SqlDbType.NVarChar).Value = user.Login;
                command.Parameters.Add("@password", System.Data.SqlDbType.NVarChar).Value = user.Password;

                command.ExecuteNonQuery();
            }
        }
    }
}
