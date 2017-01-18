using MiaBoard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MiaBoard.Controllers
{
    public class DashboardsController : Controller
    {
        //
        // GET: /Dashboards/
        public ActionResult Index()
        {
            return View();
        }

        private ApplicationDbContext _context;
        public DashboardsController()
        {
            _context = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        public ActionResult New()
        {
            var dashboard = new Dashboard();

            return View("DashboardForm", dashboard);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(Dashboard dashboard)
        {

            if(!ModelState.IsValid)
            {
                var dashboards = _context.Dashboards.ToList();
                return View("CustomerForm", dashboards);
            }

            if (dashboard.Id == 0)
                _context.Dashboards.Add(dashboard);
            else
            {
                var dashboardInDb = _context.Dashboards.Single(c => c.Id == dashboard.Id);
                dashboardInDb.Title = dashboard.Title;
                
            }

            _context.SaveChanges();

            return RedirectToAction("Index", "Dashboards");
        }

        public ActionResult Details(int id)
        {
            var dashboard = _context.Dashboards.SingleOrDefault(c => c.Id == id);//include
            if (dashboard == null)
            {
                return HttpNotFound();
            }

            return View(dashboard);
        }

        public ActionResult Edit(int id)
        {
            var dashboard = _context.Dashboards.SingleOrDefault(c => c.Id == id);
            if (dashboard == null)
                return HttpNotFound();


            return View("DashboardForm", dashboard);
        }



	}
}