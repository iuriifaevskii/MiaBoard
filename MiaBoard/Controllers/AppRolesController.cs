using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MiaBoard.Models;

namespace MiaBoard.Controllers
{
    [Authorize]
    public class AppRolesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: AppRoles
        public ActionResult Index()
        {
            var userEmail = HttpContext.User.Identity.Name;
            var user = db.AppUsers.SingleOrDefault(u => u.Email == userEmail);

            if ((user.Roles.Where(r => r.Name == "User")).Count() == 1)
            {
                return RedirectToAction("index", "dashboards");
            }

            return View(db.AppRoles.ToList());
        }
        
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
