using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using NoNameBikes.Models;
using PagedList;
using ReflectionIT.Mvc.Paging;

namespace NoNameBikes.Controllers
{
    public class BikesController : Controller
    {
        private readonly AdventureWorksLT2017Context db;

        public BikesController(AdventureWorksLT2017Context context)
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
                                        where pd.Culture == "en" &&
                                        pd.SellEndDate == null &&
                                        pd.ProductCategoryId == 6
                                        select new BikeListModel
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
                                         on  pd.ProductModelId equals p.ProductModelId
                                         where pd.Culture == "en" &&
                                         pd.SellEndDate == null &&
                                         pd.ProductCategoryId == 5
                                         select new BikeListModel
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
                                         where pd.Culture == "en" &&
                                         pd.SellEndDate == null &&
                                         pd.ProductCategoryId == 7
                                         select new BikeListModel
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
                                        where details.ProductModelId == id 
                                        select details.Description);

            ViewData["description"] = desc.First();

            if (products == null)
                return NotFound();

            return View(products);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// FRENCH VERSION
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [Obsolete]
        public async Task<IActionResult> IndexFR(string sortOrder, int page = 1)
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
        public async Task<IActionResult> RoadFR(string sortOrder, int page = 1)
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
            model.Action = "RoadFR";
            return View(model);
        }

        [Obsolete]
        public async Task<IActionResult> MountainFR(string sortOrder, int page = 1)
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
            model.Action = "MountainFR";
            return View(model);
        }

        [Obsolete]
        public async Task<IActionResult> TouringFR(string sortOrder, int page = 1)
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
            model.Action = "TouringFR";
            return View(model);
        }

        // GET: Bikes/Details/5
        public IActionResult DetailsFR(int? id)
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

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        [HttpPost]
        [ValidateAntiForgeryToken]
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

        // GET: Bikes/Create
        public IActionResult Create()
        {
            ViewData["ParentProductCategoryId"] = new SelectList(db.ProductCategory, "ProductCategoryId", "Name");
            return View();
        }

        // POST: Bikes/Create
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

        // GET: Bikes/Edit/5
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

        // POST: Bikes/Edit/5
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

        // GET: Bikes/Delete/5
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

        // POST: Bikes/Delete/5
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
