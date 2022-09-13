using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FactoryPunchList.App_Data;
using FactoryPunchList.Methods;
using FactoryPunchList.Models;

namespace FactoryPunchList.Views
{
    public class PunchlistsController : Controller
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

        public ActionResult Error(ErrorModel errorModel)
        {
            return View(errorModel);
        }

        // GET: Punchlists
        public ActionResult Index()
        {
            var punchlists = db.Punchlists.Include(p => p.Supervisor).Include(p => p.Type1);
            return View(punchlists.ToList());
        }

        // GET: Punchlists/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Punchlist punchlist = db.Punchlists.Find(id);
            if (punchlist == null)
            {
                return HttpNotFound();
            }
            return View(punchlist);
        }

        // GET: Punchlists/Create
        public ActionResult Create()
        {
            ViewBag.Assignee = new SelectList(db.Supervisors, "id", "FirstName");
            ViewBag.Type = new SelectList(db.Types, "id", "Title");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PunchlistImageModel model)
        {
            Punchlist punchList = model.Punchlists;
            HttpPostedFileBase file = model.File;

            if (ModelState.IsValid)
            {
                if(file != null)
                {
                    punchList.Image = SaveImage(file);
                }

                punchList.Assignee = Convert.ToInt32(Request.Form["Assignee"]);
                punchList.Type = Convert.ToInt32(Request.Form["Type"]);

                db.Punchlists.Add(punchList);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Assignee = new SelectList(db.Supervisors, "id", "FirstName", punchList.Assignee);
            ViewBag.Type = new SelectList(db.Types, "id", "Title", punchList.Type);
            return View(punchList);
        }

        private string SaveImage(HttpPostedFileBase file)
        {
            if (file.ContentLength > 0 && file.ContentType.Contains("image"))
            {
                Guid id = Guid.NewGuid();
                string fileName = id.ToString() + file.FileName.Substring(file.FileName.IndexOf('.'));
                string path = Path.Combine(Server.MapPath("~/UploadedImages"), fileName);

                file.SaveAs(path);

                return fileName;
            }
            else
            {
                return null;
            }
        }

        // GET: Punchlists/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Punchlist punchlist = db.Punchlists.Find(id);
            if (punchlist == null)
            {
                return HttpNotFound();
            }
            ViewBag.Assignee = new SelectList(db.Supervisors, "id", "FirstName", punchlist.Assignee);
            ViewBag.Type = new SelectList(db.Types, "id", "Title", punchlist.Type);

            PunchlistImageModel model = new PunchlistImageModel();
            model.Punchlists = punchlist;

            return View(model);
        }

        // POST: Punchlists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PunchlistImageModel model)
        {
            Punchlist punchList = model.Punchlists;
            HttpPostedFileBase file = model.File;

            if (ModelState.IsValid)
            {
                if(file != null)
                {
                    punchList.Image = SaveImage(file);
                }

                punchList.Assignee = Convert.ToInt32(Request.Form["Assignee"]);
                punchList.Type = Convert.ToInt32(Request.Form["Type"]);

                db.Entry(punchList).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Assignee = new SelectList(db.Supervisors, "id", "FirstName", punchList.Assignee);
            ViewBag.Type = new SelectList(db.Types, "id", "Title", punchList.Type);
            return View(punchList);
        }

        // GET: Punchlists/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Punchlist punchlist = db.Punchlists.Find(id);
            if (punchlist == null)
            {
                return HttpNotFound();
            }
            return View(punchlist);
        }

        // POST: Punchlists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Punchlist punchlist = db.Punchlists.Find(id);
            db.Punchlists.Remove(punchlist);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Filter()
        {
            string sort = Request.Form["sortItem"];
            string filter = Request.Form["filterItem"];

            var punchlists = db.Punchlists
                .Include(p => p.Supervisor)
                .Include(p => p.Type1)
                .ToList();

            switch (sort)
            {
                case "Description":
                    punchlists = punchlists.Where(o => o.Description.Contains(filter)).ToList();
                    break;

                case "Location":
                    punchlists = punchlists.Where(o => o.Location.Contains(filter)).ToList();
                    break;

                case "Status":
                    punchlists = punchlists.Where(o => o.Status.Contains(filter)).ToList();
                    break;

                case "DueDate":
                    punchlists = punchlists.Where(o => o.DueDate.ToString().Contains(filter)).ToList();
                    break;

                case "Assignee":
                    int id = db.Supervisors.FirstOrDefault(o => o.FirstName.Contains(filter)).id;
                    punchlists = punchlists.Where(o => o.Assignee.Value == id).ToList();
                    break;

                case "Title":
                    int idT = db.Types.FirstOrDefault(o => o.Title.Contains(filter)).id;
                    punchlists = punchlists.Where(o => o.Type.Value == idT).ToList();
                    break;

                default:
                    break;
            }

            return View("Index", punchlists);
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
