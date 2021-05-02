using MoviesManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MoviesManager.Controllers
{
    public class UsersController : Controller
    {
        private DBEntities DB = new DBEntities();

        private DateTime OnLineUsersLastUpdate
        {
            get
            {
                if (Session["OnLineUsersUpdate"] == null)
                    Session["OnLineUsersUpdate"] = new DateTime(0);
                return (DateTime)Session["OnLineUsersUpdate"];
            }
            set
            {
                Session["OnLineUsersUpdate"] = value;
            }
        }

        protected override void Dispose(bool disposing)
        {
            DB.Dispose();
            base.Dispose(disposing);
        }

        [UserAccess]
        public ActionResult Index()
        {
            OnLineUsersLastUpdate = new DateTime(0); // forcer le rafraichissement
            ViewBag.UserFullName = OnlineUsers.CurrentUser.FullName;
            return View();
        }

        public ActionResult Subscribe()
        {
            return View(new UserView());
        }
        [HttpPost]
        public ActionResult Subscribe(UserView userView)
        {
            if (ModelState.IsValid)
            {
                if (DB.UserNameExist(userView.UserName))
                {
                    ModelState.AddModelError("UserName", "Ce nom d'usager existe déjà.");
                    return View(userView);
                }
                userView.Admin = false;
                userView.Password = userView.NewPassword;
                DB.AddUser(userView);
                return RedirectToAction("Login");
            }
            return View(userView);
        }
        [HttpPost]
        public JsonResult UserNameAvailable(string Username, int Id = 0)
        {
            return Json(!DB.UserNameExist(Username, Id));
        }
        [HttpPost]
        public JsonResult UserNameExist(string Username)
        {
            return Json(DB.UserNameExist(Username));
        }
        [HttpPost]
        public JsonResult PasswordValid(string Username, string Password)
        {
            return Json(DB.PasswordValid(Username, Password));
        }
        public ActionResult Login()
        {
          
            return View();
        }
        [HttpPost]
        public ActionResult Login(LoginView loginView)
        {
            if (ModelState.IsValid)
            {
                User userFound = DB.FindByUserName(loginView.UserName);
                if (userFound != null)
                {
                    if (userFound.Password != loginView.Password)
                    {
                        ModelState.AddModelError("Password", "Mot de passe incorrect");
                        return View(loginView);
                    }
                }
                else
                {
                    ModelState.AddModelError("UserName", "Ce non d'usager n'existe pas");
                    return View(loginView);
                }
                OnlineUsers.AddSessionUser(userFound.ToUserView());

            }
            // todo
            return RedirectToAction("Index", "Actors");
        }

        public ActionResult Logout()
        {
            OnlineUsers.RemoveSessionUser();
            return RedirectToAction("Login");
        }

        [UserAccess]
        public ActionResult Profil()
        {
            UserView userView = OnlineUsers.CurrentUser;

            ViewBag.PasswordNotChangedToken = Guid.NewGuid().ToString().Substring(0,8);
            return View(userView);
        }
        [HttpPost]
        public ActionResult Profil(UserView userview)
        {
            if (ModelState.IsValid)
            {
                userview.Id = OnlineUsers.CurrentUser.Id;
                userview.Admin = OnlineUsers.CurrentUser.Admin;
                User user = DB.Users.Find(userview.Id);
                string PasswordNotChangedToken = (string)Request["PasswordNotChangedToken"];
                if (userview.NewPassword.Equals(PasswordNotChangedToken))
                    userview.Password = user.Password;
                else
                    userview.Password = userview.NewPassword;
                DB.UpdateUser(userview);
                userview.CopyToUserView(OnlineUsers.CurrentUser);
                OnlineUsers.LastUpdate = DateTime.Now;
            }
            // todo
            return RedirectToAction("Index", "Actors");
        }

        public ActionResult About()
        { 
            // todo
            return View();
        }
    }
}