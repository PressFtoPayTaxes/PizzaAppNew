using PizzaApp.DataAccess;
using PizzaApp.Models;
using PizzaApp.Services;
using PizzaApp.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaApp.Console
{
    class Program
    {
        static bool Register()
        {
            User user = new User();
            RegistrationService service = new RegistrationService();

            System.Console.Write("Enter your name: ");
            user.Name = System.Console.ReadLine();

            System.Console.Write("Enter your login: ");
            user.Login = System.Console.ReadLine();

            System.Console.Write("Enter your password: ");
            user.Password = System.Console.ReadLine();

            System.Console.Write("Repeat your password: ");
            string repeatedPassword = System.Console.ReadLine();

            System.Console.Write("Enter your phone number: ");
            user.PhoneNumber = System.Console.ReadLine();

            System.Console.Write("Enter your address: ");
            user.Address = System.Console.ReadLine();

            service.GetUserParameters(user.Name, user.Login, user.Password, repeatedPassword, user.PhoneNumber, user.Address);

            string message = service.CheckUserParameters();
            System.Console.WriteLine(message);
            if (message == RegistrationService.WellDoneMessage)
            {
                while (true)
                {
                    System.Console.WriteLine("How will you get verification code?\n1 - SMS");
                    int answer = int.Parse(System.Console.ReadLine());
                    switch (answer)
                    {
                        case 1: service.Sender = new SmsMessageSender(); break;
                        default:
                            System.Console.WriteLine("Incorrect answer");
                            continue;
                    }
                    break;
                }

                service.SendMessage();
                System.Console.Write("Enter your verification code: ");
                int verificationCode = int.Parse(System.Console.ReadLine());

                if(service.Registrate(verificationCode))
                {
                    System.Console.WriteLine("Registration is succesfully finished");
                    return true;
                }
                else
                {
                    System.Console.WriteLine("Something went wrong");
                    return false;
                }
            }
            return false;
        }

        static User LogIn(UsersTableHandler handler)
        {
            string login;
            string password;

            while (true)
            {
                System.Console.Write("Enter your login: ");
                login = System.Console.ReadLine();
                if (login == string.Empty)
                {
                    System.Console.WriteLine("Login can't be empty");
                    continue;
                }
                break;
            }

            while (true)
            {
                System.Console.Write("Enter your password: ");
                password = System.Console.ReadLine();
                if (password == string.Empty)
                {
                    System.Console.WriteLine("Pasword can't be empty");
                    continue;
                }
                break;
            }


            User user = handler.SelectUserByLoginAndPassword(login, password);

            return user;
        }

        static void SelectPizza(User user)
        {
            PizzasTableService pizzaService = new PizzasTableService();
            List<Pizza> pizzas = pizzaService.SelectAllPizzasShortInfo();

            System.Console.WriteLine("Select pizza");
            for (int i = 0; i < pizzas.Count; i++)
            {
                System.Console.Write($"{i + 1} - {pizzas[i].Name}: ");
                foreach (var ingridient in pizzas[i].Filling)
                    System.Console.Write(ingridient + ", ");
                System.Console.WriteLine("\b\b");
            }

            int answer = int.Parse(System.Console.ReadLine());

            Pizza selectedPizza = new Pizza();
            if (answer <= pizzas.Count)
                selectedPizza = pizzas[answer - 1];
            else
            {
                System.Console.WriteLine("There's no such pizza");
                return;
            }

            System.Console.WriteLine($"You selected {selectedPizza.Name}, choose size: ");
            List<Pizza> selectedPizzaSizes = pizzaService.SelectPizzaByName(selectedPizza.Name);
            for (int i = 0; i < selectedPizzaSizes.Count(); i++)
                System.Console.WriteLine($"{i + 1}: {selectedPizzaSizes[i].Size} - {selectedPizzaSizes[i].Cost}");
            answer = int.Parse(System.Console.ReadLine());

            if(answer <= selectedPizzaSizes.Count())
            {
                user.Basket.Add(selectedPizzaSizes[answer - 1]);
                System.Console.WriteLine("Pizza was successfully added to your basket");
            }
            else
            {
                System.Console.WriteLine("There's no such size");
            }

        }

        static void Main(string[] args)
        {
            UsersTableHandler handler = new UsersTableHandler();
            User currentUser = new User();

            System.Console.WriteLine("Welcome to our app! To use it you must be logged in!");

            while (true)
            {
                System.Console.WriteLine("\n----------------------------------\n");
                System.Console.WriteLine("1 - Register\n2 - Log In\n3 - Exit");
                int answer = int.Parse(System.Console.ReadLine());

                switch (answer)
                {
                    case 1: if (!Register()) continue; break;
                    case 2:
                        currentUser = LogIn(handler);
                        if (currentUser.Login == string.Empty || currentUser.Login == null)
                        {
                            System.Console.WriteLine("User with this data not found");
                            continue;
                        }
                        else
                        {
                            System.Console.WriteLine("You logged in");
                        }
                        break;
                    case 3: Environment.Exit(1); break;
                    default: System.Console.WriteLine("Incorrect answer"); continue;
                }
                break;
            }


            while (true)
            {
                System.Console.WriteLine("\n---------------------------------------\n");
                System.Console.WriteLine("What do we do?\n1 - Show menu\n2 - Make a purchase\n3 - Replenish a wallet\n4 - Exit");

                int answer = int.Parse(System.Console.ReadLine());

                switch (answer)
                {
                    case 1:
                        SelectPizza(currentUser);
                        break;
                    case 2:
                        if (currentUser.PurchaseProducts())
                        {
                            System.Console.WriteLine("You successfully purchased products! Wait a moment... We'll deliver them to " + currentUser.Address);
                            currentUser.Basket.Clear();
                        }
                        else
                            System.Console.WriteLine("You don't have enough money:( Try to replenish your wallet");
                        break;
                    case 3:
                        System.Console.Write("Enter the sum: ");
                        double sum = double.Parse(System.Console.ReadLine());
                        if (sum > 0 && sum < 99999)
                        {
                            currentUser.Money += sum;
                            handler.UpdateMoney(currentUser, sum);
                            System.Console.WriteLine("Money were successfully added");
                        }
                        else
                        {
                            System.Console.WriteLine("You entered incorrect sum");
                        }
                        break;
                    case 4: Environment.Exit(1); break;
                    default:
                        System.Console.WriteLine("Incorrect answer"); break;
                }
            }
        }
    }
}
