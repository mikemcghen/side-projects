using System;
using System.Collections.Generic;
using System.Text;

namespace Capstone.Classes
{
    public class Chip : ProductClass
    {
        public Chip(string name, decimal price,string type, string print, int quantity) : base(name, price, type, print, quantity)
        {            
        }
    }
}
