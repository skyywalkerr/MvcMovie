using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MvcMovie.Models;
using System.Linq.Expressions;


namespace MvcMovie.Controllers
{
    public class MachineShopController : Controller
    {
        private MachineShopDB db = new MachineShopDB();
        MachineShopTable mainTableObject = new MachineShopTable();

        // GET: /MachineShop/
        public ActionResult Index(string WorkCenter, string Department, string machineChoice, string deptChoice, string wcChoice, string checkBoxState, string searchString, string colList, DateTime? SingleDate)
        //public ActionResult Index(string WorkCenter, string Department, string machineChoice, string deptChoice, string wcChoice, string checkBoxState, string searchString, string colList)
        {

            //MachineShopTable mainTableObject = new MachineShopTable(); moved to global
            OperatorsModel test = new OperatorsModel();
            test.Operator = "TEST";           
            
            //VERY MAIN QUERY 
            var machineShopQry = from m in db.MainTableObj
                                 //where m.Date == System.DateTime.Today
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
            //This view data sending results of action link from HOME view to Index view of MachineShop controller
            ViewData["Department"] = Department;
            ViewData["WorkCenter"] = WorkCenter;
            
            //TempData sending data between action methods of same controller
            TempData["deptFromHome"] = Department;
            TempData["workCentrFromHome"] = WorkCenter;

            ViewBag.department = Department;
            //INCOMING DATA -end//

            #region Currently DISPLAYED

            //var machLst = new List<string>();

            var machTxtQry = from m in db.Machines
                                   where m.Department == Department && m.WorkCenter == WorkCenter
                                   select m.Machine;
            if((machTxtQry != null) && (Department !=null) && (WorkCenter !=null))
            {
                ViewBag.machineTxt = Convert.ToString(machTxtQry.First());
                //mainTableObject.Machine = Convert.ToString(machTxtQry.First());
            }

            //machLst.AddRange(machTxtQry.Distinct());
            //ViewBag.machineTxt = new SelectList(machLst);
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
            //string[] listData = new string[] { "Date", "Item No.", "Operation","Operator","Qty","Hours","Standarad Rate",
            //"Percent","Setup","Cleaning","Down","Other","Nonconf Parts","Comments" };
            string[] listData = new string[] { "Date", "Item No.", "Operation","Operator","Comments" };
            columnsList.AddRange(listData);
            ViewBag.colList = new SelectList(columnsList);

            //LISTS//end

            #endregion

            //Searcher

            if(!String.IsNullOrEmpty(searchString))
            {

                if(colList == "Date")
                {
                    machineShopQry = machineShopQry.Where(s => s.Date.Equals(Convert.ToDateTime(searchString)));
                }
                if (colList == "Item No.")
                {
                    machineShopQry = machineShopQry.Where(s => s.ItemNo.Contains(searchString));
                }
                if (colList == "Operation")
                {
                    machineShopQry = machineShopQry.Where(s => s.Operation.Contains(searchString));
                }
                if (colList == "Operator")
                {
                    machineShopQry = machineShopQry.Where(s => s.Operator.Contains(searchString));
                }
                #region COMMENTED ANOTHER COLUMNS
                //if (colList == "Qty")
                //{
                //    machineShopQry = machineShopQry.Where(s => s.Qty.Equals(searchString));
                //}
                //if (colList == "Hours")
                //{
                //    machineShopQry = machineShopQry.Where(s => s.Hours.Equals(searchString));
                //}
                //if (colList == "Actual Rate")
                //{
                //    machineShopQry = machineShopQry.Where(s => s.ActualRate.Equals(searchString));
                //}
                //if (colList == "Standard Rate")
                //{
                //    machineShopQry = machineShopQry.Where(s => s.StandardRate.Equals(searchString));
                //}
                //if (colList == "Percent")
                //{
                //    machineShopQry = machineShopQry.Where(s => s.Percent.Equals(searchString));
                //}
                //if (colList == "Setup")
                //{
                //    machineShopQry = machineShopQry.Where(s => s.Setup.Equals(searchString));
                //}
                //if (colList == "Cleaning")
                //{
                //    machineShopQry = machineShopQry.Where(s => s.Cleaning.Equals(searchString));
                //}
                //if (colList == "Down")
                //{
                //    machineShopQry = machineShopQry.Where(s => s.Down.Equals(searchString));
                //}
                //if (colList == "Other")
                //{
                //    machineShopQry = machineShopQry.Where(s => s.Other.Equals(searchString));
                //}
                //if (colList == "Nonconf Parts")
                //{
                //    machineShopQry = machineShopQry.Where(s => s.NonconfParts.Equals(searchString));
                //}
#endregion
                if (colList == "Comments")
                {
                    machineShopQry = machineShopQry.Where(s => s.Comments.Contains(searchString));
                }
                
                //machineShopQry = machineShopQry.Where(s => s.Operator.Contains(searchString));
            }

            //CHECK BOX
            // true if &checkBoxState=true&checkBoxState=false
            if (checkBoxState=="true")
            {
                //SingleDate=;
                //ReleaseDate="";
                deptChoice = "";
                wcChoice = "";
                machineChoice = "";
                searchString = "";
                machineShopQry = from m in db.MainTableObj
                                     select m;

                //s = virtual lambda variable used just to initate this function and return result
            }

            if (SingleDate.HasValue) // single date pick box
            {
                machineShopQry = machineShopQry.Where(x => x.Date.Equals(SingleDate));
            }

            //Department filter
            if (!String.IsNullOrEmpty(deptChoice))
            {
                machineShopQry = machineShopQry.Where(s => s.Department.Equals(deptChoice));


            }
            //Work center filter
            if (!String.IsNullOrEmpty(wcChoice))
            {
                machineShopQry = machineShopQry.Where(s => s.WorkCenter.Equals(wcChoice));
            }
            //Machine filter
            if (!String.IsNullOrEmpty(machineChoice))
            {
                machineShopQry = machineShopQry.Where(s => s.Machine.Equals(machineChoice));
            }

            //return View(db.MainTableObj.ToList());

            //machineShopQry - model object
            return View(machineShopQry); 

        }

        //public static Expression<Func<TEntity, bool>> GetPropertyEqualityExpression<TEntity, TProperty>(string propertyName, TProperty propertyValue)
        //{
        //    var parameter = Expression.Parameter(typeof(TEntity));
        //    var property = Expression.Property(parameter, propertyName, null);
        //    var equality = Expression.Equal(property, Expression.Constant(propertyValue));
        //    var lambda = Expression.Lambda<Func<TEntity, bool>>(equality, parameter);
        //    return lambda;
        //}


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
        public ActionResult Create(string Machine,string Department,string WorkCenter, [Bind(Include="ID,Date,ItemNo,Operation,Operator,Qty,Hours,ActualRate,StandardRate,Percent,Setup,Cleaning,Down,Other,NonconfParts,Comments")] MachineShopTable machineshoptable)
        {
            //TempData["deptFromHome"] = Department;
            //TempData["workCentrFromHome"] = WorkCenter;

            //Set Departmetn and WorkCenter values for results of choice from Home view, during creation those field will be filled out automatically
            machineshoptable.Department = Department;
            machineshoptable.WorkCenter = WorkCenter;
            machineshoptable.Machine = Machine;
            //machineshoptable.Machine = machineChoice;
            

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

        public ActionResult Test() // Action result or different ?
        {
            dateTimeTest testModel = new dateTimeTest();
            testModel.todayTest = DateTime.Now;

            return RedirectToAction("Index", testModel);
        }
    }
}

