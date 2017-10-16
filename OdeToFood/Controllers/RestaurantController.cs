using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OdeToFood.Models;
using System.IO;
using OdeToFood.Repositories;
using PagedList;

namespace OdeToFood.Controllers
{
    
    public class RestaurantController : Controller
    {
        private OdeToFoodDb db = new OdeToFoodDb();
        //private IRepository db;
        private IRepository restaurantRepository;

        public RestaurantController(IRepository restaurantRepository)
        {
            this.restaurantRepository = restaurantRepository;
        }

        public RestaurantController()
        {
            this.db = new OdeToFoodDb();
        }

        //This method is just for the purpose of unit testing
        public ViewResult MyTestMethod()
        {
            var restaurants = restaurantRepository.GetAll(); //Calls the GetAll() method declared in the IRepository 
            return View(restaurants);
        }

        //RestaurantRepository repo = new RestaurantRepository();


        public ActionResult ByDate(int page = 1)
        {
            var date= DateTime.Now.Date.AddDays(-7);
            var model = db.Restaurants.Where(m => m.Date >= date).ToList().ToPagedList(page, 10);//Getting the Restaurants that were added in the past 7 days.
            ViewBag.MyDate = "7 Days";
            return View("DisplayList",model);
        }

        public ViewResult ByDateMonth(int page = 1)
        {
            var date = DateTime.Now.Date.AddDays(-30);
            var model = db.Restaurants.Where(m => m.Date >= date).ToList().ToPagedList(page, 10);//Getting the Restaurants that were added in the past 30 days.
            ViewBag.MyDate = "30 Days";
            return View("DisplayList", model);
        }

        public ActionResult Review()
        {
            return View("ReviewRestaurant");
        }

        public ActionResult GetMyRestaurants()
        {
            var UserRestaurants = db.Restaurants.Where(m => m.UserName == User.Identity.Name).ToList();
            return View(UserRestaurants);

            //return View(repo.GetUserRestaurants());//Extra
        }

        public ActionResult Decide(string search, string searchBy, int page = 1)
        {
            if (searchBy == "Name")
            {
                var model = db.Restaurants
                    .Where(r => r.Name.StartsWith(search)).ToList().ToPagedList(page, 10);

                return View("DisplayList", model);
            }
            else
            {
                var model = db.Restaurants
                    .Where(r => r.City.StartsWith(search)).ToList().ToPagedList(page, 10);
                return View("DisplayList", model);
            }


            //return View("Index3", repo.Decide(search, searchBy));//EXTRA
        }


        public ActionResult Check(string searchTerm, int page = 1)
        {
            if(searchTerm != ""){
                var model =
                    db.Restaurants
                    .Where(r => searchTerm == null || r.Name.StartsWith(searchTerm)).ToList().ToPagedList(page,10); //The first part is saying that if the paramater is null (i.e. no specific search was entered), then return all the results.
                
                //    .Select(r => new RestaurantListViewModel //Creating an anonymous type.
                //{
                //        Id = r.Id,//These properties are coming from the ViewModel and we are assigning to them whatever is the corresponding value from 'r', which is coming from our '_db.Restaurants'.
                //    Name = r.Name,
                //        Description = r.Description,
                //        Keyword = r.Keyword,
                //        City = r.City,
                //        CountOfReviews = r.Reviews.Count()
                  

                ViewBag.SearchTerm = searchTerm;
                return View("DisplayList", model);

                //return View("Index", repo.Check(searchTerm));//EXTRA
            }else{
                return RedirectToAction("DisplayList");//If the search result comes back empty, then all results will be displayed
            }
        }


        public ActionResult DisplayList(int page = 1)
        {
            var model = db.Restaurants.ToList().ToPagedList(page, 10);
            return View(model);

            //return View(repo.DisplayList());//EXTRA
        }

        // GET: Restaurant
        [HttpGet]
        public ActionResult Index()
        {         
            return View("Landing");
        }

        [HttpPost]
        public ActionResult Index(Restaurant model)
        {
            return View(model);
            //return View(db.Restaurants.ToList());
        }

        public ActionResult Search(string search, string searchBy)
        {
            if (searchBy == "Restaurant")
            {
                var model = db.Restaurants
                    .Where(r => search == null || r.Name.StartsWith(search)).ToList();
                return View("Index", model);

                //return View("Index", repo.Search(search));//EXTRA

            }
            else
            {
                return RedirectToAction("Index", "Recipes", search);
            }
        }

        public ActionResult Top(int page = 1)
        {
            var model = db.Reviews.OrderByDescending(r => r.Rating)//This Lambda expression is retrieving the top 5 reviews
                .Take(5).ToList().ToPagedList(page, 10);
            return View(model);

            //return View(repo.Top());//EXTRA
        }

        public ActionResult County(int page = 1)
        {
            var model = db.Restaurants
                .Where(r => r.City == "Dublin").ToList().ToPagedList(page, 10);
            return View(model);

            //return View(repo.County());//EXTRA
        }

        // GET: Restaurant/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Restaurant restaurant = db.Restaurants.Find(id);

            if (restaurant == null)
            {
                return HttpNotFound();
            }
            return View(restaurant);



            //if (repo.Details(id) == null)//EXTRA
            //{
            //    return HttpNotFound();
            //}
            //return View(repo.Details(id));//EXTRA
        }

        // GET: Restaurant/Create
        //Only logged in users can create a restaurant.
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Restaurant/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Restaurant restaurant, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                //string fileName = Path.GetFileNameWithoutExtension(restaurant.ImageFile.FileName);
                //string extension = Path.GetExtension(restaurant.ImageFile.FileName);
                //fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                //restaurant.ImagePath = "~/Images/" + fileName;
                //fileName = Path.Combine(Server.MapPath("~/Images/"), fileName);
                //restaurant.ImageFile.SaveAs(fileName);

                //restaurant.ImageFile.SaveAs(Server.MapPath("~/Images/") + ".jpg");

                //This code sourced from: http://stackoverflow.com/questions/16255882/uploading-displaying-images-in-mvc-4
                if (file != null)
                {
                    file.SaveAs(HttpContext.Server.MapPath("~/Images/")
                                                          + file.FileName);
                    restaurant.ImagePath = file.FileName;
                }

                restaurant.Date = DateTime.Today;//Setting the Restaurant's date property to today's date.
                restaurant.UserName = User.Identity.Name;//This allows me to match the Restaurant's UserName property with the UserName of the currently logged in user.
                db.Restaurants.Add(restaurant);
                db.SaveChanges();

                //restaurant = repo.Create(restaurant);//EXTRA

                TempData["Message"] = "Your entry was successfully added";//This TempData will survive to the next request but not after that (i.e. if we refresh the page again, the message will disappear).
                return RedirectToAction("DisplayList");//We want to redirect the user here rather than simply return a View so the latest request in the system is a Get Request and not a Post request to the Create method, which otherwise would have been there (The latest entry in the browser's history would be a post request. Therefore, refeshing the page wpuld cause the post request to be sent to the server again).
            }else
            {
                return View(restaurant);
            }

           
        }

        // GET: Restaurant/Edit/5
        public ActionResult Edit(int? id)//The question mark means the parameter can be nullable
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Restaurant restaurant = db.Restaurants.Find(id);
            if (restaurant == null)
            {
                return HttpNotFound();
            }
            return View(restaurant);


            //if (repo.EditGet(id) == null)//EXTRA
            //{
            //    return HttpNotFound();
            //}
            //return View(repo.EditGet(id));//EXTRA
        }

        // POST: Restaurant/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit( Restaurant restaurant)
        {
            if (ModelState.IsValid)
            {
                restaurant.UserName = User.Identity.Name;
                db.Entry(restaurant).State = EntityState.Modified;
                db.SaveChanges();

                //restaurant = repo.EditPost(restaurant);//EXTRA

                return RedirectToAction("Index");
            }
            return View(restaurant);
        }

        // GET: Restaurant/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Restaurant restaurant = db.Restaurants.Find(id);
            if (restaurant == null)
            {
                return HttpNotFound();
            }
            return View(restaurant);


            //if (repo.DeleteGet(id) == null)//EXTRA
            //{
            //    return HttpNotFound();
            //}
            //return View(repo.DeleteGet(id));//EXTRA
        }

        // POST: Restaurant/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Restaurant restaurant = db.Restaurants.Find(id);
            db.Restaurants.Remove(restaurant);
            db.SaveChanges();

            //repo.DeletePost(id);//EXTRA

            return RedirectToAction("DisplayList");
        }

        //public JsonResult GetStudents(string term)
        //{
        //    List<string> model = db.Restaurants.Where(x => x.Name.StartsWith(term)).Select(y => y.Name).ToList();

        //    return Json(model, JsonRequestBehavior.AllowGet);
        //}



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
                //repo.Dispose();//EXTRA
            }
            base.Dispose(disposing);
        }

    
    }
}
