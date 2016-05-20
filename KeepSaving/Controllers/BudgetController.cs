using KeepSaving.Helpers;
using KeepSaving.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KeepSaving.Controllers
{
    public class BudgetController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Budget
        public ActionResult Index()
        {
            var householdId = User.Identity.GetHouseholdId();
            var household = db.Households.Find(householdId);
            var model = household.Budget;
            return View(model);
        }

        //GET: Budget/_AddBudgetItem
        public ActionResult _AddBudgetItem()
        {
            return PartialView();
        }

        //POST: Add Budget Item
        [HttpPost]
        public ActionResult AddBudgetItem()
        {
            return RedirectToAction("Index");
        }

        //POST: Edit Budget Item
        [HttpPost]
        public ActionResult EditBudgetItem()
        {
            return RedirectToAction("Index");
        }

        //POST: Edit Budget Amount
        [HttpPost]
        public ActionResult EditBudgetAmount()
        {
            return RedirectToAction("Index");
        }
    }
}