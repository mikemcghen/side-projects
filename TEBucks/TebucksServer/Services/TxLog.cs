using Microsoft.VisualBasic;
using System;
using System.ComponentModel;

namespace TEbucksServer.Services
{
    public class TxLog
    {       
        public string Description { get; set; }
        public string Username_From { get; set; }
        public string Username_To { get; set; }
        public decimal Amount { get; set; }

        public TxLog() { }

        public TxLog(string description, string username_from, string username_to, decimal amount)
        {
            Description = description;
            Username_From = username_from;
            Username_To = username_to;
            Amount = amount;
        }
    }
}
