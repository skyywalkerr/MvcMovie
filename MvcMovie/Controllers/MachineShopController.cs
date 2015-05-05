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
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;
using System.Xml.Linq;
using Rotativa;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;                                                                                    



namespace MvcMovie.Controllers
{
    public class MachineShopController : Controller
    {
        private MachineShopDB db = new MachineShopDB();
        //MachineShopTable mainTableObject = new MachineShopTable();

        // GET: /MachineShop/
        public ActionResult Index(string jsWorkCenter,string partChoice2, string operatorChoice2, string partChoice, string operatorChoice, string WorkCenter, string Department, string machineChoice, string deptChoice,string Departments, string wcChoice, string checkBoxState, string searchString, string colList, DateTime? SingleDate, DateTime? DateStart, DateTime? DateEnd)
        //public ActionResult Index(string WorkCenter, string Department, string machineChoice, string deptChoice, string wcChoice, string checkBoxState, string searchString, string colList)
        {
            

            Session["preciseQ"] = null;
            Session["WorkCenterAll"] = null;
            string sessionDepartmentString = null;
            string sessionWorkCenterString = null;            

            //DateTime StartDateAuto = Convert.ToDateTime(DayOfWeek.Monday);
            //DateTime EndDateAuto = Convert.ToDateTime(DayOfWeek.Friday); 
            var today = DateTime.Today;
            //DateTime friday = today.AddDays(-(int)today.DayOfWeek).AddDays(5).Date;
            
            //DateTime saturday = today.AddDays(-(int)today.DayOfWeek).AddDays(6).Date; // Saturday of current week
            DateTime saturday = today.AddDays(-(int)today.DayOfWeek - 7).AddDays(6).Date; // // Saturday of week ago

            DateTime StartDateAuto = DateTime.Now.StartOfWeek(DayOfWeek.Monday);
            DateTime EndDateAuto = saturday;

            string TimeFormat = "MM/d/yyyy";
            //Session["StartDateAutoStr"] = StartDateAuto.ToString(TimeFormat);
            //Session["EndDateAutoStr"] = EndDateAuto.ToString(TimeFormat);
            //Session["SingleDateStr"] = null;
            

            // BBY DEFAULT SHOW EVERYTHING
            var machineShopQry = from m in db.MainTableObj                                 
                                 select m;
            ////machineShopQry = machineShopQry.Where(p => p.Date >= StartDateAuto && p.Date <= EndDateAuto).OrderBy(p => p.Date);
            /////////////
            ////if ((Session["Department"] != null) && (Session["WorkCenter"] != null))
            ////{
            ////    sessionDepartmentString = Session["Department"].ToString();
            ////    sessionWorkCenterString = Session["WorkCenter"].ToString();
 
            ////    machineShopQry = machineShopQry.Where(p => p.Date >= StartDateAuto && p.Date <= EndDateAuto).OrderBy(p => p.Date);

            ////    machineShopQry = from m in db.MainTableObj
            ////                     where m.WorkCenter == sessionWorkCenterString && m.Department == sessionDepartmentString
            ////                     orderby m.Date ascending
            ////                     select m;
            ////}
            /////////////


            //machineShopQry = machineShopQry.Where(p => p.Date >= StartDateAuto && p.Date <= EndDateAuto);
            //Session["DateStr"] = StartDateAuto.ToString(TimeFormat) + " -- " + EndDateAuto.ToString(TimeFormat);

            if ((!String.IsNullOrEmpty(Department)) && (!String.IsNullOrEmpty(WorkCenter)))
            {
                Session["Department"] = Department;
                Session["WorkCenter"] = WorkCenter;

                sessionDepartmentString = Session["Department"].ToString();
                sessionWorkCenterString = Session["WorkCenter"].ToString(); 

                
                //Display results just in this date frame                

                machineShopQry = from m in db.MainTableObj
                                 where m.WorkCenter == sessionWorkCenterString && m.Department == sessionDepartmentString
                                 //where m.Date == System.DateTime.Today
                                 orderby m.Date ascending
                                 select m;
                machineShopQry = machineShopQry.Where(p => p.Date >= StartDateAuto && p.Date <= EndDateAuto).OrderBy(p => p.Date);

                //Session["DateStr"] = StartDateAuto.ToString(TimeFormat) + " -- " + EndDateAuto.ToString(TimeFormat);
                Session["DateStr"] = StartDateAuto.ToString(TimeFormat) + " -- " + EndDateAuto.ToString(TimeFormat);

                string[] wcTable = new string[1];
                wcTable[0] = sessionWorkCenterString; //adding all list elements to array
                ViewBag.manyWC = wcTable;
            }
            else
            {

                if ((Session["Department"] != null) && (Session["WorkCenter"]==null) )
                {                                        

                    sessionDepartmentString = Session["Department"].ToString();
                    sessionWorkCenterString = null; 

                    machineShopQry = from m in db.MainTableObj
                                     where m.Department == sessionDepartmentString
                                     orderby m.Date ascending
                                     //where m.Date == System.DateTime.Today
                                     select m;
                    if ((!SingleDate.HasValue) && (!DateStart.HasValue) && (!DateEnd.HasValue))
                    {
                        machineShopQry = machineShopQry.Where(p => p.Date >= StartDateAuto && p.Date <= EndDateAuto).OrderBy(p => p.Date);

                        Session["DateStr"] = StartDateAuto.ToString(TimeFormat) + " -- " + EndDateAuto.ToString(TimeFormat);
                    }

                    //DISPLAY ALL MACHINES FOR CHOOSEN DEPT                  
                    var allMachines = new List<string>();            
                    var wcQueryAll = from c in db.Machines
                          //where c.Department == deptChoice
                          where c.Department == sessionDepartmentString
                          select c.WorkCenter;
                    allMachines.AddRange(wcQueryAll.Distinct());
                    var result = String.Join(", ", allMachines.ToArray());
                    Session["WorkCenterAll"] = result;
                    
                    string[] wcTable = allMachines.ToArray(); //adding all list elements to array
                    ViewBag.manyWC = wcTable;
                          
                }

                if ((Session["Department"] == null) && (Session["WorkCenter"] == null))
                {
                    Session["Department"] = null;
                    Session["WorkCenter"] = null;

                    sessionDepartmentString = null;
                    sessionWorkCenterString = null; 

                    machineShopQry = from m in db.MainTableObj
                                     //where m.Date == System.DateTime.Today
                                     orderby m.Date ascending
                                     select m;
                    if ((!SingleDate.HasValue) && (!DateStart.HasValue) && (!DateEnd.HasValue))
                    {
                        machineShopQry = machineShopQry.Where(p => p.Date >= StartDateAuto && p.Date <= EndDateAuto).OrderBy(p => p.Date);
                        Session["DateStr"] = StartDateAuto.ToString(TimeFormat) + " -- " + EndDateAuto.ToString(TimeFormat);
                    }
                }
            }

            
            
            

            //VERY MAIN QUERY 
            

            // CALCULATE SUM variable
            ViewBag.totalSumQty = 0;
            ViewBag.totalSumHours = 0;
            ViewBag.totalSumSetup = 0;
            ViewBag.totalSumCleaning = 0;
            ViewBag.totalSumDown = 0;
            ViewBag.totalSumOther = 0;
            ViewBag.totalSumNonC = 0;

            // CALCULATE SUM - end            

            //INCOMING DATA//
            //This view data sending results of action link from HOME view to Index view of MachineShop controller
            //ViewData["Department"] = Department;
            //ViewData["WorkCenter"] = WorkCenter;
            
            //TempData sending data between action methods of same controller
            //TempData["deptFromHome"] = Department;
            //TempData["workCentrFromHome"] = WorkCenter;

            
            //INCOMING DATA -end//

            #region Currently DISPLAYED

            //var machLst = new List<string>();
            
            //return a first and only position for machine name, depend on name of depratment and workcenter.
            //just for display purposes


            var machTxtQry = from m in db.Machines
                                   where m.Department == sessionDepartmentString && m.WorkCenter == sessionWorkCenterString
                                   select m.Machine;
            
            //string machineResult = Convert.ToString(machTxtQry.First());
            //if((machTxtQry != null) && (Department !=null) && (WorkCenter !=null))
            if ((!String.IsNullOrEmpty(sessionDepartmentString)) && (!String.IsNullOrEmpty(sessionWorkCenterString)))
            {
                if (machTxtQry.Any())
                { 
                    //ViewBag.machineTxt = Convert.ToString(machTxtQry.First());
                    Session["machineTxt"] = Convert.ToString(machTxtQry.First());
                }
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

            //Session["WCamount"] = wcLst.Count();

            //Column names list

            var columnsList = new List<string>();
            //string[] listData = new string[] { "Date", "Item No.", "Operation","Operator","Qty","Hours","Standarad Rate",
            //"Percent","Setup","Cleaning","Down","Other","Nonconf Parts","Comments" };
            string[] listData = new string[] { "Item No.", "Operation","Operator","Comments" };
            columnsList.AddRange(listData);
            ViewBag.colList = new SelectList(columnsList);

            //LISTS//end

            //unique Operator list
            var operatorList = new List<string>();
            var operatorsQry = from x in db.MainTableObj
                               select x.Operator;
            operatorList.AddRange(operatorsQry.Distinct());
            ViewBag.operatorChoice = new SelectList(operatorList);

            //unique Operator list END

            //item no list
            var partsList = new List<string>();
            var partsQry = from x in db.MainTableObj
                           select x.ItemNo;
            partsList.AddRange(partsQry.Distinct());
            ViewBag.partChoice = new SelectList(partsList);
            //item no list - end

            //operator and item no list            
            ViewBag.partChoice2 = new SelectList(partsList);
            ViewBag.operatorChoice2 = new SelectList(operatorList);
            //item no list - end

            #endregion

            //Searcher

            if(!String.IsNullOrEmpty(searchString))
            {

                //if(colList == "Date")
                //{
                //    //machineShopQry = machineShopQry.Where(s => s.Date.Equals(Convert.ToDateTime(searchString)));
                //    //machineShopQry = machineShopQry.Where(x => x.Date.Equals(searchString));
                //    machineShopQry = machineShopQry.Where(x => x.Date.Equals(Convert.ToDateTime(searchString)));

                //}
                if (colList == "Item No.")
                {
                    machineShopQry = machineShopQry.Where(s => s.ItemNo.Contains(searchString)).OrderBy(s => s.Date);
                    Session["preciseQ"] += "Item No. : " + searchString + " | ";
                }
                if (colList == "Operation")
                {
                    machineShopQry = machineShopQry.Where(s => s.Operation.Contains(searchString)).OrderBy(s => s.Date);
                    Session["preciseQ"] += "Operation : " + searchString + " | ";
                }
                if (colList == "Operator")
                {
                    machineShopQry = machineShopQry.Where(s => s.Operator.Contains(searchString)).OrderBy(s => s.Date);
                    Session["preciseQ"] += "Operator : " + searchString + " | ";
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
                    machineShopQry = machineShopQry.Where(s => s.Comments.Contains(searchString)).OrderBy(s => s.Date);
                    Session["preciseQ"] += "Comments : " + searchString + " | ";
                }
                
                //machineShopQry = machineShopQry.Where(s => s.Operator.Contains(searchString));
            }

            //CHECK BOX
            // true if &checkBoxState=true&checkBoxState=false
            if (checkBoxState=="true")
            {
                //SingleDate=;
                //ReleaseDate="";

                ////deptChoice = "";
                ////wcChoice = "";
                ////machineChoice = "";
                Session["hideCRUD"] = null;

                Session["Department"] = null;
                Session["WorkCenter"] = null;
                Session["machineTxt"] = null;
                Session["DateStr"] = "From beginning of times.";

                //Session["DepartmentDescription"] = "Department 1010, Department 1020, Department 1030.";
                //Session["WorkCenterDescription"] = "All Work Centers";
                //Session["machineTxtDescription"] = "All Machines";

                searchString = "";
                machineShopQry = from m in db.MainTableObj
                                 orderby m.Date ascending
                                     select m;

                Session["preciseQ"] += "Show all data from database.";
                //s = virtual lambda variable used just to initate this function and return result
            }

            

            //Department filter
            //////if (!String.IsNullOrEmpty(deptChoice))
            //////{
            //////    //machineShopQry = from m in db.MainTableObj
            //////    //                     where m.Department == Department
            //////    //                     //where m.Date == System.DateTime.Today
            //////    //                     select m;

            //////    machineShopQry = machineShopQry.Where(s => s.Department.Equals(deptChoice));
            //////    Session["Department"] = deptChoice;
            //////}

            if (Session["Department"] != null)
            {
                string dept = Session["Department"].ToString();
                machineShopQry = machineShopQry.Where(s => s.Department.Equals(dept)).OrderBy(s => s.Date);
            }

            //Work center filter
            //////if (!String.IsNullOrEmpty(wcChoice))
            //////{
            //////     //machineShopQry = from m in db.MainTableObj
            //////     //                    where m.WorkCenter == WorkCenter
            //////     //                    //where m.Date == System.DateTime.Today
            //////     //                    select m;

            //////    machineShopQry = machineShopQry.Where(s => s.WorkCenter.Equals(wcChoice));
            //////    Session["WorkCenter"] = wcChoice;
            //////}    

            if (Session["WorkCenter"]!= null)
            {
                //machineShopQry = from m in db.MainTableObj
                //                    where m.WorkCenter == WorkCenter
                //                    //where m.Date == System.DateTime.Today
                //                    select m;
                string wc = Session["WorkCenter"].ToString();
                machineShopQry = machineShopQry.Where(s => s.WorkCenter.Equals(wc)).OrderBy(s => s.Date);

                string[] wcTable = new string[1];
                wcTable[0] = wc; //adding all list elements to array
                ViewBag.manyWC = wcTable;
            }



            //Machine filter
            //////if (!String.IsNullOrEmpty(machineChoice))
            //////{
            //////    machineShopQry = machineShopQry.Where(s => s.Machine.Equals(machineChoice));
            //////    Session["machineTxt"] = machineChoice;
            //////}

            if (Session["machineTxt"]!= null)
            {
                string mach = Session["machineTxt"].ToString();
                machineShopQry = machineShopQry.Where(s => s.Machine.Equals(mach)).OrderBy(s => s.Date);
                //Session["preciseQ"] += mach+" - ";
            }

            //Operator filter
            if (!String.IsNullOrEmpty(operatorChoice))
            {
                machineShopQry = machineShopQry.Where(s => s.Operator.Equals(operatorChoice));
                Session["preciseQ"] +="Operator : "+operatorChoice + " | ";
            }
            //Parts filter
            if (!String.IsNullOrEmpty(partChoice))
            {
                machineShopQry = machineShopQry.Where(s => s.ItemNo.Equals(partChoice)).OrderBy(s => s.Date);
                Session["preciseQ"] += "Item No. : " + partChoice + " | ";
            }
            //Operator and Parts filter
            if (!String.IsNullOrEmpty(partChoice2))
            {
                machineShopQry = machineShopQry.Where(s => s.ItemNo.Equals(partChoice2)).OrderBy(s => s.Date);
                Session["preciseQ"] += "Item No. : " + partChoice2 + " | ";
            }
            if (!String.IsNullOrEmpty(operatorChoice2))
            {
                machineShopQry = machineShopQry.Where(s => s.Operator.Equals(operatorChoice2)).OrderBy(s=>s.Date);
                Session["preciseQ"] += "Operator : " + operatorChoice2 + " | ";
            }

            if (SingleDate.HasValue) // single date pick box
            {
                //machineShopQry = machineShopQry.Where(x => x.Date.Equals(SingleDate));
                //machineShopQry = machineShopQry.Where(x => x.Date.Equals(Convert.ToDateTime(SingleDate)));

                machineShopQry = machineShopQry.Where(x => x.Date == SingleDate).OrderBy(x => x.Date);

                Session["DateStr"] = SingleDate.Value.ToString(TimeFormat);
            }

            if (DateStart.HasValue && DateEnd.HasValue)
            {
                //machineShopQry = machineShopQry.Where(x => x.Date == SingleDate);

                //machineShopQry = from x in db.MainTableObj
                //                 where DateStart >= x.Date 
                //                 && DateEnd <= x.Date
                //                 select x;
                machineShopQry = machineShopQry.Where(p => p.Date >= DateStart && p.Date <= DateEnd).OrderBy(p => p.Date);

                Session["DateStr"] = DateStart.Value.ToString(TimeFormat) + " -- " + DateEnd.Value.ToString(TimeFormat);
            }

            if ((!SingleDate.HasValue) && (!DateStart.HasValue) && (!DateEnd.HasValue))
            {
                machineShopQry = machineShopQry.Where(p => p.Date >= StartDateAuto && p.Date <= EndDateAuto).OrderBy(p => p.Date);

                Session["DateStr"] = StartDateAuto.ToString(TimeFormat) + " -- " + EndDateAuto.ToString(TimeFormat);
            }

            //Operator and Parts filter - end

            //machineShopQry - model object which will be a collection of all queries through a controller, qr = qr1 + qr2, result of all

            //List<string> headerList = new List<string>;
            //headerList.Add("1");
            //headerList.Add("2");

            //List<string> footerList = new List<string>;
            //footerList.Add("3");
            //footerList.Add("4");
            
            //var listMSQ = machineShopQry.ToList(); 
            //listMSQ.AddRange(footerList);

            Session["machineShopQry"] = machineShopQry.ToList();
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
            //machineshoptable.Department = Department;
            //machineshoptable.WorkCenter = WorkCenter;
            //machineshoptable.Machine = Machine;
            
            machineshoptable.Department = Session["Department"].ToString();
            machineshoptable.WorkCenter = Session["WorkCenter"].ToString();
            machineshoptable.Machine = Session["machineTxt"].ToString();

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
        public ActionResult Edit([Bind(Include="ID,Date,ItemNo,Operation,Operator,Qty,Hours,ActualRate,StandardRate,Percent,Setup,Cleaning,Down,Other,NonconfParts,Comments,WorkCenter,Machine,Department")] MachineShopTable machineshoptable)
        {

            machineshoptable.Department = Session["Department"].ToString();
            machineshoptable.WorkCenter = Session["WorkCenter"].ToString();
            machineshoptable.Machine = Session["machineTxt"].ToString();
             

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

         //public ActionResult ExportData2()
         //{
         //   var machineShopQry = Session["machineShopQry"];
         //   GridView gv = new GridView();
         //   //gv.DataSource = db.MainTableObj.ToList(); // Movies - name of database            
            
         //   gv.DataSource = machineShopQry; // Movies - name of database
            
         //   gv.DataBind();

            
         //   //DumpExcel(gv);

         // }

        public ActionResult ExportData()
        {
            //Excel.Application xlApp = new Excel.Application();
 
            var machineShopQry = Session["machineShopQry"];
            GridView gv = new GridView();
            //gv.DataSource = db.MainTableObj.ToList(); // Movies - name of database            
            
            gv.DataSource = machineShopQry; // Movies - name of database
            
            gv.DataBind();

            
            //gv.HeaderRow.Style.Add("background-color", "#E2F1F9");
            gv.HeaderRow.Cells[0].Visible = false;
            gv.HeaderRow.Cells[16].Visible = false;
            gv.HeaderRow.Cells[17].Visible = false;                          
            gv.HeaderRow.Cells[18].Visible = false;            

            gv.HeaderRow.Cells[1].Style.Add("background-color", "#E2F1F9");
            gv.HeaderRow.Cells[2].Style.Add("background-color", "#E2F1F9");
            gv.HeaderRow.Cells[3].Style.Add("background-color", "#E2F1F9");
            gv.HeaderRow.Cells[4].Style.Add("background-color", "#E2F1F9");
            gv.HeaderRow.Cells[5].Style.Add("background-color", "#E2F1F9");
            gv.HeaderRow.Cells[6].Style.Add("background-color", "#E2F1F9");
            gv.HeaderRow.Cells[7].Style.Add("background-color", "#E2F1F9");
            gv.HeaderRow.Cells[8].Style.Add("background-color", "#E2F1F9");
            gv.HeaderRow.Cells[9].Style.Add("background-color", "#E2F1F9");
            gv.HeaderRow.Cells[10].Style.Add("background-color", "#E2F1F9");
            gv.HeaderRow.Cells[11].Style.Add("background-color", "#E2F1F9");
            gv.HeaderRow.Cells[12].Style.Add("background-color", "#E2F1F9");
            gv.HeaderRow.Cells[13].Style.Add("background-color", "#E2F1F9");
            gv.HeaderRow.Cells[14].Style.Add("background-color", "#E2F1F9");
            gv.HeaderRow.Cells[15].Style.Add("background-color", "#E2F1F9");
            
            System.Web.UI.WebControls.GridView test = new System.Web.UI.WebControls.GridView();
            ////////////////////////
            var rowsNumber = gv.Rows.Count; // total number of rows in data table stsrts from 0
            for (int i = 0; i < gv.Rows.Count; i++)
            {

                GridViewRow row = gv.Rows[i];

                //row.Cells[1].Text = rowsNumber.ToString();
                row.Cells[0].Visible = false;
                row.Cells[16].Visible = false;
                row.Cells[17].Visible = false;
                row.Cells[18].Visible = false;
   
            }

            //test.Rows[rowsNumber + 2].Cells[6].Text = "6x6";

            //gv.Rows[rowsNumber + 2].Cells[6].Text = "6x6";
            //gv.Rows[rowsNumber + 2].Cells[7].Text = "7x6";
            //gv.Rows[rowsNumber + 2].Cells[11].Text = "11x6";
            //gv.Rows[rowsNumber + 2].Cells[12].Text = "12x6";
            //gv.Rows[rowsNumber + 2].Cells[13].Text = "13x6";
            //gv.Rows[rowsNumber + 2].Cells[14].Text = "14x6";

            ////////////////


            Response.ClearContent();
            Response.Buffer = true;
            //Response.AddHeader("content-disposition", "attachment; filename=MachineShop.pdf");
            //Response.ContentType = "application/pdf";

            Response.AddHeader("content-disposition", "attachment; filename=Marklist.xls");
            Response.ContentType = "application/ms-excel";

            Response.Charset = "";
            StringWriter sw = new StringWriter();

            HtmlTextWriter htw = new HtmlTextWriter(sw);
            gv.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

            return RedirectToAction("Index");
        }

        public JsonResult WorkCenterList(string Departments)
        {
            
            var wcLst = new List<string>();            
            var wcQuery = from c in db.Machines
                          //where c.Department == deptChoice
                          where c.Department == Departments
                          select c.WorkCenter;
            wcLst.AddRange(wcQuery.Distinct());            
            //ViewBag.wcChoice = new SelectList(wcLst);            
            Session["Department"] = Departments;
            Session["WorkCenter"] = null; //Can be initialize by js WorkCenter drop down list click
            Session["machineTxt"] = null; //Can be initialize by js WorkCenter drop down list click 

            return Json(wcLst);
            
        }

        public JsonResult MachineList(string jsWorkCenter)
        {
            var machineLst = new List<string>();
            var machineQuery = from a in db.Machines
                               where a.WorkCenter == jsWorkCenter
                               select a.Machine;
            machineLst.AddRange(machineQuery.Distinct());
            //ViewBag.machineChoice = new SelectList(machineLst);

            Session["WorkCenter"] = jsWorkCenter;
            Session["machineTxt"] = machineLst.First();

            string[] wcTable = new string[1];
            wcTable[0] = jsWorkCenter; //adding all list elements to array
            ViewBag.manyWC = wcTable;

            return Json(machineLst);
        }

        public ActionResult PrintInvoice()
        {
            return new ActionAsPdf( "Index" ) { FileName = "Invoice.pdf" };
        }
        
        /////////////
        private void DumpExcel(DataTable tbl)
        {
            using (ExcelPackage pck = new ExcelPackage())
            {
                //Create the worksheet
                ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Demo");

                //Load the datatable into the sheet, starting from cell A1. Print the column names on row 1
                ws.Cells["A1"].LoadFromDataTable(tbl, true);

                //Format the header for column 1-3
                using (ExcelRange rng = ws.Cells["A1:C1"])
                {
                    rng.Style.Font.Bold = true;
                    rng.Style.Fill.PatternType = ExcelFillStyle.Solid;                      //Set Pattern for the background to Solid
                    rng.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(79, 129, 189));  //Set color to dark blue
                    rng.Style.Font.Color.SetColor(Color.White);
                }

                //Example how to Format Column 1 as numeric 
                using (ExcelRange col = ws.Cells[2, 1, 2 + tbl.Rows.Count, 1])
                {
                    col.Style.Numberformat.Format = "#,##0.00";
                    col.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                }

                //Write it back to the client
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;  filename=ExcelDemo.xlsx");
                Response.BinaryWrite(pck.GetAsByteArray());
            }
        }

        //public ActionResult Test() // Action result or different ?
        //{
        //    dateTimeTest testModel = new dateTimeTest();
        //    testModel.todayTest = DateTime.Now;

        //    return RedirectToAction("Index", testModel);
        //}
    }

    // adding Day of a week functionality to a date time - extending existing funciton
    public static class DateTimeExtensions
    {
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = dt.DayOfWeek - startOfWeek;
            if (diff < 0)
            {
                diff += 7;
            }

            //return dt.AddDays(-1 * diff).Date; //Curretn week Monday
            return dt.AddDays(-1 * diff -7).Date;// Monday a week ago            
        }
        public static DateTime EndOfWeek(this DateTime dt, DayOfWeek endOfWeek)
        {
            var daysTillFriday = (int)dt.DayOfWeek - (int)endOfWeek;
            //return dt.AddDays(daysTillFriday).Date;//Current week Saturday
            return dt.AddDays(daysTillFriday -7).Date;//Saturday a week ago
        }
    }


}

