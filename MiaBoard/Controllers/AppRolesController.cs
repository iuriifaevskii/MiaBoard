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

        // GET: AppRoles/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AppRole appRole = db.AppRoles.Find(id);
            if (appRole == null)
            {
                return HttpNotFound();
            }
            return View(appRole);
        }

        // GET: AppRoles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AppRoles/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] AppRole appRole)
        {
            if (ModelState.IsValid)
            {
                db.AppRoles.Add(appRole);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(appRole);
        }

        // GET: AppRoles/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AppRole appRole = db.AppRoles.Find(id);
            if (appRole == null)
            {
                return HttpNotFound();
            }
            return View(appRole);
        }

        // POST: AppRoles/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] AppRole appRole)
        {
            if (ModelState.IsValid)
            {
                db.Entry(appRole).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(appRole);
        }

        // GET: AppRoles/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AppRole appRole = db.AppRoles.Find(id);
            if (appRole == null)
            {
                return HttpNotFound();
            }
            return View(appRole);
        }

        // POST: AppRoles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AppRole appRole = db.AppRoles.Find(id);
            db.AppRoles.Remove(appRole);
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
