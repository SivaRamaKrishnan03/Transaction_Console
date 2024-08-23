using System;

namespace TransactionProcessingApp.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string ? Type { get; set; }
        public decimal Amount { get; set; }
    }
}
