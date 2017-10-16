using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OdeToFood.Models
{
    public class RestaurantReview
    {
        public int Id { get; set; }

        public virtual Restaurant RestaurantName { get; set; }

        [Range(1,10)]
        [Required]
        public int Rating { get; set; }

        [Required]
        public string Title { get; set; }

        [Required, StringLength(200)]
        [Display(Name = "Review")]
        public string Body { get; set; }

        [Display(Name = "User Name")]
        [DisplayFormat(NullDisplayText ="Unknown")]//Used as the display if nothing is entered in the User Name field.
        public string ReviewerName { get; set; }

        public int RestaurantId { get; set; }//Points back to the restaurant that the review is associated with


    }
}