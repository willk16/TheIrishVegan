using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OdeToFood.Models;
using PagedList;

namespace OdeToFood.Controllers
{
    public class RecipeReviewsController : Controller
    {
        private OdeToFoodDb db = new OdeToFoodDb();

        public ActionResult SeeReviewersRecipes(string r)
        {
            var model = db.RecipeReviews.Where(m => m.ReviewerName == r);
            return View("GetMyRecipeReviews", model);
        }

        public ActionResult GetMyRecipeReviews(int page = 1)
        {
            var UserReviews = db.RecipeReviews.Where(m => m.ReviewerName == User.Identity.Name).ToList().ToPagedList(page, 10);
            return View(UserReviews);
        }

        public ActionResult Modal(int recipeId)
        {
            //RestaurantReview review = new RestaurantReview();
            return View("ModalRecipe");
        }

        // GET: RecipeReviews
        public ActionResult Index([Bind(Prefix = "id")] int recipeId)
        {
             var recipe = db.Recipes.Find(recipeId);
            if (recipe != null)
            {
                return View(recipe);

            }
            return HttpNotFound();
        }

        // GET: RecipeReviews/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RecipeReview recipeReview = db.RecipeReviews.Find(id);
            if (recipeReview == null)
            {
                return HttpNotFound();
            }
            return View(recipeReview);
        }

        // GET: RecipeReviews/Create
        public ActionResult Create(int recipeId)
        {
            return PartialView("ModalRecipe");
        }


        // POST: RecipeReviews/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(RecipeReview recipeReview)
        {
            if (ModelState.IsValid)
            {
                recipeReview.ReviewerName = User.Identity.Name;
                db.RecipeReviews.Add(recipeReview);
                db.SaveChanges();
                return RedirectToAction("Index", new { id = recipeReview.RecipeId });
            }

            return View(recipeReview);
        }

        // GET: RecipeReviews/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RecipeReview recipeReview = db.RecipeReviews.Find(id);
            if (recipeReview == null)
            {
                return HttpNotFound();
            }
            ViewBag.RecipeId = new SelectList(db.Recipes, "Id", "Name", recipeReview.RecipeId);
            return View(recipeReview);
        }

        // POST: RecipeReviews/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Rating,Title,Body,ReviewerName,RecipeId")] RecipeReview recipeReview)
        {
            if (ModelState.IsValid)
            {
                db.Entry(recipeReview).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RecipeId = new SelectList(db.Recipes, "Id", "Name", recipeReview.RecipeId);
            return View(recipeReview);
        }

        // GET: RecipeReviews/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RecipeReview recipeReview = db.RecipeReviews.Find(id);
            if (recipeReview == null)
            {
                return HttpNotFound();
            }
            return View(recipeReview);
        }

        // POST: RecipeReviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RecipeReview recipeReview = db.RecipeReviews.Find(id);
            db.RecipeReviews.Remove(recipeReview);
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
