using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CVGS;

namespace CVGS.Controllers
{
    public class CartController : Controller
    {
        private CVGSEntities db = new CVGSEntities();

        // GET: Cart
        /// <summary>
        /// Use current users' username (stroed within session "user") to filter cart data to display only that users' items
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            // Get current users' username for comparison
            string enteredUser = Session["User"].ToString();
            var carts = db.carts
                .Where(c => c.username == enteredUser);

            // Calculate total & place it within a ViewBag for sending to views
            decimal total = 0;
            foreach (var item in carts)
            {
                total = (item.game.price * item.quantity);
            }
            // Convert to currency format
            string strTotal = String.Format("Order Total: {0:C}", total);
            ViewBag.Total = strTotal;

            return View(carts.ToList().Where(c => c.username == enteredUser));
        }

        // GET: Cart/Details/5
        /// <summary>
        /// Use the selected games' id in order to dsplay its' particular data                                                                          (might not be necessary to have a details for cart items at all)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            cart cart = db.carts.Find(id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            return View(cart);
        }

        // GET: Cart/Create
        public ActionResult Create()
        {
            ViewBag.gameId = new SelectList(db.games, "id", "name");
            ViewBag.username = new SelectList(db.users, "username", "firstName");
            return View();
        }

        // POST: Cart/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.

        /// <summary>
        /// Use gameId passed from Game > Details to the controller to see it the game exists on the same row as the user already
        /// If yess, this means that the user has already added this game to their cart
        ///     > increase quantity by 1 instead of creating a new cart row
        /// Else, this means that the user does not have this item in their cart currently
        ///     > create a new cart item for this game
        ///     > gameId = id (passed from Game -> Details), username = Session["User"], quantity = 1
        /// </summary>
        /// <param name="cart"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int id,[Bind(Include = "username,gameId,quantity")] cart cart)
        {
            if (ModelState.IsValid)
            {
                // Filter list of cart items to include only the current users' items
                string currentUser = Session["User"].ToString();
                var cartItems = db.carts                  
                    .ToList();

                // See if selected games' id can be found within this list
                bool exists = false;
                int counter = 0;
                var last = cartItems.Last();
                foreach (var item in cartItems)
                {
                    counter++;
                    // If item.gameId equals id, the game exists within the cart -> Quantity++.
                    if (item.gameId == id && item.username == Session["User"].ToString())
                    {
                        //Game exists inusers' cart
                        exists = true;
                        item.quantity = item.quantity + 1;
                        db.SaveChanges();
                    }
                    // If the game does not exist by the final iteration, create a new cart item
                    else if (item.Equals(last) && exists == false)
                    {
                        //Game does not exist in users' cart
                        var newItem = db.carts.Add(cart);
                        newItem.username = Session["User"].ToString();
                        newItem.gameId = id;
                        newItem.quantity = 1;
                        db.SaveChanges();
                    }
                }

                return RedirectToAction("Index"); //maybe put inside loop if this doesnt work
            }

            ViewBag.gameId = new SelectList(db.games, "id", "name", cart.gameId);
            ViewBag.username = new SelectList(db.users, "username", "firstName", cart.username);
            return View(cart);
        }

        // GET: Cart/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            cart cart = db.carts.Find(Session["User"].ToString(), id);

            if (cart == null)
            {
                return HttpNotFound();
            }
            ViewBag.gameId = new SelectList(db.games, "id", "name", cart.gameId);
            ViewBag.username = new SelectList(db.users, "username", "firstName", cart.username);
            return View(cart);
        }

        // POST: Cart/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, [Bind(Include = "username,gameId,quantity")] cart cart)
        {
            cart.username = Session["User"].ToString();
            cart.gameId = id;
            if (ModelState.IsValid)
            {
                db.Entry(cart).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.gameId = new SelectList(db.games, "id", "name", cart.gameId);
            ViewBag.username = new SelectList(db.users, "username", "firstName", cart.username);
            return View(cart);
        }

        // GET: Cart/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            cart cart = db.carts.Find(Session["User"].ToString(), id);
            if (cart == null)
            {
                return HttpNotFound();
            }
            return View(cart);
        }

        // POST: Cart/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            cart cart = db.carts.Find(Session["User"].ToString(), id);
            db.carts.Remove(cart);
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
