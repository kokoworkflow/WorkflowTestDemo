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
using System.Data.Entity.Validation;
using WorkflowLib;
using WorkflowLib.Workflow;

namespace WorkFlowDemo.Controllers
{
    public class ClaimsController : Controller
    {
        private WorkflowDBEntities db = new WorkflowDBEntities();

        // GET: Claims
        public async Task<ActionResult> Index()
        {
            return View(await db.Claims.ToListAsync());
        }

        // GET: Claims/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Claim claim = await db.Claims.FindAsync(id);
            if (claim == null)
            {
                return HttpNotFound();
            }
            return View(claim);
        }

        // GET: Claims/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Claims/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,FirstName,LastName,EmployerName,Wage,Status,CreatedDate,LastUpdatedDate")] Claim claim)
        {
            try
            {
                
                CreateInstance();
                //var commands = WorkflowInit.Runtime.GetAvailableCommands(processId.Value, string.Empty);

                if (ModelState.IsValid)
                {
                    claim.CreatedDate = DateTime.Now;
                    claim.LastUpdatedDate = DateTime.Now;
                    claim.Status = "Submitted";
                    WorkItem WorkItemObj = new WorkItem
                    {
                        ClaimId = claim.Id,
                        WorkItemType = "",
                        WorkItemStatus = 1,
                        AssignedTo = "",
                        CreatedDate = DateTime.Now,
                        LastUpdatedDate = DateTime.Now
                    };
                    db.Claims.Add(claim);
                    db.WorkItems.Add(WorkItemObj);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            catch (DbEntityValidationException e)
            {
                String message = e.EntityValidationErrors.ToString();
                throw;
            }
            

            return View(claim);
        }

        private static void CreateInstance()
        {
            string schemeCode = "SimpleWF";
            Guid? processId = Guid.NewGuid();
            processId = Guid.NewGuid();
            try
            {
                if (WorkflowInit.Runtime.IsProcessExists(processId.Value))
                    return;

                WorkflowInit.Runtime.CreateInstance("SimpleWF", processId.Value);

                WorkflowInit.Runtime.CreateInstance(schemeCode, processId.Value);
                Console.WriteLine("CreateInstance - OK.", processId);
            }
            catch (Exception ex)
            {
                Console.WriteLine("CreateInstance - Exception: {0}", ex.Message);
                processId = null;
            }
        }

        // GET: Claims/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Claim claim = await db.Claims.FindAsync(id);
            if (claim == null)
            {
                return HttpNotFound();
            }
            return View(claim);
        }

        // POST: Claims/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,FirstName,LastName,EmployerName,Wage,Status,CreatedDate,LastUpdatedDate")] Claim claim)
        {
            if (ModelState.IsValid)
            {
                db.Entry(claim).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(claim);
        }

        // GET: Claims/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Claim claim = await db.Claims.FindAsync(id);
            if (claim == null)
            {
                return HttpNotFound();
            }
            return View(claim);
        }

        // POST: Claims/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Claim claim = await db.Claims.FindAsync(id);
            db.Claims.Remove(claim);
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
