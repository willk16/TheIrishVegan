using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations.Schema;

namespace OdeToFood.Models
{
    public class Recipe
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        public string Category { get; set; }
        public virtual ICollection<RecipeReview> RecipeReviews { get; set; }
        public string UserName { get; set; }//User will not need to fill this in. It will be automatically set to the User's Account name, when a recipe is being created.
        public DateTime? Date { get; set; }
        [Required, MaxLength(35)]
        public string Description { get; set; }
        [Required, MaxLength(200)]
        public string Instructions { get; set; }
        [Required, MaxLength(200)]
        public string Ingredients { get; set; }
        [Required]
        public int Timescale { get; set; }
    
        public string ImageFileName
        {
            get
            {
                return Name.Replace(" ", "-").ToLower() + ".jpg";
            }
        }
        public string ImagePath { get; set; }

        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }
    }
}