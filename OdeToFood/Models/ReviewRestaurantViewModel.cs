using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OdeToFood.Models
{
    public class ReviewRestaurantViewModel
    {
        public RestaurantReview RestaurantReview { get; set; }
        public Restaurant Restaurant { get; set; }
    }
}