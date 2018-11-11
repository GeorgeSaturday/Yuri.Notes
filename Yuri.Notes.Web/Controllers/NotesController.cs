using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Yuri.Notes.Web.Controllers
{
    public class NotesController : Controller
    {
        // GET: Notes
        [Authorize(Roles = "admin, user")]
        public ActionResult Index()
        {
            return View();
        }
    }
}