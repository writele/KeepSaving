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
                var categories = db.BudgetCategories.ToList();
                ViewBag.BudgetCategories = categories;
                return PartialView();
            }
            catch
            {
                return PartialView("_Error");
            }
        }

        //POST: Transaction/AddTransaction
        [HttpPost]
        public ActionResult AddTransaction([Bind(Include = "Amount, Description, BudgetCategory, TransactionType")] Transaction transaction, int? AccountId)
        {
            if (ModelState.IsValid)
            {
                transaction.Created = DateTimeOffset.Now;
                var userId = User.Identity.GetUserId();
                transaction.AuthorId = userId;
                db.Transactions.Add(transaction);
                db.SaveChanges();

                var theTransaction = db.Transactions.Find(transaction.Id);
                var account = db.Accounts.Find(AccountId);
                account.Transactions.Add(theTransaction);
                db.SaveChanges();
            }
            return RedirectToAction("Details", new { id = transaction.AccountId});
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

        //POST: Transaction/EditTransaction
        [HttpPost]
        public ActionResult EditTransaction()
        {
            return RedirectToAction("Index");
        }

        //POST: Transaction/DeleteTransaction
        [HttpPost]
        public ActionResult DeleteTransaction()
        {
            return RedirectToAction("Index");
        }
    }
}