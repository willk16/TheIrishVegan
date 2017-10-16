using OdeToFood.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OdeToFood.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        OdeToFoodDb _db = new OdeToFoodDb(); //This is a reference to our DBContext class, which is used to make a connection to a database.

        public ActionResult AutoComplete(string term)
        {
            var model = _db.Restaurants
                .Where(r => r.Name.StartsWith(term))
                .Take(10)
                .Select(r => new
                {
                    label = r.Name
                });

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public ActionResult Index(string searchTerm = null)
        {
            //This is Comprehension Query Syntax. Either this query or the Extension Method one below are fine. Both do the same thing.
            //var model =
            //   from r in _db.Restaurants
            //   orderby r.Reviews.Average(review =>review.Rating) descending //Getting the average based on the rating
            //   select new RestaurantListViewModel //Creating an anonymous type.
            //   {
            //       Id = r.Id,//These properties are coming from the ViewModel and we are assigning to them whatever is the corresponding value from 'r', which is coming from our '_db.Restaurants'.
            //       Name = r.Name,
            //       City = r.City,
            //       Country = r.Country,
            //       CountOfReviews = r.Reviews.Count()
            //   };

            //This is the Extension Method Syntax
            var model = 
                _db.Restaurants
                .OrderByDescending(r=>r.Reviews.Average(review=>review.Rating))
                .Where(r=> searchTerm == null || r.Name.StartsWith(searchTerm)) //The first part is saying that if the paramater is null (i.e. no specific search was entered), then return all the results. 
                .Select(r=> new RestaurantListViewModel //Creating an anonymous type.
                {
                    Id = r.Id,//These properties are coming from the ViewModel and we are assigning to them whatever is the corresponding value from 'r', which is coming from our '_db.Restaurants'.
                    Name = r.Name,
                    City = r.City,
                  
                    CountOfReviews = r.Reviews.Count(),
                    
                });

            //This functionality doesn't work as it stands.
            if(Request.IsAjaxRequest()) //If an asynchronous request comes in, then this code will be implemented.
            {
                return PartialView("_Restaurants", model);
            }
            return View(model); //A ViewModel is being passed in
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Score(int input)
        {
            var score = _db.Reviews.Where(c => c.Rating > input);//I can view the first 'c' in the Lambda expression as each onject within the Reviews table as it is looping through. It then pulls out every object whos rating is above the user input.

            return View(score);
        }

        protected override void Dispose(bool disposing)
        {
            if(_db != null)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}