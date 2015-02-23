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
        public ActionResult Index(string WorkCenter, string Department, string machineChoice, string deptChoice, string wcChoice, string checkBoxState, string searchString, string colList)
        {

            //main query
            var machineShopQry = from m in db.MainTableObj
                                 select m;

            // CALCULATE SUM variable
            ViewBag.totalSumQty = 0;
            ViewBag.totalSumHours = 0;
            ViewBag.totalSumSetup = 0;
            ViewBag.totalSumCleaning = 0;
            ViewBag.totalSumDown = 0;
            ViewBag.totalSumOther = 0;
            // CALCULATE SUM - end            

            //INCOMING DATA//
            ViewData["Department"] = Department;
            ViewData["WorkCenter"] = WorkCenter;
            ViewBag.department = Department;
            //INCOMING DATA -end//

            #region CURRENTYL DISPLAYED

            var machLst = new List<string>();
            var machTxtQry = from m in db.Machines
                                   where m.Department == Department && m.WorkCenter == WorkCenter
                                   select m.Machine;
            //ViewBag.machineTxt = machTxtQry.ToString();
            machLst.AddRange(machTxtQry.Distinct());
            ViewBag.machineTxt = new SelectList(machLst);
            #region another tests
            /*
            //var machTxtQry = from a in db.Machines
            //                 select a.Machine
            //                 db.Machines.Where(x => x.WorkCenter == WorkCenter && x.Department == Department);
            */

            /*
            var machTxtQry = "SELECT Machine FROM Machines WHERE Department='@Department' AND WorkCenter='@WorkCenter'";
            IEnumerable<Machines> data = db.Database.SqlQuery<Machines>(machTxtQry);
            ViewBag.machineTxt = data.ToString();
            */

            /*
            var machTxtQry = from m in db.Machines
                             select m;

            machTxtQry.Where(x => x.WorkCenter == WorkCenter && x.Department == Department);
            ViewBag.machineTxt = machTxtQry;
            */

            /*
            var machTxtQry =
            db.Machines.
            Where("Department = @0 and WorkCenter= @1", Department, WorkCenter).
            OrderBy("Machine").
            //Select("new(Machine as Machine)");
            Select("new(Machine)");
            */
            #endregion
            #endregion
            #region FILTERING TOOLS
            //LISTS//
            
            // MACHINEs LIST
            var machineLst = new List<string>();
            var machineQuery = from a in db.Machines
                           select a.Machine;
            machineLst.AddRange(machineQuery.Distinct());
            ViewBag.machineChoice = new SelectList(machineLst);

            //Departments List
            var deptLst = new List<string>();
            var deptQuery = from b in db.Machines
                               select b.Department;
            deptLst.AddRange(deptQuery.Distinct());
            ViewBag.deptChoice = new SelectList(deptLst);

            //WorkCenter List
            var wcLst = new List<string>();
            var wcQuery = from c in db.Machines
                            //where c.Department == deptChoice
                            select c.WorkCenter;
            wcLst.AddRange(wcQuery.Distinct());
            ViewBag.wcChoice = new SelectList(wcLst);

            //Column names list

            var columnsList = new List<string>();
            string[] listData = new string[] { "Date", "Item No.", "Operation","Operator","Qty","Hours","Standarad Rate",
            "Percent","Setup","Cleaning","Down","Other","Nonconf Parts","Comments" };
            columnsList.AddRange(listData);
            ViewBag.colList = new SelectList(columnsList);

            //LISTS//end

            #endregion

            //Searcher
            if(!String.IsNullOrEmpty(searchString))
            {
                machineShopQry = from a in db.MainTableObj
                                 where colList == searchString // how to include list result into LINQ search string ?
                                 select a;
            }

            //CHECK BOX
            if (checkBoxState=="true")
            {
                machineShopQry = from m in db.MainTableObj
                                     select m;

                //s = virtual lambda variable used just to initate this function and return result
            }
            

            //return View(db.MainTableObj.ToList());
            return View(machineShopQry);
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
        [Authorize(Roles = "admin,moderator")]
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
        [ValidateAntiForgeryToken,Authorize(Roles="admin,moderator")]
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
        [Authorize(Roles = "admin,moderator")]
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
        
        [ValidateAntiForgeryToken, Authorize(Roles = "admin,moderator")]
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

