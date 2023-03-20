using Capstone.Classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Net.Security;
using System.Text;

namespace CapstoneTests
{
    [TestClass]
    public class InventoryTest
    {
        [TestMethod]
        public void Selection_Happy_Path()
        {
            Inventory sut = new Inventory();
            string[] vendingMachineInventory = new string[] { };
            string[] expected = new string[] { };

            string[] actual = sut.Selection();
            CollectionAssert.AreEqual(expected, actual);
        }
        //[TestMethod]
        //public void MakeList_()
        //{
        //    Inventory sut = new Inventory();
        //    string[] input = new string[] { };
        //    string[] expected = new string[] { };

        //    string[] actual = sut.MakeList(input);
        //    CollectionAssert.AreEqual(expected, actual);
        //}


    }

}   

  