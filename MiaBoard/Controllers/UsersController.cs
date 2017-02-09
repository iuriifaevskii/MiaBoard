using MiaBoard.Models;
using MiaBoard.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MiaBoard.Controllers
{
    public class UsersController : Controller
    {
        private ApplicationDbContext _context;
        public UsersController()
        {
            _context = new ApplicationDbContext();
        }
        // GET: Users
        public ActionResult Index()
        {
            IQueryable<UserViewModel> listUsers = from data in GetAllUsers()
                                                  select new UserViewModel()
                                                  {
                                                      ID = data.Id,
                                                      Email = data.Email,
                                                      FullName = data.UserProfile != null ? data.UserProfile.LastName + " " + data.UserProfile.FirstName + " " + data.UserProfile.MidleName : "",
                                                      Roles = data.Roles.Select(r => new RoleViewModel() { Id = r.Id, Name = r.Name, })
                                                  };

            return View(listUsers);
        }

        public ActionResult Create()
        {
            CreateUserViewModel model = new CreateUserViewModel();
            model.RoleId = 1;
            var listRoles = GetAllRoles().Select(r => new ListBoxItems() { Id = r.Id, Name = r.Name }).ToList();
            //listRoles.Insert(0, new ListBoxItems() { Id = 0, Name = "" });
            ViewBag.LisRoles = new SelectList(listRoles, "Id", "Name", model.RoleId);
            return View();
        }
        [HttpPost]
        public ActionResult Create(CreateUserViewModel model)
        {
            if(ModelState.IsValid)
            {
                var user = CreateUser(model.Email, model.Password, model.LastName, model.FirstName, model.MidleName, model.Gender, model.DateRegistration, model.DateHired, model.ContactNo);
                if(user != null)
                {
                    AddRoleUser(user, model.RoleId);
                    return RedirectToAction("Index", "Users");
                }
                else
                {
                    ModelState.AddModelError("", "user with the same Email already exists");
                }
                var listRoles = GetAllRoles().Select(r => new ListBoxItems() { Id = r.Id, Name = r.Name }).ToList();
                ViewBag.LisRoles = new SelectList(listRoles, "Id", "Name", model.RoleId);

            }
            return View(model);
        }

        public AppUser FindUserByEmail(string email)
        {
            AppUser user = null;
            user = _context.AppUsers.AsQueryable().SingleOrDefault(u => u.Email == email);
            return user;
        }

        public AppUser UserRegistration(string email, string password)
        {
            AppUser user = null;
            if (this.FindUserByEmail(email) == null)
            {
                user = new AppUser() { Email = email, Password = password };
                var crypto = new SimpleCrypto.PBKDF2();
                user.Password = crypto.Compute(user.Password);
                user.PasswordSalt = crypto.Salt;
                _context.AppUsers.Add(user);
                _context.SaveChanges();
            }
            return user;
        }
        public AppUser CreateUser(string email, string password, string lastName, string name, string midleName, int gender,DateTime dateRegistration,DateTime dateHired,string contactNo)
        {

            AppUser user = UserRegistration(email, password);
            if (user != null)
            {
                UserProfile userProf = new UserProfile()
                {
                    Id = user.Id,
                    //User = user,
                    FirstName = name,
                    MidleName = midleName,
                    LastName = lastName,
                    Gender = gender,
                    DateRegistration = dateRegistration,
                    DateHired = dateHired,
                    ContactNo = contactNo,
                };
                _context.UserProfiles.Add(userProf);
                _context.SaveChanges(); ;
            }

            return user;
        }
        public void AddRoleUser(AppUser user, int RoleId)
        {
            AppRole role = _context.AppRoles.SingleOrDefault(r => r.Id == RoleId);
            if (role != null)
            {
                role.AppUsers.Add(user);
                _context.SaveChanges();
            }
        }









        public IQueryable<AppUser> GetAllUsers()
        {
            IQueryable<AppUser> listUsers = from data in _context.AppUsers.AsQueryable() select data;
            return listUsers;
        }
        public IList<AppRole> GetAllRoles()
        {
            return _context.AppRoles.ToList();
        }

    }
}