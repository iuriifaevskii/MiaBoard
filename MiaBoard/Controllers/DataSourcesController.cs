using MiaBoard.Models;
using MiaBoard.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MiaBoard.Controllers
{
    [Authorize]
    public class DataSourcesController : Controller
    {
        //
        // GET: /DataSources/
        public ActionResult Index()
        {
            var model = new DataSourceIndexViewModel();
            
            model.CurrentUser = new AppUser();
            model.DataSources = new List<DataSource>();
            model.NewDataSource = new _DataSourcesCreateViewModel();
            model.UpdateDataSource = new DataSource();

            var userEmail = HttpContext.User.Identity.Name;
            var user = _context.AppUsers.SingleOrDefault(u => u.Email == userEmail);
            
            model.CurrentUser = user;
            model.NewDataSource.CurrentUser = user;

            model.IsSuperAdmin = (user.Roles.Where(r => r.Name == "SuperAdmin")).Count() == 1;
            model.IsCompanyAdmin = (user.Roles.Where(r => r.Name == "CompanyAdmin")).Count() == 1;

            if (!model.IsSuperAdmin && !model.IsCompanyAdmin)
                return HttpNotFound();

            var usersList = _context.AppUsers.ToList();

            List<AppUser> candidatsCompanyAdminList = new List<AppUser>();

            foreach (var item in usersList)
            {
                var role = item.Roles.ToList();
                if (role[0].Name == "CompanyAdmin")
                {
                    item.UserProfile = _context.UserProfiles.SingleOrDefault(x => x.Id == item.Id);
                    candidatsCompanyAdminList.Add(item);
                }
            }

            var companyCAList = candidatsCompanyAdminList.Select(r => new ListBoxItems() { Id = r.Id, Name = r.UserProfile.FirstName + " " + r.UserProfile.LastName }).ToList();

            ViewBag.CandidatsCompanyAdmin = new SelectList(companyCAList, "Id", "Name", 0);

            return View(model);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        //public ActionResult Save(DataSource dataSource)
        public ActionResult Save(_DataSourcesCreateViewModel _viewModel)
        {
            
            if(!ModelState.IsValid)
            {
                var model = new DataSourceIndexViewModel();

                model.CurrentUser = new AppUser();
                model.DataSources = new List<DataSource>();
                model.NewDataSource = new _DataSourcesCreateViewModel();
                model.UpdateDataSource = new DataSource();

                var userEmail = HttpContext.User.Identity.Name;
                var user = _context.AppUsers.SingleOrDefault(u => u.Email == userEmail);

                model.CurrentUser = user;
                model.NewDataSource.CurrentUser = user;

                model.IsSuperAdmin = (user.Roles.Where(r => r.Name == "SuperAdmin")).Count() == 1;
                model.IsCompanyAdmin = (user.Roles.Where(r => r.Name == "CompanyAdmin")).Count() == 1;

                if (!model.IsSuperAdmin && !model.IsCompanyAdmin)
                    return HttpNotFound();

                var usersList = _context.AppUsers.ToList();

                List<AppUser> candidatsCompanyAdminList = new List<AppUser>();

                foreach (var item in usersList)
                {
                    var role = item.Roles.ToList();
                    if (role[0].Name == "CompanyAdmin")
                    {
                        item.UserProfile = _context.UserProfiles.SingleOrDefault(x => x.Id == item.Id);
                        candidatsCompanyAdminList.Add(item);
                    }
                }

                var companyCAList = candidatsCompanyAdminList.Select(r => new ListBoxItems() { Id = r.Id, Name = r.UserProfile.FirstName + " " + r.UserProfile.LastName }).ToList();

                ViewBag.CandidatsCompanyAdmin = new SelectList(companyCAList, "Id", "Name", 0);

                return View("Index", model);
            }

            if (_viewModel.NewDataSource.Id == 0)
                _context.DataSources.Add(_viewModel.NewDataSource);
            else
            {
                var dataSourceInDb = _context.DataSources.Single(c => c.Id == _viewModel.NewDataSource.Id);
                dataSourceInDb.Type = _viewModel.NewDataSource.Type;
                dataSourceInDb.UserName = _viewModel.NewDataSource.UserName;
                dataSourceInDb.ServerName = _viewModel.NewDataSource.ServerName;
                dataSourceInDb.Password = _viewModel.NewDataSource.Password;
                dataSourceInDb.DatabaseName = _viewModel.NewDataSource.DatabaseName;
                dataSourceInDb.ConnectionString = _viewModel.NewDataSource.ConnectionString;
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

            return View("Edit", dataSource);
        }

	}
}