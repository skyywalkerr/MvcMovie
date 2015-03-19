using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MvcMovie.Models;
using System.IO;
using System.Web.UI.WebControls;
using System.Web.UI;
using RazorPDF;

namespace MvcMovie.Controllers
{

    public class MoviesController : Controller
    {
        private MovieDBContext db = new MovieDBContext(); 
        // MOST IMPORTANT CONNECTION OBJECT WITH DATABASE, instance of MovieDBContect class
        // derived from DbContext class in Movie.cs model
        // db -  is a object of a class from Movie Model class, who can access the database.
        // read, write, delete, etc.

        //ORYGINAL COPY
        //// GET: /Movies/
        //public ActionResult Index()
        //{
        //    return View(db.Movies.ToList());
        //}

        // GET: /Movies/
        //public ActionResult Index(string id) // use ID portion of URL to transport search string
        //{
        //    string searchString = id;
        //    var movies = from m in db.Movies
        //                 select m;

        //    if(!String.IsNullOrEmpty(searchString))
        //    {
        //        movies = movies.Where(s => s.Contains.Title(searchString)); 
        //        //s = virtual lambda variable used just to initate this function and return result
        //    }

        //    return View(movies);
        //}

        public ActionResult Index(string movieGenre, string searchString) // use ID portion of URL to transport search string
        {

            //test of session var from machine controller
            //ViewBag.sessionTest = Session["test1"].ToString();
            var GenreLst = new List<string>(); // list to store results

            var GenreQry = from a in db.Movies
                           orderby a.Genre 
                           select a.Genre; //get particular column

            GenreLst.AddRange(GenreQry.Distinct()); //ADD THE SPECYFIED ELEMENTS TO THE END IF A LIST
            ViewBag.movieGenre = new SelectList(GenreLst);
        

            var listOptions = new List<string>();
            string[] listData = new string[] { "Title", "ReleaseDate","Genre","" };         
            listOptions.AddRange(listData);
            ViewBag.movieList = new SelectList(listOptions);


            var movies = from m in db.Movies
                         select m;

            if (!String.IsNullOrEmpty(searchString)) // check if string isa not mull
            {
                movies = movies.Where(s => s.Title.Contains(searchString));
                //s = virtual lambda variable used just to initate this function and return result
            }

            if (!String.IsNullOrEmpty(movieGenre))
            {
                movies = movies.Where(x => x.Genre == movieGenre);
                //s = virtual lambda variable used just to initate this function and return result
            }


            // CALCULATE SUM

            ViewBag.totalSUM = 0;

            // CALCULATE SUM - end


            return View(movies);
        }

        // GET: /Movies/Details/5
        public ActionResult Details(int? id) 
            // id is a element of routing data- from URL, 
            // localhost:1234/movies/details/1 
            // Action = Details
            // ID = 1
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movies.Find(id); // ?????

            //that what is does ?
            //Movie movie = new Movie();
            //movie.ID = db.Movies.Find(id);

            // instance object of a Movie Model class, this way CONTROLLER ACCESSING MODEL
            // instance object "movie" from Movie controller is being set by 
            // result of "db" instance object from the MveDBContect class from Model.
            // MovieDbContect derived from DbContect entity framework class.
            // and using one of methods : FIND 

            //Finds an entity with the given primary key values. 
            //If an entity with the given primary key values exists in the context,
            //then it is returned immediately without making a request to the store. 
            //Otherwise, a request is made to the store for an entity with the given 
            //primary key values and this entity, if found, is attached to the context
            //and returned. If no entity is found in the context or the store, then null is returned.

            // link: msdn.microsoft.com/en-us/library/gg696418%28v=vs.113%29.aspx
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie); // if movie is found an instance of Movie model is passed to the details view.
        }

        // GET: /Movies/Create
        public ActionResult Create()
        {   
            
            return View();
        }

        // POST: /Movies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost] // POST IS CREATION METHOD - create new record in respond to cinfomration from view
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID,Title,ReleaseDate,Genre,Price,Rating")] Movie movie)
        {
            if (ModelState.IsValid)
            {
                //db.Movies.
                db.Movies.Add(movie);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(movie);
        }

        // GET: /Movies/Edit/5
        public ActionResult Edit(int? id) // ID value will be taken from ActionMethod from Index VIEW

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        // POST: /Movies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost] 
        // attribute.  
        //This attribute specifies that that overload of the Edit method can be invoked only for POST requests.
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,Title,ReleaseDate,Genre,Price,Rating")] Movie movie)
        {
            if (ModelState.IsValid)
            {
                db.Entry(movie).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(movie);
        }

        // GET: /Movies/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        // POST: /Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Movie movie = db.Movies.Find(id);
            db.Movies.Remove(movie);
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

        public ActionResult ExportData()
        {
            GridView gv = new GridView();
            gv.DataSource = db.Movies.ToList(); // Movies - name of database
            gv.DataBind();
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

            return RedirectToAction("StudentDetails");
        }

        //public ActionResult Pdf()
        //{
        //    var 
        //}

    }
}
