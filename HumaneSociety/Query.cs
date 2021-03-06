﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumaneSociety
{
    public static class Query
    {
        public delegate int StringToIntFunction(string s);
        internal static List<USState> GetStates()
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();

            List<USState> allStates = db.USStates.ToList();

            return allStates;
        }

        internal static Client GetClient(string userName, string password)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();

            Client client = db.Clients.Where(c => c.UserName == userName && c.Password == password).Single();

            return client;
        }

        internal static List<Client> GetClients()
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();

            List<Client> allClients = db.Clients.ToList();

            return allClients;
        }

        internal static void AddNewClient(string firstName, string lastName, string username, string password, string email, string streetAddress, int zipCode, int stateId)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();

            Client newClient = new Client();

            newClient.FirstName = firstName;
            newClient.LastName = lastName;
            newClient.UserName = username;
            newClient.Password = password;
            newClient.Email = email;

            Address addressFromDb = db.Addresses.Where(a => a.AddressLine1 == streetAddress && a.Zipcode == zipCode && a.USStateId == stateId).FirstOrDefault();

            // if the address isn't found in the Db, create and insert it
            if (addressFromDb == null)
            {
                Address newAddress = new Address();
                newAddress.AddressLine1 = streetAddress;
                newAddress.AddressLine2 = null;
                newAddress.Zipcode = zipCode;
                newAddress.USStateId = stateId;

                db.Addresses.InsertOnSubmit(newAddress);
                db.SubmitChanges();

                addressFromDb = newAddress;
            }

            // attach AddressId to clientFromDb.AddressId
            newClient.AddressId = addressFromDb.AddressId;

            db.Clients.InsertOnSubmit(newClient);

            db.SubmitChanges();
        }

        internal static void UpdateClient(Client clientWithUpdates)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();

            // find corresponding Client from Db
            Client clientFromDb = db.Clients.Where(c => c.ClientId == clientWithUpdates.ClientId).Single();

            // update clientFromDb information with the values on clientWithUpdates (aside from address)
            clientFromDb.FirstName = clientWithUpdates.FirstName;
            clientFromDb.LastName = clientWithUpdates.LastName;
            clientFromDb.UserName = clientWithUpdates.UserName;
            clientFromDb.Password = clientWithUpdates.Password;
            clientFromDb.Email = clientWithUpdates.Email;

            // get address object from clientWithUpdates
            Address clientAddress = clientWithUpdates.Address;

            // look for existing Address in Db (null will be returned if the address isn't already in the Db
            Address updatedAddress = db.Addresses.Where(a => a.AddressLine1 == clientAddress.AddressLine1 && a.USStateId == clientAddress.USStateId && a.Zipcode == clientAddress.Zipcode).FirstOrDefault();

            // if the address isn't found in the Db, create and insert it
            if(updatedAddress == null)
            {
                Address newAddress = new Address();
                newAddress.AddressLine1 = clientAddress.AddressLine1;
                newAddress.AddressLine2 = null;
                newAddress.Zipcode = clientAddress.Zipcode;
                newAddress.USStateId = clientAddress.USStateId;

                db.Addresses.InsertOnSubmit(newAddress);
                db.SubmitChanges();

                updatedAddress = newAddress;
            }

            // attach AddressId to clientFromDb.AddressId
            clientFromDb.AddressId = updatedAddress.AddressId;
            
            // submit changes
            db.SubmitChanges();
        }
        internal static void Adopt(Animal animal, Client client)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();
            Adoption adoption = new Adoption();
            adoption.Client = client;
            adoption.ApprovalStatus = "Pending";
            animal.AdoptionStatus = "Pending";
            if (UserInterface.GetBitData("The adoption fee is $75 and by agreeing to this adoption you are commiting to this payment. Should this adoption be processed?"))
            {
                adoption.AdoptionFee = 75;
                db.SubmitChanges();
            }


        }
        internal static List<Adoption> GetPendingAdoptions()
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();
            List<Adoption> adoptions = new List<Adoption>();
            adoptions = db.Adoptions.Where(a => a.ApprovalStatus.Contains("pending")).ToList();
            return adoptions;
        }
        internal static Employee RetrieveEmployeeUser(string email, int employeeNumber)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();

            Employee employeeFromDb = db.Employees.Where(e => e.Email == email && e.EmployeeNumber == employeeNumber).FirstOrDefault();

            if(employeeFromDb == null)
            {
                throw new NullReferenceException();            
            }
            else
            {
                return employeeFromDb;
            }            
        }
        internal static void UpdateAdoption(bool approval, Adoption adoption)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();
            Adoption adoptionFromDB = db.Adoptions.Where(c => c.AdoptionId == adoption.AdoptionId).Single();
            if (approval == true)
            {
                adoptionFromDB.PaymentCollected = true;
                adoptionFromDB.ApprovalStatus = "Approved";
                db.SubmitChanges();
            }
            else
            {
                adoptionFromDB.ApprovalStatus = "Denied";
                db.SubmitChanges();
            }
        }

        internal static Employee EmployeeLogin(string userName, string password)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();

            Employee employeeFromDb = db.Employees.Where(e => e.UserName == userName && e.Password == password).FirstOrDefault();

            return employeeFromDb;
        }

        internal static bool CheckEmployeeUserNameExist(string userName)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();

            Employee employeeWithUserName = db.Employees.Where(e => e.UserName == userName).FirstOrDefault();
            bool conditional = true;
            if (employeeWithUserName == null)
            {
                conditional = false;
            }
            return conditional;
        }
        internal static void CreateEmployee(Employee employee)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();
            db.Employees.InsertOnSubmit(employee);
            db.SubmitChanges();
        }
        internal static void GetEmployeeByEmployeeNumber(Employee employee)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();
            Employee employeeFromDb = db.Employees.Where(e => e.EmployeeNumber == employee.EmployeeNumber).Single();
            UserInterface.DisplayEmployeeInfo(employeeFromDb);

        }
        internal static void UpdateEmployeeFromDB(Employee employee)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();
            Employee employeeFromDB = db.Employees.Where(e => e.EmployeeId == employee.EmployeeId).Single();
            employeeFromDB.FirstName = employee.FirstName;
            employeeFromDB.LastName = employee.LastName;
            employeeFromDB.EmployeeNumber = employee.EmployeeNumber;
            employeeFromDB.Email = employee.Email;
            db.SubmitChanges();
        }
        internal static void DeleteEmployee(Employee employee)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();
            Employee employeefromDB = db.Employees.Where(e => e.EmployeeNumber == employee.EmployeeNumber && e.FirstName == employee.FirstName).Single();
            List<Animal> animalsAffected = db.Animals.Where(a => a.EmployeeId == employee.EmployeeId).ToList();
            foreach(Animal animal in animalsAffected)
            {
                animal.EmployeeId = null;
            }
            db.Employees.DeleteOnSubmit(employeefromDB);
            db.SubmitChanges();
        }
        internal static void RunEmployeeQueries(Employee employee, string words)
        {
            
            switch (words)
            {
                case "create":
                    CreateEmployee(employee);                             
                    break;
                    
                case "read":
                    GetEmployeeByEmployeeNumber(employee);
                    break;

                case "update":
                    UpdateEmployeeFromDB(employee);
                    break;

                case "delete":
                    DeleteEmployee(employee);
                    break;

            }

        }

        internal static void AddUsernameAndPassword(Employee employee)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();

            Employee employeeFromDb = db.Employees.Where(e => e.EmployeeId == employee.EmployeeId).FirstOrDefault();

            employeeFromDb.UserName = employee.UserName;
            employeeFromDb.Password = employee.Password;

            db.SubmitChanges();
        }
        internal static int? GetCategoryId()
        {

            HumaneSocietyDataContext db = new HumaneSocietyDataContext();
            string input = UserInterface.GetUserInputWithOutput("What type of animal are you cataloging?");
            int ?category = db.Categories.Where(c => c.Name == input).Select(c => c.CategoryId).Single();
            if (category == null)
            {
                Console.WriteLine("This species does not exist.");
                int newCategory = AddCategoryName();
                return newCategory;
            }
            return category;
        }
        internal static int AddCategoryName()
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();
            Category newCategory = new Category();
            
            string input = UserInterface.GetUserInputWithOutput("What would you like to name this new species?");
            newCategory.Name = input;
            db.Categories.InsertOnSubmit(newCategory);
            db.SubmitChanges();
            int category = db.Categories.Where(c => c.Name == input).Select(c => c.CategoryId).Single();
            
            return category;
        }
        internal static Animal GetAnimalByID(int AnimalId, string name)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();

            Animal animal = db.Animals.Where(a => a.AnimalId == AnimalId && a.Name == name).Single();

            return animal;
        }
        public static int GetDietPlanId(string name)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();

            int dietId = db.DietPlans.Where(d => d.Name == name).Select(d => d.DietPlanId).Single();
            return dietId;
        }
        public static void UpdateDietPlan(int dietPlanId)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();
            DietPlan oldDP = db.DietPlans.Where(d => d.DietPlanId == dietPlanId).Single();
            Console.WriteLine("What would you like to change? \n Diet Name = 1 \n Food Type = 2 \n Amount of food in cups = 3 \n All = 4");
            int decision = UserInterface.GetIntegerData();
            switch (decision)
            {
                case 1:
                    oldDP.Name = UserInterface.GetUserInputWithOutput($"\nCurrent Diet Plan Name is {oldDP.Name}. What would you like to rename it to?");
                    break;
                case 2:
                    oldDP.FoodType = UserInterface.GetUserInputWithOutput($"\nCurrent Diet Plan Food Type is {oldDP.FoodType}. What would you like to change it to?");
                    break;
                case 3:
                    Console.WriteLine($"\nCurrent Diet Plan Cups of Food Amount is {oldDP.FoodAmountInCups} What would you like to change it to?");
                    oldDP.FoodAmountInCups = UserInterface.GetIntegerData();
                    break;
                case 4:
                    oldDP.Name = UserInterface.GetUserInputWithOutput($"Current Diet Plan Name is {oldDP.Name}. What would you like to rename it to?");
                    oldDP.FoodType = UserInterface.GetUserInputWithOutput($"\nCurrent Diet Plan Food Type is {oldDP.FoodType}. What would you like to change it to?");
                    Console.WriteLine($"\nCurrent Diet Plan Cups of Food Amount is {oldDP.FoodAmountInCups} What would you like to change it to?");
                    oldDP.FoodAmountInCups = UserInterface.GetIntegerData();
                    break;
            }
            db.SubmitChanges();

        }
        public static void CreateNewDietPlan()
        {

            HumaneSocietyDataContext db = new HumaneSocietyDataContext();
            DietPlan newDP = new DietPlan();
            newDP.Name = UserInterface.GetUserInputWithOutput("What do you want to name this diet? E.X. 'Dog Diet'");
            newDP.FoodType = UserInterface.GetUserInputWithOutput("What type of food is this diet? E.X. 'Dog Food'");
            newDP.FoodAmountInCups = Convert.ToInt16(UserInterface.GetUserInputWithOutput("How much food, in cups, would you like to add as the standard? E.X. '5'"));
            db.DietPlans.InsertOnSubmit(newDP);
            db.SubmitChanges();
            
        }
        internal static List<Animal> SearchForAnimalByMultipleTraits()
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();
            string traitToSearch = null;
            List<Animal> allAnimals = db.Animals.ToList();
           
            
            do
            {
                Console.WriteLine("Type a(nother) search criteria \n Type 'finished' when done. \n\n Search criteria are : name, weight, age, demeanor, kid friendly, pet friendly, gender, and adoption status");
                traitToSearch = Console.ReadLine().ToLower();
                switch (traitToSearch)
                {
                    case "name":
                        Console.WriteLine("What name are you searching for?");
                        string Name = Console.ReadLine();
                        allAnimals = allAnimals.Where(a => a.Name.ToLower() == Name.ToLower()).ToList();
                        break;
                    case "weight":
                        Console.WriteLine("What weight are you searching for?");
                        int weight = Convert.ToInt16(Console.ReadLine());
                        allAnimals = allAnimals.Where(a => a.Weight == weight).ToList();

                        break;
                    case "age":
                        Console.WriteLine("What age are you searching for?");
                        int? age = Convert.ToInt16(Console.ReadLine());
                        allAnimals = allAnimals.Where(a => a.Age == age).ToList();

                        break;
                    case "demeanor":
                        Console.WriteLine("What demeanor are you looking for?");
                        string demeanor = Console.ReadLine();
                        allAnimals = allAnimals.Where(a => a.Demeanor.ToLower() == demeanor.ToLower()).ToList();


                        break;
                    case "kid friendly":
                        allAnimals = allAnimals.Where(a => a.PetFriendly == true).ToList();

                        break;
                    case "pet friendly":
                        allAnimals = allAnimals.Where(a => a.PetFriendly == true).ToList();

                        break;
                    case "gender":
                        Console.WriteLine("What gender are you searching for?");
                        string gender = Console.ReadLine();
                        allAnimals = allAnimals.Where(a => a.Gender.ToLower() == gender.ToLower()).ToList();

                        break;
                    case "adoption status":
                        Console.WriteLine("What adoption status are you searching for?");
                        string adoptionStatus = Console.ReadLine();
                        allAnimals = allAnimals.Where(a => a.AdoptionStatus.ToLower() == adoptionStatus.ToLower()).ToList();

                        break;
                }
            }
            while (traitToSearch != "finished" && traitToSearch != "done");
            
            
            return allAnimals;
        }
        internal static void AddAnimal(Animal animal)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();
            Employee emp = new Employee();
            emp.EmployeeNumber = Convert.ToInt16(UserInterface.GetUserInputWithOutput("What is your employee number?"));
            animal.EmployeeId = db.Employees.Where(e => e.EmployeeNumber == emp.EmployeeNumber).Select(e => e.EmployeeId).Single();
         
            db.Animals.InsertOnSubmit(animal);
            db.SubmitChanges();
            UpdateRoom(animal);

            
        }
        internal static void EnterAnimalUpdate(Animal animal, Dictionary<int, string> updates)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();
            //grab Dictionary key call from updates and add it to Animal parameter 
            if (updates.ContainsKey(1))
            {
                var name = updates.Where(u => u.Key == 1).Select(u => u.Value).Single();
                animal.Name = name;
            }
            if (updates.ContainsKey(2))
            {
                var weight = Convert.ToInt16(updates.Where(u => u.Key == 2).Select(u => u.Value).Single());
                animal.Weight = weight;
            }
            if (updates.ContainsKey(3))
            {
                var age = Convert.ToInt16(updates.Where(u => u.Key == 3).Select(u => u.Value).Single());
                animal.Age = age;
            }
            if (updates.ContainsKey(4))
            {
                var demeanor = updates.Where(u => u.Key == 4).Select(u => u.Value).Single();
                animal.Demeanor = demeanor;

            }
            if (updates.ContainsKey(5))
            {
                var kidFriendly = Convert.ToBoolean(updates.Where(u => u.Key == 5).Select(u => u.Value).Single());
                animal.KidFriendly = kidFriendly;
            }
            if (updates.ContainsKey(6))
            {
                var petFriendly = Convert.ToBoolean(updates.Where(u => u.Key == 6).Select(u => u.Value).Single());
                animal.PetFriendly = petFriendly;
            }
            if (updates.ContainsKey(7))
            {
                var gender = updates.Where(u => u.Key == 7).Select(u => u.Value).Single();
                animal.Gender = gender;
            }
            if (updates.ContainsKey(8))
            {
                var adoptionStatus = updates.Where(u => u.Key == 8).Select(u => u.Value).Single();
                animal.AdoptionStatus = adoptionStatus;
            }
            if (updates.ContainsKey(9))
            {
                UpdateRoom(animal);
            }

            db.SubmitChanges();

        }
        internal static void RemoveAnimal(Animal animal)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();
            Animal animalToDelete = db.Animals.Where(a => a.AnimalId == animal.AnimalId).Single();
            List<Room> roomsAffected = db.Rooms.Where(r => r.AnimalId == animalToDelete.AnimalId).ToList();
            foreach (Room room in roomsAffected)
            {
                room.AnimalId = null;
            }
            animalToDelete.EmployeeId = null;
            animalToDelete.AdoptionStatus = "Removed";
            db.SubmitChanges();
        }
        internal static List<AnimalShot> GetShots(Animal animal)
        {
                HumaneSocietyDataContext db = new HumaneSocietyDataContext();
                List<AnimalShot> animalShots = new List<AnimalShot>();
                animalShots = db.AnimalShots.Where(s => s.AnimalId == animal.AnimalId).ToList();
            
            return animalShots;
        }
        internal static void UpdateShot(string booster, Animal animal)
        {
            int year;
            int month;
            int day;
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();
            AnimalShot animalShot = new AnimalShot();
            if (UserInterface.GetBitData($"Was {booster} what you meant to type?"))
            {
                //DateTime get
                Console.WriteLine("What year was the shot administered?");
                year = Convert.ToInt16(Console.ReadLine());
                Console.WriteLine("What month was the shot administered?");
                month = Convert.ToInt16(Console.ReadLine());
                Console.WriteLine("What day was the shot administered?");
                day = Convert.ToInt16(Console.ReadLine());
                DateTime date = new DateTime(year, month, day);
                animalShot.DateReceived = date;
                animalShot.AnimalId = animal.AnimalId;
                animalShot.ShotId = db.Shots.Where(s => s.Name == booster).Select(s=>s.ShotId).Single();
                db.AnimalShots.InsertOnSubmit(animalShot);
                db.SubmitChanges();
            }
        }
        internal static Room GetRoom(int AnimalId)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();
            Room currentRoom = db.Rooms.Where(r => r.AnimalId == AnimalId).Single();
            return currentRoom;
        
        }
        internal static Animal UpdateRoom(Animal animal)
        {
            HumaneSocietyDataContext db = new HumaneSocietyDataContext();
            if (UserInterface.GetBitData($"Would you like to modify the room that {animal.Name} is being kept?"))
            {
                Console.WriteLine($"What room are you moving {animal.Name} to?");
                int roomNumber=UserInterface.GetIntegerData();
                int? currentRoomNumber=db.Rooms.Where(r => r.AnimalId == animal.AnimalId).Select(r=>r.RoomNumber).SingleOrDefault();
                int? roomNumberOfExistingRoom = db.Rooms.Where(r => r.RoomNumber == roomNumber).Select(r => r.RoomNumber).SingleOrDefault();
                bool roomExist = roomNumberOfExistingRoom.HasValue;
                int? animalInRoom = db.Rooms.Where(r => r.RoomNumber == roomNumber).Select(r => r.AnimalId).Single();
                try
                { 
                    if (roomExist == true && roomNumber != currentRoomNumber && animalInRoom == null)
                    {

                        Room roomAsignment = db.Rooms.Where(r => r.RoomNumber == roomNumber).Single();
                        List<Room> roomsAffected = db.Rooms.Where(r => r.AnimalId == animal.AnimalId).ToList();
                        foreach (Room room in roomsAffected)
                        {
                            room.AnimalId = null;
                        }
                        roomAsignment.AnimalId = animal.AnimalId;
                        db.SubmitChanges();
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("This room is either the current room or does not exist./n Please try a different room number");
                }

                
            }
            return animal;
        }
    }

}