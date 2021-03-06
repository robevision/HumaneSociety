﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumaneSociety
{
    public static class UserInterface
    {
        public static void DisplayUserOptions(List<string> options)
        {
            foreach(var option in options)
            {
                Console.WriteLine(option);
            }
        }
        public static void DisplayUserOptions(string options)
        {
            Console.WriteLine(options);
        }
        public static string GetUserInput()
        {
            
            string input = Console.ReadLine();
            switch (input.ToLower())
            {
                case "reset":
                    Console.Clear();
                    PointOfEntry.Run();
                    Environment.Exit(1);
                    break;
                case "exit":
                    Environment.Exit(1);
                    break;
                default:
                    break;
            }

            return input;
        }
        public static string GetStringData(string parameter, string target)
        {
            string data;
            DisplayUserOptions($"What is {target} {parameter}?");
            data = GetUserInput();
            return data;
        }

        internal static bool? GetBitData(List<string> options)
        {
            DisplayUserOptions(options);
            string input = GetUserInput();
            if (input.ToLower() == "yes" || input.ToLower() == "y")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool? GetBitData()
        {
            string input = GetUserInput();
            if (input.ToLower() == "yes" || input.ToLower() == "y")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        internal static bool? GetBitData(string target, string parameter)
        {
            DisplayUserOptions($"Is {target} {parameter}?");
            string input = GetUserInput();
            if (input.ToLower() == "yes" || input.ToLower() == "y")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        internal static void DisplayAnimals(List<Animal> animals)
        {
            foreach(Animal animal in animals)
            {
                try
                {
                    Console.WriteLine(animal.AnimalId + " " + animal.Name + " " + animal.Category.Name);
                }
                catch (NullReferenceException)
                {
                    Console.WriteLine($"\n\n\n********{animal.AnimalId} {animal.Name} doesn't have all needed values to show! \n********Removed from list. \n********Please update Null values!");
                    Console.ReadKey();
                }
            }
        }

        internal static int GetIntegerData()
        {
            try
            {
                int data = int.Parse(GetUserInput());
                return data;
            }
            catch
            {
                Console.Clear();
                DisplayUserOptions("Incorrect input please enter an integer number.");
                return GetIntegerData();
            }
        }

        public static int GetIntegerData(string parameter, string target)
        {
            try
            {
                int data = int.Parse(GetStringData(parameter, target));
                return data;
            }
            catch
            {
                Console.Clear();
                DisplayUserOptions("Incorrect input please enter an integer number.");
                return GetIntegerData(parameter, target);
            }
        }

        internal static void DisplayClientInfo(Client client)
        {
            List<string> info = new List<string>() { client.FirstName, client.LastName, client.Email, client.Address.USState.Name };
            DisplayUserOptions(info);
            Console.ReadLine();
        }

        internal static void DisplayEmployeeInfo(Employee employee)
        {
            List<string> info = new List<string>() { "First Name is: " + employee.FirstName, "Last Name is: "+ employee.LastName, "UserName is: " + employee.UserName, "Password is: " + employee.Password, "Employee Number is: " + employee.EmployeeNumber.ToString(), "Email is: " + employee.Email };
            DisplayUserOptions(info);
            Console.ReadLine();
        }
        public static void DisplayAnimalInfo(Animal animal)
        {
            Room animalRoom = Query.GetRoom(animal.AnimalId);
            List<string> info = new List<string>() {"ID: " + animal.AnimalId, animal.Name, animal.Age + "years old", "Demeanour: " + animal.Demeanor, "Kid friendly: " + BoolToYesNo(animal.KidFriendly), "pet friendly: " + BoolToYesNo(animal.PetFriendly), $"Location: Room " + animalRoom.RoomNumber, "Weight: " + animal.Weight.ToString(),  "Food amoumnt in cups:" + animal.DietPlan.FoodAmountInCups, "Adoption Status: " +  animal.AdoptionStatus};
            DisplayUserOptions(info);
            Console.ReadLine();

        }

      

        private static string BoolToYesNo(bool? input)
        {
            if (input == true)
            {
                return "yes";
            }
            else
            {
                return "no";
            }
        }

        public static bool GetBitData(string option)
        {
            DisplayUserOptions(option);
            string input = GetUserInput();
            if (input.ToLower() == "yes" || input.ToLower() == "y")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
 
        public static Dictionary<int, string> EnterSearchCriteria(Dictionary<int, string> searchParameters, string input, Animal animal)
        {
            Console.Clear();
            switch (input)
            {
                case "1":
                    searchParameters.Add(1, UserInterface.GetStringData("name", "the animal's"));
                    return searchParameters;
                case "2":
                    searchParameters.Add(2, UserInterface.GetIntegerData("weight", "the animal's").ToString());
                    return searchParameters;
                case "3":
                    searchParameters.Add(3, UserInterface.GetIntegerData("age", "the animal's").ToString());
                    return searchParameters;
                case "4":
                    searchParameters.Add(4, UserInterface.GetStringData("demeanor", "the animal's"));
                    return searchParameters;
                case "5":
                    searchParameters.Add(5, UserInterface.GetBitData("the animal", "kid friendly").ToString());
                    return searchParameters;
                case "6":
                    searchParameters.Add(6, UserInterface.GetBitData("the animal", "pet friendly").ToString());
                    return searchParameters;
                case "7":
                    searchParameters.Add(7, UserInterface.GetStringData("gender", "the animal's"));
                    return searchParameters;
                case "8":
                    searchParameters.Add(8, UserInterface.GetStringData("Adoption Status", "the animal's"));
                    return searchParameters;
                case "9":
                    searchParameters.Add(9, "room");
                    return searchParameters;
                default:
                    UserInterface.DisplayUserOptions("Input not recognized please try again");
                    return searchParameters;
            }
        }

        public static string GetUserInputWithOutput(string words)
        {
            Console.WriteLine(words);
            string input = Console.ReadLine();
            switch (input.ToLower())
             
            {
                case "reset":
                    PointOfEntry.Run();
                    Environment.Exit(1);
                    break;
                case "exit":
                    Environment.Exit(1);
                    break;
                default:
                    break;
            }
            string output = CapitalizeFirstLetter(input);
            return input;
        }

        internal static string CapitalizeFirstLetter(string input)
        {
            string output = input;
           
            return output.First().ToString().ToUpper() + output.Substring(1);

        }
    }
}
