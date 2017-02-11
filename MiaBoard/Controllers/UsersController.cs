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
                                                      Gender = data.UserProfile.Gender,
                                                      DateRegistration = data.UserProfile.DateRegistration,
                                                      DateHired = data.UserProfile.DateHired,
                                                      ContactNo = data.UserProfile.ContactNo,
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

        public AppUser GetUserById(int id)
        {
            return _context.AppUsers.AsQueryable().Where(u => u.Id == id).SingleOrDefault();
        }
        public void EditRoleUser(AppUser user, int RoleId)
        {
            AppRole role = _context.AppRoles.SingleOrDefault(r => r.Id == RoleId);
            
            if (role != null)
            {
                role.AppUsers.Remove(user);
                role.AppUsers.Add(user);
                _context.SaveChanges();
            }
        }
        public void EditeUser(int id, string email,string firstName,string lastName,string midleName, int gender, DateTime dateHired, string contactNo , int RoleId)
        {
            AppUser user = GetUserById(id);
            if (user != null)
            {
                if (this.FindUserByEmail(email) == null)
                {
                    user.Email = email;
                    user.UserProfile.FirstName = firstName;
                    user.UserProfile.LastName = lastName;
                    user.UserProfile.MidleName = midleName;
                    user.UserProfile.Gender = gender;
                    user.UserProfile.DateHired = dateHired;
                    user.UserProfile.ContactNo = contactNo;
                    EditRoleUser(user, RoleId);
                    _context.SaveChanges();
                }
            }
        }
        public bool ChangePassword(int id, string oldPassword, string newPassword)
        {
            AppUser user = GetUserById(id);
            if (user != null)
            {
                if (user.Password == oldPassword)
                {
                    user.Password = newPassword;
                    _context.SaveChanges();
                    return true;
                }
            }
            return false;
        }
        public ActionResult Edit(int id)
        {
            AppUser user = GetUserById(id);
            UserEditViewModel model = new UserEditViewModel()
            {
                ID = user.Id,
                Email = user.Email,
                Name = user.UserProfile.FirstName,
                MidleName = user.UserProfile.MidleName,
                LastName = user.UserProfile.LastName,
                Gender = user.UserProfile.Gender,
                ContactNo = user.UserProfile.ContactNo,
                DateHired = user.UserProfile.DateHired,
                
            };
            var listRoles = GetAllRoles().Select(r => new ListBoxItems() { Id = r.Id, Name = r.Name }).ToList();
            ViewBag.ListinRoles = new SelectList(listRoles, "Id", "Name", model.RoleId);
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(UserEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                EditeUser(model.ID, model.Email, model.Name, model.LastName, model.MidleName, model.Gender, model.DateHired, model.ContactNo, model.RoleId);   
                ChangePassword(model.ID, model.OldPassword, model.Password);
            }
            return RedirectToAction("Index", "Users");

        }
        [Authorize]
        public ActionResult Delete(int id)
        {
            AppUser user = GetUserById(id);
            UserInfoViewModel model = new UserInfoViewModel()
            {
                ID = user.Id,
                Email = user.Email
            };
            return View(model);
        }

        public void RemoveUserById(int id)
        {
            AppUser user = GetUserById(id);
            if (user != null)
            {
                var userProfile = _context.UserProfiles.SingleOrDefault(x => x.Id == id);
                _context.UserProfiles.Remove(userProfile);
                _context.AppUsers.Remove(user);
                _context.SaveChanges();
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult Delete(UserInfoViewModel model)
        {
            RemoveUserById(model.ID);
            return RedirectToAction("Index", "Users");
        }
    
        public ActionResult AddUserToDashboardView()
        {
            return View("AddUserToDashboard");
        }

        //[Authorize(Roles = "Admin")]
        public ActionResult AddUserToDashboard()// GET: Users
        {
            AddUserToDashboardViewModel model = new AddUserToDashboardViewModel();
            //Заповнити всі пости
            model.ListDashboards = _context.Dashboards.Select(p => new ListBoxItems() { Id = p.Id, Name = p.Title });
            //Заповнити всі юзери
            model.ListUsers = _context.AppUsers.Select(p => new ListBoxItems() { Id = p.Id, Name = p.UserProfile.LastName + " " + p.UserProfile.FirstName });
            return View(model);
        }
        [HttpPost]
        public ActionResult AddUserToDashboard(AddUserToDashboardViewModel model)// GET: Users
        {
            if (ModelState.IsValid)
            {
                Dashboard findDashboard = _context.Dashboards.SingleOrDefault(p => p.Id == model.DashboardIdSected);
                if (findDashboard != null)
                {
                    AppUser findUser = _context.AppUsers.SingleOrDefault(u => u.Id == model.UserIdSected);
                    if (findUser != null)
                    {
                        findDashboard.AppUsers.Add(findUser);
                        _context.SaveChanges();
                    }
                }
            }
            //Заповнити всі пости
            model.ListDashboards = _context.Dashboards.Select(p => new ListBoxItems() { Id = p.Id, Name = p.Title });
            //Заповнити всі юзери
            model.ListUsers = _context.AppUsers.Select(p => new ListBoxItems() { Id = p.Id, Name = p.UserProfile.LastName + " " + p.UserProfile.FirstName });
            return View(model);
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