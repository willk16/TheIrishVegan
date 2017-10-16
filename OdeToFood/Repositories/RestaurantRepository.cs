using Microsoft.AspNet.Identity;
using OdeToFood.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Net;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web.Mvc;




namespace OdeToFood.Repositories
{
    

    public class RestaurantRepository : Controller
    {
        private OdeToFoodDb db = new OdeToFoodDb();

        public ICollection<Restaurant> GetUserRestaurants()
        {
            var UserRestaurants = db.Restaurants.Where(m => m.UserName == User.Identity.Name).ToList();
            return UserRestaurants;
        }

        public Restaurant Create(Restaurant restaurant)
        {
            db.Restaurants.Add(restaurant);
            db.SaveChanges();
            return restaurant;
        }

        public ICollection<Restaurant> County()
        {
            var model = db.Restaurants
                .Where(r => r.City == "Dublin").ToList();
            return model;
        }

        public ICollection<Restaurant> Decide(string search, string searchBy)
        {
            if (searchBy == "Name")
            {
                var model = db.Restaurants
                    .Where(r => r.Name.StartsWith(search)).ToList();

                return (model);
            }
            else
            {
                var model = db.Restaurants
                    .Where(r => r.City.StartsWith(search)).ToList();
                return model;
            }
        }

        public IQueryable<RestaurantListViewModel> Check(string searchTerm)
        {
            var model =
                db.Restaurants
                .Where(r => searchTerm == null || r.Name.StartsWith(searchTerm)) //The first part is saying that if the paramater is null (i.e. no specific search was entered), then return all the results.
                .Select(r => new RestaurantListViewModel //Creating an anonymous type.
                {
                    Id = r.Id,//These properties are coming from the ViewModel and we are assigning to them whatever is the corresponding value from 'r', which is coming from our '_db.Restaurants'.
                    Name = r.Name,
                    Description = r.Description,
                    Keyword = r.Keyword,
                    City = r.City,

                    CountOfReviews = r.Reviews.Count()
                });

            return model;
        }

        public ICollection<Restaurant> DisplayList()
        {
            return db.Restaurants.ToList();
        }

        public ICollection<Restaurant> Search(string search)
        {
            var model = db.Restaurants
                    .Where(r => search == null || r.Name.StartsWith(search)).ToList();
            return model;
        }

        public ICollection<RestaurantReview> Top()
        {
            var model = db.Reviews.OrderByDescending(r => r.Rating)//This Lambda expression is retrieving the top 5 reviews
               .Take(5).ToList();
            return model;
        }

        public Restaurant Details(int? id)
        {
            Restaurant restaurant = db.Restaurants.Find(id);
            return restaurant;
        }

        public Restaurant EditGet(int? id)
        {
            Restaurant restaurant = db.Restaurants.Find(id);
            return restaurant;
        }

        public Restaurant EditPost(Restaurant restaurant)
        {
            db.Entry(restaurant).State = EntityState.Modified;
            db.SaveChanges();
            return restaurant;
        }

        public Restaurant DeleteGet(int? id)
        {
            Restaurant restaurant = db.Restaurants.Find(id);
            return restaurant;
        }

        public Restaurant DeletePost(int id)
        {
            Restaurant restaurant = db.Restaurants.Find(id);
            db.Restaurants.Remove(restaurant);
            db.SaveChanges();
            return restaurant;
        }

        public void Dispose()
        {
            db.Dispose();
        }

        
    }
}