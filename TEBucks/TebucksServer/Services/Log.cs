using System;
using TEbucksServer.Models;

namespace TEbucksServer.Services
{
    public class Log
    {
        public string Description { get; set; }
        public string Username_From { get; set; }
        public string Username_To { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedDate { get; set; }

        public Log() { }

        public Log(string description, string username_from, string username_to, decimal amount, DateTime createDate)
        {
            Description = description;
            Username_From = username_from;
            Username_To = username_to;
            Amount = amount;
            CreatedDate = createDate;

        }
    }
}
