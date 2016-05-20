﻿using KeepSaving.Helpers;
using KeepSaving.Models;
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

        // GET: Transaction/Details/2
        public ActionResult Details(int? id)
        {
            return View();
        }

        //POST: Transaction/RenameAccount
        [HttpPost]
        public ActionResult RenameAccount()
        {
            return RedirectToAction("Index");
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

        //POST: Transaction/ReconcileAccount
        [HttpPost]
        public ActionResult ReconcileAccount()
        {
            return RedirectToAction("Index");
        }

        //POST: Transaction/AddTransaction
        [HttpPost]
        public ActionResult AddTransaction()
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