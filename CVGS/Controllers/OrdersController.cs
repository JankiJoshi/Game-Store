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
    public class OrdersController : Controller
    {
        private CVGSEntities db = new CVGSEntities();

        // GET: Orders
        public ActionResult Index()
        {
            string user = Session["User"].ToString();
            var orders = db.orders.Include(o => o.game).Include(o => o.orderStatu).Include(o => o.user);

            if (Session["Emp"] == null)
            {
                orders = db.orders.Include(o => o.game).Include(o => o.orderStatu)
                    .Include(o => o.user).Where(o => o.username == user);
            }
            else
            {
                orders = db.orders.Include(o => o.game).Include(o => o.orderStatu).Include(o => o.user);
            }
            return View(orders.ToList());
        }

        // GET: Orders/Details/5
        public ActionResult Details(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            order order = db.orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            ViewBag.gameId = new SelectList(db.games, "id", "name");
            ViewBag.status = new SelectList(db.orderStatus, "status", "status");
            ViewBag.username = new SelectList(db.users, "username", "firstName");
            return View();
        }

        // POST: Orders/Order
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "firstName,lastName,email,shipAddress,mailAdress")] user user, [Bind(Include = "cardHolder,cardType,cardNumber,expiryDate,securityCode,username")] creditInformation creditInformation, [Bind(Include = "username,orderId,gameId,orderDate,shipDate,status")] order order)
        {
            if (ModelState.IsValid)
            {
                string currentUser = Session["User"].ToString();
                var cartItems = db.carts.ToList();
                
                foreach (var item in cartItems)
                {
                    if (item.username == currentUser)
                    {
                        for (int i = 0; i < item.quantity; i++)
                        {
                            var newOrder = db.orders.Add(order);
                            newOrder.username = currentUser;
                            // Find id of last order within list, auto increment
                            var orderItems = db.orders.ToList();
                            var lastOrder = orderItems.Last();
                            decimal orderId = lastOrder.orderId + 1;
                            newOrder.orderId = orderId;
                            newOrder.gameId = item.gameId;
                            newOrder.orderDate = DateTime.Today;
                            newOrder.shipDate = null;
                            newOrder.status = "Received";
                            db.SaveChanges();
                        }
                    }
                    // Remove users' items from cart                    
                    foreach (var cartItem in cartItems)
                    {
                        if (cartItem.username == currentUser && cartItem.gameId == item.gameId)
                        {
                            db.carts.Attach(cartItem);
                            db.carts.Remove(cartItem);
                        }
                    }                    
                }

                // Save checkout information   
                // User info
                string firstName = "";
                string lastName = "";
                var userList = db.users.ToList();
                foreach (var userItem in userList)
                {
                    if (userItem.username == currentUser)
                    {
                        firstName = user.firstName;
                        lastName = user.lastName;
                        userItem.email = user.email;
                        userItem.shipAddress = user.shipAddress;
                        userItem.mailAddress = user.mailAddress;
                        db.SaveChanges();
                    }
                }
                // Payment info
                bool exists = false;
                var creditList = db.creditInformations.ToList();
                var last = creditList.Last();
                foreach (var creditItem in creditList)
                {
                    // If user has an entry in creditInformation, update that entry
                    if (creditItem.username == currentUser)
                    {
                        exists = true;
                        creditItem.cardNumber = creditInformation.cardNumber;
                        // Concatenate first & last names for cardHolder
                        string cardHolder = firstName + " " + lastName;
                        creditItem.cardHolder = cardHolder;                        
                        creditItem.cardType = creditInformation.cardType;
                        creditItem.username = Session["User"].ToString();
                        creditItem.expiryDate = creditInformation.expiryDate;
                        creditItem.securityCode = creditInformation.securityCode;
                        db.SaveChanges();
                    }
                    // Else, create a new entry for them
                    else if (creditItem.Equals(last) && exists == false)
                    {
                        var newCredit = db.creditInformations.Add(creditInformation);
                        newCredit.cardNumber = creditInformation.cardNumber;
                        // Concatenate first & last names for cardHolder
                        string cardHolder = firstName + " " + lastName;
                        newCredit.cardHolder = cardHolder;
                        newCredit.cardType = creditInformation.cardType;
                        newCredit.username = Session["User"].ToString();
                        newCredit.expiryDate = creditInformation.expiryDate;
                        newCredit.securityCode = creditInformation.securityCode;
                        db.SaveChanges();
                    }
                }
                return RedirectToAction("Index", "Game");
            }

            ViewBag.gameId = new SelectList(db.games, "id", "name", order.gameId);
            ViewBag.status = new SelectList(db.orderStatus, "status", "status", order.status);
            ViewBag.username = new SelectList(db.users, "username", "firstName", order.username);
            return View(order);
        }
                
        // GET: Orders/Edit/5
        public ActionResult Edit(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            order order = db.orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.gameId = new SelectList(db.games, "id", "name", order.gameId);
            ViewBag.status = new SelectList(db.orderStatus, "status", "status", order.status);
            ViewBag.username = new SelectList(db.users, "username", "firstName", order.username);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "username,orderId,gameId,orderDate,shipDate,status")] order order)
        {
            
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.gameId = new SelectList(db.games, "id", "name", order.gameId);
            ViewBag.status = new SelectList(db.orderStatus, "status", "status", order.status);
            ViewBag.username = new SelectList(db.users, "username", "firstName", order.username);
            return View(order);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            order order = db.orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
            order order = db.orders.Find(id);
            db.orders.Remove(order);
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
