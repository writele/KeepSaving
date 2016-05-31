using System;
using System.Collections.Generic;

namespace KeepSaving.Models
{
    public class Account
    {
        public int Id { get; set; }

        public Account()
        {
            this.Transactions = new HashSet<Transaction>();
        }

        public int? HouseholdId { get; set; }
        public DateTimeOffset Created { get; set; }
        public decimal Balance { get; set; }
        public string Name { get; set; }
        public bool IsReconciled { get; set; }
        public bool IsArchived { get; set; }

        public virtual Household Household { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}