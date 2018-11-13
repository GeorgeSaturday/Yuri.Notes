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
        static long NoteId { get; set; }

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

        //получить и показать все публичные заметки
        public ActionResult PublicNotes()
        {
            Author = UserRepository.FindIdByLogin(User.Identity.Name);

            var notesPublic = NoteRepository.GetPublicNotes(Author);

            return View(notesPublic);
        }

        //получить и показать мои заметки
        [HttpGet]
        public ActionResult SavedNotes()
        {
            Author = UserRepository.FindIdByLogin(User.Identity.Name);

            var myNotes = NoteRepository.GetMyNotes(Author);

            return View(myNotes);
        }

        //Вью редактирования
        [HttpGet]
        public ActionResult EditNote(Note model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            NoteId = model.Id;

            return View(model);
        }

        [HttpGet]
        public ActionResult CreateNote(Note model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            NoteId = model.Id;

            return View(model);
        }

        //сохранить заметку
        [HttpPost]
        public PartialViewResult SaveNote(Note model)
        {
            model.Id = NoteId;
            model.Author = new User() { Id = Author };

            NoteRepository.Save(model);

            return PartialView();

        }

        //[HttpPost]
        //public PartialViewResult DeleteNote(Note model)
        //{
        //    model.Id = NoteId;
        //    model.Author = new User() { Id = Author };

        //    NoteRepository.DeleteNote(model);
        //    NoteRepository.Delete
        //    return PartialView();
        //}

    }
}