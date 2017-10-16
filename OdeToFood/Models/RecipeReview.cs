using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OdeToFood.Models
{
    public class RecipeReview
    {
        public int Id { get; set; }
        [Display(Name ="Recipe Name")]
        public virtual Recipe RecipeName { get; set; }

        [Range(1, 10)]
        [Required]
        public int Rating { get; set; }

        [Required]
        public string Title { get; set; }

        [Required, StringLength(200)]
        [Display(Name = "Review")]
        public string Body { get; set; }

        [Display(Name = "User Name")]
        [DisplayFormat(NullDisplayText = "Unknown")]//Used as the display if nothing is entered in the User Name field.
        public string ReviewerName { get; set; }

        public int RecipeId { get; set; }
    }
}