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
    public class DashletsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Dashlets
        public ActionResult Index()
        {
            var dashlets = db.Dashlets.Include(d => d.Dashboard).Include(d => d.DataSource);
            return View(dashlets.ToList());
        }

        // GET: Dashlets/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dashlet dashlet = db.Dashlets.Find(id);
            if (dashlet == null)
            {
                return HttpNotFound();
            }
            return View(dashlet);
        }

        // GET: Dashlets/Create
        public ActionResult Create()
        {
            ViewBag.DashboardId = new SelectList(db.Dashboards, "Id", "Title");
            ViewBag.DataSourceId = new SelectList(db.DataSources, "Id", "ConnectionString");
            return View();
        }

        // POST: Dashlets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Body,DashboardId,DataSourceId,Sql,Styles")] Dashlet dashlet)
        {
            if (ModelState.IsValid)
            {
                db.Dashlets.Add(dashlet);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DashboardId = new SelectList(db.Dashboards, "Id", "Title", dashlet.DashboardId);
            ViewBag.DataSourceId = new SelectList(db.DataSources, "Id", "ConnectionString", dashlet.DataSourceId);
            return View(dashlet);
        }

        // GET: Dashlets/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dashlet dashlet = db.Dashlets.Find(id);
            if (dashlet == null)
            {
                return HttpNotFound();
            }
            ViewBag.DashboardId = new SelectList(db.Dashboards, "Id", "Title", dashlet.DashboardId);
            ViewBag.DataSourceId = new SelectList(db.DataSources, "Id", "ConnectionString", dashlet.DataSourceId);
            return View(dashlet);
        }

        // POST: Dashlets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Body,DashboardId,DataSourceId,Sql,Styles")] Dashlet dashlet)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dashlet).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DashboardId = new SelectList(db.Dashboards, "Id", "Title", dashlet.DashboardId);
            ViewBag.DataSourceId = new SelectList(db.DataSources, "Id", "ConnectionString", dashlet.DataSourceId);
            return View(dashlet);
        }

        // GET: Dashlets/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dashlet dashlet = db.Dashlets.Find(id);
            if (dashlet == null)
            {
                return HttpNotFound();
            }
            return View(dashlet);
        }

        // POST: Dashlets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Dashlet dashlet = db.Dashlets.Find(id);
            db.Dashlets.Remove(dashlet);
            db.SaveChanges();
            return RedirectToAction("Index");
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
