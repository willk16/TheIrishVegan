using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OdeToFood.Models
{
    public class OdeToFoodDb: DbContext
    {
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<RestaurantReview> Reviews { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<RecipeReview> RecipeReviews { get; set; }

       
    }
}