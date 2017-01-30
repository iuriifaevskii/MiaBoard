using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using MiaBoard.Models;
using MiaBoard.ViewModels;

namespace MiaBoard.Controllers
{
    public class DashboardsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Dashboards/View/5
        public ActionResult View(int id)
        {
            if (id == 0)
                return HttpNotFound();

            var viewDashboardViewModel = new ViewDashboardViewModel();

            viewDashboardViewModel.DashletsSqlResult = new Dictionary<int, string>();
            
            viewDashboardViewModel.DashletsFirstCol = new List<Dashlet>();
            viewDashboardViewModel.DashletsSecondCol = new List<Dashlet>();
            viewDashboardViewModel.DashletsThirdCol = new List<Dashlet>();

            viewDashboardViewModel.DashboardList = db.Dashboards.ToList();
            viewDashboardViewModel.Dashboard = db.Dashboards.SingleOrDefault(d => d.Id == id);
            viewDashboardViewModel.Dashlets = db.Dashlets.Include(d => d.DataSource).Where(d => d.DashboardId == id).OrderBy(d => d.Position).ToList();
            

            foreach (var dashlet in viewDashboardViewModel.Dashlets)
            {
                switch (dashlet.DataSource.Type)
                {
                    case "MSSQL":
                        try
                        {
                            using (SqlConnection connection = new SqlConnection())
                            {
                                connection.ConnectionString = dashlet.DataSource.ConnectionString;
                                connection.Open();

                                SqlCommand command = new SqlCommand(dashlet.Sql, connection);

                                using (SqlDataReader reader = command.ExecuteReader())
                                {
                                    if (reader.Read())
                                    {
                                        viewDashboardViewModel.DashletsSqlResult.Add(dashlet.Id, reader[0].ToString());
                                    }
                                }

                                connection.Close();
                            }
                        }
                        catch
                        {
                            viewDashboardViewModel.DashletsSqlResult.Add(dashlet.Id,"SQL queary is incorrect!");
                            //continue;
                            //return Content("Invalid Connection to Database in Dashhlet: " + dashlet.Id);
                        }
                        break;
                    default:
                        viewDashboardViewModel.DashletsSqlResult.Add(dashlet.Id, "Type of Database is unknown!");
                        //continue;
                        //return Content("Invalid Type of Database in Dashhlet: " + dashlet.Id);
                        break;
                }

                switch (dashlet.Column)
                {
                    case 1:
                        viewDashboardViewModel.DashletsFirstCol.Add(dashlet);
                        break;
                    case 2:
                        viewDashboardViewModel.DashletsSecondCol.Add(dashlet);
                        break;
                    case 3:
                        viewDashboardViewModel.DashletsThirdCol.Add(dashlet);
                        break;
                    default:
                        Console.WriteLine("Toggle is broke. It's equal to " + dashlet.Column);
                        break;
                }

            }

            return View(viewDashboardViewModel);
        }

        // GET: Dashboards/View/5
        public ActionResult ViewUser(int id)
        {
            if (id == 0)
                return HttpNotFound();

            var viewDashboardViewModel = new ViewDashboardViewModel();

            viewDashboardViewModel.DashletsSqlResult = new Dictionary<int, string>();

            viewDashboardViewModel.DashletsFirstCol = new List<Dashlet>();
            viewDashboardViewModel.DashletsSecondCol = new List<Dashlet>();
            viewDashboardViewModel.DashletsThirdCol = new List<Dashlet>();

            viewDashboardViewModel.DashboardList = db.Dashboards.ToList();
            viewDashboardViewModel.Dashboard = db.Dashboards.SingleOrDefault(d => d.Id == id);
            viewDashboardViewModel.Dashlets = db.Dashlets.Include(d => d.DataSource).Where(d => d.DashboardId == id).OrderBy(d => d.Position).ToList();


            foreach (var dashlet in viewDashboardViewModel.Dashlets)
            {
                switch (dashlet.DataSource.Type)
                {
                    case "MSSQL":
                        try
                        {
                            using (SqlConnection connection = new SqlConnection())
                            {
                                connection.ConnectionString = dashlet.DataSource.ConnectionString;
                                connection.Open();

                                SqlCommand command = new SqlCommand(dashlet.Sql, connection);

                                using (SqlDataReader reader = command.ExecuteReader())
                                {
                                    if (reader.Read())
                                    {
                                        viewDashboardViewModel.DashletsSqlResult.Add(dashlet.Id, reader[0].ToString());
                                    }
                                }

                                connection.Close();
                            }
                        }
                        catch
                        {
                            viewDashboardViewModel.DashletsSqlResult.Add(dashlet.Id, "SQL queary is incorrect!");
                            //continue;
                            //return Content("Invalid Connection to Database in Dashhlet: " + dashlet.Id);
                        }
                        break;
                    default:
                        viewDashboardViewModel.DashletsSqlResult.Add(dashlet.Id, "Type of Database is unknown!");
                        //continue;
                        //return Content("Invalid Type of Database in Dashhlet: " + dashlet.Id);
                        break;
                }

                switch (dashlet.Column)
                {
                    case 1:
                        viewDashboardViewModel.DashletsFirstCol.Add(dashlet);
                        break;
                    case 2:
                        viewDashboardViewModel.DashletsSecondCol.Add(dashlet);
                        break;
                    case 3:
                        viewDashboardViewModel.DashletsThirdCol.Add(dashlet);
                        break;
                    default:
                        Console.WriteLine("Toggle is broke. It's equal to " + dashlet.Column);
                        break;
                }

            }

            return View(viewDashboardViewModel);
        }

        // GET: Dashboards
        public ActionResult Index()
        {
            return View(db.Dashboards.ToList());
        }

        // GET: Dashboards/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dashboard dashboard = db.Dashboards.Find(id);
            if (dashboard == null)
            {
                return HttpNotFound();
            }
            return View(dashboard);
        }

        // GET: Dashboards/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Dashboards/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title")] Dashboard dashboard)
        {
            if (ModelState.IsValid)
            {
                db.Dashboards.Add(dashboard);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(dashboard);
        }

        // GET: Dashboards/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dashboard dashboard = db.Dashboards.Find(id);
            if (dashboard == null)
            {
                return HttpNotFound();
            }
            return View(dashboard);
        }

        // POST: Dashboards/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title")] Dashboard dashboard)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dashboard).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(dashboard);
        }

        // GET: Dashboards/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dashboard dashboard = db.Dashboards.Find(id);
            if (dashboard == null)
            {
                return HttpNotFound();
            }
            return View(dashboard);
        }

        // POST: Dashboards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Dashboard dashboard = db.Dashboards.Find(id);
            db.Dashboards.Remove(dashboard);
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
