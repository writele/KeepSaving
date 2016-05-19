using KeepSaving.Helpers;
using KeepSaving.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace KeepSaving.Controllers
{
    [RequireHttps]
    public class HouseholdController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [AuthorizeHouseholdRequired]
        // GET: Household
        public ActionResult Index()
        {
            return View();
        }

        // GET: Household/JoinHousehold
        public ActionResult JoinHousehold()
        {
            return View();
        }

        // POST: Household/JoinHousehold
        [HttpPost]
        public ActionResult JoinHousehold(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // POST: Household/CreateHousehold
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateHousehold([Bind(Include = "Name")] Household household)
        {
            if (ModelState.IsValid)
            {
                //Create household
                household.Create = System.DateTimeOffset.Now;
                db.Households.Add(household);
                db.SaveChanges();
                //Add user to household
                var userId = User.Identity.GetUserId();
                var user = db.Users.Find(userId);
                var thisHousehold = db.Households.Find(household.Id);
                thisHousehold.Users.Add(user);
                //Create budget for household
                Budget budget = new Budget();
                budget.Created = System.DateTimeOffset.Now;
                budget.Amount = 0;
                budget.HouseholdId = thisHousehold.Id;
                budget.Household = thisHousehold;
                db.Budgets.Add(budget);
                //Update household to include budget
                thisHousehold.Budget = budget;
                thisHousehold.BudgetId = budget.Id;
                db.Households.Attach(thisHousehold);
                db.Entry(thisHousehold).Property("BudgetId").IsModified = true;
                db.SaveChanges();
                //Refresh cookies to add new household Id
                await ControllerContext.HttpContext.RefreshAuthentication(user);
                return RedirectToAction("Index");
            }

            return View();
        }

        // POST: Household/InviteUser/5
        [HttpPost]
        public ActionResult InviteUser(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // POST: Household/LeaveHousehold/5
        [HttpPost]
        public ActionResult LeaveHousehold(int id, FormCollection collection)
        {
            //initialize identity manager
            //get user
            //change user.HouseholdId to null
            //save db changes
            return RedirectToAction("JoinHousehold");
        }
    }
}
