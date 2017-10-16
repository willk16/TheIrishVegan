using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OdeToFood.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using PagedList;

namespace OdeToFood.Controllers
{
    public class RecipesController : Controller
    {
        private OdeToFoodDb db = new OdeToFoodDb();

        public ActionResult ByDateWeek(int page = 1)
        {
            var date = DateTime.Now.Date.AddDays(-7);
            var model = db.Recipes.Where(m => m.Date >= date).ToList().ToPagedList(page, 10); ;//Getting the Restaurants that were added in the past 7 days.
            ViewBag.MyDate = "7 Days";
            return View("DisplayList", model);
        }

        public ActionResult ByDateMonth(int page = 1)
        {
            var date = DateTime.Now.Date.AddDays(-30);
            var model = db.Recipes.Where(m => m.Date >= date).ToList().ToPagedList(page, 10);//Getting the Restaurants that were added in the past 30 days.
            ViewBag.MyDate = "30 Days";
            return View("DisplayList", model);
        }

        public ActionResult GetRecipes(int page = 1)
        {
            var UserRecipes = db.Recipes.Where(m => m.UserName == User.Identity.Name).ToList().ToPagedList(page, 10); 
            return View(UserRecipes);
        }

        public ActionResult ByTime(int? time, int page = 1)
        {
            if (time.Equals(""))//If the user clicks search without entering a category, the LandingRecipes page will be re-displayed.
            {
                TempData["Message"] = "Please Select a Time";
                return View("LandingRecipes");
            }

            var model = db.Recipes.Where(r => r.Timescale < time).ToList().ToPagedList(page, 10);//Returning all recipes that have a timescale less than the user's input.
            return View("GlutenFree",model);
        }


        public ActionResult CheckRecipe(string searchTerm)
        {
            var model =
                db.Recipes
                .Where(r => searchTerm == null || r.Name.StartsWith(searchTerm)) //The first part is saying that if the paramater is null (i.e. no specific search was entered), then return all the results.
                .Select(r => new Recipe //Creating an anonymous type.
                {
                    Id = r.Id,//These properties are coming from the Model and we are assigning to them whatever is the corresponding value from 'r', which is coming from our '_db.Restaurants'.
                    Name = r.Name,
                    Description = r.Description,
                    Instructions = r.Instructions,
                    Ingredients = r.Ingredients,
                    Timescale = r.Timescale,

                    
                });
            return View("Index", model);
        }

        //This action is used on the 'Quick and easy recipes' option on the home page
        public ActionResult Time(int page = 1)
        {
            var model = db.Recipes.Where(r => r.Timescale < 20).ToList().ToPagedList(page, 10);
            ViewBag.Title = "Under 20 Minutes";
            return View("GlutenFree", model);
        }

        public ActionResult GlutenFree(int page = 1)
        {
            var model = db.Recipes.Where(r => r.Category == "Gluten Free").ToList().ToPagedList(page, 10);
            ViewBag.Title = "Gluten Free";
            return View(model);
        }

        public ActionResult Smoothie(int page = 1)
        {
            var model = db.Recipes.Where(r => r.Category == "Smoothie").ToList().ToPagedList(page, 10);
            ViewBag.Title = "Smoothies";
            return View("GlutenFree", model);
        }


        public ActionResult Landing()
        {
            var dictionary = new Dictionary<int, string>//I'm using this dictionary to create values that will populate a DropdownList in the View.
                {
                    { 10, "10" },
                    { 15, "15" },
                    { 20, "20" },
                    { 25, "25" },
                    { 30, "30" },
                    { 40, "40" },
                    { 50, "50" }
                };

            ViewBag.SelectList = new SelectList(dictionary, "Key", "Value");//Being sent through a Viewbag in a key-value pairing.
            return View("LandingRecipes");
        }

         public ActionResult DisplayList(int page = 1)
        {
            return View(db.Recipes.ToList().ToPagedList(page, 10));
        }

        // GET: Recipes
        public ActionResult Index(string search, int page = 1)
        {
            if (search == "")
            {
                return RedirectToAction("DisplayList");//If the search result comes back empty, then all results will be displayed
            }
            else
            {
                var model = db.Recipes
                        .Where(r => search == null || r.Name.StartsWith(search)).ToList().ToPagedList(page, 10);

                //return View(db.Recipes.ToList());
                ViewBag.SearchTerm = search;
                return View("DisplayList", model);
            }
        }

        public ActionResult Category(string category, int page = 1)
        {
            if (category.Equals(""))//If the user clicks search without entering a category, the LandingRecipes page will be re-displayed.
            {
                TempData["Message"] = "Please Select a Category";
                return View("LandingRecipes");
            }
            else
            {
                var model = db.Recipes.Where(r => r.Category == category).ToList().ToPagedList(page, 10);
                return View("GlutenFree", model);
            }
        }

        // GET: Recipes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Recipe recipe = db.Recipes.Find(id);
            if (recipe == null)
            {
                return HttpNotFound();
            }
            return View(recipe);
        }

        // GET: Recipes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Recipes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Recipe recipe, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    file.SaveAs(HttpContext.Server.MapPath("~/Images/")
                                                          + file.FileName);
                    recipe.ImagePath = file.FileName;
                }

                recipe.UserName = User.Identity.Name;//This allows me to match the Recipe's UserName property with the UserName of the currently logged in user.

                recipe.Date = DateTime.Today;
                db.Recipes.Add(recipe);
                db.SaveChanges();
                return RedirectToAction("DisplayList");
            }

            return View(recipe);
        }

        // GET: Recipes/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Recipe recipe = db.Recipes.Find(id);
            if (recipe == null)
            {
                return HttpNotFound();
            }
            return View(recipe);
        }

        // POST: Recipes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Recipe recipe)
        {
            if (ModelState.IsValid)
            {
                recipe.UserName = User.Identity.Name;
                db.Entry(recipe).State = EntityState.Modified;//Tells the Entity Framework not to treat this like a new entry but tather like an existing one that is being updated.
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(recipe);
        }

        // GET: Recipes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Recipe recipe = db.Recipes.Find(id);
            if (recipe == null)
            {
                return HttpNotFound();
            }
            return View(recipe);
        }

        // POST: Recipes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Recipe recipe = db.Recipes.Find(id);
            db.Recipes.Remove(recipe);
            db.SaveChanges();
            return RedirectToAction("DisplayList");
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
