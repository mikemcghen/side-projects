using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace Capstone.Classes
{
    public class Inventory
    {
        public Dictionary<string, ProductClass> vendingMachineValues { get; set; } = new Dictionary<string, ProductClass>();
        string filePath = "C:\\Desktop\\Side-Projects\\Vendo-Matic-800\\Capstone\\vendingmachine.csv";        
        public Inventory()
        {
            MakeList();
        }            
        public void MakeList()
        { 
            try
            { 
            
                using (StreamReader streamReader = new StreamReader(filePath))
                {
                    while (!streamReader.EndOfStream)
                    {
                        string line = streamReader.ReadLine();
                        string[] vendingMachineInventory = line.Split("|");
                        if (vendingMachineInventory[3].Equals("Chip"))
                        {
                            Chip chip = new Chip(vendingMachineInventory[1], decimal.Parse(vendingMachineInventory[2]), vendingMachineInventory[3], "Crunch Crunch, Yum!", 5);
                            this.vendingMachineValues.Add(vendingMachineInventory[0], chip);
                        }
                        else if (vendingMachineInventory[3].Equals("Gum"))
                        {
                            Gum gum = new Gum(vendingMachineInventory[1], decimal.Parse(vendingMachineInventory[2]), vendingMachineInventory[3], "Chew Chew, Yum!", 5);
                            this.vendingMachineValues.Add(vendingMachineInventory[0], gum);
                        }
                        else if (vendingMachineInventory[3].Equals("Drink"))
                        {
                            Drink drink = new Drink(vendingMachineInventory[1], decimal.Parse(vendingMachineInventory[2]), vendingMachineInventory[3], "Glug Glug, Yum!", 5);
                            this.vendingMachineValues.Add(vendingMachineInventory[0], drink);
                        }
                        else if (vendingMachineInventory[3].Equals("Candy"))
                        {
                            Candy candy = new Candy(vendingMachineInventory[1], decimal.Parse(vendingMachineInventory[2]), vendingMachineInventory[3], "Munch Munch, Yum!", 5);
                            this.vendingMachineValues.Add(vendingMachineInventory[0], candy);
                        }
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("Error reading file");
            }
        }
        public string[] Selection()// is void but i cannot test it
        {
            Change change = new Change();
            string userInput2;
            decimal currentMoney = 0M;
            string userDeposit = "";
            do
            {
                Console.WriteLine($"Pleace choose your selection: \n\nCurrent Balance: {currentMoney:C}\n\n(1) Feed Money \n(2) Select Product\n(3) Finish Transaction");
                userInput2 = Console.ReadLine();
                Console.WriteLine();
                if (userInput2 == "1")           
                {
                    Console.WriteLine("Enter whole dollar amount: ");
                    userDeposit = Console.ReadLine();
                    Console.WriteLine();
                    if (decimal.Parse(userDeposit) % 1 == 0 && decimal.Parse(userDeposit) > 0)
                    {

                        currentMoney += decimal.Parse(userDeposit); 
                        Log.WriteLog($"FEED MONEY: {decimal.Parse(userDeposit):C} {currentMoney:C}");

                    }
                    else
                    {
                        Console.WriteLine("Please enter a full number (no decimals)");
                    }                    
                }
                else if (userInput2 == "2")
                {
                    foreach(KeyValuePair<string, ProductClass> item in vendingMachineValues)
                    {
                        Console.Write($"{item.Key} | {item.Value.Name} | {item.Value.Price:C} | {item.Value.Type} | ");
                        if (item.Value.Quantity > 0)
                        {
                            Console.WriteLine(item.Value.Quantity);
                        }
                        else
                        {
                            Console.WriteLine("SOLD OUT");
                        }
                    }
                    Console.WriteLine("\nEnter Code of Selection: ");
                    string selectionCode = Console.ReadLine().ToUpper();
                    Console.WriteLine();
                    if (!vendingMachineValues.ContainsKey(selectionCode))
                    {
                        Console.WriteLine("Invalid selection. Please enter a valid selection: ");
                    }
                    else if (currentMoney < vendingMachineValues[selectionCode].Price)
                    {
                        Console.WriteLine("Get outta here. Ya bother me! \nPlease insert more money. \n");
                    }
                    else if (vendingMachineValues[selectionCode].Quantity == 0)
                    {
                        Console.WriteLine("SOLD OUT");
                    }
                    else
                    {
                        Console.WriteLine(vendingMachineValues[selectionCode].Name);
                        Console.WriteLine("$" + vendingMachineValues[selectionCode].Price);
                        Console.WriteLine(vendingMachineValues[selectionCode].Print);
                        vendingMachineValues[selectionCode].Quantity--;
                        if (vendingMachineValues[selectionCode].Quantity > 0)
                        {
                            Console.WriteLine(vendingMachineValues[selectionCode].Quantity + " remaining");
                        }
                        else
                        {
                            Console.WriteLine("You got the last one!");
                        }
                        currentMoney -= vendingMachineValues[selectionCode].Price;
                        Log.WriteLog($"{vendingMachineValues[selectionCode].Name} {selectionCode} {vendingMachineValues[selectionCode].Price:C} {currentMoney:C}");
                    }
                }
                else if (userInput2 == "3")
                {
                    change.MakeChange(currentMoney);
                }                        
            } while (userInput2 != "3");
            return null;
        }
    }              
}
