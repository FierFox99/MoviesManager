using MoviesManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MoviesManager.Controllers
{
    [UserAccess]
    public class ActorsController : Controller
    {
        private DBEntities DB = new DBEntities();
        // GET: Actors

        [UserAccess]
        public ActionResult Index()
        {
            ViewBag.AdminAccess = OnlineUsers.CurrentUser.Admin;
            return View(DB.ActorViewList());
        }
        [AdminAccess]
        public ActionResult Create()
        {
            ViewBag.SelectedActor = new List<ListItem>();
            ViewBag.Actor = DB.ActorListItem();
            ViewBag.CountryNameList = DB.CountryNameList();
            return View(new ActorView());
        }

        [HttpPost]
        public ActionResult Create(ActorView actorView)
        {
            if (ModelState.IsValid)
            {
                actorView.CountryId = DB.GetCountryId(actorView.CountryName);
                DB.AddActor(actorView);
            }
            return RedirectToAction("Index");
        }
    }
}