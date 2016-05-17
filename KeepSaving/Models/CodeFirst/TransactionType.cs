using System.ComponentModel.DataAnnotations;

namespace KeepSaving.Models
{
    public enum TransactionType
    {
        [Display(Name = "Income")]
        Income,
        [Display(Name = "Expense")]
        Expense
    }
}