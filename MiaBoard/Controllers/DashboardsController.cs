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
    [Authorize]
    public class DashboardsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Dashboards/View/5
        public ActionResult View(int? id)
        {
            if (id == 0)
                return HttpNotFound();

            var model = new ViewDashboardViewModel();
            model.Dashlets = new List<Dashlet>();
            model.DataSources = new List<DataSource>();
            model.DashletsSqlResult = new Dictionary<int, string>();
            model.DashletsFirstCol = new List<Dashlet>();
            model.DashletsSecondCol = new List<Dashlet>();
            model.DashletsThirdCol = new List<Dashlet>();

            var userEmail = HttpContext.User.Identity.Name;
            var user = db.AppUsers.SingleOrDefault(u => u.Email == userEmail);

            model.IsCompanyAdmin = (user.Roles.Where(r => r.Name == "CompanyAdmin")).Count() == 1;
            model.IsSuperAdmin = (user.Roles.Where(r => r.Name == "SuperAdmin")).Count() == 1;
            model.IsUser = (user.Roles.Where(r => r.Name == "User")).Count() == 1;

            model.Dashboard = db.Dashboards.SingleOrDefault(d => d.Id == id);
            model.DashboardList = db.Dashboards.ToList();
            model.DashboardListToUser = user.Dashboards.Select(p => new DashboardItemViewModel() { DashboardId = p.Id, DashboardName = p.Title }).ToList();

            model.Dashlets = db.Dashlets.Include(m => m.DataSource).Where(d => d.DashboardId == id).OrderBy(d => d.Position).ToList();

            model.CurrentUser = user;
            model.Email = user.Email;
            model.ID = user.Id;

            if(id == null){
                if (model.IsSuperAdmin)
                {
                    return RedirectToAction("index", "dashboards");
                }
                else if(model.IsCompanyAdmin){
                    return RedirectToAction("index", "dashboards");
                }
                else if(model.IsUser){
                    return RedirectToAction("index", "dashboards");
                }
                else {
                    return HttpNotFound();
                }
            }

            model.DataSources = db.DataSources.Where(x => x.OwnerId == model.Dashboard.IdOwner).ToList();

            foreach (var dashlet in model.Dashlets)
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
                                        model.DashletsSqlResult.Add(dashlet.Id, reader[0].ToString());
                                    }
                                }

                                connection.Close();
                            }
                        }
                        catch
                        {
                            model.DashletsSqlResult.Add(dashlet.Id, "SQL queary is incorrect!");
                            //continue;
                            //return Content("Invalid Connection to Database in Dashhlet: " + dashlet.Id);
                        }
                        break;
                    default:
                        model.DashletsSqlResult.Add(dashlet.Id, "Type of Database is unknown!");
                        //continue;
                        //return Content("Invalid Type of Database in Dashhlet: " + dashlet.Id);
                        break;
                }

                switch (dashlet.Column)
                {
                    case 1:
                        model.DashletsFirstCol.Add(dashlet);
                        break;
                    case 2:
                        model.DashletsSecondCol.Add(dashlet);
                        break;
                    case 3:
                        model.DashletsThirdCol.Add(dashlet);
                        break;
                    default:
                        Console.WriteLine("Toggle is broke. It's equal to " + dashlet.Column);
                        break;
                }

            }

            model.IsOwner = model.Dashboard.IdOwner == user.Id;
            model.IsDashboardAdmin = model.Dashboard.IdDashboardAdmin == user.Id;

            if (model.IsSuperAdmin)
            {
                return View(model);
            }
            else if (model.IsOwner)
            {
                model.DashboardList = model.DashboardList.Where(x => x.IdOwner == user.Id).ToList();
                return View("ViewCompanyAdmin", model);
            }
            else if (model.IsDashboardAdmin)
            {
                model.DashboardList = model.DashboardList.Where(x => x.IdDashboardAdmin == user.Id).ToList();

                return View("ViewUser", model);
            }
            else if (model.IsUser)
            {
                var isAllowedDashboard = user.Dashboards.SingleOrDefault(x => x.Id == id);
                if (isAllowedDashboard != null)
                {
                    return View("ViewUserReadOnly", model);
                }
                else {
                    return HttpNotFound();
                }
            }

            return RedirectToAction("index", "home", null);
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
            var viewModel = new DashboardIndexViewModel();
            viewModel.CurrentUser = new AppUser();
            viewModel.DashboardsList = new List<Dashboard>();

            var userEmail = HttpContext.User.Identity.Name;
            var user = db.AppUsers.SingleOrDefault(u => u.Email == userEmail);

            viewModel.IsCompanyAdmin = (user.Roles.Where(r => r.Name == "CompanyAdmin")).Count() == 1;
            viewModel.IsSuperAdmin = (user.Roles.Where(r => r.Name == "SuperAdmin")).Count() == 1;
            viewModel.IsUser = (user.Roles.Where(r => r.Name == "User")).Count() == 1;

            if (!viewModel.IsCompanyAdmin && !viewModel.IsSuperAdmin && !viewModel.IsUser)
                return View("~/Views/Shared/Error_403.cshtml");

            viewModel.CurrentUser = user;

            if (viewModel.IsUser)
            {
                viewModel.DashboardsList = db.Dashboards.Where(x => x.IdDashboardAdmin == user.Id).ToList();
            }
            else if (viewModel.IsCompanyAdmin) {
                viewModel.DashboardsList = db.Dashboards.Where(x => x.IdOwner == user.Id).ToList();
            }
            
            if(viewModel.IsSuperAdmin){
                viewModel.DashboardsList = db.Dashboards.ToList();
            }

            return View(viewModel);
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
            var model = new DashboardCreateViewModel();
            var usersList = db.AppUsers.ToList();

            List<AppUser> candidatsCompanyAdminList = new List<AppUser>();
            List<AppUser> candidatsDashboardAdminList = new List<AppUser>();

            foreach(var item in usersList){
                var role = item.Roles.ToList();
                if(role[0].Name == "CompanyAdmin"){
                    item.UserProfile = db.UserProfiles.SingleOrDefault(x => x.Id == item.Id);
                    candidatsCompanyAdminList.Add(item);
                }
                else if (role[0].Name == "User")
                {
                    item.UserProfile = db.UserProfiles.SingleOrDefault(x => x.Id == item.Id);
                    candidatsDashboardAdminList.Add(item);
                }
            }

            var companyCAList = candidatsCompanyAdminList.Select(r => new ListBoxItems() { Id = r.Id, Name = r.UserProfile.FirstName + " " + r.UserProfile.LastName }).ToList();
            var candidatsDAList = candidatsDashboardAdminList.Select(r => new ListBoxItems() { Id = r.Id, Name = r.UserProfile.FirstName + " " + r.UserProfile.LastName }).ToList();

            ViewBag.CandidatsCompanyAdmin = new SelectList(companyCAList, "Id", "Name", 0);
            ViewBag.CandidatsDashboardAdmin = new SelectList(candidatsDAList, "Id", "Name", 0);

            return View(model);
        }

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
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var viewModel = new DashboardEditViewModel();
            viewModel.CurrentUser = new AppUser();
            viewModel.Dashboard = new Dashboard();

            viewModel.Dashboard = db.Dashboards.SingleOrDefault(x => x.Id == id);

            if (viewModel.Dashboard == null)
                return HttpNotFound();

            var userEmail = HttpContext.User.Identity.Name;
            var user = db.AppUsers.SingleOrDefault(u => u.Email == userEmail);

            viewModel.IsCompanyAdmin = (user.Roles.Where(r => r.Name == "CompanyAdmin")).Count() == 1;
            viewModel.IsSuperAdmin = (user.Roles.Where(r => r.Name == "SuperAdmin")).Count() == 1;
            viewModel.IsUser = (user.Roles.Where(r => r.Name == "User")).Count() == 1;

            if (viewModel.IsUser && viewModel.Dashboard.IdDashboardAdmin != user.Id)
                return HttpNotFound();

            return View(viewModel);
        }

        // POST: Dashboards/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title")] DashboardEditViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(viewModel.Dashboard).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(viewModel);
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
