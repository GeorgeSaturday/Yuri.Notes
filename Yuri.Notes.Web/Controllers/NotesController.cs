using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Yuri.Notes.DB;

namespace Yuri.Notes.Web.Controllers
{
    public class NotesController : Controller
    {

        INoteRepository NoteRepository;
        IUserRepository UserRepository;

        static long Author { get; set; }

        public NotesController()
        {
            NoteRepository = new NHNoteRepository();
            UserRepository = new NHUserRepository();
        }


        // GET: Notes
        [Authorize(Roles = "admin, user")]
        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public ActionResult SavedNotes()
        {
            Author = UserRepository.FindIdByLogin(User.Identity.Name);

            var myNotes = NoteRepository.GetMyNotes(Author);

            return View(myNotes);
        }
    }
}