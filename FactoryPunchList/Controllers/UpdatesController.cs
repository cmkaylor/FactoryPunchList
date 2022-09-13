using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FactoryPunchList.App_Data;
using FactoryPunchList.Methods;

namespace FactoryPunchList.Views
{
    public class UpdatesController : Controller
    {
        private factorydbEntities db = new factorydbEntities();
        private Authenticate authenticate = new Authenticate();
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (authenticate.CookieExists(Request))
            {
                base.OnActionExecuted(filterContext);
            }

            else
            {
                filterContext.Result = RedirectToAction("Signin", "Account");
                base.OnActionExecuted(filterContext);
            }
        }

        // GET: Updates
        public ActionResult Index(int id)
        {
            var updates = db.Updates.Include(u => u.Punchlist)
                .Include(u => u.Supervisor)
                .Where(o => o.PunchlistID == id);

            ViewBag.PunchListID = id;
            ViewBag.Description = db.Punchlists.Find(id).Description;

            return View(updates.ToList());
        }

        // GET: Updates/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Update update = db.Updates.Find(id);
            if (update == null)
            {
                return HttpNotFound();
            }
            return View(update);
        }

        // GET: Updates/Create
        public ActionResult Create(int id)
        {
            ViewBag.Author = new SelectList(db.Supervisors, "id", "FirstName");
            Update update = new Update();
            update.PunchlistID = id;
            return View(update);
        }

        // POST: Updates/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,Description,PunchlistID,Author,UpdateDate")] Update update)
        {
            if (ModelState.IsValid)
            {
                update.UpdateDate = DateTime.Now.Date;
                db.Updates.Add(update);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = update.PunchlistID });
            }

            ViewBag.PunchlistID = new SelectList(db.Punchlists, "id", "Description", update.PunchlistID);
            ViewBag.Author = new SelectList(db.Supervisors, "id", "FirstName", update.Author);
            return View(update);
        }

        // GET: Updates/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Update update = db.Updates.Find(id);
            if (update == null)
            {
                return HttpNotFound();
            }
            ViewBag.PunchlistID = new SelectList(db.Punchlists, "id", "Description", update.PunchlistID);
            ViewBag.Author = new SelectList(db.Supervisors, "id", "FirstName", update.Author);
            return View(update);
        }

        // POST: Updates/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,Description,PunchlistID,Author,UpdateDate")] Update update)
        {
            if (ModelState.IsValid)
            {
                db.Entry(update).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", new { id = update.PunchlistID });
            }
            ViewBag.PunchlistID = new SelectList(db.Punchlists, "id", "Description", update.PunchlistID);
            ViewBag.Author = new SelectList(db.Supervisors, "id", "FirstName", update.Author);
            return View(update);
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
