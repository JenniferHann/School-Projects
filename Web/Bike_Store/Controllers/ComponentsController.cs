using System;
using System.Collections.Generic;
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
    public class ComponentsController : Controller
    {
        private readonly AdventureWorksLT2017Context db;

        public ComponentsController(AdventureWorksLT2017Context context)
        {
            db = context;
        }

        // GET: Components
        [Obsolete]
        public async Task<IActionResult> Index(string sortOrder, int page = 1)
        {
            ViewBag.NameSortDParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.NameSortAParm = String.IsNullOrEmpty(sortOrder) ? "name_asc" : "";

            var componentsList = from components in db.ProductCategory
                                 where components.ParentProductCategoryId == 2 &&
                                 components.Name != "Forks" &&
                                 components.Name != "Headsets" &&
                                 components.Name != "Wheels"
                                 select components;

            var qry = componentsList.AsNoTracking().OrderBy(p => p.Name);

            if (sortOrder == "name_desc")
            {
                qry = componentsList.AsNoTracking().OrderByDescending(p => p.Name);
            }
            else
            {
                qry = componentsList.AsNoTracking().OrderBy(p => p.Name);
            }

            
            var model = await PagingList<ProductCategory>.CreateAsync(qry, 5, page);

            return View(model);
        }

        [Obsolete]
        public async Task<IActionResult> Handlebars(string sortOrder, int page = 1)
        {
            ViewBag.NameSortDParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.NameSortAParm = String.IsNullOrEmpty(sortOrder) ? "name_asc" : "";
            ViewBag.PriceSortDParm = String.IsNullOrEmpty(sortOrder) ? "price_desc" : "";
            ViewBag.PriceSortAParm = String.IsNullOrEmpty(sortOrder) ? "price_asc" : "";
            ViewBag.SellDateSortDParm = String.IsNullOrEmpty(sortOrder) ? "sellDate_desc" : "";
            ViewBag.SellDateSortAParm = String.IsNullOrEmpty(sortOrder) ? "sellDate_asc" : "";

            /*var validHandlebars = (from handlerbars in db.VProductAndDescription
                                  where handlerbars.Culture == "en" &&
                                  handlerbars.ProductCategoryId == 8 &&
                                  handlerbars.SellEndDate == null
                                  select new ComponentListModel { 
                                        ProductModel = handlerbars.ProductModel,
                                        Description = handlerbars.Description,
                                        ProductModelID = handlerbars.ProductModelId
                                  }).Distinct();*/

            var validHandlebars = (from pd in db.VProductAndDescription
                                     join p in db.Product
                                     on pd.ProductModelId equals p.ProductModelId
                                     where pd.Culture == "en" &&
                                     pd.SellEndDate == null &&
                                     pd.ProductCategoryId == 8
                                     select new ComponentListModel
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

            var qry = validHandlebars.AsNoTracking().OrderBy(p => p.ProductModel);

            switch (sortOrder)
            {
                case "name_desc":
                    qry = validHandlebars.AsNoTracking().OrderByDescending(p => p.ProductModel);
                    break;
                case "price_asc":
                    qry = validHandlebars.AsNoTracking().OrderBy(p => p.Price);
                    break;
                case "price_desc":
                    qry = validHandlebars.AsNoTracking().OrderByDescending(p => p.Price);
                    break;
                case "sellDate_asc":
                    qry = validHandlebars.AsNoTracking().OrderBy(p => p.StartSellDate);
                    break;
                case "sellDate_desc":
                    qry = validHandlebars.AsNoTracking().OrderByDescending(p => p.StartSellDate);
                    break;
                default:
                    qry = validHandlebars.AsNoTracking().OrderBy(p => p.ProductModel);
                    break;
            }
            
            var model = await PagingList<ComponentListModel>.CreateAsync(qry, 5, page);
            model.Action = "Handlebars";
            return View(model);
        }

        [Obsolete]
        public async Task<IActionResult> BottomBrackets(string sortOrder, int page = 1)
        {
            ViewBag.NameSortDParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.NameSortAParm = String.IsNullOrEmpty(sortOrder) ? "name_asc" : "";
            ViewBag.PriceSortDParm = String.IsNullOrEmpty(sortOrder) ? "price_desc" : "";
            ViewBag.PriceSortAParm = String.IsNullOrEmpty(sortOrder) ? "price_asc" : "";
            ViewBag.SellDateSortDParm = String.IsNullOrEmpty(sortOrder) ? "sellDate_desc" : "";
            ViewBag.SellDateSortAParm = String.IsNullOrEmpty(sortOrder) ? "sellDate_asc" : "";

            /*var validBottomBrackets = (from BottomBrackets in db.VProductAndDescription
                                   where BottomBrackets.Culture == "en" &&
                                   BottomBrackets.ProductCategoryId == 9 &&
                                   BottomBrackets.SellEndDate == null
                                   select new ComponentListModel
                                   {
                                       ProductModel = BottomBrackets.ProductModel,
                                       Description = BottomBrackets.Description,
                                       ProductModelID = BottomBrackets.ProductModelId
                                   }).Distinct();*/

            var validBottomBrackets = (from pd in db.VProductAndDescription
                                   join p in db.Product
                                   on pd.ProductModelId equals p.ProductModelId
                                   where pd.Culture == "en" &&
                                   pd.SellEndDate == null &&
                                   pd.ProductCategoryId == 9
                                   select new ComponentListModel
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

            var qry = validBottomBrackets.AsNoTracking().OrderBy(p => p.ProductModel);

            switch (sortOrder)
            {
                case "name_desc":
                    qry = validBottomBrackets.AsNoTracking().OrderByDescending(p => p.ProductModel);
                    break;
                case "price_asc":
                    qry = validBottomBrackets.AsNoTracking().OrderBy(p => p.Price);
                    break;
                case "price_desc":
                    qry = validBottomBrackets.AsNoTracking().OrderByDescending(p => p.Price);
                    break;
                case "sellDate_asc":
                    qry = validBottomBrackets.AsNoTracking().OrderBy(p => p.StartSellDate);
                    break;
                case "sellDate_desc":
                    qry = validBottomBrackets.AsNoTracking().OrderByDescending(p => p.StartSellDate);
                    break;
                default:
                    qry = validBottomBrackets.AsNoTracking().OrderBy(p => p.ProductModel);
                    break;
            }

            var model = await PagingList<ComponentListModel>.CreateAsync(qry, 5, page);
            model.Action = "BottomBrackets";
            return View(model);
        }

        [Obsolete]
        public async Task<IActionResult> Brakes(string sortOrder, int page = 1)
        {
            ViewBag.NameSortDParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.NameSortAParm = String.IsNullOrEmpty(sortOrder) ? "name_asc" : "";
            ViewBag.PriceSortDParm = String.IsNullOrEmpty(sortOrder) ? "price_desc" : "";
            ViewBag.PriceSortAParm = String.IsNullOrEmpty(sortOrder) ? "price_asc" : "";
            ViewBag.SellDateSortDParm = String.IsNullOrEmpty(sortOrder) ? "sellDate_desc" : "";
            ViewBag.SellDateSortAParm = String.IsNullOrEmpty(sortOrder) ? "sellDate_asc" : "";

            /*var validBrakes = (from brakes in db.VProductAndDescription
                                   where brakes.Culture == "en" &&
                                   brakes.ProductCategoryId == 10 &&
                                   brakes.SellEndDate == null
                                   select new ComponentListModel
                                   {
                                       ProductModel = brakes.ProductModel,
                                       Description = brakes.Description,
                                       ProductModelID = brakes.ProductModelId
                                   }).Distinct();*/

            var validBrakes = (from pd in db.VProductAndDescription
                                   join p in db.Product
                                   on pd.ProductModelId equals p.ProductModelId
                                   where pd.Culture == "en" &&
                                   pd.SellEndDate == null &&
                                   pd.ProductCategoryId == 10
                                   select new ComponentListModel
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

            var qry = validBrakes.AsNoTracking().OrderBy(p => p.ProductModel);

            switch (sortOrder)
            {
                case "name_desc":
                    qry = validBrakes.AsNoTracking().OrderByDescending(p => p.ProductModel);
                    break;
                case "price_asc":
                    qry = validBrakes.AsNoTracking().OrderBy(p => p.Price);
                    break;
                case "price_desc":
                    qry = validBrakes.AsNoTracking().OrderByDescending(p => p.Price);
                    break;
                case "sellDate_asc":
                    qry = validBrakes.AsNoTracking().OrderBy(p => p.StartSellDate);
                    break;
                case "sellDate_desc":
                    qry = validBrakes.AsNoTracking().OrderByDescending(p => p.StartSellDate);
                    break;
                default:
                    qry = validBrakes.AsNoTracking().OrderBy(p => p.ProductModel);
                    break;
            }

            var model = await PagingList<ComponentListModel>.CreateAsync(qry, 5, page);
            model.Action = "Brakes";
            return View(model);
        }

        [Obsolete]
        public async Task<IActionResult> Chains(string sortOrder, int page = 1)
        {
            ViewBag.NameSortDParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.NameSortAParm = String.IsNullOrEmpty(sortOrder) ? "name_asc" : "";
            ViewBag.PriceSortDParm = String.IsNullOrEmpty(sortOrder) ? "price_desc" : "";
            ViewBag.PriceSortAParm = String.IsNullOrEmpty(sortOrder) ? "price_asc" : "";
            ViewBag.SellDateSortDParm = String.IsNullOrEmpty(sortOrder) ? "sellDate_desc" : "";
            ViewBag.SellDateSortAParm = String.IsNullOrEmpty(sortOrder) ? "sellDate_asc" : "";

            /*var validChains = (from chains in db.VProductAndDescription
                                   where chains.Culture == "en" &&
                                   chains.ProductCategoryId == 11 &&
                                   chains.SellEndDate == null
                                   select new ComponentListModel
                                   {
                                       ProductModel = chains.ProductModel,
                                       Description = chains.Description,
                                       ProductModelID = chains.ProductModelId
                                   }).Distinct();*/

            var validChains = (from pd in db.VProductAndDescription
                                   join p in db.Product
                                   on pd.ProductModelId equals p.ProductModelId
                                   where pd.Culture == "en" &&
                                   pd.SellEndDate == null &&
                                   pd.ProductCategoryId == 11
                                   select new ComponentListModel
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

            var qry = validChains.AsNoTracking().OrderBy(p => p.ProductModel);

            switch (sortOrder)
            {
                case "name_desc":
                    qry = validChains.AsNoTracking().OrderByDescending(p => p.ProductModel);
                    break;
                case "price_asc":
                    qry = validChains.AsNoTracking().OrderBy(p => p.Price);
                    break;
                case "price_desc":
                    qry = validChains.AsNoTracking().OrderByDescending(p => p.Price);
                    break;
                case "sellDate_asc":
                    qry = validChains.AsNoTracking().OrderBy(p => p.StartSellDate);
                    break;
                case "sellDate_desc":
                    qry = validChains.AsNoTracking().OrderByDescending(p => p.StartSellDate);
                    break;
                default:
                    qry = validChains.AsNoTracking().OrderBy(p => p.ProductModel);
                    break;
            }

            var model = await PagingList<ComponentListModel>.CreateAsync(qry, 5, page);
            model.Action = "Chains";
            return View(model);
        }

        [Obsolete]
        public async Task<IActionResult> Cranksets(string sortOrder, int page = 1)
        {
            ViewBag.NameSortDParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.NameSortAParm = String.IsNullOrEmpty(sortOrder) ? "name_asc" : "";
            ViewBag.PriceSortDParm = String.IsNullOrEmpty(sortOrder) ? "price_desc" : "";
            ViewBag.PriceSortAParm = String.IsNullOrEmpty(sortOrder) ? "price_asc" : "";
            ViewBag.SellDateSortDParm = String.IsNullOrEmpty(sortOrder) ? "sellDate_desc" : "";
            ViewBag.SellDateSortAParm = String.IsNullOrEmpty(sortOrder) ? "sellDate_asc" : "";

            /*var validCranksets = (from cranksets in db.VProductAndDescription
                                   where cranksets.Culture == "en" &&
                                   cranksets.ProductCategoryId == 12 &&
                                   cranksets.SellEndDate == null
                                   select new ComponentListModel
                                   {
                                       ProductModel = cranksets.ProductModel,
                                       Description = cranksets.Description,
                                       ProductModelID = cranksets.ProductModelId
                                   }).Distinct();*/

            var validCranksets = (from pd in db.VProductAndDescription
                                   join p in db.Product
                                   on pd.ProductModelId equals p.ProductModelId
                                   where pd.Culture == "en" &&
                                   pd.SellEndDate == null &&
                                   pd.ProductCategoryId == 12
                                   select new ComponentListModel
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

            var qry = validCranksets.AsNoTracking().OrderBy(p => p.ProductModel);

            switch (sortOrder)
            {
                case "name_desc":
                    qry = validCranksets.AsNoTracking().OrderByDescending(p => p.ProductModel);
                    break;
                case "price_asc":
                    qry = validCranksets.AsNoTracking().OrderBy(p => p.Price);
                    break;
                case "price_desc":
                    qry = validCranksets.AsNoTracking().OrderByDescending(p => p.Price);
                    break;
                case "sellDate_asc":
                    qry = validCranksets.AsNoTracking().OrderBy(p => p.StartSellDate);
                    break;
                case "sellDate_desc":
                    qry = validCranksets.AsNoTracking().OrderByDescending(p => p.StartSellDate);
                    break;
                default:
                    qry = validCranksets.AsNoTracking().OrderBy(p => p.ProductModel);
                    break;
            }

            var model = await PagingList<ComponentListModel>.CreateAsync(qry, 5, page);
            model.Action = "Cranksets";
            return View(model);
        }

        [Obsolete]
        public async Task<IActionResult> Derailleurs(string sortOrder, int page = 1)
        {
            ViewBag.NameSortDParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.NameSortAParm = String.IsNullOrEmpty(sortOrder) ? "name_asc" : "";
            ViewBag.PriceSortDParm = String.IsNullOrEmpty(sortOrder) ? "price_desc" : "";
            ViewBag.PriceSortAParm = String.IsNullOrEmpty(sortOrder) ? "price_asc" : "";
            ViewBag.SellDateSortDParm = String.IsNullOrEmpty(sortOrder) ? "sellDate_desc" : "";
            ViewBag.SellDateSortAParm = String.IsNullOrEmpty(sortOrder) ? "sellDate_asc" : "";

            /*var validDerailleurs = (from derailleurs in db.VProductAndDescription
                                   where derailleurs.Culture == "en" &&
                                   derailleurs.ProductCategoryId == 13 &&
                                   derailleurs.SellEndDate == null
                                   select new ComponentListModel
                                   {
                                       ProductModel = derailleurs.ProductModel,
                                       Description = derailleurs.Description,
                                       ProductModelID = derailleurs.ProductModelId
                                   }).Distinct().ToList();*/

            var validDerailleurs = (from pd in db.VProductAndDescription
                                   join p in db.Product
                                   on pd.ProductModelId equals p.ProductModelId
                                   where pd.Culture == "en" &&
                                   pd.SellEndDate == null &&
                                   pd.ProductCategoryId == 13
                                   select new ComponentListModel
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

            var qry = validDerailleurs.AsNoTracking().OrderBy(p => p.ProductModel);

            switch (sortOrder)
            {
                case "name_desc":
                    qry = validDerailleurs.AsNoTracking().OrderByDescending(p => p.ProductModel);
                    break;
                case "price_asc":
                    qry = validDerailleurs.AsNoTracking().OrderBy(p => p.Price);
                    break;
                case "price_desc":
                    qry = validDerailleurs.AsNoTracking().OrderByDescending(p => p.Price);
                    break;
                case "sellDate_asc":
                    qry = validDerailleurs.AsNoTracking().OrderBy(p => p.StartSellDate);
                    break;
                case "sellDate_desc":
                    qry = validDerailleurs.AsNoTracking().OrderByDescending(p => p.StartSellDate);
                    break;
                default:
                    qry = validDerailleurs.AsNoTracking().OrderBy(p => p.ProductModel);
                    break;
            }

            var model = await PagingList<ComponentListModel>.CreateAsync(qry, 5, page);
            model.Action = "Derailleurs";
            return View(model);
        }

        public IActionResult Forks(string sortOrder)
        {
            ViewBag.NameSortDParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.NameSortAParm = String.IsNullOrEmpty(sortOrder) ? "name_asc" : "";
            ViewBag.PriceSortDParm = String.IsNullOrEmpty(sortOrder) ? "price_desc" : "";
            ViewBag.PriceSortAParm = String.IsNullOrEmpty(sortOrder) ? "price_asc" : "";
            ViewBag.SellDateSortDParm = String.IsNullOrEmpty(sortOrder) ? "sellDate_desc" : "";
            ViewBag.SellDateSortAParm = String.IsNullOrEmpty(sortOrder) ? "sellDate_asc" : "";

            /*var validForks = (from forks in db.VProductAndDescription
                                   where forks.Culture == "en" &&
                                   forks.ProductCategoryId == 14 &&
                                   forks.SellEndDate == null
                                   select new ComponentListModel
                                   {
                                       ProductModel = forks.ProductModel,
                                       Description = forks.Description,
                                       ProductModelID = forks.ProductModelId
                                   }).Distinct().ToList();*/

            var validForks = (from pd in db.VProductAndDescription
                                   join p in db.Product
                                   on pd.ProductModelId equals p.ProductModelId
                                   where pd.Culture == "en" &&
                                   pd.SellEndDate == null &&
                                   pd.ProductCategoryId == 14
                                   select new ComponentListModel
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

            switch (sortOrder)
            {
                case "name_desc":
                    validForks = validForks.OrderByDescending(item => item.ProductModel);
                    break;
                case "price_asc":
                    validForks = validForks.OrderBy(item => item.Price);
                    break;
                case "price_desc":
                    validForks = validForks.OrderByDescending(item => item.Price);
                    break;
                case "sellDate_asc":
                    validForks = validForks.OrderBy(item => item.StartSellDate);
                    break;
                case "sellDate_desc":
                    validForks = validForks.OrderByDescending(item => item.StartSellDate);
                    break;
                default:
                    validForks = validForks.OrderBy(item => item.ProductModel);
                    break;
            }

            return View(validForks);
        }

        public IActionResult Headsets()
        {
            var validHeadsets = (from headsets in db.VProductAndDescription
                                   where headsets.Culture == "en" &&
                                   headsets.ProductCategoryId == 15 &&
                                   headsets.SellEndDate == null
                                   select new ComponentListModel
                                   {
                                       ProductModel = headsets.ProductModel,
                                       Description = headsets.Description,
                                       ProductModelID = headsets.ProductModelId
                                   }).Distinct().ToList();
            return View(validHeadsets);
        }

        [Obsolete]
        public async Task<IActionResult> MountainFrames(string sortOrder, int page = 1)
        {
            ViewBag.NameSortDParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.NameSortAParm = String.IsNullOrEmpty(sortOrder) ? "name_asc" : "";
            ViewBag.PriceSortDParm = String.IsNullOrEmpty(sortOrder) ? "price_desc" : "";
            ViewBag.PriceSortAParm = String.IsNullOrEmpty(sortOrder) ? "price_asc" : "";
            ViewBag.SellDateSortDParm = String.IsNullOrEmpty(sortOrder) ? "sellDate_desc" : "";
            ViewBag.SellDateSortAParm = String.IsNullOrEmpty(sortOrder) ? "sellDate_asc" : "";

            /*var validMountainFrames = (from frames in db.VProductAndDescription
                                   where frames.Culture == "en" &&
                                   frames.ProductCategoryId == 16 &&
                                   frames.SellEndDate == null
                                   select new ComponentListModel
                                   {
                                       ProductModel = frames.ProductModel,
                                       Description = frames.Description,
                                       ProductModelID = frames.ProductModelId
                                   }).Distinct().ToList();*/

            var validMountainFrames = (from pd in db.VProductAndDescription
                                   join p in db.Product
                                   on pd.ProductModelId equals p.ProductModelId
                                   where pd.Culture == "en" &&
                                   pd.SellEndDate == null &&
                                   pd.ProductCategoryId == 16
                                   select new ComponentListModel
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

            var qry = validMountainFrames.AsNoTracking().OrderBy(p => p.ProductModel);

            switch (sortOrder)
            {
                case "name_desc":
                    qry = validMountainFrames.AsNoTracking().OrderByDescending(p => p.ProductModel);
                    break;
                case "price_asc":
                    qry = validMountainFrames.AsNoTracking().OrderBy(p => p.Price);
                    break;
                case "price_desc":
                    qry = validMountainFrames.AsNoTracking().OrderByDescending(p => p.Price);
                    break;
                case "sellDate_asc":
                    qry = validMountainFrames.AsNoTracking().OrderBy(p => p.StartSellDate);
                    break;
                case "sellDate_desc":
                    qry = validMountainFrames.AsNoTracking().OrderByDescending(p => p.StartSellDate);
                    break;
                default:
                    qry = validMountainFrames.AsNoTracking().OrderBy(p => p.ProductModel);
                    break;
            }

            var model = await PagingList<ComponentListModel>.CreateAsync(qry, 5, page);
            model.Action = "MountainFrames";
            return View(model);
        }

        [Obsolete]
        public async Task<IActionResult> Pedals(string sortOrder, int page = 1)
        {
            ViewBag.NameSortDParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.NameSortAParm = String.IsNullOrEmpty(sortOrder) ? "name_asc" : "";
            ViewBag.PriceSortDParm = String.IsNullOrEmpty(sortOrder) ? "price_desc" : "";
            ViewBag.PriceSortAParm = String.IsNullOrEmpty(sortOrder) ? "price_asc" : "";
            ViewBag.SellDateSortDParm = String.IsNullOrEmpty(sortOrder) ? "sellDate_desc" : "";
            ViewBag.SellDateSortAParm = String.IsNullOrEmpty(sortOrder) ? "sellDate_asc" : "";

            /*var validPedals = (from pedals in db.VProductAndDescription
                                   where pedals.Culture == "en" &&
                                   pedals.ProductCategoryId == 17 &&
                                   pedals.SellEndDate == null
                                   select new ComponentListModel
                                   {
                                       ProductModel = pedals.ProductModel,
                                       Description = pedals.Description,
                                       ProductModelID = pedals.ProductModelId
                                   }).Distinct().ToList();*/

            var validPedals = (from pd in db.VProductAndDescription
                                   join p in db.Product
                                   on pd.ProductModelId equals p.ProductModelId
                                   where pd.Culture == "en" &&
                                   pd.SellEndDate == null &&
                                   pd.ProductCategoryId == 17
                                   select new ComponentListModel
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

            var qry = validPedals.AsNoTracking().OrderBy(p => p.ProductModel);

            switch (sortOrder)
            {
                case "name_desc":
                    qry = validPedals.AsNoTracking().OrderByDescending(p => p.ProductModel);
                    break;
                case "price_asc":
                    qry = validPedals.AsNoTracking().OrderBy(p => p.Price);
                    break;
                case "price_desc":
                    qry = validPedals.AsNoTracking().OrderByDescending(p => p.Price);
                    break;
                case "sellDate_asc":
                    qry = validPedals.AsNoTracking().OrderBy(p => p.StartSellDate);
                    break;
                case "sellDate_desc":
                    qry = validPedals.AsNoTracking().OrderByDescending(p => p.StartSellDate);
                    break;
                default:
                    qry = validPedals.AsNoTracking().OrderBy(p => p.ProductModel);
                    break;
            }

            var model = await PagingList<ComponentListModel>.CreateAsync(qry, 5, page);
            model.Action = "Pedals";
            return View(model);
        }

        [Obsolete]
        public async Task<IActionResult> RoadFrames(string sortOrder, int page = 1)
        {
            ViewBag.NameSortDParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.NameSortAParm = String.IsNullOrEmpty(sortOrder) ? "name_asc" : "";
            ViewBag.PriceSortDParm = String.IsNullOrEmpty(sortOrder) ? "price_desc" : "";
            ViewBag.PriceSortAParm = String.IsNullOrEmpty(sortOrder) ? "price_asc" : "";
            ViewBag.SellDateSortDParm = String.IsNullOrEmpty(sortOrder) ? "sellDate_desc" : "";
            ViewBag.SellDateSortAParm = String.IsNullOrEmpty(sortOrder) ? "sellDate_asc" : "";

            /*var validRoadFrames = (from frames in db.VProductAndDescription
                                   where frames.Culture == "en" &&
                                   frames.ProductCategoryId == 18 &&
                                   frames.SellEndDate == null
                                   select new ComponentListModel
                                   {
                                       ProductModel = frames.ProductModel,
                                       Description = frames.Description,
                                       ProductModelID = frames.ProductModelId
                                   }).Distinct().ToList();*/

            var validRoadFrames = (from pd in db.VProductAndDescription
                                   join p in db.Product
                                   on pd.ProductModelId equals p.ProductModelId
                                   where pd.Culture == "en" &&
                                   pd.SellEndDate == null &&
                                   pd.ProductCategoryId == 18
                                   select new ComponentListModel
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

            var qry = validRoadFrames.AsNoTracking().OrderBy(p => p.ProductModel);

            switch (sortOrder)
            {
                case "name_desc":
                    qry = validRoadFrames.AsNoTracking().OrderByDescending(p => p.ProductModel);
                    break;
                case "price_asc":
                    qry = validRoadFrames.AsNoTracking().OrderBy(p => p.Price);
                    break;
                case "price_desc":
                    qry = validRoadFrames.AsNoTracking().OrderByDescending(p => p.Price);
                    break;
                case "sellDate_asc":
                    qry = validRoadFrames.AsNoTracking().OrderBy(p => p.StartSellDate);
                    break;
                case "sellDate_desc":
                    qry = validRoadFrames.AsNoTracking().OrderByDescending(p => p.StartSellDate);
                    break;
                default:
                    qry = validRoadFrames.AsNoTracking().OrderBy(p => p.ProductModel);
                    break;
            }

            var model = await PagingList<ComponentListModel>.CreateAsync(qry, 5, page);
            model.Action = "RoadFrames";
            return View(model);
        }

        [Obsolete]
        public async Task<IActionResult> Saddles(string sortOrder, int page = 1)
        {
            ViewBag.NameSortDParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.NameSortAParm = String.IsNullOrEmpty(sortOrder) ? "name_asc" : "";
            ViewBag.PriceSortDParm = String.IsNullOrEmpty(sortOrder) ? "price_desc" : "";
            ViewBag.PriceSortAParm = String.IsNullOrEmpty(sortOrder) ? "price_asc" : "";
            ViewBag.SellDateSortDParm = String.IsNullOrEmpty(sortOrder) ? "sellDate_desc" : "";
            ViewBag.SellDateSortAParm = String.IsNullOrEmpty(sortOrder) ? "sellDate_asc" : "";

            /*var validSaddles = (from saddles in db.VProductAndDescription
                                   where saddles.Culture == "en" &&
                                   saddles.ProductCategoryId == 19 &&
                                   saddles.SellEndDate == null
                                   select new ComponentListModel
                                   {
                                       ProductModel = saddles.ProductModel,
                                       Description = saddles.Description,
                                       ProductModelID = saddles.ProductModelId
                                   }).Distinct().ToList();*/

            var validSaddles = (from pd in db.VProductAndDescription
                                   join p in db.Product
                                   on pd.ProductModelId equals p.ProductModelId
                                   where pd.Culture == "en" &&
                                   pd.SellEndDate == null &&
                                   pd.ProductCategoryId == 19
                                   select new ComponentListModel
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

            var qry = validSaddles.AsNoTracking().OrderBy(p => p.ProductModel);

            switch (sortOrder)
            {
                case "name_desc":
                    qry = validSaddles.AsNoTracking().OrderByDescending(p => p.ProductModel);
                    break;
                case "price_asc":
                    qry = validSaddles.AsNoTracking().OrderBy(p => p.Price);
                    break;
                case "price_desc":
                    qry = validSaddles.AsNoTracking().OrderByDescending(p => p.Price);
                    break;
                case "sellDate_asc":
                    qry = validSaddles.AsNoTracking().OrderBy(p => p.StartSellDate);
                    break;
                case "sellDate_desc":
                    qry = validSaddles.AsNoTracking().OrderByDescending(p => p.StartSellDate);
                    break;
                default:
                    qry = validSaddles.AsNoTracking().OrderBy(p => p.ProductModel);
                    break;
            }

            var model = await PagingList<ComponentListModel>.CreateAsync(qry, 5, page);
            model.Action = "Saddles";
            return View(model);
        }

        [Obsolete]
        public async Task<IActionResult> TouringFrames(string sortOrder, int page = 1)
        {
            ViewBag.NameSortDParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.NameSortAParm = String.IsNullOrEmpty(sortOrder) ? "name_asc" : "";
            ViewBag.PriceSortDParm = String.IsNullOrEmpty(sortOrder) ? "price_desc" : "";
            ViewBag.PriceSortAParm = String.IsNullOrEmpty(sortOrder) ? "price_asc" : "";
            ViewBag.SellDateSortDParm = String.IsNullOrEmpty(sortOrder) ? "sellDate_desc" : "";
            ViewBag.SellDateSortAParm = String.IsNullOrEmpty(sortOrder) ? "sellDate_asc" : "";

            /*var validTouringFrames = (from frames in db.VProductAndDescription
                                   where frames.Culture == "en" &&
                                   frames.ProductCategoryId == 20 &&
                                   frames.SellEndDate == null
                                   select new ComponentListModel
                                   {
                                       ProductModel = frames.ProductModel,
                                       Description = frames.Description,
                                       ProductModelID = frames.ProductModelId
                                   }).Distinct().ToList();*/

            var validTouringFrames = (from pd in db.VProductAndDescription
                                   join p in db.Product
                                   on pd.ProductModelId equals p.ProductModelId
                                   where pd.Culture == "en" &&
                                   pd.SellEndDate == null &&
                                   pd.ProductCategoryId == 20
                                   select new ComponentListModel
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

            var qry = validTouringFrames.AsNoTracking().OrderBy(p => p.ProductModel);

            switch (sortOrder)
            {
                case "name_desc":
                    qry = validTouringFrames.AsNoTracking().OrderByDescending(p => p.ProductModel);
                    break;
                case "price_asc":
                    qry = validTouringFrames.AsNoTracking().OrderBy(p => p.Price);
                    break;
                case "price_desc":
                    qry = validTouringFrames.AsNoTracking().OrderByDescending(p => p.Price);
                    break;
                case "sellDate_asc":
                    qry = validTouringFrames.AsNoTracking().OrderBy(p => p.StartSellDate);
                    break;
                case "sellDate_desc":
                    qry = validTouringFrames.AsNoTracking().OrderByDescending(p => p.StartSellDate);
                    break;
                default:
                    qry = validTouringFrames.AsNoTracking().OrderBy(p => p.ProductModel);
                    break;
            }

            var model = await PagingList<ComponentListModel>.CreateAsync(qry, 5, page);
            model.Action = "TouringFrames";
            return View(model);
        }

        public IActionResult Wheels()
        {
            var validWheels = (from wheels in db.VProductAndDescription
                                   where wheels.Culture == "en" &&
                                   wheels.ProductCategoryId == 21 &&
                                   wheels.SellEndDate == null
                                   select new ComponentListModel
                                   {
                                       ProductModel = wheels.ProductModel,
                                       Description = wheels.Description,
                                       ProductModelID = wheels.ProductModelId
                                   }).Distinct().ToList();
            return View(validWheels);
        }

        // GET: Components/Details/5
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = from components in db.Product
                           where components.ProductModelId == id &&
                           components.SellEndDate == null
                           select components;

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

            var componentsList = from components in db.ProductCategory
                                 where components.ParentProductCategoryId == 2 &&
                                 components.Name != "Forks" &&
                                 components.Name != "Headsets" &&
                                 components.Name != "Wheels"
                                 select components;

            var qry = componentsList.AsNoTracking().OrderBy(p => p.Name);

            if (sortOrder == "name_desc")
            {
                qry = componentsList.AsNoTracking().OrderByDescending(p => p.Name);
            }
            else
            {
                qry = componentsList.AsNoTracking().OrderBy(p => p.Name);
            }


            var model = await PagingList<ProductCategory>.CreateAsync(qry, 5, page);

            return View(model);
        }

        [Obsolete]
        public async Task<IActionResult> HandlebarsFR(string sortOrder, int page = 1)
        {
            ViewBag.NameSortDParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.NameSortAParm = String.IsNullOrEmpty(sortOrder) ? "name_asc" : "";
            ViewBag.PriceSortDParm = String.IsNullOrEmpty(sortOrder) ? "price_desc" : "";
            ViewBag.PriceSortAParm = String.IsNullOrEmpty(sortOrder) ? "price_asc" : "";
            ViewBag.SellDateSortDParm = String.IsNullOrEmpty(sortOrder) ? "sellDate_desc" : "";
            ViewBag.SellDateSortAParm = String.IsNullOrEmpty(sortOrder) ? "sellDate_asc" : "";

            /*var validHandlebars = (from handlerbars in db.VProductAndDescription
                                  where handlerbars.Culture == "en" &&
                                  handlerbars.ProductCategoryId == 8 &&
                                  handlerbars.SellEndDate == null
                                  select new ComponentListModel { 
                                        ProductModel = handlerbars.ProductModel,
                                        Description = handlerbars.Description,
                                        ProductModelID = handlerbars.ProductModelId
                                  }).Distinct();*/

            var validHandlebars = (from pd in db.VProductAndDescription
                                   join p in db.Product
                                   on pd.ProductModelId equals p.ProductModelId
                                   where pd.Culture == "fr" &&
                                   pd.SellEndDate == null &&
                                   pd.ProductCategoryId == 8
                                   select new ComponentListModel
                                   {
                                       ProductModel = pd.ProductModel,
                                       Description = pd.Description,
                                       ProductModelID = pd.ProductModelId,
                                       Price = p.ListPrice,
                                       Photo = p.ThumbNailPhoto,
                                       StartSellDate = p.SellStartDate
                                   }).Distinct();

            var qry = validHandlebars.AsNoTracking().OrderBy(p => p.ProductModel);

            switch (sortOrder)
            {
                case "name_desc":
                    qry = validHandlebars.AsNoTracking().OrderByDescending(p => p.ProductModel);
                    break;
                case "price_asc":
                    qry = validHandlebars.AsNoTracking().OrderBy(p => p.Price);
                    break;
                case "price_desc":
                    qry = validHandlebars.AsNoTracking().OrderByDescending(p => p.Price);
                    break;
                case "sellDate_asc":
                    qry = validHandlebars.AsNoTracking().OrderBy(p => p.StartSellDate);
                    break;
                case "sellDate_desc":
                    qry = validHandlebars.AsNoTracking().OrderByDescending(p => p.StartSellDate);
                    break;
                default:
                    qry = validHandlebars.AsNoTracking().OrderBy(p => p.ProductModel);
                    break;
            }

            var model = await PagingList<ComponentListModel>.CreateAsync(qry, 5, page);
            model.Action = "HandlebarsFR";
            return View(model);
        }

        [Obsolete]
        public async Task<IActionResult> BottomBracketsFR(string sortOrder, int page = 1)
        {
            ViewBag.NameSortDParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.NameSortAParm = String.IsNullOrEmpty(sortOrder) ? "name_asc" : "";
            ViewBag.PriceSortDParm = String.IsNullOrEmpty(sortOrder) ? "price_desc" : "";
            ViewBag.PriceSortAParm = String.IsNullOrEmpty(sortOrder) ? "price_asc" : "";
            ViewBag.SellDateSortDParm = String.IsNullOrEmpty(sortOrder) ? "sellDate_desc" : "";
            ViewBag.SellDateSortAParm = String.IsNullOrEmpty(sortOrder) ? "sellDate_asc" : "";

            /*var validBottomBrackets = (from BottomBrackets in db.VProductAndDescription
                                   where BottomBrackets.Culture == "en" &&
                                   BottomBrackets.ProductCategoryId == 9 &&
                                   BottomBrackets.SellEndDate == null
                                   select new ComponentListModel
                                   {
                                       ProductModel = BottomBrackets.ProductModel,
                                       Description = BottomBrackets.Description,
                                       ProductModelID = BottomBrackets.ProductModelId
                                   }).Distinct();*/

            var validBottomBrackets = (from pd in db.VProductAndDescription
                                       join p in db.Product
                                       on pd.ProductModelId equals p.ProductModelId
                                       where pd.Culture == "fr" &&
                                       pd.SellEndDate == null &&
                                       pd.ProductCategoryId == 9
                                       select new ComponentListModel
                                       {
                                           ProductModel = pd.ProductModel,
                                           Description = pd.Description,
                                           ProductModelID = pd.ProductModelId,
                                           Price = p.ListPrice,
                                           Photo = p.ThumbNailPhoto,
                                           StartSellDate = p.SellStartDate
                                       }).Distinct();

            var qry = validBottomBrackets.AsNoTracking().OrderBy(p => p.ProductModel);

            switch (sortOrder)
            {
                case "name_desc":
                    qry = validBottomBrackets.AsNoTracking().OrderByDescending(p => p.ProductModel);
                    break;
                case "price_asc":
                    qry = validBottomBrackets.AsNoTracking().OrderBy(p => p.Price);
                    break;
                case "price_desc":
                    qry = validBottomBrackets.AsNoTracking().OrderByDescending(p => p.Price);
                    break;
                case "sellDate_asc":
                    qry = validBottomBrackets.AsNoTracking().OrderBy(p => p.StartSellDate);
                    break;
                case "sellDate_desc":
                    qry = validBottomBrackets.AsNoTracking().OrderByDescending(p => p.StartSellDate);
                    break;
                default:
                    qry = validBottomBrackets.AsNoTracking().OrderBy(p => p.ProductModel);
                    break;
            }

            var model = await PagingList<ComponentListModel>.CreateAsync(qry, 5, page);
            model.Action = "BottomBracketsFR";
            return View(model);
        }

        [Obsolete]
        public async Task<IActionResult> BrakesFR(string sortOrder, int page = 1)
        {
            ViewBag.NameSortDParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.NameSortAParm = String.IsNullOrEmpty(sortOrder) ? "name_asc" : "";
            ViewBag.PriceSortDParm = String.IsNullOrEmpty(sortOrder) ? "price_desc" : "";
            ViewBag.PriceSortAParm = String.IsNullOrEmpty(sortOrder) ? "price_asc" : "";
            ViewBag.SellDateSortDParm = String.IsNullOrEmpty(sortOrder) ? "sellDate_desc" : "";
            ViewBag.SellDateSortAParm = String.IsNullOrEmpty(sortOrder) ? "sellDate_asc" : "";

            /*var validBrakes = (from brakes in db.VProductAndDescription
                                   where brakes.Culture == "en" &&
                                   brakes.ProductCategoryId == 10 &&
                                   brakes.SellEndDate == null
                                   select new ComponentListModel
                                   {
                                       ProductModel = brakes.ProductModel,
                                       Description = brakes.Description,
                                       ProductModelID = brakes.ProductModelId
                                   }).Distinct();*/

            var validBrakes = (from pd in db.VProductAndDescription
                               join p in db.Product
                               on pd.ProductModelId equals p.ProductModelId
                               where pd.Culture == "fr" &&
                               pd.SellEndDate == null &&
                               pd.ProductCategoryId == 10
                               select new ComponentListModel
                               {
                                   ProductModel = pd.ProductModel,
                                   Description = pd.Description,
                                   ProductModelID = pd.ProductModelId,
                                   Price = p.ListPrice,
                                   Photo = p.ThumbNailPhoto,
                                   StartSellDate = p.SellStartDate
                               }).Distinct();

            var qry = validBrakes.AsNoTracking().OrderBy(p => p.ProductModel);

            switch (sortOrder)
            {
                case "name_desc":
                    qry = validBrakes.AsNoTracking().OrderByDescending(p => p.ProductModel);
                    break;
                case "price_asc":
                    qry = validBrakes.AsNoTracking().OrderBy(p => p.Price);
                    break;
                case "price_desc":
                    qry = validBrakes.AsNoTracking().OrderByDescending(p => p.Price);
                    break;
                case "sellDate_asc":
                    qry = validBrakes.AsNoTracking().OrderBy(p => p.StartSellDate);
                    break;
                case "sellDate_desc":
                    qry = validBrakes.AsNoTracking().OrderByDescending(p => p.StartSellDate);
                    break;
                default:
                    qry = validBrakes.AsNoTracking().OrderBy(p => p.ProductModel);
                    break;
            }

            var model = await PagingList<ComponentListModel>.CreateAsync(qry, 5, page);
            model.Action = "BrakesFR";
            return View(model);
        }

        [Obsolete]
        public async Task<IActionResult> ChainsFR(string sortOrder, int page = 1)
        {
            ViewBag.NameSortDParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.NameSortAParm = String.IsNullOrEmpty(sortOrder) ? "name_asc" : "";
            ViewBag.PriceSortDParm = String.IsNullOrEmpty(sortOrder) ? "price_desc" : "";
            ViewBag.PriceSortAParm = String.IsNullOrEmpty(sortOrder) ? "price_asc" : "";
            ViewBag.SellDateSortDParm = String.IsNullOrEmpty(sortOrder) ? "sellDate_desc" : "";
            ViewBag.SellDateSortAParm = String.IsNullOrEmpty(sortOrder) ? "sellDate_asc" : "";

            /*var validChains = (from chains in db.VProductAndDescription
                                   where chains.Culture == "en" &&
                                   chains.ProductCategoryId == 11 &&
                                   chains.SellEndDate == null
                                   select new ComponentListModel
                                   {
                                       ProductModel = chains.ProductModel,
                                       Description = chains.Description,
                                       ProductModelID = chains.ProductModelId
                                   }).Distinct();*/

            var validChains = (from pd in db.VProductAndDescription
                               join p in db.Product
                               on pd.ProductModelId equals p.ProductModelId
                               where pd.Culture == "fr" &&
                               pd.SellEndDate == null &&
                               pd.ProductCategoryId == 11
                               select new ComponentListModel
                               {
                                   ProductModel = pd.ProductModel,
                                   Description = pd.Description,
                                   ProductModelID = pd.ProductModelId,
                                   Price = p.ListPrice,
                                   Photo = p.ThumbNailPhoto,
                                   StartSellDate = p.SellStartDate
                               }).Distinct();

            var qry = validChains.AsNoTracking().OrderBy(p => p.ProductModel);

            switch (sortOrder)
            {
                case "name_desc":
                    qry = validChains.AsNoTracking().OrderByDescending(p => p.ProductModel);
                    break;
                case "price_asc":
                    qry = validChains.AsNoTracking().OrderBy(p => p.Price);
                    break;
                case "price_desc":
                    qry = validChains.AsNoTracking().OrderByDescending(p => p.Price);
                    break;
                case "sellDate_asc":
                    qry = validChains.AsNoTracking().OrderBy(p => p.StartSellDate);
                    break;
                case "sellDate_desc":
                    qry = validChains.AsNoTracking().OrderByDescending(p => p.StartSellDate);
                    break;
                default:
                    qry = validChains.AsNoTracking().OrderBy(p => p.ProductModel);
                    break;
            }

            var model = await PagingList<ComponentListModel>.CreateAsync(qry, 5, page);
            model.Action = "ChainsFR";
            return View(model);
        }

        [Obsolete]
        public async Task<IActionResult> CranksetsFR(string sortOrder, int page = 1)
        {
            ViewBag.NameSortDParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.NameSortAParm = String.IsNullOrEmpty(sortOrder) ? "name_asc" : "";
            ViewBag.PriceSortDParm = String.IsNullOrEmpty(sortOrder) ? "price_desc" : "";
            ViewBag.PriceSortAParm = String.IsNullOrEmpty(sortOrder) ? "price_asc" : "";
            ViewBag.SellDateSortDParm = String.IsNullOrEmpty(sortOrder) ? "sellDate_desc" : "";
            ViewBag.SellDateSortAParm = String.IsNullOrEmpty(sortOrder) ? "sellDate_asc" : "";

            /*var validCranksets = (from cranksets in db.VProductAndDescription
                                   where cranksets.Culture == "en" &&
                                   cranksets.ProductCategoryId == 12 &&
                                   cranksets.SellEndDate == null
                                   select new ComponentListModel
                                   {
                                       ProductModel = cranksets.ProductModel,
                                       Description = cranksets.Description,
                                       ProductModelID = cranksets.ProductModelId
                                   }).Distinct();*/

            var validCranksets = (from pd in db.VProductAndDescription
                                  join p in db.Product
                                  on pd.ProductModelId equals p.ProductModelId
                                  where pd.Culture == "fr" &&
                                  pd.SellEndDate == null &&
                                  pd.ProductCategoryId == 12
                                  select new ComponentListModel
                                  {
                                      ProductModel = pd.ProductModel,
                                      Description = pd.Description,
                                      ProductModelID = pd.ProductModelId,
                                      Price = p.ListPrice,
                                      Photo = p.ThumbNailPhoto,
                                      StartSellDate = p.SellStartDate
                                  }).Distinct();

            var qry = validCranksets.AsNoTracking().OrderBy(p => p.ProductModel);

            switch (sortOrder)
            {
                case "name_desc":
                    qry = validCranksets.AsNoTracking().OrderByDescending(p => p.ProductModel);
                    break;
                case "price_asc":
                    qry = validCranksets.AsNoTracking().OrderBy(p => p.Price);
                    break;
                case "price_desc":
                    qry = validCranksets.AsNoTracking().OrderByDescending(p => p.Price);
                    break;
                case "sellDate_asc":
                    qry = validCranksets.AsNoTracking().OrderBy(p => p.StartSellDate);
                    break;
                case "sellDate_desc":
                    qry = validCranksets.AsNoTracking().OrderByDescending(p => p.StartSellDate);
                    break;
                default:
                    qry = validCranksets.AsNoTracking().OrderBy(p => p.ProductModel);
                    break;
            }

            var model = await PagingList<ComponentListModel>.CreateAsync(qry, 5, page);
            model.Action = "CranksetsFR";
            return View(model);
        }

        [Obsolete]
        public async Task<IActionResult> DerailleursFR(string sortOrder, int page = 1)
        {
            ViewBag.NameSortDParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.NameSortAParm = String.IsNullOrEmpty(sortOrder) ? "name_asc" : "";
            ViewBag.PriceSortDParm = String.IsNullOrEmpty(sortOrder) ? "price_desc" : "";
            ViewBag.PriceSortAParm = String.IsNullOrEmpty(sortOrder) ? "price_asc" : "";
            ViewBag.SellDateSortDParm = String.IsNullOrEmpty(sortOrder) ? "sellDate_desc" : "";
            ViewBag.SellDateSortAParm = String.IsNullOrEmpty(sortOrder) ? "sellDate_asc" : "";

            /*var validDerailleurs = (from derailleurs in db.VProductAndDescription
                                   where derailleurs.Culture == "en" &&
                                   derailleurs.ProductCategoryId == 13 &&
                                   derailleurs.SellEndDate == null
                                   select new ComponentListModel
                                   {
                                       ProductModel = derailleurs.ProductModel,
                                       Description = derailleurs.Description,
                                       ProductModelID = derailleurs.ProductModelId
                                   }).Distinct().ToList();*/

            var validDerailleurs = (from pd in db.VProductAndDescription
                                    join p in db.Product
                                    on pd.ProductModelId equals p.ProductModelId
                                    where pd.Culture == "fr" &&
                                    pd.SellEndDate == null &&
                                    pd.ProductCategoryId == 13
                                    select new ComponentListModel
                                    {
                                        ProductModel = pd.ProductModel,
                                        Description = pd.Description,
                                        ProductModelID = pd.ProductModelId,
                                        Price = p.ListPrice,
                                        Photo = p.ThumbNailPhoto,
                                        StartSellDate = p.SellStartDate
                                    }).Distinct();

            var qry = validDerailleurs.AsNoTracking().OrderBy(p => p.ProductModel);

            switch (sortOrder)
            {
                case "name_desc":
                    qry = validDerailleurs.AsNoTracking().OrderByDescending(p => p.ProductModel);
                    break;
                case "price_asc":
                    qry = validDerailleurs.AsNoTracking().OrderBy(p => p.Price);
                    break;
                case "price_desc":
                    qry = validDerailleurs.AsNoTracking().OrderByDescending(p => p.Price);
                    break;
                case "sellDate_asc":
                    qry = validDerailleurs.AsNoTracking().OrderBy(p => p.StartSellDate);
                    break;
                case "sellDate_desc":
                    qry = validDerailleurs.AsNoTracking().OrderByDescending(p => p.StartSellDate);
                    break;
                default:
                    qry = validDerailleurs.AsNoTracking().OrderBy(p => p.ProductModel);
                    break;
            }

            var model = await PagingList<ComponentListModel>.CreateAsync(qry, 5, page);
            model.Action = "DerailleursFR";
            return View(model);
        }

        public IActionResult ForksFR(string sortOrder)
        {
            ViewBag.NameSortDParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.NameSortAParm = String.IsNullOrEmpty(sortOrder) ? "name_asc" : "";
            ViewBag.PriceSortDParm = String.IsNullOrEmpty(sortOrder) ? "price_desc" : "";
            ViewBag.PriceSortAParm = String.IsNullOrEmpty(sortOrder) ? "price_asc" : "";
            ViewBag.SellDateSortDParm = String.IsNullOrEmpty(sortOrder) ? "sellDate_desc" : "";
            ViewBag.SellDateSortAParm = String.IsNullOrEmpty(sortOrder) ? "sellDate_asc" : "";

            /*var validForks = (from forks in db.VProductAndDescription
                                   where forks.Culture == "en" &&
                                   forks.ProductCategoryId == 14 &&
                                   forks.SellEndDate == null
                                   select new ComponentListModel
                                   {
                                       ProductModel = forks.ProductModel,
                                       Description = forks.Description,
                                       ProductModelID = forks.ProductModelId
                                   }).Distinct().ToList();*/

            var validForks = (from pd in db.VProductAndDescription
                              join p in db.Product
                              on pd.ProductModelId equals p.ProductModelId
                              where pd.Culture == "fr" &&
                              pd.SellEndDate == null &&
                              pd.ProductCategoryId == 14
                              select new ComponentListModel
                              {
                                  ProductModel = pd.ProductModel,
                                  Description = pd.Description,
                                  ProductModelID = pd.ProductModelId,
                                  Price = p.ListPrice,
                                  Photo = p.ThumbNailPhoto,
                                  StartSellDate = p.SellStartDate
                              }).Distinct();

            switch (sortOrder)
            {
                case "name_desc":
                    validForks = validForks.OrderByDescending(item => item.ProductModel);
                    break;
                case "price_asc":
                    validForks = validForks.OrderBy(item => item.Price);
                    break;
                case "price_desc":
                    validForks = validForks.OrderByDescending(item => item.Price);
                    break;
                case "sellDate_asc":
                    validForks = validForks.OrderBy(item => item.StartSellDate);
                    break;
                case "sellDate_desc":
                    validForks = validForks.OrderByDescending(item => item.StartSellDate);
                    break;
                default:
                    validForks = validForks.OrderBy(item => item.ProductModel);
                    break;
            }

            return View(validForks);
        }

        public IActionResult HeadsetsFR()
        {
            var validHeadsets = (from headsets in db.VProductAndDescription
                                 where headsets.Culture == "fr" &&
                                 headsets.ProductCategoryId == 15 &&
                                 headsets.SellEndDate == null
                                 select new ComponentListModel
                                 {
                                     ProductModel = headsets.ProductModel,
                                     Description = headsets.Description,
                                     ProductModelID = headsets.ProductModelId
                                 }).Distinct().ToList();
            return View(validHeadsets);
        }

        [Obsolete]
        public async Task<IActionResult> MountainFramesFR(string sortOrder, int page = 1)
        {
            ViewBag.NameSortDParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.NameSortAParm = String.IsNullOrEmpty(sortOrder) ? "name_asc" : "";
            ViewBag.PriceSortDParm = String.IsNullOrEmpty(sortOrder) ? "price_desc" : "";
            ViewBag.PriceSortAParm = String.IsNullOrEmpty(sortOrder) ? "price_asc" : "";
            ViewBag.SellDateSortDParm = String.IsNullOrEmpty(sortOrder) ? "sellDate_desc" : "";
            ViewBag.SellDateSortAParm = String.IsNullOrEmpty(sortOrder) ? "sellDate_asc" : "";

            /*var validMountainFrames = (from frames in db.VProductAndDescription
                                   where frames.Culture == "en" &&
                                   frames.ProductCategoryId == 16 &&
                                   frames.SellEndDate == null
                                   select new ComponentListModel
                                   {
                                       ProductModel = frames.ProductModel,
                                       Description = frames.Description,
                                       ProductModelID = frames.ProductModelId
                                   }).Distinct().ToList();*/

            var validMountainFrames = (from pd in db.VProductAndDescription
                                       join p in db.Product
                                       on pd.ProductModelId equals p.ProductModelId
                                       where pd.Culture == "fr" &&
                                       pd.SellEndDate == null &&
                                       pd.ProductCategoryId == 16
                                       select new ComponentListModel
                                       {
                                           ProductModel = pd.ProductModel,
                                           Description = pd.Description,
                                           ProductModelID = pd.ProductModelId,
                                           Price = p.ListPrice,
                                           Photo = p.ThumbNailPhoto,
                                           StartSellDate = p.SellStartDate
                                       }).Distinct();

            var qry = validMountainFrames.AsNoTracking().OrderBy(p => p.ProductModel);

            switch (sortOrder)
            {
                case "name_desc":
                    qry = validMountainFrames.AsNoTracking().OrderByDescending(p => p.ProductModel);
                    break;
                case "price_asc":
                    qry = validMountainFrames.AsNoTracking().OrderBy(p => p.Price);
                    break;
                case "price_desc":
                    qry = validMountainFrames.AsNoTracking().OrderByDescending(p => p.Price);
                    break;
                case "sellDate_asc":
                    qry = validMountainFrames.AsNoTracking().OrderBy(p => p.StartSellDate);
                    break;
                case "sellDate_desc":
                    qry = validMountainFrames.AsNoTracking().OrderByDescending(p => p.StartSellDate);
                    break;
                default:
                    qry = validMountainFrames.AsNoTracking().OrderBy(p => p.ProductModel);
                    break;
            }

            var model = await PagingList<ComponentListModel>.CreateAsync(qry, 5, page);
            model.Action = "MountainFramesFR";
            return View(model);
        }

        [Obsolete]
        public async Task<IActionResult> PedalsFR(string sortOrder, int page = 1)
        {
            ViewBag.NameSortDParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.NameSortAParm = String.IsNullOrEmpty(sortOrder) ? "name_asc" : "";
            ViewBag.PriceSortDParm = String.IsNullOrEmpty(sortOrder) ? "price_desc" : "";
            ViewBag.PriceSortAParm = String.IsNullOrEmpty(sortOrder) ? "price_asc" : "";
            ViewBag.SellDateSortDParm = String.IsNullOrEmpty(sortOrder) ? "sellDate_desc" : "";
            ViewBag.SellDateSortAParm = String.IsNullOrEmpty(sortOrder) ? "sellDate_asc" : "";

            /*var validPedals = (from pedals in db.VProductAndDescription
                                   where pedals.Culture == "en" &&
                                   pedals.ProductCategoryId == 17 &&
                                   pedals.SellEndDate == null
                                   select new ComponentListModel
                                   {
                                       ProductModel = pedals.ProductModel,
                                       Description = pedals.Description,
                                       ProductModelID = pedals.ProductModelId
                                   }).Distinct().ToList();*/

            var validPedals = (from pd in db.VProductAndDescription
                               join p in db.Product
                               on pd.ProductModelId equals p.ProductModelId
                               where pd.Culture == "fr" &&
                               pd.SellEndDate == null &&
                               pd.ProductCategoryId == 17
                               select new ComponentListModel
                               {
                                   ProductModel = pd.ProductModel,
                                   Description = pd.Description,
                                   ProductModelID = pd.ProductModelId,
                                   Price = p.ListPrice,
                                   Photo = p.ThumbNailPhoto,
                                   StartSellDate = p.SellStartDate
                               }).Distinct();

            var qry = validPedals.AsNoTracking().OrderBy(p => p.ProductModel);

            switch (sortOrder)
            {
                case "name_desc":
                    qry = validPedals.AsNoTracking().OrderByDescending(p => p.ProductModel);
                    break;
                case "price_asc":
                    qry = validPedals.AsNoTracking().OrderBy(p => p.Price);
                    break;
                case "price_desc":
                    qry = validPedals.AsNoTracking().OrderByDescending(p => p.Price);
                    break;
                case "sellDate_asc":
                    qry = validPedals.AsNoTracking().OrderBy(p => p.StartSellDate);
                    break;
                case "sellDate_desc":
                    qry = validPedals.AsNoTracking().OrderByDescending(p => p.StartSellDate);
                    break;
                default:
                    qry = validPedals.AsNoTracking().OrderBy(p => p.ProductModel);
                    break;
            }

            var model = await PagingList<ComponentListModel>.CreateAsync(qry, 5, page);
            model.Action = "PedalsFR";
            return View(model);
        }

        [Obsolete]
        public async Task<IActionResult> RoadFramesFR(string sortOrder, int page = 1)
        {
            ViewBag.NameSortDParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.NameSortAParm = String.IsNullOrEmpty(sortOrder) ? "name_asc" : "";
            ViewBag.PriceSortDParm = String.IsNullOrEmpty(sortOrder) ? "price_desc" : "";
            ViewBag.PriceSortAParm = String.IsNullOrEmpty(sortOrder) ? "price_asc" : "";
            ViewBag.SellDateSortDParm = String.IsNullOrEmpty(sortOrder) ? "sellDate_desc" : "";
            ViewBag.SellDateSortAParm = String.IsNullOrEmpty(sortOrder) ? "sellDate_asc" : "";

            /*var validRoadFrames = (from frames in db.VProductAndDescription
                                   where frames.Culture == "en" &&
                                   frames.ProductCategoryId == 18 &&
                                   frames.SellEndDate == null
                                   select new ComponentListModel
                                   {
                                       ProductModel = frames.ProductModel,
                                       Description = frames.Description,
                                       ProductModelID = frames.ProductModelId
                                   }).Distinct().ToList();*/

            var validRoadFrames = (from pd in db.VProductAndDescription
                                   join p in db.Product
                                   on pd.ProductModelId equals p.ProductModelId
                                   where pd.Culture == "fr" &&
                                   pd.SellEndDate == null &&
                                   pd.ProductCategoryId == 18
                                   select new ComponentListModel
                                   {
                                       ProductModel = pd.ProductModel,
                                       Description = pd.Description,
                                       ProductModelID = pd.ProductModelId,
                                       Price = p.ListPrice,
                                       Photo = p.ThumbNailPhoto,
                                       StartSellDate = p.SellStartDate
                                   }).Distinct();

            var qry = validRoadFrames.AsNoTracking().OrderBy(p => p.ProductModel);

            switch (sortOrder)
            {
                case "name_desc":
                    qry = validRoadFrames.AsNoTracking().OrderByDescending(p => p.ProductModel);
                    break;
                case "price_asc":
                    qry = validRoadFrames.AsNoTracking().OrderBy(p => p.Price);
                    break;
                case "price_desc":
                    qry = validRoadFrames.AsNoTracking().OrderByDescending(p => p.Price);
                    break;
                case "sellDate_asc":
                    qry = validRoadFrames.AsNoTracking().OrderBy(p => p.StartSellDate);
                    break;
                case "sellDate_desc":
                    qry = validRoadFrames.AsNoTracking().OrderByDescending(p => p.StartSellDate);
                    break;
                default:
                    qry = validRoadFrames.AsNoTracking().OrderBy(p => p.ProductModel);
                    break;
            }

            var model = await PagingList<ComponentListModel>.CreateAsync(qry, 5, page);
            model.Action = "RoadFramesFR";
            return View(model);
        }

        [Obsolete]
        public async Task<IActionResult> SaddlesFR(string sortOrder, int page = 1)
        {
            ViewBag.NameSortDParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.NameSortAParm = String.IsNullOrEmpty(sortOrder) ? "name_asc" : "";
            ViewBag.PriceSortDParm = String.IsNullOrEmpty(sortOrder) ? "price_desc" : "";
            ViewBag.PriceSortAParm = String.IsNullOrEmpty(sortOrder) ? "price_asc" : "";
            ViewBag.SellDateSortDParm = String.IsNullOrEmpty(sortOrder) ? "sellDate_desc" : "";
            ViewBag.SellDateSortAParm = String.IsNullOrEmpty(sortOrder) ? "sellDate_asc" : "";

            /*var validSaddles = (from saddles in db.VProductAndDescription
                                   where saddles.Culture == "en" &&
                                   saddles.ProductCategoryId == 19 &&
                                   saddles.SellEndDate == null
                                   select new ComponentListModel
                                   {
                                       ProductModel = saddles.ProductModel,
                                       Description = saddles.Description,
                                       ProductModelID = saddles.ProductModelId
                                   }).Distinct().ToList();*/

            var validSaddles = (from pd in db.VProductAndDescription
                                join p in db.Product
                                on pd.ProductModelId equals p.ProductModelId
                                where pd.Culture == "fr" &&
                                pd.SellEndDate == null &&
                                pd.ProductCategoryId == 19
                                select new ComponentListModel
                                {
                                    ProductModel = pd.ProductModel,
                                    Description = pd.Description,
                                    ProductModelID = pd.ProductModelId,
                                    Price = p.ListPrice,
                                    Photo = p.ThumbNailPhoto,
                                    StartSellDate = p.SellStartDate
                                }).Distinct();

            var qry = validSaddles.AsNoTracking().OrderBy(p => p.ProductModel);

            switch (sortOrder)
            {
                case "name_desc":
                    qry = validSaddles.AsNoTracking().OrderByDescending(p => p.ProductModel);
                    break;
                case "price_asc":
                    qry = validSaddles.AsNoTracking().OrderBy(p => p.Price);
                    break;
                case "price_desc":
                    qry = validSaddles.AsNoTracking().OrderByDescending(p => p.Price);
                    break;
                case "sellDate_asc":
                    qry = validSaddles.AsNoTracking().OrderBy(p => p.StartSellDate);
                    break;
                case "sellDate_desc":
                    qry = validSaddles.AsNoTracking().OrderByDescending(p => p.StartSellDate);
                    break;
                default:
                    qry = validSaddles.AsNoTracking().OrderBy(p => p.ProductModel);
                    break;
            }

            var model = await PagingList<ComponentListModel>.CreateAsync(qry, 5, page);
            model.Action = "SaddlesFR";

            ViewData["page"] = page;

            return View(model);
        }

        [Obsolete]
        public async Task<IActionResult> TouringFramesFR(string sortOrder, int page = 1)
        {
            ViewBag.NameSortDParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.NameSortAParm = String.IsNullOrEmpty(sortOrder) ? "name_asc" : "";
            ViewBag.PriceSortDParm = String.IsNullOrEmpty(sortOrder) ? "price_desc" : "";
            ViewBag.PriceSortAParm = String.IsNullOrEmpty(sortOrder) ? "price_asc" : "";
            ViewBag.SellDateSortDParm = String.IsNullOrEmpty(sortOrder) ? "sellDate_desc" : "";
            ViewBag.SellDateSortAParm = String.IsNullOrEmpty(sortOrder) ? "sellDate_asc" : "";

            /*var validTouringFrames = (from frames in db.VProductAndDescription
                                   where frames.Culture == "en" &&
                                   frames.ProductCategoryId == 20 &&
                                   frames.SellEndDate == null
                                   select new ComponentListModel
                                   {
                                       ProductModel = frames.ProductModel,
                                       Description = frames.Description,
                                       ProductModelID = frames.ProductModelId
                                   }).Distinct().ToList();*/

            var validTouringFrames = (from pd in db.VProductAndDescription
                                      join p in db.Product
                                      on pd.ProductModelId equals p.ProductModelId
                                      where pd.Culture == "fr" &&
                                      pd.SellEndDate == null &&
                                      pd.ProductCategoryId == 20
                                      select new ComponentListModel
                                      {
                                          ProductModel = pd.ProductModel,
                                          Description = pd.Description,
                                          ProductModelID = pd.ProductModelId,
                                          Price = p.ListPrice,
                                          Photo = p.ThumbNailPhoto,
                                          StartSellDate = p.SellStartDate
                                      }).Distinct();

            var qry = validTouringFrames.AsNoTracking().OrderBy(p => p.ProductModel);

            switch (sortOrder)
            {
                case "name_desc":
                    qry = validTouringFrames.AsNoTracking().OrderByDescending(p => p.ProductModel);
                    break;
                case "price_asc":
                    qry = validTouringFrames.AsNoTracking().OrderBy(p => p.Price);
                    break;
                case "price_desc":
                    qry = validTouringFrames.AsNoTracking().OrderByDescending(p => p.Price);
                    break;
                case "sellDate_asc":
                    qry = validTouringFrames.AsNoTracking().OrderBy(p => p.StartSellDate);
                    break;
                case "sellDate_desc":
                    qry = validTouringFrames.AsNoTracking().OrderByDescending(p => p.StartSellDate);
                    break;
                default:
                    qry = validTouringFrames.AsNoTracking().OrderBy(p => p.ProductModel);
                    break;
            }

            var model = await PagingList<ComponentListModel>.CreateAsync(qry, 5, page);
            model.Action = "TouringFramesFR";
            return View(model);
        }

        public IActionResult WheelsFR()
        {
            var validWheels = (from wheels in db.VProductAndDescription
                               where wheels.Culture == "fr" &&
                               wheels.ProductCategoryId == 21 &&
                               wheels.SellEndDate == null
                               select new ComponentListModel
                               {
                                   ProductModel = wheels.ProductModel,
                                   Description = wheels.Description,
                                   ProductModelID = wheels.ProductModelId
                               }).Distinct().ToList();
            return View(validWheels);
        }

        // GET: Components/Details/5
        public IActionResult DetailsFR(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var products = from components in db.Product
                           where components.ProductModelId == id &&
                           components.SellEndDate == null
                           select components;

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

        // GET: Components/Create
        public IActionResult Create()
        {
            ViewData["ParentProductCategoryId"] = new SelectList(db.ProductCategory, "ProductCategoryId", "Name");
            return View();
        }

        // POST: Components/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductCategoryId,ParentProductCategoryId,Name,Rowguid,ModifiedDate")] ProductCategory productCategory)
        {
            if (ModelState.IsValid)
            {
                db.Add(productCategory);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ParentProductCategoryId"] = new SelectList(db.ProductCategory, "ProductCategoryId", "Name", productCategory.ParentProductCategoryId);
            return View(productCategory);
        }

        // GET: Components/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productCategory = await db.ProductCategory.FindAsync(id);
            if (productCategory == null)
            {
                return NotFound();
            }
            ViewData["ParentProductCategoryId"] = new SelectList(db.ProductCategory, "ProductCategoryId", "Name", productCategory.ParentProductCategoryId);
            return View(productCategory);
        }

        // POST: Components/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductCategoryId,ParentProductCategoryId,Name,Rowguid,ModifiedDate")] ProductCategory productCategory)
        {
            if (id != productCategory.ProductCategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(productCategory);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductCategoryExists(productCategory.ProductCategoryId))
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
            ViewData["ParentProductCategoryId"] = new SelectList(db.ProductCategory, "ProductCategoryId", "Name", productCategory.ParentProductCategoryId);
            return View(productCategory);
        }

        // GET: Components/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productCategory = await db.ProductCategory
                .Include(p => p.ParentProductCategory)
                .FirstOrDefaultAsync(m => m.ProductCategoryId == id);
            if (productCategory == null)
            {
                return NotFound();
            }

            return View(productCategory);
        }

        // POST: Components/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productCategory = await db.ProductCategory.FindAsync(id);
            db.ProductCategory.Remove(productCategory);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductCategoryExists(int id)
        {
            return db.ProductCategory.Any(e => e.ProductCategoryId == id);
        }
    }
}
