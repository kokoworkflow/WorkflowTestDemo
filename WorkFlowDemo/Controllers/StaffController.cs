using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WorkFlowDemo.Models;

namespace WorkFlowDemo.Controllers
{
    public class StaffController : Controller
    {
        private WorkflowDBEntities db = new WorkflowDBEntities();

        // GET: Staff
        public async Task<ActionResult> Index()
        {
            var workItems = db.WorkItems.Include(w => w.Claim);
            return View(await workItems.ToListAsync());
        }

        // GET: Staff/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorkItem workItem = await db.WorkItems.FindAsync(id);
            if (workItem == null)
            {
                return HttpNotFound();
            }
            return View(workItem);
        }

        // GET: Staff/Create
        public ActionResult Create()
        {
            ViewBag.ClaimId = new SelectList(db.Claims, "Id", "FirstName");
            return View();
        }

        // POST: Staff/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,ClaimId,WorkItemType,WorkItemStatus,AssignedTo,Comment,CreatedDate,LastUpdatedDate")] WorkItem workItem)
        {
            if (ModelState.IsValid)
            {
                db.WorkItems.Add(workItem);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ClaimId = new SelectList(db.Claims, "Id", "FirstName", workItem.ClaimId);
            return View(workItem);
        }

        // GET: Staff/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorkItem workItem = await db.WorkItems.FindAsync(id);
            if (workItem == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClaimId = new SelectList(db.Claims, "Id", "FirstName", workItem.ClaimId);
            return View(workItem);
        }

        // POST: Staff/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,ClaimId,WorkItemType,WorkItemStatus,AssignedTo,Comment,CreatedDate,LastUpdatedDate")] WorkItem workItem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(workItem).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ClaimId = new SelectList(db.Claims, "Id", "FirstName", workItem.ClaimId);
            return View(workItem);
        }

        // GET: Staff/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            WorkItem workItem = await db.WorkItems.FindAsync(id);
            if (workItem == null)
            {
                return HttpNotFound();
            }
            return View(workItem);
        }

        // POST: Staff/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            WorkItem workItem = await db.WorkItems.FindAsync(id);
            db.WorkItems.Remove(workItem);
            await db.SaveChangesAsync();
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
