using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CVGS;
using System.IO;


namespace CVGS.Controllers
{
    public class GameController : Controller
    {
        //public readonly CVGSEntities_context;
        private CVGSEntities db = new CVGSEntities();


        //Get games list for index view
        public ActionResult Index()
        {
            var games = db.games.Include(g => g.esrb_rating).Include(g => g.genre1).Include(g => g.genre2);
            return View(games.ToList());
        }

        //Search functionality on index page
        [HttpGet]
        public ActionResult Index(string Search)
        {
            string s1 = Search;
            
            var games = db.games.Include(g => g.esrb_rating).Include(g => g.genre1).Include(g => g.genre2);

            if (Search != null)
            {
                return View(db.games.Where(g => g.name.Contains(Search)));
            }
            else
            {
                return View(games.ToList());
            }
            
        }

        // GET: Game/Details
        public ActionResult Details(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            game game = db.games.Find(id);
            if (game == null)
            {
                return HttpNotFound();
            }            
            return View(game);
        }

        // GET: Game/Create
        public ActionResult Create()
        {
            ViewBag.rating = new SelectList(db.esrb_rating, "ratingCode", "description");
            ViewBag.genre = new SelectList(db.genres, "genreKey", "name");
            ViewBag.genre = new SelectList(db.genres, "genreKey", "name");
            return View();
        }

        // POST: Game/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,description,publisher,publishDate,genre,rating,price")] game game)
        {
            if (ModelState.IsValid)
            {
                db.games.Add(game);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.rating = new SelectList(db.esrb_rating, "ratingCode", "description", game.rating);
            ViewBag.genre = new SelectList(db.genres, "genreKey", "name", game.genre);
            ViewBag.genre = new SelectList(db.genres, "genreKey", "name", game.genre);
            return View(game);
        }

        // GET: Game/Edit/5
        public ActionResult Edit(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            game game = db.games.Find(id);
            if (game == null)
            {
                return HttpNotFound();
            }
            ViewBag.rating = new SelectList(db.esrb_rating, "ratingCode", "description", game.rating);
            ViewBag.genre = new SelectList(db.genres, "genreKey", "name", game.genre);
            ViewBag.genre = new SelectList(db.genres, "genreKey", "name", game.genre);
            return View(game);
        }

        // POST: Game/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,description,publisher,publishDate,genre,rating,price")] game game)
        {
            if (ModelState.IsValid)
            {
                db.Entry(game).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.rating = new SelectList(db.esrb_rating, "ratingCode", "description", game.rating);
            ViewBag.genre = new SelectList(db.genres, "genreKey", "name", game.genre);
            ViewBag.genre = new SelectList(db.genres, "genreKey", "name", game.genre);
            return View(game);
        }

        // GET: Game/Delete/5
        public ActionResult Delete(decimal id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            game game = db.games.Find(id);
            if (game == null)
            {
                return HttpNotFound();
            }
            return View(game);
        }

        // POST: game/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(decimal id)
        {
            game game = db.games.Find(id);
            db.games.Remove(game);
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

        public ActionResult Download()
        {
            string path = Server.MapPath("~/Images/");
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            FileInfo[] files = dirInfo.GetFiles("*.*");
            List<string> lst = new List<string>(files.Length);
            foreach (var item in files)
            {
                lst.Add(item.Name);
            }
            return View(lst);
        }

        public ActionResult DownloadFile(string filename)
        {
            if(Path.GetExtension(filename) == ".png")
            {
                string fullPath = Path.Combine(Server.MapPath("~/Images/"), filename);
                return File(fullPath, "Images/png");
            }
            else
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.Forbidden);   
            }
        }


    }
}
