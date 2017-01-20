using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MiaBoard.Controllers
{
    public class TestController : Controller
    {
        //
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