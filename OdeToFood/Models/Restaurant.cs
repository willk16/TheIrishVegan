using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OdeToFood.Models
{
    public class Restaurant
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
     
        //[MaxLength(200, ErrorMessage = "The Description cannot be long than 200 characters")]
        public string Description { get; set; }
        public string Keyword { get; set; }
        public string City { get; set; }
        [Display(Name= "User Name")]
        public string UserName { get; set; }
        [Display(Name ="Date Added")]
        public DateTime? Date { get; set; }

        
        public virtual ICollection<RestaurantReview> Reviews { get; set; } //By addding virtual I am ensuring that Reviews isn't null by default
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