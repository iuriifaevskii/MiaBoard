using MiaBoard.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MiaBoard
{
    public static class CustomRoleInLayout
    {
        private static ApplicationDbContext _context = new ApplicationDbContext();

        public static AppUser FindUserByEmail(string email)
        {
            AppUser user = null;
            user = _context.AppUsers.AsQueryable().SingleOrDefault(u => u.Email == email);
            return user;
        }
        public static MvcHtmlString UserToRole(this HtmlHelper helper, string email)
        {
            var user = FindUserByEmail(email);
            TagBuilder tag = new TagBuilder("a");
            tag.SetInnerText(email);
            if (user != null)
            {
                if ((user.Roles.Where(r => r.Name == "SuperAdmin")).Count() == 1)
                {

                    tag.MergeAttribute("href", "/AppRoles/Index");
                    return new MvcHtmlString(tag.ToString());
                }
                else if ((user.Roles.Where(r => r.Name == "User")).Count() == 1)
                {
                    tag.MergeAttribute("href", "/Test/ViewUserReadOnly");
                    return new MvcHtmlString(tag.ToString());
                }
                else if ((user.Roles.Where(r => r.Name == "CompanyAdmin")).Count() == 1)
                {
                    tag.MergeAttribute("href", "/CompanyAdmin/CompanyAdmin");
                    return new MvcHtmlString(tag.ToString());
                }
                else if ((user.Roles.Where(r => r.Name == "UserDashletEditor")).Count() == 1)
                {
                    tag.MergeAttribute("href", "/UserDashletEditor/UserDashletEditor");
                    return new MvcHtmlString(tag.ToString());
                }
                else {
                    return new MvcHtmlString(tag.ToString());
                }
            }
            else
            {
                return new MvcHtmlString(tag.ToString());
            }



        }
    }
}