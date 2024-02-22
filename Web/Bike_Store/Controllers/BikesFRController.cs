using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NoNameBikes.Models;
using ReflectionIT.Mvc.Paging;

namespace NoNameBikes.Controllers
{
    public class BikesFRController : Controller
    {
        private readonly AdventureWorksLT2017Context db;

        public BikesFRController(AdventureWorksLT2017Context context)
        {
            db = context;
        }

        // GET: Bikes
        [Obsolete]
        public async Task<IActionResult> Index(string sortOrder, int page = 1)
        {
            ViewBag.NameSortDParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.NameSortAParm = String.IsNullOrEmpty(sortOrder) ? "name_asc" : "";

            var bikeList = from bikes in db.ProductCategory
                           where bikes.ParentProductCategoryId == 1
                           select bikes;

            var qry = bikeList.AsNoTracking().OrderBy(p => p.Name);

            if (sortOrder == "name_desc")
            {
                qry = bikeList.AsNoTracking().OrderByDescending(p => p.Name);
            }
            else
            {
                qry = bikeList.AsNoTracking().OrderBy(p => p.Name);
            }

            var model = await PagingList<ProductCategory>.CreateAsync(qry, 5, page);

            return View(model);
        }

        [Obsolete]
        public async Task<IActionResult> Road(string sortOrder, int page = 1)
        {
            ViewBag.NameSortDParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.NameSortAParm = String.IsNullOrEmpty(sortOrder) ? "name_asc" : "";
            ViewBag.PriceSortDParm = String.IsNullOrEmpty(sortOrder) ? "price_desc" : "";
            ViewBag.PriceSortAParm = String.IsNullOrEmpty(sortOrder) ? "price_asc" : "";
            ViewBag.SellDateSortDParm = String.IsNullOrEmpty(sortOrder) ? "sellDate_desc" : "";
            ViewBag.SellDateSortAParm = String.IsNullOrEmpty(sortOrder) ? "sellDate_asc" : "";

            /*var validRoadProducts = (from bikes in db.VProductAndDescription
                                    where bikes.Culture == "en" &&
                                    bikes.SellEndDate == null &&
                                    bikes.ProductCategoryId == 6
                                    select new BikeListModel
                                    {
                                        ProductModel = bikes.ProductModel,
                                        Description = bikes.Description,
                                        ProductModelID = bikes.ProductModelId
                                    }).Distinct();*/

            var validRoadProducts = (from pd in db.VProductAndDescription
                                     join p in db.Product
                                     on pd.ProductModelId equals p.ProductModelId
                                     where pd.Culture == "fr" &&
                                     pd.SellEndDate == null &&
                                     pd.ProductCategoryId == 6
                                     select new BikeListModel
                                     {
                                         ProductModel = pd.ProductModel,
                                         Description = pd.Description,
                                         ProductModelID = pd.ProductModelId,
                                         Price = p.ListPrice,
                                         Photo = p.ThumbNailPhoto,
                                         StartSellDate = p.SellStartDate
                                     }).Distinct();

            var qry = validRoadProducts.AsNoTracking().OrderBy(p => p.ProductModel);

            switch (sortOrder)
            {
                case "name_desc":
                    qry = validRoadProducts.AsNoTracking().OrderByDescending(p => p.ProductModel);
                    break;
                case "price_asc":
                    qry = validRoadProducts.AsNoTracking().OrderBy(p => p.Price);
                    break;
                case "price_desc":
                    qry = validRoadProducts.AsNoTracking().OrderByDescending(p => p.Price);
                    break;
                case "sellDate_asc":
                    qry = validRoadProducts.AsNoTracking().OrderBy(p => p.StartSellDate);
                    break;
                case "sellDate_desc":
                    qry = validRoadProducts.AsNoTracking().OrderByDescending(p => p.StartSellDate);
                    break;
                default:
                    qry = validRoadProducts.AsNoTracking().OrderBy(p => p.ProductModel);
                    break;
            }

            var model = await PagingList<BikeListModel>.CreateAsync(qry, 5, page);
            model.Action = "Road";
            return View(model);
        }

        [Obsolete]
        public async Task<IActionResult> Mountain(string sortOrder, int page = 1)
        {
            ViewBag.NameSortDParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.NameSortAParm = String.IsNullOrEmpty(sortOrder) ? "name_asc" : "";
            ViewBag.PriceSortDParm = String.IsNullOrEmpty(sortOrder) ? "price_desc" : "";
            ViewBag.PriceSortAParm = String.IsNullOrEmpty(sortOrder) ? "price_asc" : "";
            ViewBag.SellDateSortDParm = String.IsNullOrEmpty(sortOrder) ? "sellDate_desc" : "";
            ViewBag.SellDateSortAParm = String.IsNullOrEmpty(sortOrder) ? "sellDate_asc" : "";

            var validMountainProducts = (from pd in db.VProductAndDescription
                                         join p in db.Product
                                         on pd.ProductModelId equals p.ProductModelId
                                         where pd.Culture == "fr" &&
                                         pd.SellEndDate == null &&
                                         pd.ProductCategoryId == 5
                                         select new BikeListModel
                                         {
                                             ProductModel = pd.ProductModel,
                                             Description = pd.Description,
                                             ProductModelID = pd.ProductModelId,
                                             Price = p.ListPrice,
                                             Photo = p.ThumbNailPhoto,
                                             StartSellDate = p.SellStartDate
                                         }).Distinct();

            var qry = validMountainProducts.AsNoTracking().OrderBy(p => p.ProductModel);

            switch (sortOrder)
            {
                case "name_desc":
                    qry = validMountainProducts.AsNoTracking().OrderByDescending(p => p.ProductModel);
                    break;
                case "price_asc":
                    qry = validMountainProducts.AsNoTracking().OrderBy(p => p.Price);
                    break;
                case "price_desc":
                    qry = validMountainProducts.AsNoTracking().OrderByDescending(p => p.Price);
                    break;
                case "sellDate_asc":
                    qry = validMountainProducts.AsNoTracking().OrderBy(p => p.StartSellDate);
                    break;
                case "sellDate_desc":
                    qry = validMountainProducts.AsNoTracking().OrderByDescending(p => p.StartSellDate);
                    break;
                default:
                    qry = validMountainProducts.AsNoTracking().OrderBy(p => p.ProductModel);
                    break;
            }

            var model = await PagingList<BikeListModel>.CreateAsync(qry, 5, page);
            model.Action = "Mountain";
            return View(model);
        }

        [Obsolete]
        public async Task<IActionResult> Touring(string sortOrder, int page = 1)
        {
            ViewBag.NameSortDParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.NameSortAParm = String.IsNullOrEmpty(sortOrder) ? "name_asc" : "";
            ViewBag.PriceSortDParm = String.IsNullOrEmpty(sortOrder) ? "price_desc" : "";
            ViewBag.PriceSortAParm = String.IsNullOrEmpty(sortOrder) ? "price_asc" : "";
            ViewBag.SellDateSortDParm = String.IsNullOrEmpty(sortOrder) ? "sellDate_desc" : "";
            ViewBag.SellDateSortAParm = String.IsNullOrEmpty(sortOrder) ? "sellDate_asc" : "";

            /*var validTouringProducts = (from bikes in db.VProductAndDescription
                                     where bikes.Culture == "en" &&
                                     bikes.SellEndDate == null &&
                                     bikes.ProductCategoryId == 7
                                     select new BikeListModel
                                     {
                                         ProductModel = bikes.ProductModel,
                                         Description = bikes.Description,
                                         ProductModelID = bikes.ProductModelId
                                     }).Distinct();*/

            var validTouringProducts = (from pd in db.VProductAndDescription
                                        join p in db.Product
                                        on pd.ProductModelId equals p.ProductModelId
                                        where pd.Culture == "fr" &&
                                        pd.SellEndDate == null &&
                                        pd.ProductCategoryId == 7
                                        select new BikeListModel
                                        {
                                            ProductModel = pd.ProductModel,
                                            Description = pd.Description,
                                            ProductModelID = pd.ProductModelId,
                                            Price = p.ListPrice,
                                            Photo = p.ThumbNailPhoto,
                                            StartSellDate = p.SellStartDate
                                        }).Distinct();

            var qry = validTouringProducts.AsNoTracking().OrderBy(p => p.ProductModel);

            switch (sortOrder)
            {
                case "name_desc":
                    qry = validTouringProducts.AsNoTracking().OrderByDescending(p => p.ProductModel);
                    break;
                case "price_asc":
                    qry = validTouringProducts.AsNoTracking().OrderBy(p => p.Price);
                    break;
                case "price_desc":
                    qry = validTouringProducts.AsNoTracking().OrderByDescending(p => p.Price);
                    break;
                case "sellDate_asc":
                    qry = validTouringProducts.AsNoTracking().OrderBy(p => p.StartSellDate);
                    break;
                case "sellDate_desc":
                    qry = validTouringProducts.AsNoTracking().OrderByDescending(p => p.StartSellDate);
                    break;
                default:
                    qry = validTouringProducts.AsNoTracking().OrderBy(p => p.ProductModel);
                    break;
            }

            var model = await PagingList<BikeListModel>.CreateAsync(qry, 5, page);
            model.Action = "Touring";
            return View(model);
        }

        // GET: Bikes/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
                return NotFound();

            IEnumerable<Product> products = (from bikes in db.Product
                                             where bikes.ProductModelId == id && bikes.SellEndDate == null
                                             select bikes);
            IEnumerable<string> desc = (from details in db.VProductAndDescription
                                        where details.ProductModelId == id &&
                                        details.Culture == "fr"
                                        select details.Description);

            ViewData["description"] = desc.First();

            if (products == null)
                return NotFound();

            return View(products);
        }

        public IActionResult CompareBikes(int? id_first, int? id_second)
        {
            //if (id_first == null || id_second == null)
            //    return NotFound();

            //IEnumerable<Product> products = (from bikes in db.Product
            //                                 where bikes.ProductModelId == id_first && bikes.SellEndDate == null 
            //                                 || bikes.ProductModelId == id_second && bikes.SellEndDate == null
            //                                 select bikes);
            //IEnumerable<string> desc = (from details in db.VProductAndDescription
            //                            where details.ProductModelId == id_first || details.ProductModelId == id_second
            //                            select details.Description);

            //ViewData["description"] = desc.First();

            //if (products == null)
            //    return NotFound();

            //return View(products);

            var products = (from bikes in db.Product
                            where bikes.ProductModelId == id_first && bikes.SellEndDate == null
                            || bikes.ProductModelId == id_second && bikes.SellEndDate == null
                            select bikes);
            return View(products);

        }

        public IActionResult Sales()
        {

            return View();
        }
    }
}
