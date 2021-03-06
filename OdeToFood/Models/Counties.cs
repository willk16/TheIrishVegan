﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OdeToFood.Models
{


    public class Counties
    {

        //Used to store my list of counties. It is static so I don't have to create an instance of it in the View that is using it.

        public static SelectListItem[] SelectListItems = new[]
       {
        new SelectListItem() {Value = "Antrim", Text = "Antrim" },
        new SelectListItem() {Value = "Armagh", Text = "Armagh" },
        new SelectListItem() {Value = "Carlow", Text = "Carlow" },
        new SelectListItem() {Value = "Cavan", Text = "Cavan" },
        new SelectListItem() {Value = "Clare", Text = "Clare" },
        new SelectListItem() {Value = "Cork", Text = "Cork" },
        new SelectListItem() {Value = "Derry", Text = "Derry" },
        new SelectListItem() {Value = "Donegal", Text = "Donegal" },
        new SelectListItem() {Value = "Down", Text = "Down" },
        new SelectListItem() {Value = "Dublin", Text = "Dublin" },
        new SelectListItem() {Value = "Fermanagh", Text = "Fermanagh" },
        new SelectListItem() {Value = "Galway", Text = "Galway" },
        new SelectListItem() {Value = "Kerry", Text = "Kerry" },
        new SelectListItem() {Value = "Kildare", Text = "Kildare" },
        new SelectListItem() {Value = "Kilkenny", Text = "Kilkenny" },
        new SelectListItem() {Value = "Laois", Text = "Laois" },
        new SelectListItem() {Value = "Leitrim", Text = "Leitrim" },
        new SelectListItem() {Value = "Limerick", Text = "Limerick" },
        new SelectListItem() {Value = "Longford", Text = "Longford" },
        new SelectListItem() {Value = "Louth", Text = "Louth" },
        new SelectListItem() {Value = "Mayo", Text = "Mayo" },
        new SelectListItem() {Value = "Meath", Text = "Meath" },
        new SelectListItem() {Value = "Monaghan", Text = "Monaghan" },
        new SelectListItem() {Value = "Offaly", Text = "Offaly" },
        new SelectListItem() {Value = "Roscommon", Text = "Roscommon" },
        new SelectListItem() {Value = "Sligo", Text = "Sligo" },
        new SelectListItem() {Value = "Tipperary", Text = "Tipperary" },
        new SelectListItem() {Value = "Tyrone", Text = "Tyrone" },
        new SelectListItem() {Value = "Waterford", Text = "Waterford" },
        new SelectListItem() {Value = "Westmeath", Text = "Westmeath" },
        new SelectListItem() {Value = "Wexford", Text = "Wexford" },
        new SelectListItem() {Value = "Wicklow", Text = "Wicklow" },
    };

    }
}