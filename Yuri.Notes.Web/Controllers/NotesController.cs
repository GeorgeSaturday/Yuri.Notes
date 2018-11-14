using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;
using Yuri.Notes.DB;

namespace Yuri.Notes.Web.Controllers
{
    // GET: Notes
    [Authorize(Roles = "admin, user")]
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

        //View редактирования заметки
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

        //View создания заметки
        [HttpGet]
        public ActionResult CreateNote()
        {
           
            return View();
        }

        //сохранить заметку
        [HttpPost]
        public ActionResult SaveNote(Note model, HttpPostedFileBase download)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("а", "Введите название заметки");
                return RedirectToAction("CreateNote", model);
            }

            if (download != null)
            {
                string fileName = System.IO.Path.GetFileName(download.FileName);
                model.BinaryFile = fileName;

                download.SaveAs(Server.MapPath("~/Downloads/" + fileName));
            }

            model.Author = new User() { Id = Author };

            NoteRepository.Save(model);

            return RedirectToAction("SavedNotes", "Notes");
            //return PartialView();

        }

        public ActionResult DownloadMyFile(string fileName)
        {
            if (fileName != null)
            {
                string file_path = Server.MapPath("~/Downloads/" + fileName);
                return File(file_path, MediaTypeNames.Application.Octet, fileName);
            }

            return RedirectToAction("SavedNotes");
        }

        //уничтожить заметку
        public ActionResult EraseNote(long id)
        {
            NoteRepository.Delete(id);

            return RedirectToAction("SavedNotes");

        }


    }
}