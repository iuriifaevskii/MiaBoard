using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using MiaBoard.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Web.Security;
using MiaBoard.ViewModels;

namespace MiaBoard.Controllers
{
    //[Authorize]
    public class AccountController : Controller
    {
        private ApplicationDbContext _context;
        public AccountController()
        {
            _context = new ApplicationDbContext();
        }
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (LoginTrue(model.Login, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.Login, model.IsRememberMe);
                    var user = this.FindUserByEmail(model.Login);
                    if (user != null)
                    {
                        if((user.Roles.Where(r => r.Name == "SuperAdmin")).Count() == 1)
                        {
                            return RedirectToAction("Index", "AppRoles");
                        }
                        else if((user.Roles.Where(r => r.Name == "User")).Count() == 1)
                        {
                            return RedirectToAction("ViewUserReadOnly", "Test");
                        }
                        else if ((user.Roles.Where(r => r.Name == "CompanyAdmin")).Count() == 1)
                        {
                            return RedirectToAction("CompanyAdmin", "CompanyAdmin");
                        }
                        else if ((user.Roles.Where(r => r.Name == "UserDashletEditor")).Count() == 1)
                        {
                            return RedirectToAction("UserDashletEditor", "UserDashletEditor");
                        }

                    }

                    
                }
                else
                {
                    ModelState.AddModelError("", "Login and password incorrect.");
                }
            }
            return View(model);
        }
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }

        public bool LoginTrue(string email, string password)
        {
            AppUser user = FindUser(email, password);
            if (user != null)
            {
                return true;
            }
            else
                return false;
        }
        public AppUser FindUser(string email, string password)
        {
            var user = FindUserByEmail(email);
            if (user != null)
            {
                var crypto = new SimpleCrypto.PBKDF2();
                if (user.Password == crypto.Compute(password, user.PasswordSalt))
                    return user;
                else
                    return null;
            }
            return user;
        }
        public AppUser FindUserByEmail(string email)
        {
            AppUser user = null;
            user = _context.AppUsers.AsQueryable().SingleOrDefault(u => u.Email == email);
            return user;
        }
        public AppUser CreateUser(string email, string password)
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
        public AppUser CreateUser(string email, string password, string lastName, string name, string midleName, int gender, DateTime ? dateHired, string contactNo)
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
                    DateRegistration = DateTime.Now,
                    DateHired = dateHired,
                    ContactNo = contactNo,
                };
                _context.UserProfiles.Add(userProf);
                _context.SaveChanges(); ;
            }

            return user;
        }
        public ActionResult Register()
        {
            return View();
        }
       
        [HttpPost]
        public ActionResult Register(CreateUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = CreateUser(model.Email, model.Password, model.LastName, model.FirstName, model.MidleName, model.Gender, model.DateHired, model.ContactNo);
                if (user != null)
                {
                    AddRoleUser(user, 2);
                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    ModelState.AddModelError("", "user with the same Email already exists");
                }
                model.RoleId = 2;

            }
            return View(model);
        }
        public AppUser GetUserById(int id)
        {
            return _context.AppUsers.AsQueryable().Where(u => u.Id == id).SingleOrDefault();
        }
        public bool ChangePassword(int id, string oldPassword, string newPassword)
        {
            AppUser user = this.GetUserById(id);
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
        public void RemoveUserById(int id)
        {
            AppUser user = this.GetUserById(id);
            if (user != null)
            {
                _context.AppUsers.Remove(user);
                _context.SaveChanges();
            }
        }
        public void EditeUser(int id, string email)
        {
            AppUser user = this.GetUserById(id);
            if (user != null)
            {
                if (this.FindUserByEmail(email) == null)
                {
                    user.Email = email;
                    _context.SaveChanges();
                }
            }
        }

        public IQueryable<AppUser> GetAllUsers()
        {
            IQueryable<AppUser> listUsers = from data in _context.AppUsers.AsQueryable() select data;
            return listUsers;
        }
        public AppRole AddRole(string name)
        {
            AppRole role = new AppRole()
            { Name = name };
            _context.AppRoles.Add(role);
            _context.SaveChanges();
            return role;
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
        public IList<AppRole> GetAllRoles()
        {
            return _context.AppRoles.ToList();
        }
        public string[] GetRolesForUser(string email)
        {
            var user = this.FindUserByEmail(email);
            if (user != null)
            {
                return user.Roles.Select(r => r.Name).ToArray();
            }
            return null;
        }
        
    





    }
}