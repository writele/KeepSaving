using System.ComponentModel.DataAnnotations;

namespace KeepSaving.Models
{
    public enum TransactionType
    {
        [Display(Name = "Expense")]
        Expense,
        [Display(Name = "Income")]
        Income
    }
}