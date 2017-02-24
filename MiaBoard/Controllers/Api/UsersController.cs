using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using MiaBoard.Models;

namespace MiaBoard.Controllers.Api
{
    public class CreateUser  {
        public string Email { get; set; }
        public string RoleName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string MidleName { get; set; }
        public string LastName  { get; set; }
        public int Gender { get; set; }
        public string ContactNo { get; set; }
        public long DateHired { get; set; }

        public DateTime? DateRegistration { get; set; }
    }

    public class UsersController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Users
        public IQueryable<AppUser> GetAppUsers()
        {
            return db.AppUsers;
        }

        // GET: api/Users/5
        [ResponseType(typeof(AppUser))]
        public IHttpActionResult GetAppUser(int id)
        {
            AppUser appUser = db.AppUsers.Find(id);
            if (appUser == null)
            {
                return NotFound();
            }

            return Ok(appUser);
        }

        // PUT: api/Users/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAppUser(int id, AppUser appUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != appUser.Id)
            {
                return BadRequest();
            }

            db.Entry(appUser).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AppUserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        public AppUser UserRegistration(string email, string password)
        {
            AppUser user = null;
            if (db.AppUsers.SingleOrDefault(u => u.Email == email) == null)
            {
                user = new AppUser() { Email = email, Password = password };
                var crypto = new SimpleCrypto.PBKDF2();
                user.Password = crypto.Compute(user.Password);
                user.PasswordSalt = crypto.Salt;
                db.AppUsers.Add(user);
                db.SaveChanges();
            }
            return user;
        }
        public AppUser CreateUser(string email, string password, string lastName, string name, string midleName, int gender, DateTime? dateRegistration, long dateHired, string contactNo)
        {
            DateTime _dateHired = new DateTime();

            if (dateHired != 0)
            {
                _dateHired = new DateTime(dateHired); 
            }

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
                    DateHired = _dateHired,
                    ContactNo = contactNo,
                };
                db.UserProfiles.Add(userProf);
                db.SaveChanges(); ;
            }

            return user;
        }
        public void AddRoleUser(AppUser user, int RoleId)
        {
            AppRole role = db.AppRoles.SingleOrDefault(r => r.Id == RoleId);
            if (role != null)
            {
                role.AppUsers.Add(user);
                db.SaveChanges();
            }
        }

        // POST: api/Users
        [ResponseType(typeof(AppUser))]
        public IHttpActionResult PostAppUser(CreateUser userInfo)
        {
            userInfo.DateRegistration = DateTime.Now;
           
            var user = CreateUser(userInfo.Email, userInfo.Password, userInfo.LastName, userInfo.FirstName, userInfo.MidleName, userInfo.Gender, userInfo.DateRegistration, userInfo.DateHired, userInfo.ContactNo);
            
            if (user != null)
            {
                AddRoleUser(user, db.AppRoles.SingleOrDefault(x => x.Name == userInfo.RoleName).Id);
            }

            return Ok(user.Id);
        }

        // DELETE: api/Users/5
        [ResponseType(typeof(AppUser))]
        public IHttpActionResult DeleteAppUser(int id)
        {
            AppUser appUser = db.AppUsers.Find(id);
            if (appUser == null)
            {
                return NotFound();
            }

            db.AppUsers.Remove(appUser);
            db.SaveChanges();

            return Ok(appUser);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AppUserExists(int id)
        {
            return db.AppUsers.Count(e => e.Id == id) > 0;
        }
    }
}