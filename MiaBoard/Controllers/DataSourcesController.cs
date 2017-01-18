using MiaBoard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MiaBoard.Controllers
{
    public class DataSourcesController : Controller
    {
        //
        // GET: /DataSources/
        public ActionResult Index()
        {
            return View();
        }

        private ApplicationDbContext _context;
        public DataSourcesController()
        {
            _context = new ApplicationDbContext();
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }
        public ActionResult New()
        {
            var dataSources = new DataSource();

            return View("Index", dataSources);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Save(DataSource dataSource)
        {

            if(!ModelState.IsValid)
            {
                var dataSourcesModel = dataSource;
                return View("Index", dataSourcesModel);
            }

            if (dataSource.Id == 0)
                _context.DataSources.Add(dataSource);
            else
            {
                var dataSourceInDb = _context.DataSources.Single(c => c.Id == dataSource.Id);
                dataSourceInDb.Type = dataSource.Type;
                dataSourceInDb.UserName = dataSource.UserName;
                dataSourceInDb.ServerName = dataSource.ServerName;
                dataSourceInDb.Password = dataSource.Password;
                dataSourceInDb.DatabaseName = dataSource.DatabaseName;
                dataSourceInDb.ConnectionString = dataSource.ConnectionString;
            }

            _context.SaveChanges();

            return RedirectToAction("Index", "DataSources");
        }

        public ActionResult Details(int id)
        {
            var dataSource = _context.DataSources.SingleOrDefault(c => c.Id == id);//include
            if (dataSource == null)
            {
                return HttpNotFound();
            }

            return View(dataSource);
        }

        public ActionResult Edit(int id)
        {
            var dataSource = _context.DataSources.SingleOrDefault(c => c.Id == id);
            if (dataSource == null)
                return HttpNotFound();


            return View("Index", dataSource);
        }



	}
}