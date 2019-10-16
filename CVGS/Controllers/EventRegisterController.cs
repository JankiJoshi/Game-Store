using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CVGS.Controllers
{
    public class EventRegisterController : Controller
    {
        private CVGSEntities db = new CVGSEntities();

        // GET: EventRegister
        public ActionResult Index()
        {
            return View();
        }

        // GET: Cart/Create
        public ActionResult Create()
        {
            ViewBag.gameId = new SelectList(db.eventDatas, "eventId", "name");
            ViewBag.username = new SelectList(db.users, "username", "firstName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int id, [Bind(Include = "username,gameId,quantity")] event_register registery)
        {
            TempData["Error"] = "";
            if (ModelState.IsValid)
            {
                // Gather list of register items to ensure the user doesn't double-register for a single event
                string currentUser = Session["User"].ToString();
                var registerItems = db.event_register
                    .ToList();

                // See if user can be found within this list
                bool exists = false;
                int counter = 0;
                var last = registerItems.Last();
                foreach (var item in registerItems)
                {
                    counter++;
                    // If item.eventId equals id, the user has already registered for this event...
                    if (item.eventId == id && item.username == Session["User"].ToString())
                    {
                        //User has already signed up for this event
                        exists = true;
                    }
                    // If the user has not registered already, create a new event_register entry
                    else if (item.Equals(last) && exists == false)
                    {
                        var newItem = db.event_register.Add(registery);
                        int registerId = registerItems.Count + 1;
                        newItem.registerId = registerId;
                        newItem.username = Session["User"].ToString();
                        newItem.eventId = id;
                        db.SaveChanges();                        
                    }
                }
                return RedirectToAction("Index", "Event");
            }

            ViewBag.gameId = new SelectList(db.games, "id", "name", registery.eventId);
            ViewBag.username = new SelectList(db.users, "username", "firstName", registery.username);
            return View(registery);
        }
    }
}