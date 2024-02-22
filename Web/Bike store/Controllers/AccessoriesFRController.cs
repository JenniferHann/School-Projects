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
    public class AccessoriesFRController : Controller
    {
        private readonly AdventureWorksLT2017Context db;

        public AccessoriesFRController(AdventureWorksLT2017Context context)
        {
            db = context;
        }

        // GET: Accessories
        [Obsolete]
        public async Task<IActionResult> Index(string sortOrder, int page = 1)
        {
            ViewBag.NameSortDParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.NameSortAParm = String.IsNullOrEmpty(sortOrder) ? "name_asc" : "";

            var accessoriesList = from accessories in db.ProductCategory
                                  where accessories.ParentProductCategoryId == 4 &&
                                  accessories.Name != "Lights" &&
                                  accessories.Name != "Locks" &&
                                  accessories.Name != "Panniers" &&
                                  accessories.Name != "Pumps"
                                  select accessories;

            var qry = accessoriesList.AsNoTracking().OrderBy(p => p.Name);

            if (sortOrder == "name_desc")
            {
                qry = accessoriesList.AsNoTracking().OrderByDescending(p => p.Name);
            }
            else
            {
                qry = accessoriesList.AsNoTracking().OrderBy(p => p.Name);
            }

            var model = await PagingList<ProductCategory>.CreateAsync(qry, 5, page);

            return View(model);
        }

        [Obsolete]
        public async Task<IActionResult> BikeRacks(string sortOrder, int page = 1)
        {
            ViewBag.NameSortDParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.NameSortAParm = String.IsNullOrEmpty(sortOrder) ? "name_asc" : "";
            ViewBag.PriceSortDParm = String.IsNullOrEmpty(sortOrder) ? "price_desc" : "";
            ViewBag.PriceSortAParm = String.IsNullOrEmpty(sortOrder) ? "price_asc" : "";
            ViewBag.SellDateSortDParm = String.IsNullOrEmpty(sortOrder) ? "sellDate_desc" : "";
            ViewBag.SellDateSortAParm = String.IsNullOrEmpty(sortOrder) ? "sellDate_asc" : "";

            /*var validList = (from items in db.VProductAndDescription
                             where items.Culture == "en" &&
                             items.ProductCategoryId == 30 &&
                             items.SellEndDate == null
                             select new AccessoriesListModel
                             {
                                 ProductModel = items.ProductModel,
                                 Description = items.Description,
                                 ProductModelID = items.ProductModelId
                             }).Distinct().ToList();*/

            var validList = (from pd in db.VProductAndDescription
                             join p in db.Product
                             on pd.ProductModelId equals p.ProductModelId
                             where pd.Culture == "fr" &&
                             pd.SellEndDate == null &&
                             pd.ProductCategoryId == 30
                             select new AccessoriesListModel
                             {
                                 ProductModel = pd.ProductModel,
                                 Description = pd.Description,
                                 ProductModelID = pd.ProductModelId,
                                 Price = p.ListPrice,
                                 Photo = p.ThumbNailPhoto,
                                 StartSellDate = p.SellStartDate
                             }).Distinct();

            var qry = validList.AsNoTracking().OrderBy(p => p.ProductModel);

            switch (sortOrder)
            {
                case "name_desc":
                    qry = validList.AsNoTracking().OrderByDescending(p => p.ProductModel);
                    break;
                case "price_asc":
                    qry = validList.AsNoTracking().OrderBy(p => p.Price);
                    break;
                case "price_desc":
                    qry = validList.AsNoTracking().OrderByDescending(p => p.Price);
                    break;
                case "sellDate_asc":
                    qry = validList.AsNoTracking().OrderBy(p => p.StartSellDate);
                    break;
                case "sellDate_desc":
                    qry = validList.AsNoTracking().OrderByDescending(p => p.StartSellDate);
                    break;
                default:
                    qry = validList.AsNoTracking().OrderBy(p => p.ProductModel);
                    break;
            }

            var model = await PagingList<AccessoriesListModel>.CreateAsync(qry, 5, page);
            model.Action = "BikeRacks";
            return View(model);
        }

        [Obsolete]
        public async Task<IActionResult> BikeStands(string sortOrder, int page = 1)
        {
            ViewBag.NameSortDParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.NameSortAParm = String.IsNullOrEmpty(sortOrder) ? "name_asc" : "";
            ViewBag.PriceSortDParm = String.IsNullOrEmpty(sortOrder) ? "price_desc" : "";
            ViewBag.PriceSortAParm = String.IsNullOrEmpty(sortOrder) ? "price_asc" : "";
            ViewBag.SellDateSortDParm = String.IsNullOrEmpty(sortOrder) ? "sellDate_desc" : "";
            ViewBag.SellDateSortAParm = String.IsNullOrEmpty(sortOrder) ? "sellDate_asc" : "";

            /*var validList = (from items in db.VProductAndDescription
                             where items.Culture == "en" &&
                             items.ProductCategoryId == 31 &&
                             items.SellEndDate == null
                             select new AccessoriesListModel
                             {
                                 ProductModel = items.ProductModel,
                                 Description = items.Description,
                                 ProductModelID = items.ProductModelId
                             }).Distinct().ToList();*/

            var validList = (from pd in db.VProductAndDescription
                             join p in db.Product
                             on pd.ProductModelId equals p.ProductModelId
                             where pd.Culture == "fr" &&
                             pd.SellEndDate == null &&
                             pd.ProductCategoryId == 31
                             select new AccessoriesListModel
                             {
                                 ProductModel = pd.ProductModel,
                                 Description = pd.Description,
                                 ProductModelID = pd.ProductModelId,
                                 Price = p.ListPrice,
                                 Photo = p.ThumbNailPhoto,
                                 StartSellDate = p.SellStartDate
                             }).Distinct();

            var qry = validList.AsNoTracking().OrderBy(p => p.ProductModel);

            switch (sortOrder)
            {
                case "name_desc":
                    qry = validList.AsNoTracking().OrderByDescending(p => p.ProductModel);
                    break;
                case "price_asc":
                    qry = validList.AsNoTracking().OrderBy(p => p.Price);
                    break;
                case "price_desc":
                    qry = validList.AsNoTracking().OrderByDescending(p => p.Price);
                    break;
                case "sellDate_asc":
                    qry = validList.AsNoTracking().OrderBy(p => p.StartSellDate);
                    break;
                case "sellDate_desc":
                    qry = validList.AsNoTracking().OrderByDescending(p => p.StartSellDate);
                    break;
                default:
                    qry = validList.AsNoTracking().OrderBy(p => p.ProductModel);
                    break;
            }

            var model = await PagingList<AccessoriesListModel>.CreateAsync(qry, 5, page);
            model.Action = "BikeStands";
            return View(model);
        }

        [Obsolete]
        public async Task<IActionResult> BottlesAndCages(string sortOrder, int page = 1)
        {
            ViewBag.NameSortDParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.NameSortAParm = String.IsNullOrEmpty(sortOrder) ? "name_asc" : "";
            ViewBag.PriceSortDParm = String.IsNullOrEmpty(sortOrder) ? "price_desc" : "";
            ViewBag.PriceSortAParm = String.IsNullOrEmpty(sortOrder) ? "price_asc" : "";
            ViewBag.SellDateSortDParm = String.IsNullOrEmpty(sortOrder) ? "sellDate_desc" : "";
            ViewBag.SellDateSortAParm = String.IsNullOrEmpty(sortOrder) ? "sellDate_asc" : "";

            /*var validList = (from items in db.VProductAndDescription
                             where items.Culture == "en" &&
                             items.ProductCategoryId == 32 &&
                             items.SellEndDate == null
                             select new AccessoriesListModel
                             {
                                 ProductModel = items.ProductModel,
                                 Description = items.Description,
                                 ProductModelID = items.ProductModelId
                             }).Distinct().ToList();*/

            var validList = (from pd in db.VProductAndDescription
                             join p in db.Product
                             on pd.ProductModelId equals p.ProductModelId
                             where pd.Culture == "fr" &&
                             pd.SellEndDate == null &&
                             pd.ProductCategoryId == 32
                             select new AccessoriesListModel
                             {
                                 ProductModel = pd.ProductModel,
                                 Description = pd.Description,
                                 ProductModelID = pd.ProductModelId,
                                 Price = p.ListPrice,
                                 Photo = p.ThumbNailPhoto,
                                 StartSellDate = p.SellStartDate
                             }).Distinct();

            var qry = validList.AsNoTracking().OrderBy(p => p.ProductModel);

            switch (sortOrder)
            {
                case "name_desc":
                    qry = validList.AsNoTracking().OrderByDescending(p => p.ProductModel);
                    break;
                case "price_asc":
                    qry = validList.AsNoTracking().OrderBy(p => p.Price);
                    break;
                case "price_desc":
                    qry = validList.AsNoTracking().OrderByDescending(p => p.Price);
                    break;
                case "sellDate_asc":
                    qry = validList.AsNoTracking().OrderBy(p => p.StartSellDate);
                    break;
                case "sellDate_desc":
                    qry = validList.AsNoTracking().OrderByDescending(p => p.StartSellDate);
                    break;
                default:
                    qry = validList.AsNoTracking().OrderBy(p => p.ProductModel);
                    break;
            }

            var model = await PagingList<AccessoriesListModel>.CreateAsync(qry, 5, page);
            model.Action = "BottlesAndCages";
            return View(model);
        }

        [Obsolete]
        public async Task<IActionResult> Cleaners(string sortOrder, int page = 1)
        {
            ViewBag.NameSortDParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.NameSortAParm = String.IsNullOrEmpty(sortOrder) ? "name_asc" : "";
            ViewBag.PriceSortDParm = String.IsNullOrEmpty(sortOrder) ? "price_desc" : "";
            ViewBag.PriceSortAParm = String.IsNullOrEmpty(sortOrder) ? "price_asc" : "";
            ViewBag.SellDateSortDParm = String.IsNullOrEmpty(sortOrder) ? "sellDate_desc" : "";
            ViewBag.SellDateSortAParm = String.IsNullOrEmpty(sortOrder) ? "sellDate_asc" : "";

            /*var validList = (from items in db.VProductAndDescription
                             where items.Culture == "en" &&
                             items.ProductCategoryId == 33 &&
                             items.SellEndDate == null
                             select new AccessoriesListModel
                             {
                                 ProductModel = items.ProductModel,
                                 Description = items.Description,
                                 ProductModelID = items.ProductModelId
                             }).Distinct().ToList();*/

            var validList = (from pd in db.VProductAndDescription
                             join p in db.Product
                             on pd.ProductModelId equals p.ProductModelId
                             where pd.Culture == "fr" &&
                             pd.SellEndDate == null &&
                             pd.ProductCategoryId == 33
                             select new AccessoriesListModel
                             {
                                 ProductModel = pd.ProductModel,
                                 Description = pd.Description,
                                 ProductModelID = pd.ProductModelId,
                                 Price = p.ListPrice,
                                 Photo = p.ThumbNailPhoto,
                                 StartSellDate = p.SellStartDate
                             }).Distinct();

            var qry = validList.AsNoTracking().OrderBy(p => p.ProductModel);

            switch (sortOrder)
            {
                case "name_desc":
                    qry = validList.AsNoTracking().OrderByDescending(p => p.ProductModel);
                    break;
                case "price_asc":
                    qry = validList.AsNoTracking().OrderBy(p => p.Price);
                    break;
                case "price_desc":
                    qry = validList.AsNoTracking().OrderByDescending(p => p.Price);
                    break;
                case "sellDate_asc":
                    qry = validList.AsNoTracking().OrderBy(p => p.StartSellDate);
                    break;
                case "sellDate_desc":
                    qry = validList.AsNoTracking().OrderByDescending(p => p.StartSellDate);
                    break;
                default:
                    qry = validList.AsNoTracking().OrderBy(p => p.ProductModel);
                    break;
            }

            var model = await PagingList<AccessoriesListModel>.CreateAsync(qry, 5, page);
            model.Action = "Cleaners";
            return View(model);
        }

        [Obsolete]
        public async Task<IActionResult> Fenders(string sortOrder, int page = 1)
        {
            ViewBag.NameSortDParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.NameSortAParm = String.IsNullOrEmpty(sortOrder) ? "name_asc" : "";
            ViewBag.PriceSortDParm = String.IsNullOrEmpty(sortOrder) ? "price_desc" : "";
            ViewBag.PriceSortAParm = String.IsNullOrEmpty(sortOrder) ? "price_asc" : "";
            ViewBag.SellDateSortDParm = String.IsNullOrEmpty(sortOrder) ? "sellDate_desc" : "";
            ViewBag.SellDateSortAParm = String.IsNullOrEmpty(sortOrder) ? "sellDate_asc" : "";

            /*var validList = (from items in db.VProductAndDescription
                             where items.Culture == "en" &&
                             items.ProductCategoryId == 34 &&
                             items.SellEndDate == null
                             select new AccessoriesListModel
                             {
                                 ProductModel = items.ProductModel,
                                 Description = items.Description,
                                 ProductModelID = items.ProductModelId
                             }).Distinct().ToList();*/

            var validList = (from pd in db.VProductAndDescription
                             join p in db.Product
                             on pd.ProductModelId equals p.ProductModelId
                             where pd.Culture == "fr" &&
                             pd.SellEndDate == null &&
                             pd.ProductCategoryId == 34
                             select new AccessoriesListModel
                             {
                                 ProductModel = pd.ProductModel,
                                 Description = pd.Description,
                                 ProductModelID = pd.ProductModelId,
                                 Price = p.ListPrice,
                                 Photo = p.ThumbNailPhoto,
                                 StartSellDate = p.SellStartDate
                             }).Distinct();

            var qry = validList.AsNoTracking().OrderBy(p => p.ProductModel);

            switch (sortOrder)
            {
                case "name_desc":
                    qry = validList.AsNoTracking().OrderByDescending(p => p.ProductModel);
                    break;
                case "price_asc":
                    qry = validList.AsNoTracking().OrderBy(p => p.Price);
                    break;
                case "price_desc":
                    qry = validList.AsNoTracking().OrderByDescending(p => p.Price);
                    break;
                case "sellDate_asc":
                    qry = validList.AsNoTracking().OrderBy(p => p.StartSellDate);
                    break;
                case "sellDate_desc":
                    qry = validList.AsNoTracking().OrderByDescending(p => p.StartSellDate);
                    break;
                default:
                    qry = validList.AsNoTracking().OrderBy(p => p.ProductModel);
                    break;
            }

            var model = await PagingList<AccessoriesListModel>.CreateAsync(qry, 5, page);
            model.Action = "Fenders";
            return View(model);
        }

        [Obsolete]
        public async Task<IActionResult> Helmets(string sortOrder, int page = 1)
        {
            ViewBag.NameSortDParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.NameSortAParm = String.IsNullOrEmpty(sortOrder) ? "name_asc" : "";
            ViewBag.PriceSortDParm = String.IsNullOrEmpty(sortOrder) ? "price_desc" : "";
            ViewBag.PriceSortAParm = String.IsNullOrEmpty(sortOrder) ? "price_asc" : "";
            ViewBag.SellDateSortDParm = String.IsNullOrEmpty(sortOrder) ? "sellDate_desc" : "";
            ViewBag.SellDateSortAParm = String.IsNullOrEmpty(sortOrder) ? "sellDate_asc" : "";

            /*var validList = (from items in db.VProductAndDescription
                             where items.Culture == "en" &&
                             items.ProductCategoryId == 35 &&
                             items.SellEndDate == null
                             select new AccessoriesListModel
                             {
                                 ProductModel = items.ProductModel,
                                 Description = items.Description,
                                 ProductModelID = items.ProductModelId
                             }).Distinct().ToList();*/

            var validList = (from pd in db.VProductAndDescription
                             join p in db.Product
                             on pd.ProductModelId equals p.ProductModelId
                             where pd.Culture == "fr" &&
                             pd.SellEndDate == null &&
                             pd.ProductCategoryId == 35
                             select new AccessoriesListModel
                             {
                                 ProductModel = pd.ProductModel,
                                 Description = pd.Description,
                                 ProductModelID = pd.ProductModelId,
                                 Price = p.ListPrice,
                                 Photo = p.ThumbNailPhoto,
                                 StartSellDate = p.SellStartDate
                             }).Distinct();

            var qry = validList.AsNoTracking().OrderBy(p => p.ProductModel);

            switch (sortOrder)
            {
                case "name_desc":
                    qry = validList.AsNoTracking().OrderByDescending(p => p.ProductModel);
                    break;
                case "price_asc":
                    qry = validList.AsNoTracking().OrderBy(p => p.Price);
                    break;
                case "price_desc":
                    qry = validList.AsNoTracking().OrderByDescending(p => p.Price);
                    break;
                case "sellDate_asc":
                    qry = validList.AsNoTracking().OrderBy(p => p.StartSellDate);
                    break;
                case "sellDate_desc":
                    qry = validList.AsNoTracking().OrderByDescending(p => p.StartSellDate);
                    break;
                default:
                    qry = validList.AsNoTracking().OrderBy(p => p.ProductModel);
                    break;
            }

            var model = await PagingList<AccessoriesListModel>.CreateAsync(qry, 5, page);
            model.Action = "Helmets";
            return View(model);
        }

        [Obsolete]
        public async Task<IActionResult> HydrationPacks(string sortOrder, int page = 1)
        {
            ViewBag.NameSortDParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.NameSortAParm = String.IsNullOrEmpty(sortOrder) ? "name_asc" : "";
            ViewBag.PriceSortDParm = String.IsNullOrEmpty(sortOrder) ? "price_desc" : "";
            ViewBag.PriceSortAParm = String.IsNullOrEmpty(sortOrder) ? "price_asc" : "";
            ViewBag.SellDateSortDParm = String.IsNullOrEmpty(sortOrder) ? "sellDate_desc" : "";
            ViewBag.SellDateSortAParm = String.IsNullOrEmpty(sortOrder) ? "sellDate_asc" : "";

            /*var validList = (from items in db.VProductAndDescription
                             where items.Culture == "en" &&
                             items.ProductCategoryId == 36 &&
                             items.SellEndDate == null
                             select new AccessoriesListModel
                             {
                                 ProductModel = items.ProductModel,
                                 Description = items.Description,
                                 ProductModelID = items.ProductModelId
                             }).Distinct().ToList();*/

            var validList = (from pd in db.VProductAndDescription
                             join p in db.Product
                             on pd.ProductModelId equals p.ProductModelId
                             where pd.Culture == "fr" &&
                             pd.SellEndDate == null &&
                             pd.ProductCategoryId == 36
                             select new AccessoriesListModel
                             {
                                 ProductModel = pd.ProductModel,
                                 Description = pd.Description,
                                 ProductModelID = pd.ProductModelId,
                                 Price = p.ListPrice,
                                 Photo = p.ThumbNailPhoto,
                                 StartSellDate = p.SellStartDate
                             }).Distinct();

            var qry = validList.AsNoTracking().OrderBy(p => p.ProductModel);

            switch (sortOrder)
            {
                case "name_desc":
                    qry = validList.AsNoTracking().OrderByDescending(p => p.ProductModel);
                    break;
                case "price_asc":
                    qry = validList.AsNoTracking().OrderBy(p => p.Price);
                    break;
                case "price_desc":
                    qry = validList.AsNoTracking().OrderByDescending(p => p.Price);
                    break;
                case "sellDate_asc":
                    qry = validList.AsNoTracking().OrderBy(p => p.StartSellDate);
                    break;
                case "sellDate_desc":
                    qry = validList.AsNoTracking().OrderByDescending(p => p.StartSellDate);
                    break;
                default:
                    qry = validList.AsNoTracking().OrderBy(p => p.ProductModel);
                    break;
            }

            var model = await PagingList<AccessoriesListModel>.CreateAsync(qry, 5, page);
            model.Action = "HydrationPacks";
            return View(model);
        }

        public IActionResult Lights()
        {
            var validList = (from items in db.VProductAndDescription
                             where items.Culture == "fr" &&
                             items.ProductCategoryId == 37 &&
                             items.SellEndDate == null
                             select new AccessoriesListModel
                             {
                                 ProductModel = items.ProductModel,
                                 Description = items.Description,
                                 ProductModelID = items.ProductModelId
                             }).Distinct().ToList();
            return View(validList);
        }

        public IActionResult Locks()
        {
            var validList = (from items in db.VProductAndDescription
                             where items.Culture == "fr" &&
                             items.ProductCategoryId == 38 &&
                             items.SellEndDate == null
                             select new AccessoriesListModel
                             {
                                 ProductModel = items.ProductModel,
                                 Description = items.Description,
                                 ProductModelID = items.ProductModelId
                             }).Distinct().ToList();
            return View(validList);
        }

        public IActionResult Panniers()
        {
            var validList = (from items in db.VProductAndDescription
                             where items.Culture == "fr" &&
                             items.ProductCategoryId == 39 &&
                             items.SellEndDate == null
                             select new AccessoriesListModel
                             {
                                 ProductModel = items.ProductModel,
                                 Description = items.Description,
                                 ProductModelID = items.ProductModelId
                             }).Distinct().ToList();
            return View(validList);
        }

        public IActionResult Pumps()
        {
            var validList = (from items in db.VProductAndDescription
                             where items.Culture == "fr" &&
                             items.ProductCategoryId == 40 &&
                             items.SellEndDate == null
                             select new AccessoriesListModel
                             {
                                 ProductModel = items.ProductModel,
                                 Description = items.Description,
                                 ProductModelID = items.ProductModelId
                             }).Distinct().ToList();
            return View(validList);
        }

        [Obsolete]
        public async Task<IActionResult> TiresAndTubes(string sortOrder, int page = 1)
        {
            ViewBag.NameSortDParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.NameSortAParm = String.IsNullOrEmpty(sortOrder) ? "name_asc" : "";
            ViewBag.PriceSortDParm = String.IsNullOrEmpty(sortOrder) ? "price_desc" : "";
            ViewBag.PriceSortAParm = String.IsNullOrEmpty(sortOrder) ? "price_asc" : "";
            ViewBag.SellDateSortDParm = String.IsNullOrEmpty(sortOrder) ? "sellDate_desc" : "";
            ViewBag.SellDateSortAParm = String.IsNullOrEmpty(sortOrder) ? "sellDate_asc" : "";

            /*var validList = (from items in db.VProductAndDescription
                             where items.Culture == "en" &&
                             items.ProductCategoryId == 41 &&
                             items.SellEndDate == null
                             select new AccessoriesListModel
                             {
                                 ProductModel = items.ProductModel,
                                 Description = items.Description,
                                 ProductModelID = items.ProductModelId
                             }).Distinct().ToList();*/

            var validList = (from pd in db.VProductAndDescription
                             join p in db.Product
                             on pd.ProductModelId equals p.ProductModelId
                             where pd.Culture == "fr" &&
                             pd.SellEndDate == null &&
                             pd.ProductCategoryId == 41
                             select new AccessoriesListModel
                             {
                                 ProductModel = pd.ProductModel,
                                 Description = pd.Description,
                                 ProductModelID = pd.ProductModelId,
                                 Price = p.ListPrice,
                                 Photo = p.ThumbNailPhoto,
                                 StartSellDate = p.SellStartDate
                             }).Distinct();

            var qry = validList.AsNoTracking().OrderBy(p => p.ProductModel);

            switch (sortOrder)
            {
                case "name_desc":
                    qry = validList.AsNoTracking().OrderByDescending(p => p.ProductModel);
                    break;
                case "price_asc":
                    qry = validList.AsNoTracking().OrderBy(p => p.Price);
                    break;
                case "price_desc":
                    qry = validList.AsNoTracking().OrderByDescending(p => p.Price);
                    break;
                case "sellDate_asc":
                    qry = validList.AsNoTracking().OrderBy(p => p.StartSellDate);
                    break;
                case "sellDate_desc":
                    qry = validList.AsNoTracking().OrderByDescending(p => p.StartSellDate);
                    break;
                default:
                    qry = validList.AsNoTracking().OrderBy(p => p.ProductModel);
                    break;
            }

            var model = await PagingList<AccessoriesListModel>.CreateAsync(qry, 5, page);
            model.Action = "TiresAndTubes";
            return View(model);
        }

        // GET: Accessories/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
                return NotFound();

            var products = from clothing in db.Product
                           where clothing.ProductModelId == id &&
                           clothing.SellEndDate == null
                           select clothing;

            IEnumerable<string> desc = (from details in db.VProductAndDescription
                                        where details.ProductModelId == id &&
                                        details.Culture == "fr"
                                        select details.Description);

            ViewData["description"] = desc.First();

            if (products == null)
                return NotFound();

            return View(products.ToList());
        }
    }
}
