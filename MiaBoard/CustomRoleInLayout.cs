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
            tag.MergeAttribute("href", "/Users/Edit/" + user.Id);

            return new MvcHtmlString(tag.ToString());
        }
    }
}