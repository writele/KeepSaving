using KeepSaving.Helpers;
using KeepSaving.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KeepSaving.Controllers
{
    [AuthorizeHouseholdRequired]
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

        //POST: Add Budget Item
        [HttpPost]
        public ActionResult AddBudgetItem([Bind(Include = "Amount, Frequency")] BudgetItem item, string CategoryName)
        {
            if (ModelState.IsValid)
            {
                var householdId = User.Identity.GetHouseholdId();
                var budget = db.Budgets.FirstOrDefault(b => b.HouseholdId == householdId);
                item.Created = DateTimeOffset.Now;
                db.BudgetItems.Add(item);
                budget.BudgetItems.Add(item);

                budget.Amount += item.Amount * item.Frequency / 12;
                db.Budgets.Attach(budget);
                db.Entry(budget).Property("Amount").IsModified = true;
                db.SaveChanges();

                // need to validate that category name is unique
                BudgetCategory category = new BudgetCategory();
                category.Name = CategoryName;
                category.BudgetItemId = item.Id;
                category.BudgetItem = item;      
                db.BudgetCategories.Add(category);                    
                db.SaveChanges();
            }
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