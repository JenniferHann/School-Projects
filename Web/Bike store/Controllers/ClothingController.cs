using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using NoNameBikes.Models;
using PagedList;
using ReflectionIT.Mvc.Paging;

namespace NoNameBikes.Controllers
{
    public class ClothingController : Controller
    {
        private readonly AdventureWorksLT2017Context db;

        public ClothingController(AdventureWorksLT2017Context context)
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
                                   where items.Culture == "en" &&
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
                             where pd.Culture == "en" &&
                             pd.SellEndDate == null &&
                             pd.ProductCategoryId == 23
                             select new ClothingListModel
                             {
                                 ProductModel = pd.ProductModel,
                                 Description = pd.Description,
                                 ProductModelID = pd.ProductModelId,
                                 Price = p.ListPrice,
                                 Photo = p.ThumbNailPhoto,
                                 StartSellDate = p.SellStartDate,
                                 SalePrice = p.SalePrice,
                                 SaleStartDate = p.SaleStartDate,
                                 SaleEndDate = p.SaleEndDate
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
                             where pd.Culture == "en" &&
                             pd.SellEndDate == null &&
                             pd.ProductCategoryId == 24
                             select new ClothingListModel
                             {
                                 ProductModel = pd.ProductModel,
                                 Description = pd.Description,
                                 ProductModelID = pd.ProductModelId,
                                 Price = p.ListPrice,
                                 Photo = p.ThumbNailPhoto,
                                 StartSellDate = p.SellStartDate,
                                 SalePrice = p.SalePrice,
                                 SaleStartDate = p.SaleStartDate,
                                 SaleEndDate = p.SaleEndDate
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
                             where pd.Culture == "en" &&
                             pd.SellEndDate == null &&
                             pd.ProductCategoryId == 25
                             select new ClothingListModel
                             {
                                 ProductModel = pd.ProductModel,
                                 Description = pd.Description,
                                 ProductModelID = pd.ProductModelId,
                                 Price = p.ListPrice,
                                 Photo = p.ThumbNailPhoto,
                                 StartSellDate = p.SellStartDate,
                                 SalePrice = p.SalePrice,
                                 SaleStartDate = p.SaleStartDate,
                                 SaleEndDate = p.SaleEndDate
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
                             where pd.Culture == "en" &&
                             pd.SellEndDate == null &&
                             pd.ProductCategoryId == 26
                             select new ClothingListModel
                             {
                                 ProductModel = pd.ProductModel,
                                 Description = pd.Description,
                                 ProductModelID = pd.ProductModelId,
                                 Price = p.ListPrice,
                                 Photo = p.ThumbNailPhoto,
                                 StartSellDate = p.SellStartDate,
                                 SalePrice = p.SalePrice,
                                 SaleStartDate = p.SaleStartDate,
                                 SaleEndDate = p.SaleEndDate
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
                             where pd.Culture == "en" &&
                             pd.SellEndDate == null &&
                             pd.ProductCategoryId == 27
                             select new ClothingListModel
                             {
                                 ProductModel = pd.ProductModel,
                                 Description = pd.Description,
                                 ProductModelID = pd.ProductModelId,
                                 Price = p.ListPrice,
                                 Photo = p.ThumbNailPhoto,
                                 StartSellDate = p.SellStartDate,
                                 SalePrice = p.SalePrice,
                                 SaleStartDate = p.SaleStartDate,
                                 SaleEndDate = p.SaleEndDate
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
                             where items.Culture == "en" &&
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
                             where pd.Culture == "en" &&
                             pd.SellEndDate == null &&
                             pd.ProductCategoryId == 29
                             select new ClothingListModel
                             {
                                 ProductModel = pd.ProductModel,
                                 Description = pd.Description,
                                 ProductModelID = pd.ProductModelId,
                                 Price = p.ListPrice,
                                 Photo = p.ThumbNailPhoto,
                                 StartSellDate = p.SellStartDate,
                                 SalePrice = p.SalePrice,
                                 SaleStartDate = p.SaleStartDate,
                                 SaleEndDate = p.SaleEndDate
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
                                        where details.ProductModelId == id
                                        select details.Description);
            
            ViewData["description"] = desc.First();

            if (products == null)
            {
                return NotFound();
            }

            return View(products.ToList());
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// FRENCH VERSION
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [Obsolete]
        public async Task<IActionResult> IndexFR(string sortOrder, int page = 1)
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

        public IActionResult BibShortsFR()
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
        public async Task<IActionResult> CapsFR(string sortOrder, int page = 1)
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
            model.Action = "CapsFR";
            return View(model);
        }

        [Obsolete]
        public async Task<IActionResult> GlovesFR(string sortOrder, int page = 1)
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
            model.Action = "GlovesFR";
            return View(model);
        }

        [Obsolete]
        public async Task<IActionResult> JerseysFR(string sortOrder, int page = 1)
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
            model.Action = "JerseysFR";
            return View(model);
        }

        [Obsolete]
        public async Task<IActionResult> ShortsFR(string sortOrder, int page = 1)
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
            model.Action = "ShortsFR";
            return View(model);
        }

        [Obsolete]
        public async Task<IActionResult> SocksFR(string sortOrder, int page = 1)
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
            model.Action = "SocksFR";
            return View(model);
        }

        public IActionResult TightsFR()
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
        public async Task<IActionResult> VestsFR(string sortOrder, int page = 1)
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
            model.Action = "VestsFR";
            return View(model);
        }

        // GET: Clothing/Details/5
        public IActionResult DetailsFR(int? id)
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

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        // GET: Clothing/Create
        public IActionResult Create()
        {
            ViewData["ProductCategoryId"] = new SelectList(db.ProductCategory, "ProductCategoryId", "Name");
            ViewData["ProductModelId"] = new SelectList(db.ProductModel, "ProductModelId", "Name");
            return View();
        }

        // POST: Clothing/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,Name,ProductNumber,Color,StandardCost,ListPrice,Size,Weight,ProductCategoryId,ProductModelId,SellStartDate,SellEndDate,DiscontinuedDate,ThumbNailPhoto,ThumbnailPhotoFileName,Rowguid,ModifiedDate")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Add(product);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductCategoryId"] = new SelectList(db.ProductCategory, "ProductCategoryId", "Name", product.ProductCategoryId);
            ViewData["ProductModelId"] = new SelectList(db.ProductModel, "ProductModelId", "Name", product.ProductModelId);
            return View(product);
        }

        // GET: Clothing/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await db.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["ProductCategoryId"] = new SelectList(db.ProductCategory, "ProductCategoryId", "Name", product.ProductCategoryId);
            ViewData["ProductModelId"] = new SelectList(db.ProductModel, "ProductModelId", "Name", product.ProductModelId);
            return View(product);
        }

        // POST: Clothing/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,Name,ProductNumber,Color,StandardCost,ListPrice,Size,Weight,ProductCategoryId,ProductModelId,SellStartDate,SellEndDate,DiscontinuedDate,ThumbNailPhoto,ThumbnailPhotoFileName,Rowguid,ModifiedDate")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(product);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductCategoryId"] = new SelectList(db.ProductCategory, "ProductCategoryId", "Name", product.ProductCategoryId);
            ViewData["ProductModelId"] = new SelectList(db.ProductModel, "ProductModelId", "Name", product.ProductModelId);
            return View(product);
        }

        // GET: Clothing/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await db.Product
                .Include(p => p.ProductCategory)
                .Include(p => p.ProductModel)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Clothing/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await db.Product.FindAsync(id);
            db.Product.Remove(product);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return db.Product.Any(e => e.ProductId == id);
        }
    }
}
