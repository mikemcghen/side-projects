using Capstone.Classes;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace CapstoneTests
{
    [TestClass]
    public class ChangeTest
    {
        [TestMethod]
        public void MakeChange_HappyPath() //name of method
        {
            Change sut = new Change(); // name of class
            int currentMoney = 1;
            //int quarters = 0;
            int expected = 4;
            int actual = sut.MakeChange(currentMoney);
            Assert.AreEqual(expected, actual, $"Expected 4 quarters in change from a dollar");
        }
        [TestMethod]
        public void MakeChange_Exact_Nickel()
        {
            Change sut = new Change();
            decimal currentMoney = .05M;
            //int nickels = 0;
            int expected = 1;
            int actual = sut.MakeChange(currentMoney);
            Assert.AreEqual(expected, actual, $"Expected 1 nickel in change from 5cents");
        }
        [TestMethod]
        public void MakeChange_ExactDime()
        {
            Change sut = new Change();
            decimal currentMoney = .10M;
            //int dimes = 0;
            int expected = 1;
            int actual = sut.MakeChange(currentMoney);
            Assert.AreEqual(expected, actual, $"Expected 1 dime in change from 10cents");
        }
        [TestMethod]
        public void MakeChange_1994Quarters() 
        {
            Change sut = new Change();
            decimal currentMoney = 489.50M;
            //int quarters = 0;
            int expected = 1958;
            int actual = sut.MakeChange(currentMoney);
            Assert.AreEqual(expected, actual, $"Expected 1958 quarters in change from 498.50");
        }
        [TestMethod]
        public void MakeChange_Zero_Balance()
        {
            Change sut = new Change();
            decimal currentMoney = 0.00M;
            //int quarters = 0;
            int expected = 0;
            int actual = sut.MakeChange(currentMoney);
            Assert.AreEqual(expected, actual, $"Expected no change back from a zero remaining balance");
        }
        [TestMethod]
        public void MakeChange_Quarter_Dime()
        {
            Change sut = new Change();
            decimal currentMoney = 0.35M;
            //int quarters = 0;
            int expected = 2; //2 coins
            int actual = sut.MakeChange(currentMoney);
            Assert.AreEqual(expected, actual, $"Expected 2, 1 quarter and 1 dime");
        }
    }
}

