using OdeToFood.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace OdeToFood.Controllers
{
    public class ReviewsController : Controller
    {
        OdeToFoodDb _db = new OdeToFoodDb();

        //[ChildActionOnly]//This would ensure that this contoller cannot be called through the URL
        //public ActionResult BestReview()//This action will display the besst rated review from the lisr of review, at the bottom of every page (because it is called from the shared _Layout View).
        //{
        //    var bestReview = from r in _reviews
        //                     orderby r.Rating descending
        //                     select r;

        //    return PartialView("_Review", bestReview.First());
        //}

        public ActionResult SeeReviewersRestaurantsReviews(string r)
        {
            var model = _db.Reviews.Where(m => m.ReviewerName == r);
            return View("GetMyReviews", model);
        }


        public ActionResult GetMyReviews(int page = 1)
        {
            var UserReviews = _db.Reviews.Where(m => m.ReviewerName == User.Identity.Name).ToList().ToPagedList(page, 10);
            return View(UserReviews);
        }
        
        public ActionResult BestReviews(int page = 1)
        {
            var model = _db.Reviews.Where(m => m.Rating == 10).ToList().ToPagedList(page, 10);
            return View(model);
        }

        public ActionResult Test(int restaurantId)
        {
            //RestaurantReview review = new RestaurantReview();
            return View("_Test");
        }

        // GET: Reviews
        public ActionResult Index([Bind(Prefix = "id")] int restaurantId)  //This says when looking for the restaurantId parameter value, instead look for something called 'id'.
        {
            var restaurant = _db.Restaurants.Find(restaurantId);
            if (restaurant != null)
            {
                return View(restaurant);

            }
            return HttpNotFound();
        }

       

        [HttpGet]
        
        public ActionResult Create(int restaurantId)
        {
            return PartialView("_Test");
        }
        
        
        [HttpPost]
        public ActionResult Create(RestaurantReview review)
        {
            if (ModelState.IsValid)
            {
                review.ReviewerName = User.Identity.Name;
                _db.Reviews.Add(review);//If IsValid returns true, then add this review to the Reviews collections
                _db.SaveChanges();//This will actually save it to the database
                return RedirectToAction("Index", new { id = review.RestaurantId });//Second paramter ensures the user can then view their newly entered review (as we've passed the id through).
                

            }
            return View(review);//Passing 'review' as a parameter here ensures that the user doesn't lose their already inputted data.
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var model = _db.Reviews.Find(id);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(RestaurantReview review)
        {
            if (ModelState.IsValid)
            {
                _db.Entry(review).State = EntityState.Modified;//Here is a review i want you to track so you can make changes in the database, but its not a new review so I don't want it inserted into the database. Instead its a review that's already in the database.
                _db.SaveChanges();
                return RedirectToAction("Index", new { id = review.RestaurantId });
            }
            return View(review);
        }


        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RestaurantReview restaurantReview = _db.Reviews.Find(id);
            if (restaurantReview == null)
            {
                return HttpNotFound();
            }
            return View(restaurantReview);
        }

        protected override void Dispose(bool disposing)
        {
            _db.Dispose();
            base.Dispose(disposing);    
        }


    }
}
