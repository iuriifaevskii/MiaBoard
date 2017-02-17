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

            var viewModel = new UserIndexViewModel();

            var userEmail = HttpContext.User.Identity.Name;
            var user = _context.AppUsers.SingleOrDefault(u => u.Email == userEmail);

            viewModel.IsCompanyAdmin = (user.Roles.Where(r => r.Name == "CompanyAdmin")).Count() == 1;
            viewModel.IsSuperAdmin = (user.Roles.Where(r => r.Name == "SuperAdmin")).Count() == 1;
            viewModel.IsUser = (user.Roles.Where(r => r.Name == "User")).Count() == 1;
            
            viewModel.CurrentUser = user;

            if (viewModel.IsUser)
            {
                viewModel.UsersList = _context.AppUsers.Where(x => x.Id == user.Id).ToList();
            }
            if (viewModel.IsCompanyAdmin)
            {
                viewModel.UsersList = _context.AppUsers.ToList();
            }
            if (viewModel.IsSuperAdmin)
            {
                viewModel.UsersList = _context.AppUsers.ToList();
            }

            return View(viewModel);
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
        public AppUser CreateUser(string email, string password, string lastName, string name, string midleName, int gender,DateTime ? dateRegistration,DateTime ? dateHired,string contactNo)
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

        public void EditeUser(int id, string email, string firstName, string lastName, string midleName, int gender, DateTime? dateHired, string contactNo, int RoleId)
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
            var user = _context.AppUsers.SingleOrDefault(x => x.Id == id);
            var userEmail = HttpContext.User.Identity.Name;
            var currentUser = _context.AppUsers.SingleOrDefault(u => u.Email == userEmail);

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
                RoleId = user.Roles.ToList()[0].Id
            };

            model.IsCompanyAdmin = (currentUser.Roles.Where(r => r.Name == "CompanyAdmin")).Count() == 1;
            model.IsSuperAdmin = (currentUser.Roles.Where(r => r.Name == "SuperAdmin")).Count() == 1;
            model.IsUser = (currentUser.Roles.Where(r => r.Name == "User")).Count() == 1;

            var listRoles = GetAllRoles().Select(r => new ListBoxItems() { Id = r.Id, Name = r.Name }).ToList();
            ViewBag.ListinRoles = new SelectList(listRoles, "Id", "Name", model.RoleId);

            if (model.IsUser)
            {
                if (id == currentUser.Id)
                    return View("~/Views/Users/EditUser.cshtml", model);
                else
                    return View("~/Views/Shared/Errors/Error_403.cshtml");
            }

            if (model.IsCompanyAdmin)
            {
                if (id == currentUser.Id)
                    return View("~/Views/Users/EditUser.cshtml", model);
                else
                    return View("~/Views/Shared/Errors/Error_403.cshtml");
            }

            if (model.IsUser)
                return View("~/Views/Users/EditUser.cshtml", model);
            else
                return View(model);
        }

        [HttpPost]
        public ActionResult Edit(UserEditViewModel model)
        {
            var userEmail = HttpContext.User.Identity.Name;
            var currentUser = _context.AppUsers.SingleOrDefault(u => u.Email == userEmail);

            model.IsCompanyAdmin = (currentUser.Roles.Where(r => r.Name == "CompanyAdmin")).Count() == 1;
            model.IsSuperAdmin = (currentUser.Roles.Where(r => r.Name == "SuperAdmin")).Count() == 1;
            model.IsUser = (currentUser.Roles.Where(r => r.Name == "User")).Count() == 1;

            if (ModelState.IsValid)
            {
                var userInDb = _context.AppUsers.SingleOrDefault(x => x.Id == model.ID);
                var userNewRole = new AppRole();
                var userRoles = userInDb.Roles.ToList();
                var userOldRole = userRoles[0];
                userNewRole = _context.AppRoles.SingleOrDefault(x => x.Id == model.RoleId);

                if (!String.IsNullOrEmpty(model.Name))
                    userInDb.UserProfile.FirstName = model.Name;
                
                if (!String.IsNullOrEmpty(model.MidleName))
                    userInDb.UserProfile.MidleName = model.MidleName;
                
                if (!String.IsNullOrEmpty(model.LastName))
                    userInDb.UserProfile.LastName = model.LastName;
                
                if (!String.IsNullOrEmpty(model.DateHired.ToString()))
                    userInDb.UserProfile.DateHired = model.DateHired;

                if (!String.IsNullOrEmpty(model.ContactNo.ToString()))
                    userInDb.UserProfile.ContactNo = model.ContactNo;

                userInDb.Roles.Remove(userOldRole);
                userInDb.Roles.Add(userNewRole);
                userInDb.UserProfile.Gender = model.Gender;

                if (!String.IsNullOrEmpty(model.OldPassword) &&
                    !String.IsNullOrEmpty(model.Password) &&
                    !String.IsNullOrEmpty(model.ConfirmPassword))
                {
                    if (userInDb.Password == model.OldPassword)
                    {
                        userInDb.Password = model.Password;
                    }
                }

                try
                {
                    _context.SaveChanges();
                } catch(Exception e){
                    model.ExceptionMessage = e.Message;
                }

            }

            var listRoles = GetAllRoles().Select(r => new ListBoxItems() { Id = r.Id, Name = r.Name }).ToList();
            ViewBag.ListinRoles = new SelectList(listRoles, "Id", "Name", model.RoleId);

            if (model.IsUser)
                return View("~/Views/Users/EditUser.cshtml", model);
            else
                return View(model);
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
        public ActionResult AddUserToDashboard()
        {
            var model = new AddUserToDashboardViewModel();
            var userEmail = HttpContext.User.Identity.Name;
            var user = _context.AppUsers.SingleOrDefault(u => u.Email == userEmail);

            model.IsCompanyAdmin = (user.Roles.Where(r => r.Name == "CompanyAdmin")).Count() == 1;
            model.IsSuperAdmin = (user.Roles.Where(r => r.Name == "SuperAdmin")).Count() == 1;
            model.IsUser = (user.Roles.Where(r => r.Name == "User")).Count() == 1;

            if (model.IsUser)
                return View("~/Views/Shared/Errors/Error_403.cshtml");

            var usersList = _context.AppUsers.ToList();

            List<AppUser> candidatsReadOnly= new List<AppUser>();

            foreach (var item in usersList)
            {
                var role = item.Roles.ToList();
                if (role[0].Name == "User")
                {
                    item.UserProfile = _context.UserProfiles.SingleOrDefault(x => x.Id == item.Id);
                    candidatsReadOnly.Add(item);
                }
            }

            var companyROList = candidatsReadOnly.Select(r => new ListBoxItems() { Id = r.Id, Name = r.UserProfile.FirstName + " " + r.UserProfile.LastName });

            model.ListDashboards = model.IsSuperAdmin 
                ? _context.Dashboards.Select(p => new ListBoxItems() { Id = p.Id, Name = p.Title })
                : _context.Dashboards.Where(x => x.IdOwner == user.Id).Select(p => new ListBoxItems() { Id = p.Id, Name = p.Title });

            model.ListUsers = companyROList;
            
            return View(model);
        }

        [HttpPost]
        public ActionResult AddUserToDashboard(AddUserToDashboardViewModel model)
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

        public ActionResult AddUserToRoleView()
        {
            return View("AddUserToRole");
        }
        //[Authorize(Roles = "Admin")]
        public ActionResult AddUserToRole()// GET: Users
        {
            AddUserToRoleViewModel model = new AddUserToRoleViewModel();
            //Заповнити всі пости
            model.ListRoles = _context.AppRoles.Select(p => new ListBoxItems() { Id = p.Id, Name = p.Name });
            //Заповнити всі юзери
            model.ListUsers = _context.AppUsers.Select(p => new ListBoxItems() { Id = p.Id, Name = p.UserProfile.LastName + " " + p.UserProfile.FirstName });
            return View(model);
        }
        [HttpPost]
        public ActionResult AddUserToRole(AddUserToRoleViewModel model)// GET: Users
        {
            if (ModelState.IsValid)
            {
                AppRole findRole = _context.AppRoles.SingleOrDefault(p => p.Id == model.RoleIdSected);
                if (findRole != null)
                {
                    AppUser findUser = _context.AppUsers.SingleOrDefault(u => u.Id == model.UserIdSected);
                    if (findUser != null)
                    {
                        if (findUser.Roles.Count() >= 1)
                        {
                            
                            Response.Write("<script>alert('User will not have a few roles!');</script>");

                        }
                        else
                        {
                            findRole.AppUsers.Add(findUser);
                            _context.SaveChanges();
                        }
                    }
                }
            }
            //Заповнити всі пости
            model.ListRoles = _context.AppRoles.Select(p => new ListBoxItems() { Id = p.Id, Name = p.Name });
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