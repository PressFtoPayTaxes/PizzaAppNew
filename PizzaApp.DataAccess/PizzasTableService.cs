using PizzaApp.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaApp.DataAccess
{
    public class PizzasTableService
    {
        private readonly string _connectionString;

        public PizzasTableService()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
        }

        public List<Pizza> SelectAllPizzasShortInfo()
        {
            List<Pizza> pizzas = new List<Pizza>();

            using (var connection = new SqlConnection(_connectionString))
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.CommandText = "select distinct Name, Filling from Pizzas";

                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string[] fillingInString = reader["FillingList"].ToString().Split(' ');
                    Ingridients[] filling = new Ingridients[fillingInString.Count()];

                    for (int i = 0; i < fillingInString.Count(); i++)
                    {
                        Enum.TryParse(fillingInString[i], out filling[i]);
                    }


                    pizzas.Add(new Pizza
                    {
                        Name = reader["Name"].ToString(),
                        Filling = filling,
                        Cost = 0,
                        Size = 0
                    });
                }
            }
            return pizzas;
        }

        public List<Pizza> SelectPizzaByName(string name)
        {
            List<Pizza> pizza = new List<Pizza>();

            using (var connection = new SqlConnection(_connectionString))
            using (var command = connection.CreateCommand())
            {
                connection.Open();

                command.CommandText = "select * from Pizzas where Name = @name";
                command.Parameters.Add("@name", System.Data.SqlDbType.NVarChar).Value = name;

                var reader = command.ExecuteReader();

                while(reader.Read())
                {
                    pizza.Add(new Pizza
                    {
                        Name = reader["Name"].ToString(),
                        Cost = int.Parse(reader["Cost"].ToString()),
                        Size = int.Parse(reader["Size"].ToString())
                    });
                }

                return pizza;
            }
        }
    }
}
