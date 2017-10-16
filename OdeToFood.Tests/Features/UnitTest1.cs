using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OdeToFood.Controllers;
using System.Web.Mvc;
using Telerik.JustMock;
using OdeToFood.Models;
using System.Collections.Generic;
using System.Linq;

namespace OdeToFood.Tests.Features
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var controller = new RecipesController();
            var result = controller.Landing() as ViewResult;
            Assert.AreEqual("LandingRecipes", result.ViewName);

        }

        [TestMethod]
        public void TestMethod2()
        {
            var controller = new RestaurantController();
            var result = controller.Delete(null);
            Assert.IsInstanceOfType(result, typeof(HttpStatusCodeResult));
        }

        [TestMethod]
        public void TestMethod3()
        {
            // Arrange
            var controller = new RestaurantController();

            // Act
            ViewResult result = controller.ByDateMonth() as ViewResult;

            // Assert
            Assert.AreEqual("30 Days", result.ViewBag.MyDate);
        }

        //To test the MyTestMethod() method in the Restaurant Controller
         [TestMethod]
        public void ReturnsAllRestaurantsInDB()
        {
            //Arrange
            var restaurantRepository = Mock.Create<IRepository>();//Creating an instance of the mock database
            Mock.Arrange( () => restaurantRepository.GetAll()).
                Returns(new List<Restaurant>()//This will return a list containing two new restaurants
                {
                    new Restaurant { Name="Cornacopia", City="Dublin", Keyword="Good"},
                    new Restaurant { Name="Olive Garden", City="Cork", Keyword="Bad"},
                }).MustBeCalled();//This part says that the test will fail if the getAll() method is not called.

            //Act
            RestaurantController controller = new RestaurantController(restaurantRepository);
            ViewResult viewResult = controller.MyTestMethod();
            var model = viewResult.Model as IEnumerable<Restaurant>;//Getting the model from the ViewResult 
                                                                    //which will be an IEnumerable<Restaurant>

            //Assert
            Assert.AreEqual(2, model.Count());

        }

   }
}
