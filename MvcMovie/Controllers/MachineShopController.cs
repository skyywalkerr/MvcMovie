using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MvcMovie.Models;

namespace MvcMovie.Controllers
{
    public class MachineShopController : Controller
    {
        private MachineShopDB db = new MachineShopDB();

        // GET: /MachineShop/
        public ActionResult Index()
        {
            // CALCULATE SUM variable
            ViewBag.totalSumQty = 0;
            ViewBag.totalSumHours = 0;
            ViewBag.totalSumSetup = 0;
            ViewBag.totalSumCleaning = 0;
            ViewBag.totalSumDown = 0;
            ViewBag.totalSumOther = 0;
            // CALCULATE SUM - end

            return View(db.MainTableObj.ToList());
        }

        // GET: /MachineShop/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MachineShopTable machineshoptable = db.MainTableObj.Find(id);
            if (machineshoptable == null)
            {
                return HttpNotFound();
            }
            return View(machineshoptable);
        }

        // GET: /MachineShop/Create\

        //by using this attribute system will let create new things just to registered users
        //just user "luke" can use this method
        //[Authorize(Users="luke")] 
        [Authorize(Roles="admin")]
        //or you can use a role to authorize
        public ActionResult Create()
        {
            return View();
        }

        // POST: /MachineShop/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken, Authorize(Roles="admin")] // need to authorize by login
        public ActionResult Create([Bind(Include="ID,Date,ItemNo,Operation,Operator,Qty,Hours,ActualRate,StandardRate,Percent,Setup,Cleaning,Down,Other,NonconfParts,Comments")] MachineShopTable machineshoptable)
        {
            if (ModelState.IsValid)
            {
                db.MainTableObj.Add(machineshoptable);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(machineshoptable);
        }

        // GET: /MachineShop/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MachineShopTable machineshoptable = db.MainTableObj.Find(id);
            if (machineshoptable == null)
            {
                return HttpNotFound();
            }
            return View(machineshoptable);
        }

        // POST: /MachineShop/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,Date,ItemNo,Operation,Operator,Qty,Hours,ActualRate,StandardRate,Percent,Setup,Cleaning,Down,Other,NonconfParts,Comments")] MachineShopTable machineshoptable)
        {
            if (ModelState.IsValid)
            {
                db.Entry(machineshoptable).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(machineshoptable);
        }

        // GET: /MachineShop/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MachineShopTable machineshoptable = db.MainTableObj.Find(id);
            if (machineshoptable == null)
            {
                return HttpNotFound();
            }
            return View(machineshoptable);
        }

        // POST: /MachineShop/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            MachineShopTable machineshoptable = db.MainTableObj.Find(id);
            db.MainTableObj.Remove(machineshoptable);
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
