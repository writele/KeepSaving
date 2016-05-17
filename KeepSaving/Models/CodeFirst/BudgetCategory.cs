using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KeepSaving.Models
{
    public class BudgetCategory
    {
        public int Id { get; set; }

        public BudgetCategory()
        {
            this.Transactions = new HashSet<Transaction>();
        }

        public string Name { get; set; }
        public int BudgetItemId { get; set; }

        [Required]
        public virtual BudgetItem BudgetItem { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}