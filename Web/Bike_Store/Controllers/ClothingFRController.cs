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
    public class ClothingFRController : Controller
    {
        private readonly AdventureWorksLT2017Context db;

        public ClothingFRController(AdventureWorksLT2017Context context)
        {
            db = context;
        }

        // GET: Clothing
        [Obsolete]
        public async Task<IActionResult> Index(string sortOrder, int page = 1)
        {
            ViewBag.NameSortDParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.NameSortAParm = String.IsNullOrEmpty(sortOrder) ? "name_asc" : "";

            var clothingList = from clothings in db.ProductCategory
                               where clothings.ParentProductCategoryId == 3 &&
                               clothings.Name != "Bib-Shorts" &&
                               clothings.Name != "Tights"
                               select clothings;

            var qry = clothingList.AsNoTracking().OrderBy(p => p.Name);

            if (sortOrder == "name_desc")
            {
                qry = clothingList.AsNoTracking().OrderByDescending(p => p.Name);
            }
            else
            {
                qry = clothingList.AsNoTracking().OrderBy(p => p.Name);
            }


            var model = await PagingList<ProductCategory>.CreateAsync(qry, 5, page);

            return View(model);
        }

        public IActionResult BibShorts()
        {
            var validList = (from items in db.VProductAndDescription
                             where items.Culture == "fr" &&
                             items.ProductCategoryId == 22 &&
                             items.SellEndDate == null
                             select new ClothingListModel
                             {
                                 ProductModel = items.ProductModel,
                                 Description = items.Description,
                                 ProductModelID = items.ProductModelId
                             }).Distinct().ToList();
            return View(validList);
        }

        [Obsolete]
        public async Task<IActionResult> Caps(string sortOrder, int page = 1)
        {
            ViewBag.NameSortDParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.NameSortAParm = String.IsNullOrEmpty(sortOrder) ? "name_asc" : "";
            ViewBag.PriceSortDParm = String.IsNullOrEmpty(sortOrder) ? "price_desc" : "";
            ViewBag.PriceSortAParm = String.IsNullOrEmpty(sortOrder) ? "price_asc" : "";
            ViewBag.SellDateSortDParm = String.IsNullOrEmpty(sortOrder) ? "sellDate_desc" : "";
            ViewBag.SellDateSortAParm = String.IsNullOrEmpty(sortOrder) ? "sellDate_asc" : "";

            /*var validList = (from items in db.VProductAndDescription
                             where items.Culture == "en" &&
                             items.ProductCategoryId == 23 &&
                             items.SellEndDate == null
                             select new ClothingListModel
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
                             pd.ProductCategoryId == 23
                             select new ClothingListModel
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

            var model = await PagingList<ClothingListModel>.CreateAsync(qry, 5, page);
            model.Action = "Caps";
            return View(model);
        }

        [Obsolete]
        public async Task<IActionResult> Gloves(string sortOrder, int page = 1)
        {
            ViewBag.NameSortDParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.NameSortAParm = String.IsNullOrEmpty(sortOrder) ? "name_asc" : "";
            ViewBag.PriceSortDParm = String.IsNullOrEmpty(sortOrder) ? "price_desc" : "";
            ViewBag.PriceSortAParm = String.IsNullOrEmpty(sortOrder) ? "price_asc" : "";
            ViewBag.SellDateSortDParm = String.IsNullOrEmpty(sortOrder) ? "sellDate_desc" : "";
            ViewBag.SellDateSortAParm = String.IsNullOrEmpty(sortOrder) ? "sellDate_asc" : "";

            /*var validList = (from items in db.VProductAndDescription
                             where items.Culture == "en" &&
                             items.ProductCategoryId == 24 &&
                             items.SellEndDate == null
                             select new ClothingListModel
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
                             pd.ProductCategoryId == 24
                             select new ClothingListModel
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

            var model = await PagingList<ClothingListModel>.CreateAsync(qry, 5, page);
            model.Action = "Gloves";
            return View(model);
        }

        [Obsolete]
        public async Task<IActionResult> Jerseys(string sortOrder, int page = 1)
        {
            ViewBag.NameSortDParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.NameSortAParm = String.IsNullOrEmpty(sortOrder) ? "name_asc" : "";
            ViewBag.PriceSortDParm = String.IsNullOrEmpty(sortOrder) ? "price_desc" : "";
            ViewBag.PriceSortAParm = String.IsNullOrEmpty(sortOrder) ? "price_asc" : "";
            ViewBag.SellDateSortDParm = String.IsNullOrEmpty(sortOrder) ? "sellDate_desc" : "";
            ViewBag.SellDateSortAParm = String.IsNullOrEmpty(sortOrder) ? "sellDate_asc" : "";

            /*var validList = (from items in db.VProductAndDescription
                             where items.Culture == "en" &&
                             items.ProductCategoryId == 25 &&
                             items.SellEndDate == null
                             select new ClothingListModel
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
                             pd.ProductCategoryId == 25
                             select new ClothingListModel
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

            var model = await PagingList<ClothingListModel>.CreateAsync(qry, 5, page);
            model.Action = "Jerseys";
            return View(model);
        }

        [Obsolete]
        public async Task<IActionResult> Shorts(string sortOrder, int page = 1)
        {
            ViewBag.NameSortDParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.NameSortAParm = String.IsNullOrEmpty(sortOrder) ? "name_asc" : "";
            ViewBag.PriceSortDParm = String.IsNullOrEmpty(sortOrder) ? "price_desc" : "";
            ViewBag.PriceSortAParm = String.IsNullOrEmpty(sortOrder) ? "price_asc" : "";
            ViewBag.SellDateSortDParm = String.IsNullOrEmpty(sortOrder) ? "sellDate_desc" : "";
            ViewBag.SellDateSortAParm = String.IsNullOrEmpty(sortOrder) ? "sellDate_asc" : "";

            /*var validList = (from items in db.VProductAndDescription
                             where items.Culture == "en" &&
                             items.ProductCategoryId == 26 &&
                             items.SellEndDate == null
                             select new ClothingListModel
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
                             pd.ProductCategoryId == 26
                             select new ClothingListModel
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

            var model = await PagingList<ClothingListModel>.CreateAsync(qry, 5, page);
            model.Action = "Shorts";
            return View(model);
        }

        [Obsolete]
        public async Task<IActionResult> Socks(string sortOrder, int page = 1)
        {
            ViewBag.NameSortDParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.NameSortAParm = String.IsNullOrEmpty(sortOrder) ? "name_asc" : "";
            ViewBag.PriceSortDParm = String.IsNullOrEmpty(sortOrder) ? "price_desc" : "";
            ViewBag.PriceSortAParm = String.IsNullOrEmpty(sortOrder) ? "price_asc" : "";
            ViewBag.SellDateSortDParm = String.IsNullOrEmpty(sortOrder) ? "sellDate_desc" : "";
            ViewBag.SellDateSortAParm = String.IsNullOrEmpty(sortOrder) ? "sellDate_asc" : "";

            /*var validList = (from items in db.VProductAndDescription
                             where items.Culture == "en" &&
                             items.ProductCategoryId == 27 &&
                             items.SellEndDate == null
                             select new ClothingListModel
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
                             pd.ProductCategoryId == 27
                             select new ClothingListModel
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

            var model = await PagingList<ClothingListModel>.CreateAsync(qry, 5, page);
            model.Action = "Socks";
            return View(model);
        }

        public IActionResult Tights()
        {
            var validList = (from items in db.VProductAndDescription
                             where items.Culture == "fr" &&
                             items.ProductCategoryId == 28 &&
                             items.SellEndDate == null
                             select new ClothingListModel
                             {
                                 ProductModel = items.ProductModel,
                                 Description = items.Description,
                                 ProductModelID = items.ProductModelId
                             }).Distinct().ToList();
            return View(validList);
        }

        [Obsolete]
        public async Task<IActionResult> Vests(string sortOrder, int page = 1)
        {
            ViewBag.NameSortDParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.NameSortAParm = String.IsNullOrEmpty(sortOrder) ? "name_asc" : "";
            ViewBag.PriceSortDParm = String.IsNullOrEmpty(sortOrder) ? "price_desc" : "";
            ViewBag.PriceSortAParm = String.IsNullOrEmpty(sortOrder) ? "price_asc" : "";
            ViewBag.SellDateSortDParm = String.IsNullOrEmpty(sortOrder) ? "sellDate_desc" : "";
            ViewBag.SellDateSortAParm = String.IsNullOrEmpty(sortOrder) ? "sellDate_asc" : "";

            /*var validList = (from items in db.VProductAndDescription
                             where items.Culture == "en" &&
                             items.ProductCategoryId == 29 &&
                             items.SellEndDate == null
                             select new ClothingListModel
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
                             pd.ProductCategoryId == 29
                             select new ClothingListModel
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

            var model = await PagingList<ClothingListModel>.CreateAsync(qry, 5, page);
            model.Action = "Vests";
            return View(model);
        }

        // GET: Clothing/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

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
            {
                return NotFound();
            }

            return View(products.ToList());
        }
    }
}
