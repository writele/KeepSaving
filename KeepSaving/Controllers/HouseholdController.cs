using KeepSaving.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KeepSaving.Controllers
{
    public class HouseholdController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

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
        public ActionResult CreateHousehold([Bind(Include = "Name")] Household household)
        {
            if (ModelState.IsValid)
            {
                household.Create = System.DateTimeOffset.Now;
                var userId = User.Identity.GetUserId();
                var user = db.Users.Find(userId);
                db.Households.Add(household);
                db.SaveChanges();
                var thisHousehold = db.Households.Find(household.Id);
                thisHousehold.Users.Add(user);

                db.SaveChanges();
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
