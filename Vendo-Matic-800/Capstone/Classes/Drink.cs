using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Classes
{
    public class Drink : ProductClass
    {                
        public Drink(string name, decimal price, string type, string print, int quantity) : base(name, price, type, print, quantity)
        {           
        }        
    }
}
