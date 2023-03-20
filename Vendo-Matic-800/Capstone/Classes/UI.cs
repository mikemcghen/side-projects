using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

namespace Capstone.Classes
{
    public class UI
    {
        Inventory inventory = new Inventory();       
        public void Prompt()
        {
            Console.WriteLine("Welcome to the Vendo-Matic 800 from Umbrella Corp. \n");
            string userInput = "";
            do
            {
                Console.WriteLine("Pleace choose your selection:\n");
                Console.WriteLine("(1) Display Vending Machine Items");
                Console.WriteLine("(2) Purchase");
                Console.WriteLine("(3) Exit");

                userInput = Console.ReadLine();

                Console.WriteLine();
               
                if (userInput.Equals("1"))
                {                    
                    foreach (KeyValuePair<string, ProductClass> item in inventory.vendingMachineValues)
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
                    Console.WriteLine();
                }
                else if (userInput.Equals("2"))
                {
                    inventory.Selection();
                }
            } while (!userInput.Equals("3"));
            Console.WriteLine("Thank you for shopping with Umbrella Corp.,\nHave a nice day!");
        }
    }          
}


