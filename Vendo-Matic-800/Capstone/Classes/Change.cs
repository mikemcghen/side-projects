using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace Capstone.Classes
{
    public class Change
    {
        public int MakeChange(decimal currentMoney) //void
        {
            Console.WriteLine("Your change is: ");
            decimal initialBalance = currentMoney;
            int quarters = 0;
            int dimes = 0;
            int nickels = 0;
            while (currentMoney >= .25M)
            {
                currentMoney -= .25M;
                quarters++;
            }
            while (currentMoney >= .10M)
            {
                currentMoney -= .10M;
                dimes++;
            }
            while (currentMoney >= .05M)
            {
                currentMoney -= .05M;
                nickels++;
            }
            if (quarters > 0)
            {
                Console.WriteLine($"{quarters} Quarter(s)");
            }
            if (dimes > 0)
            {
                Console.WriteLine($"{dimes} Dime(s)");
            }
            if (nickels > 0)
            {
                Console.WriteLine($"{nickels} Nickel(s)");
            }
            Log.WriteLog($"GIVE CHANGE: {initialBalance:C} {currentMoney:C}");
            Console.WriteLine();


            return quarters + dimes + nickels;
            

            //return 1; //added for unit testing
        } 
             
    }
}    
