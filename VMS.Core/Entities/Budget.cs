using System.ComponentModel.DataAnnotations;

namespace VMS.Core.Entities
{
    public class Budget
    {
        [Key]
        public int BudgetId { get; set; }

        public double MoneyBalance { get; set; }

        public int ScoreBalance { get; set; }
    }
}