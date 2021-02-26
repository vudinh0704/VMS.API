using System;
using System.ComponentModel.DataAnnotations;

namespace VMS.Core.Entities
{
    public class TransactionBA
    {
        [Key]
        public int TransactionId { get; set; }

        public int BudgetId { get; set; }

        public string AccountId { get; set; }

        public int Type { get; set; }

        public double MoneyBalance { get; set; }

        public int ScoreBalance { get; set; }

        public string Content { get; set; }

        public DateTime Date { get; set; }
    }
}