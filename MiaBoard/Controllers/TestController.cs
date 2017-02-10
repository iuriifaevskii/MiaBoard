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
    public class TestController : Controller
    {
        private ApplicationDbContext db = null;

        public TestController() {
                db = new ApplicationDbContext();
        }

        public ActionResult ViewUser(int id)
        {
            if (id == 0)
                return HttpNotFound();

            var model = new DashboardViewViewUserViewModel();

            model.Dashlets = new List<Dashlet>();
            model.DataSources = new List<DataSource>();

            model.DashletsSqlResult = new Dictionary<int, string>();

            model.DashletsFirstCol = new List<Dashlet>();
            model.DashletsSecondCol = new List<Dashlet>();
            model.DashletsThirdCol = new List<Dashlet>();

            model.DataSources = db.DataSources.ToList();
            model.DashboardList = db.Dashboards.ToList();
            model.Dashboard = db.Dashboards.SingleOrDefault(d => d.Id == id);
            //model.Dashlets = db.Dashlets.Where(lam => lam.DashboardId == id ).OrderBy(lam => lam.Position).ToList();
            model.Dashlets = db.Dashlets.Include(m => m.DataSource).Where(d => d.DashboardId == id).OrderBy(d => d.Position).ToList();


            foreach (var dashlet in model.Dashlets)
            {
                //switch (dashlet.DataSource.Type)
                //{
                //    case "MSSQL":
                //        try
                //        {
                //            using (SqlConnection connection = new SqlConnection())
                //            {
                //                connection.ConnectionString = dashlet.DataSource.ConnectionString;
                //                connection.Open();

                //                SqlCommand command = new SqlCommand(dashlet.Sql, connection);

                //                using (SqlDataReader reader = command.ExecuteReader())
                //                {
                //                    if (reader.Read())
                //                    {
                //                        viewDashboardViewModel.DashletsSqlResult.Add(dashlet.Id, reader[0].ToString());
                //                    }
                //                }

                //                connection.Close();
                //            }
                //        }
                //        catch
                //        {
                //            viewDashboardViewModel.DashletsSqlResult.Add(dashlet.Id,"SQL queary is incorrect!");
                //            //continue;
                //            //return Content("Invalid Connection to Database in Dashhlet: " + dashlet.Id);
                //        }
                //        break;
                //    default:
                //        viewDashboardViewModel.DashletsSqlResult.Add(dashlet.Id, "Type of Database is unknown!");
                //        //continue;
                //        //return Content("Invalid Type of Database in Dashhlet: " + dashlet.Id);
                //        break;
                //}

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

            return View("~/Views/Dashboards/ViewUser.cshtml", model);
        }

        public AppUser FindUserByEmail(string email)
        {
            AppUser user = null;
            user = db.AppUsers.AsQueryable().SingleOrDefault(u => u.Email == email);
            return user;
        }

        [Authorize]
        public ActionResult ViewUserReadOnly(int id)
        {
            if (id == 0)
                return HttpNotFound();
            var userEmail = HttpContext.User.Identity.Name;
            var user = db.AppUsers.SingleOrDefault(u => u.Email == userEmail);
            var model = new DashboardViewViewUserReadOnlyViewModel();

            model.Dashlets = new List<Dashlet>();
            model.DataSources = new List<DataSource>();

            model.DashletsSqlResult = new Dictionary<int, string>();

            model.DashletsFirstCol = new List<Dashlet>();
            model.DashletsSecondCol = new List<Dashlet>();
            model.DashletsThirdCol = new List<Dashlet>();

            model.DataSources = db.DataSources.ToList();

            model.ID = user.Id;
            model.Email = user.Email;
            model.DashboardListToUser = user.Dashboards.Select( p=> new DashboardItemViewModel() { DashboardId = p.Id, DashboardName = p.Title }).ToList();
            model.Dashboard = db.Dashboards.SingleOrDefault(d => d.Id == id);
            //model.Dashlets = db.Dashlets.Where(lam => lam.DashboardId == id ).OrderBy(lam => lam.Position).ToList();
            model.Dashlets = db.Dashlets.Include(m => m.DataSource).Where(d => d.DashboardId == id).OrderBy(d => d.Position).ToList();


            foreach (var dashlet in model.Dashlets)
            {
                //switch (dashlet.DataSource.Type)
                //{
                //    case "MSSQL":
                //        try
                //        {
                //            using (SqlConnection connection = new SqlConnection())
                //            {
                //                connection.ConnectionString = dashlet.DataSource.ConnectionString;
                //                connection.Open();

                //                SqlCommand command = new SqlCommand(dashlet.Sql, connection);

                //                using (SqlDataReader reader = command.ExecuteReader())
                //                {
                //                    if (reader.Read())
                //                    {
                //                        viewDashboardViewModel.DashletsSqlResult.Add(dashlet.Id, reader[0].ToString());
                //                    }
                //                }

                //                connection.Close();
                //            }
                //        }
                //        catch
                //        {
                //            viewDashboardViewModel.DashletsSqlResult.Add(dashlet.Id,"SQL queary is incorrect!");
                //            //continue;
                //            //return Content("Invalid Connection to Database in Dashhlet: " + dashlet.Id);
                //        }
                //        break;
                //    default:
                //        viewDashboardViewModel.DashletsSqlResult.Add(dashlet.Id, "Type of Database is unknown!");
                //        //continue;
                //        //return Content("Invalid Type of Database in Dashhlet: " + dashlet.Id);
                //        break;
                //}

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

            return View("~/Views/Dashboards/ViewUserReadOnly.cshtml", model);
        }
        
        public ActionResult ViewCompanyAdmin(int id)
        {
            if (id == 0)
                return HttpNotFound();

            var model = new DashboardViewViewCompanyAdminViewModel();

            model.Dashlets = new List<Dashlet>();
            model.DataSources = new List<DataSource>();

            model.DashletsSqlResult = new Dictionary<int, string>();

            model.DashletsFirstCol = new List<Dashlet>();
            model.DashletsSecondCol = new List<Dashlet>();
            model.DashletsThirdCol = new List<Dashlet>();

            model.DataSources = db.DataSources.ToList();
            model.DashboardList = db.Dashboards.ToList();
            model.Dashboard = db.Dashboards.SingleOrDefault(d => d.Id == id);
            //model.Dashlets = db.Dashlets.Where(lam => lam.DashboardId == id ).OrderBy(lam => lam.Position).ToList();
            model.Dashlets = db.Dashlets.Include(m => m.DataSource).Where(d => d.DashboardId == id).OrderBy(d => d.Position).ToList();


            foreach (var dashlet in model.Dashlets)
            {
                //switch (dashlet.DataSource.Type)
                //{
                //    case "MSSQL":
                //        try
                //        {
                //            using (SqlConnection connection = new SqlConnection())
                //            {
                //                connection.ConnectionString = dashlet.DataSource.ConnectionString;
                //                connection.Open();

                //                SqlCommand command = new SqlCommand(dashlet.Sql, connection);

                //                using (SqlDataReader reader = command.ExecuteReader())
                //                {
                //                    if (reader.Read())
                //                    {
                //                        viewDashboardViewModel.DashletsSqlResult.Add(dashlet.Id, reader[0].ToString());
                //                    }
                //                }

                //                connection.Close();
                //            }
                //        }
                //        catch
                //        {
                //            viewDashboardViewModel.DashletsSqlResult.Add(dashlet.Id,"SQL queary is incorrect!");
                //            //continue;
                //            //return Content("Invalid Connection to Database in Dashhlet: " + dashlet.Id);
                //        }
                //        break;
                //    default:
                //        viewDashboardViewModel.DashletsSqlResult.Add(dashlet.Id, "Type of Database is unknown!");
                //        //continue;
                //        //return Content("Invalid Type of Database in Dashhlet: " + dashlet.Id);
                //        break;
                //}

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

            return View("~/Views/Dashboards/ViewCompanyAdmin.cshtml", model);
        }

        
        // GET: /Test/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SuperAdmin()
        {
            return View();
        }

        public ActionResult CompanyAdmin()
        {
            return View();
        }

        public ActionResult User()
        {
            return View();
        }
    }
}