﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NoNameBikes.Models;

namespace NoNameBikes.Controllers
{
    public class CategoriesFRController : Controller
    {
        private readonly AdventureWorksLT2017Context db;

        public CategoriesFRController(AdventureWorksLT2017Context context)
        {
            db = context;
        }

        public IActionResult Index()
        {
            //get list of all categories that are parent categories
            var categoryList = from productCategory in db.ProductCategory
                               where productCategory.ParentProductCategoryId == null
                               select productCategory;

            return View(categoryList);
        }
    }
}
