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
    public class AppRolesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: AppRoles
        public ActionResult Index()
        {
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
