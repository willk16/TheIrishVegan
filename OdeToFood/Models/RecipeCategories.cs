using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OdeToFood.Models
{
    public class RecipeCategories
    {
        public static SelectListItem[] SelectListItems = new[]
        {
            new SelectListItem() {Value = "Gluten Free", Text = "Gluten Free" },
            new SelectListItem() {Value = "Low Fat", Text = "Low Fat" },
            new SelectListItem() {Value = "Health", Text = "Health" },
            new SelectListItem() {Value = "Muscle Building", Text = "Muscle Building" },
            new SelectListItem() {Value = "Party Food", Text = "Party Food" },
            new SelectListItem() {Value = "Breakfast", Text = "Breakfast" },
            new SelectListItem() {Value = "Snack", Text = "Snack" },
            new SelectListItem() {Value = "Smoothie", Text = "Smoothie" },
        };

        public static SelectListItem[] SelectListItemByTime = new[]
        {
            new SelectListItem() {Value= "5", Text = "5" },
            new SelectListItem() {Value= "10", Text = "10" },
            new SelectListItem() {Value= "15", Text = "15" },
            new SelectListItem() {Value= "20", Text = "20" },
            new SelectListItem() {Value= "25", Text = "25" },
        };

   

    }
}