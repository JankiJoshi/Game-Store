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
    public class creditInformationController : Controller
    {
        private CVGSEntities db = new CVGSEntities();

        // GET: creditInformation
        public ActionResult Index()
        {
            string user = Session["User"].ToString();

            var creditInformations = db.creditInformations.Include(c => c.cardType1)
                .Include(c => c.user).Where(c => c.username == user);
            return View(creditInformations.ToList());
        }

        // GET: creditInformation/Details/5
        public ActionResult Details(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            creditInformation creditInformation = db.creditInformations.Find(id);
            if (creditInformation == null)
            {
                return HttpNotFound();
            }
            return View(creditInformation);
        }

        // GET: creditInformation/Create
        public ActionResult Create()
        {
            ViewBag.cardType = new SelectList(db.cardTypes, "type", "type");
            ViewBag.username = new SelectList(db.users, "username", "firstName");
            return View();
        }

        // POST: creditInformation/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "cardNumber,cardHolder,cardType,username,expiryDate,securityCode")] creditInformation creditInformation)
        {
            string user = Session["User"].ToString();
            creditInformation.username = user;
            if (ModelState.IsValid)
            {
                db.creditInformations.Add(creditInformation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.cardType = new SelectList(db.cardTypes, "type", "type", creditInformation.cardType);
            ViewBag.username = new SelectList(db.users, "username", "firstName", creditInformation.username);
            return View(creditInformation);
        }

        // GET: creditInformation/Edit/5
        public ActionResult Edit(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            creditInformation creditInformation = db.creditInformations.Find(id);
            if (creditInformation == null)
            {
                return HttpNotFound();
            }
            ViewBag.cardType = new SelectList(db.cardTypes, "type", "type", creditInformation.cardType);
            ViewBag.username = new SelectList(db.users, "username", "firstName", creditInformation.username);
            return View(creditInformation);
        }

        // POST: creditInformation/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "cardNumber,cardHolder,cardType,username,expiryDate,securityCode")] creditInformation creditInformation)
        {
            string user = Session["User"].ToString();
            creditInformation.username = user;
            if (ModelState.IsValid)
            {
                db.Entry(creditInformation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.cardType = new SelectList(db.cardTypes, "type", "type", creditInformation.cardType);
            ViewBag.username = new SelectList(db.users, "username", "firstName", creditInformation.username);
            return View(creditInformation);
        }

        // GET: creditInformation/Delete/5
        public ActionResult Delete(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            creditInformation creditInformation = db.creditInformations.Find(id);
            if (creditInformation == null)
            {
                return HttpNotFound();
            }
            return View(creditInformation);
        }

        // POST: creditInformation/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
            creditInformation creditInformation = db.creditInformations.Find(id);
            db.creditInformations.Remove(creditInformation);
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
