using KeepSaving.Helpers;
using KeepSaving.Models;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KeepSaving.Controllers
{
    [RequireHttps]
    [AuthorizeHouseholdRequired]
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            var householdId = User.Identity.GetHouseholdId();
            var model = db.Households.Find(householdId);
            return View(model);
        }

        public ActionResult _TransactionsByMonth(int month, int year)
        {
            try
            {
                //get transactions
                var householdId = User.Identity.GetHouseholdId();
                var household = db.Households.Find(householdId);
                var previousMonth = new DateTimeOffset(year, month - 1, 1, 0, 0, 0, new TimeSpan(-4, 0, 0));
                var nextMonth = new DateTimeOffset(year, month + 1, 1, 0, 0, 0, new TimeSpan(-4, 0, 0));
                var transactions = household.Accounts.SelectMany(m => m.Transactions).Where(t => t.Created.CompareTo(nextMonth) < 0 && t.Created.CompareTo(previousMonth) > 0 && t.TransactionType == TransactionType.Expense).ToList();
                //y: transaction amount
                //indexLabel: transaction category name
                var data = (from item in transactions
                            select new
                            {
                                y = item.Amount,
                                indexLabel = item.BudgetCategory.Name,
                            }).ToArray();
                //return PartialView();
                return Content(JsonConvert.SerializeObject(data), "application/json");
            }
            catch
            {
                return PartialView("_Error");
            }
            
        }
    }
}