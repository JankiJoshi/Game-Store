using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CVGS;
using CVGS.Models;
using Microsoft.AspNet.Identity;

namespace CVGS.Controllers
{
    public class WishListController : Controller
    {
        private CVGSEntities db = new CVGSEntities();

        // GET: WishList
        public ActionResult Index(string id)
        {
            //Get current user and assign it to currentUser
            string currentUser = Session["User"].ToString();

            //Get wishlists where username is equal to id and the user's wishlist is public
            var wishLists = db.wishLists.Include(w => w.game).Include(w => w.user)
                .Where(w => w.username == id)
                .Where(w => w.user.publicWishlist == true);

            //Return wishlist 
            return View(wishLists.ToList());
        }

        // GET: WishList/Details
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            wishList wishList = db.wishLists.Find(Session["User"].ToString(), id);
            if (wishList == null)
            {
                return HttpNotFound();
            }
            return View(wishList);
        }

        // GET: WishList/Create
        public ActionResult Create()
        {
            ViewBag.gameId = new SelectList(db.games, "id", "name");
            ViewBag.username = new SelectList(db.users, "username", "firstName");
            return View();
        }

        // POST: WishList/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int id, [Bind(Include = "username,gameId,dateAdded")] wishList wishList)
        {
            if (ModelState.IsValid)
            {
                //get current user
                string currentUser = Session["User"].ToString();

                //Check if game already exists on wishlist
                var gameExists = db.wishLists.FirstOrDefault(m => m.gameId == id);
                

                if (gameExists != null)
                {
                    //Add to wishlist
                    var newItem = db.wishLists.Add(wishList);
                    newItem.username = Session["User"].ToString();
                    newItem.gameId = id;
                    newItem.dateAdded = null;
                    db.SaveChanges();
                }

                //Redirect to games page
                return RedirectToAction("Index", "Game");

            }

            ViewBag.gameId = new SelectList(db.games, "id", "name", wishList.gameId);
            ViewBag.username = new SelectList(db.users, "username", "firstName", wishList.username);
            return View(wishList);
        }

        // GET: WishList/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            wishList wishList = db.wishLists.Find(Session["User"].ToString(), id);
            if (wishList == null)
            {
                return HttpNotFound();
            }
            ViewBag.gameId = new SelectList(db.games, "id", "name", wishList.gameId);
            ViewBag.username = new SelectList(db.users, "username", "firstName", wishList.username);
            return View(wishList);
        }

        // POST: WishList/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "username,gameId,dateAdded")] wishList wishList)
        {
            if (ModelState.IsValid)
            {
                db.Entry(wishList).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.gameId = new SelectList(db.games, "id", "name", wishList.gameId);
            ViewBag.username = new SelectList(db.users, "username", "firstName", wishList.username);
            return View(wishList);
        }

        // GET: WishList/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            wishList wishList = db.wishLists.Find(Session["User"].ToString(), id);
            if (wishList == null)
            {
                return HttpNotFound();
            }
            return View(wishList);
        }

        // POST: WishList/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            wishList wishList = db.wishLists.Find(Session["User"].ToString(), id);
            db.wishLists.Remove(wishList);
            db.SaveChanges();
            return RedirectToAction("Index", "Wishlist");
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
