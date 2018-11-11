
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Yuri.Notes.DB;
using Yuri.Notes.Web.Model;

namespace Yuri.Notes.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        IUserRepository UserRepository;

        public AccountController()
        {
            UserRepository = new NHUserRepository();
        }

        // GET: Account
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Что-то пошло не так! 😊");
                return View(model);
            }

            var user = UserRepository.LoadByName(model.Login);

            if (user == null || user.Password != model.Password)
            {
                ModelState.AddModelError("", "Неверный логин или пароль");
                return View(model);
            }

            FormsAuthentication.SetAuthCookie(user.Login, false);

            return RedirectToAction("Index", "Notes");
        }

        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }

        [AllowAnonymous]
        public ActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Registration(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = UserRepository.LoadByLogin(model.Login);

            if (user != null)
            {
                ModelState.AddModelError("", "Пользователь с таким логином уже зарегистрирован");
                return View(model);
            }

            UserRepository.UserRegistration(model.Login, model.Password);

            return RedirectToAction("Login", "Account");
        }
    }
}