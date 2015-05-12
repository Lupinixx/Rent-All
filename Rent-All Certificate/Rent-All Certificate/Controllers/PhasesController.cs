using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Helpers;
using Rent_All_Certificate.Models;
using PagedList;
using Rent_All_Certificate.Attributes;

namespace Rent_All_Certificate.Controllers
{
    [LoginValidRole(ValidRoleId = new[]{Roles.TechnicalStaff, Roles.TechnicalAdministrator})]
    public class PhasesController : Controller
    {
        private RentAllEntities db = new RentAllEntities();

        // GET: Phases
        public ActionResult Index(string search, int? page)
        {
            if (search == null)
            {
                return View(db.Phase
                    .ToList().ToPagedList(page ?? 1, 20));
            }
            else
            {
                return View(db.Phase.Where(x => x.PhaseName.StartsWith(search))
                    .ToList().ToPagedList(page ?? 1, 20));
            }
        }

        // GET: Phases/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Phases/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PhaseID,PhaseName")] Phase phase)
        {
            ValidatePhase(phase);
            if (ModelState.IsValid)
            {
                db.Phase.Add(phase);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(phase);
        }

        // GET: Phases/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Phase phase = db.Phase.Find(id);
            if (phase == null)
            {
                return HttpNotFound();
            }
            return View(phase);
        }

        // POST: Phases/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PhaseID,PhaseName")] Phase phase)
        {
            ValidatePhase(phase);
            if (ModelState.IsValid)
            {
                db.Entry(phase).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(phase);
        }

        // GET: Phases/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Phase phase = db.Phase.Find(id);
            if (phase == null)
            {
                return HttpNotFound();
            }
            if (phase.Product.Count == 0)
            {
                db.Phase.Remove(phase);
                db.SaveChanges();
            }
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

        private void ValidatePhase(Phase phase)
        {
            if (db.Phase.Any(p => p.PhaseName.Equals(phase.PhaseName) && p.PhaseID != phase.PhaseID))
                ModelState.AddModelError("", "Phase must have a unique name.");

        }
    }
}
