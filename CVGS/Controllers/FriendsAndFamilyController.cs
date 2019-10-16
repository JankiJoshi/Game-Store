using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace CVGS.Controllers
{
    public class FriendsAndFamilyController : Controller
    {
        private CVGSEntities db = new CVGSEntities();

        // GET: FriendsAndFamily
        public ActionResult Index()
        {
            var loginUser = Session["User"].ToString();
            var friends = db.FriendsLists.Where(f => f.username == loginUser);
            return View(friends.ToList());
        }

        // GET: FriendsAndFamilyList/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: FriendsAndFamilyList/Create
        public ActionResult Create()
        {
            var loginUser = Session["User"].ToString();
            var users = db.users.Include(u => u.login).Where(u => u.username != loginUser);
            return View(users.ToList());
        }

        // POST: FriendsAndFamilyList/Create
        [HttpPost]
        public ActionResult Create(String username)
        {

            try
            {
                // TODO: Add insert logic here
                FriendsList friend = new FriendsList();
                friend.FriendIId = username;
                friend.username = Session["User"].ToString();
                if (ModelState.IsValid)
                {
                    db.FriendsLists.Add(friend);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return RedirectToAction("Index");
            }
        }

        // GET: FriendsAndFamilyList/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: FriendsAndFamilyList/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: FriendsAndFamilyList/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: FriendsAndFamilyList/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}