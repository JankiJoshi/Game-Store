using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace CVGS
{
    public class UserController : Controller
    {
        private CVGSEntities db = new CVGSEntities();

        // GET: users
        public ActionResult Index()
        {
            var users = db.users.Include(u => u.login);
            return View(users.ToList());
        }

        // GET: users/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            user user = db.users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: users/Create
        public ActionResult Create()
        {
            ViewBag.username = new SelectList(db.logins, "username", "password");
            return View();
        }

        // POST: users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "username,firstName,lastName,email,mailAddress,shipAddress,age,employee")] user user)
        {
            if (ModelState.IsValid)
            {
                db.users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.username = new SelectList(db.logins, "username", "password", user.username);
            return View(user);
        }

        // GET: users/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            user user = db.users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.username = new SelectList(db.logins, "username", "password", user.username);
            ViewBag.favPlatform = new SelectList(db.platforms, "platformId", "platformName", user.favPlatform);
            ViewBag.favGenre = new SelectList(db.genres, "genreKey", "name", user.favGenre);
            ViewBag.favPlatform2 = new SelectList(db.platforms, "platformId", "platformName", user.favPlatform2);
            ViewBag.favGenre2 = new SelectList(db.genres, "genreKey", "name", user.favGenre2);
            return View(user);
        }

        // POST: users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "username,firstName,lastName,email,mailAddress,shipAddress," +
            "age,employee,promoEmails,publicWishList,favPlatform, favPlatform2, favGenre,favGenre2")] user user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.username = new SelectList(db.logins, "username", "password", user.username);
            ViewBag.favPlatform = new SelectList(db.platforms, "platformId", "platformName", user.favPlatform);
            ViewBag.favGenre = new SelectList(db.genres, "genreKey", "name", user.favGenre);
            ViewBag.favPlatform2 = new SelectList(db.platforms, "platformId", "platformName", user.favPlatform2);
            ViewBag.favGenre2 = new SelectList(db.genres, "genreKey", "name", user.favGenre2);
            return View(user);
        }

        // GET: users/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            user user = db.users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            user user = db.users.Find(id);
            db.users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
