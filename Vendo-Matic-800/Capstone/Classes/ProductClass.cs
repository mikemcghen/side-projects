using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Capstone.Classes
{
    public abstract class ProductClass
    {
        public string InventoryNumber { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Type { get; set; }
        public string Print { get; set; }
        public int Quantity { get; set; }
        public ProductClass(string name, decimal price, string type, string print, int quantity)
        {
            Name = name;
            Price = price;
            Type = type;
            Print = print;
            Quantity = quantity;
        }       
    }
}