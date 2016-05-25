using KeepSaving.Helpers;
using KeepSaving.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KeepSaving.Controllers
{
    [AuthorizeHouseholdRequired]
    public class TransactionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Transaction
        public ActionResult Index()
        {
            var householdId = User.Identity.GetHouseholdId();
            var model = db.Accounts.Where(a => a.HouseholdId == householdId).ToList();
            return View(model);
        }

        //POST: Transaction/AddAccount
        [HttpPost]
        public ActionResult AddAccount(string AccountName)
        {
            Account account = new Account();
            account.Name = AccountName;
            account.Balance = 0;
            account.Created = DateTimeOffset.Now;
            account.IsReconciled = false;
            db.Accounts.Add(account);
            db.SaveChanges();
            var householdId = User.Identity.GetHouseholdId();
            var household = db.Households.Find(householdId);
            household.Accounts.Add(account);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Transaction/Details/2
        public ActionResult Details(int? id)
        {
            try {
                var model = db.Accounts.Find(id);
                return View(model);
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        // GET: Add Transaction
        public ActionResult _AddTransaction(int? id)
        {
            try {
                ViewBag.AccountId = id;
                var householdId = User.Identity.GetHouseholdId();
                var household = db.Households.Find(householdId);
                var categories = household.Budget.BudgetItems.Select(b => b.BudgetCategory).Distinct().ToList();
                ViewBag.BudgetCategories = categories;
                return PartialView();
            }
            catch
            {
                return PartialView("_Error");
            }
        }

        //Helper function: Update account balance
        public bool UpdateAccountBalance(bool IsIncome, decimal Amount, int? AccountId)
        {
            var account = db.Accounts.Find(AccountId);
            if (IsIncome)
            {
                account.Balance += Amount;
            }
            else
            {
                account.Balance -= Amount;
            }
            db.Accounts.Attach(account);
            db.Entry(account).Property("Balance").IsModified = true;
            db.SaveChanges();

            return true;
        }

        //POST: Transaction/AddTransaction
        [HttpPost]
        public ActionResult AddTransaction([Bind(Include = "Amount, Description, TransactionType")] Transaction transaction, int? AccountId, int? BudgetCategoryId)
        {
            if (ModelState.IsValid)
            {
                transaction.Created = DateTimeOffset.Now;
                var userId = User.Identity.GetUserId();
                transaction.AuthorId = userId;
                if (transaction.TransactionType == TransactionType.Expense)
                {
                    transaction.BudgetCategoryId = BudgetCategoryId;
                    UpdateAccountBalance(false, transaction.Amount, AccountId);
                }
                else
                {
                    UpdateAccountBalance(true, transaction.Amount, AccountId);
                }
                db.Transactions.Add(transaction);
                db.SaveChanges();

                var theTransaction = db.Transactions.Find(transaction.Id);
                var account = db.Accounts.Find(AccountId);
                account.Transactions.Add(theTransaction);
                db.SaveChanges();
            }
            return RedirectToAction("Details", new { id = transaction.AccountId});
        }

        public ActionResult _EditTransaction(int? id)
        {
            try
            {
                var householdId = User.Identity.GetHouseholdId();
                var household = db.Households.Find(householdId);
                var categories = household.Budget.BudgetItems.Select(b => b.BudgetCategory).Distinct().ToList();
                ViewBag.BudgetCategories = categories;
                var transaction = db.Transactions.Find(id);
                return PartialView(transaction);
            }
            catch
            {
                return PartialView("_Error");
            }
        }

        //POST: Transaction/EditTransaction
        [HttpPost]
        public ActionResult EditTransaction()
        {
            return RedirectToAction("Index");
        }

        // GET: Delete Transaction
        public ActionResult _DeleteTransaction(int? id)
        {
            try
            {
                var model = db.Transactions.Find(id);
                return PartialView(model);
            }
            catch
            {
                return PartialView("_Error");
            }
        }

        //POST: Transaction/DeleteTransaction
        [HttpPost]
        public ActionResult DeleteTransaction(int Id, int AccountId)
        {
            var transaction = db.Transactions.Find(Id);
            db.Transactions.Remove(transaction);
            db.SaveChanges();
            return RedirectToAction("Details", new { id = AccountId });
        }

        //POST: Transaction/RenameAccount
        [HttpPost]
        public ActionResult RenameAccount()
        {
            return RedirectToAction("Index");
        }

        //POST: Transaction/ReconcileAccount
        [HttpPost]
        public ActionResult ReconcileAccount()
        {
            return RedirectToAction("Index");
        }

    }
}