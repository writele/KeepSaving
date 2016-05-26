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


        // GET: Add BudgetItem
        public ActionResult _AddBudgetItem(int? id)
        {
            try
            {
                return PartialView();
            }
            catch
            {
                return PartialView("_Error");
            }
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

        // GET: Edit BudgetItem
        public ActionResult _EditBudgetItem(int? id)
        {
            try
            {
                var model = db.BudgetItems.Find(id);
                return PartialView(model);
            }
            catch
            {
                return PartialView("_Error");
            }
        }

        //POST: Edit Budget Item
        [HttpPost]
        public ActionResult EditBudgetItem([Bind(Include = "Id, BudgetId, Created, Amount, Frequency, BudgetCategory.Id")] BudgetItem item, string CategoryName)
        {
            if (ModelState.IsValid)
            {
                var householdId = User.Identity.GetHouseholdId();
                var budget = db.Budgets.FirstOrDefault(b => b.HouseholdId == householdId);
                var oldItem = db.BudgetItems.AsNoTracking().FirstOrDefault(m => m.Id == item.Id);
                item.Modified = DateTimeOffset.Now;
                
                budget.Amount -= oldItem.Amount * oldItem.Frequency / 12;
                budget.Amount += item.Amount * item.Frequency / 12;

                db.BudgetItems.Attach(item);
                db.Entry(item).Property("Amount").IsModified = true;
                db.Entry(item).Property("Frequency").IsModified = true;
                db.Entry(item).Property("Modified").IsModified = true;
                db.Budgets.Attach(budget);
                db.Entry(budget).Property("Amount").IsModified = true;
                db.SaveChanges();

                // need to validate that category name is unique
                var category = db.BudgetCategories.Find(oldItem.BudgetCategory.Id);
                category.Name = CategoryName;
                db.BudgetCategories.Attach(category);
                db.Entry(category).Property("Name").IsModified = true;
                db.SaveChanges();
            }
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