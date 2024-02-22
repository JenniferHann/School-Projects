using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NoNameBikes.Models;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace NoNameBikes.Controllers
{
    public class HomeController : Controller
    {
        public const String CART_COOKIE = "cart"; //the string name of cookie that stores the cart

        private readonly ILogger<HomeController> _logger;
        private readonly AdventureWorksLT2017Context db;
        
        public HomeController(AdventureWorksLT2017Context context, ILogger<HomeController> logger)
        {
            db = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var bikeList = from bikes in db.ProductCategory
                           where bikes.ParentProductCategoryId == 1
                           select bikes;

            var salesList = (from pd in db.VProductAndDescription
                                     join p in db.Product
                                     on pd.ProductModelId equals p.ProductModelId
                                     where pd.Culture == "en" &&
                                     pd.SellEndDate == null && 
                                     p.SalePrice != null &&
                                     DateTime.Compare((DateTime)p.SaleEndDate, DateTime.Now) > 0
                                     select new SaleProduct
                                     {
                                         ProductModel = pd.ProductModel,
                                         ProductCategory = p.ProductCategory,
                                         Description = pd.Description,
                                         ProductModelID = pd.ProductModelId,
                                         Price = p.ListPrice,
                                         Photo = p.ThumbNailPhoto,
                                         StartSellDate = p.SellStartDate,
                                         SalePrice = p.SalePrice,
                                         SaleStartDate = p.SaleStartDate,
                                         SaleEndDate = p.SaleEndDate
                                     }).Distinct();

            /*salesList.GroupBy(i => i.ProductModelID);*/


            ViewData["SalesList"] = salesList.ToList();


            return View(bikeList.ToList());
        }

        [Obsolete]
        public async Task<IActionResult> Sales(string sortOrder, int page = 1)
        {
            ViewBag.NameSortDParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.NameSortAParm = String.IsNullOrEmpty(sortOrder) ? "name_asc" : "";
            ViewBag.PriceSortDParm = String.IsNullOrEmpty(sortOrder) ? "price_desc" : "";
            ViewBag.PriceSortAParm = String.IsNullOrEmpty(sortOrder) ? "price_asc" : "";
            ViewBag.SellDateSortDParm = String.IsNullOrEmpty(sortOrder) ? "sellDate_desc" : "";
            ViewBag.SellDateSortAParm = String.IsNullOrEmpty(sortOrder) ? "sellDate_asc" : "";

            var salesList = (from pd in db.VProductAndDescription
                             join p in db.Product
                             on pd.ProductModelId equals p.ProductModelId
                             where pd.Culture == "en" &&
                             pd.SellEndDate == null &&
                             p.SalePrice != null &&
                             DateTime.Compare((DateTime)p.SaleEndDate, DateTime.Now) > 0
                             select new SaleProduct
                             {
                                 ProductModel = pd.ProductModel,
                                 ProductCategory = p.ProductCategory,
                                 Description = pd.Description,
                                 ProductModelID = pd.ProductModelId,
                                 Price = p.ListPrice,
                                 Photo = p.ThumbNailPhoto,
                                 StartSellDate = p.SellStartDate,
                                 SalePrice = p.SalePrice,
                                 SaleStartDate = p.SaleStartDate,
                                 SaleEndDate = p.SaleEndDate
                             }).Distinct();

            var qry = salesList.AsNoTracking().OrderBy(p => p.ProductModel);

            switch (sortOrder)
            {
                case "name_desc":
                    qry = salesList.AsNoTracking().OrderByDescending(p => p.ProductModel);
                    break;
                case "price_asc":
                    qry = salesList.AsNoTracking().OrderBy(p => p.SalePrice);
                    break;
                case "price_desc":
                    qry = salesList.AsNoTracking().OrderByDescending(p => p.SalePrice);
                    break;
                case "sellDate_asc":
                    qry = salesList.AsNoTracking().OrderBy(p => p.StartSellDate);
                    break;
                case "sellDate_desc":
                    qry = salesList.AsNoTracking().OrderByDescending(p => p.StartSellDate);
                    break;
                default:
                    qry = salesList.AsNoTracking().OrderBy(p => p.ProductModel);
                    break;
            }

            var model = await PagingList<SaleProduct>.CreateAsync(qry, 5, page);
            model.Action = "Sales";

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult AboutUs()
        {
            return View();
        }

        public IActionResult Careers()
        {
            return View();
        }

        public IActionResult CyclingSafety()
        {
            return View();
        }

        public IActionResult Services()
        {
            return View();
        }

        public IActionResult Returns()
        {
            return View();
        }

        public IActionResult ContactUs()
        {
            return View();
        }

        public IActionResult CovidRules()
        {
            return View();
        }

        public IActionResult LogIn()
        {
            return View();
        }

        public IActionResult Cart()
        {
            //Check if "cart" cookie exists
            if (HttpContext.Request.Cookies.ContainsKey(CART_COOKIE))
            {
                //get cookie
                String cartJson = HttpContext.Request.Cookies[CART_COOKIE]; //get cart as json-formatted string

                //read json into the dictionary of products in the cart that links product id to quantities
                Dictionary<int, int> cartProductId = JsonConvert.DeserializeObject<Dictionary<int, int>>(cartJson);

                //change this into a dictionary that stores the entire product model and quantity
                Dictionary<Product, int> cartProduct = new Dictionary<Product, int>();
                foreach (KeyValuePair<int, int> item in cartProductId)
                {
                    //get product from product id (key)
                    Product product = db.Product.First(products => products.ProductId == item.Key);

                    //add product to cartProduct
                    cartProduct.Add(product, item.Value);
                }

                return View(cartProduct);
            }

            //if no cart cookie is set, cart is empty
            return View();
        }

        // Add an Item To the cart in cookes
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddToCart([Bind("ProductId,Name,ProductNumber,Color,ListPrice,Size,Weight,ProductCategoryId,ProductModelId,SellStartDate,SellEndDate,DiscontinuedDate,ThumbNailPhoto,ThumbnailPhotoFileName,Rowguid,ModifiedDate")] Product product, int quantity)
        {
            //get product from db based on values supplied by form
            Product selectedProduct = db.Product.First(products => products.ProductModelId == product.ProductModelId //product Model is the same
                                                                                                && (product.Size == null || products.Size == product.Size) //product size is the same or null (size does not exist)
                                                                                                && (product.Color == null || products.Color == product.Color)); //product color is the same or null (size does not exist)

            //the dictionary of products in the cart links product id to quantities
            Dictionary<int, int> cartProductId = new Dictionary<int, int>();

            //Check if "cart" cookie already exists
            if (HttpContext.Request.Cookies.ContainsKey(CART_COOKIE))
            {
                //get cookie
                String cart = HttpContext.Request.Cookies[CART_COOKIE]; //get cart as json-formatted string
                //read json
                cartProductId = JsonConvert.DeserializeObject<Dictionary<int, int>>(cart);
                //check if this product already in cart
                if (cartProductId.ContainsKey(selectedProduct.ProductId))
                    //increment selectedProduct quantity by 1
                    cartProductId[selectedProduct.ProductId]++;
                else
                    //add new entry to cartProductId
                    cartProductId.Add(selectedProduct.ProductId, quantity);
            }
            else
                //add new entry to cartProductId
                cartProductId.Add(selectedProduct.ProductId, quantity);

            //serialize cartProducts to json
            String cartJson = JsonConvert.SerializeObject(cartProductId);
            //set cookie
            HttpContext.Response.Cookies.Append(CART_COOKIE, cartJson);

            //get parent product category name so we can go back to controller
            ProductCategory productCategory = db.ProductCategory.First(ProductCategory => ProductCategory.ProductCategoryId == selectedProduct.ProductCategoryId);
            string parentProductCategoryName = db.ProductCategory.First(ProductCategory => ProductCategory.ProductCategoryId == productCategory.ParentProductCategoryId).Name;

            //go back to product page
            return RedirectToAction("Details", parentProductCategoryName, new { id = selectedProduct.ProductModelId });
        }

        //Remove a single item from a cart
        public IActionResult RemoveItemFromCart ([Bind("ProductId,Name,ProductNumber,Color,ListPrice,Size,Weight,ProductCategoryId,ProductModelId,SellStartDate,SellEndDate,DiscontinuedDate,ThumbNailPhoto,ThumbnailPhotoFileName,Rowguid,ModifiedDate")] Product product)
        {
            //the dictionary of products in the cart links product id to quantities
            Dictionary<int, int> cartProductId = new Dictionary<int, int>();

            //get cookie
            String cart = HttpContext.Request.Cookies[CART_COOKIE]; //get cart as json-formatted string
            //read json
            cartProductId = JsonConvert.DeserializeObject<Dictionary<int, int>>(cart);
            //remove this from dictionary completly
            cartProductId.Remove(product.ProductId);

            //if cart is empty, remove cookie
            if (cartProductId.Count == 0)
            {
                HttpContext.Response.Cookies.Delete(CART_COOKIE);
                return RedirectToAction("Cart");
            }

            //serialize cartProducts to json
            String cartJson = JsonConvert.SerializeObject(cartProductId);
            //set cookie
            HttpContext.Response.Cookies.Append(CART_COOKIE, cartJson);

            //go back to cart
            return RedirectToAction("Cart");
        }

        //Update quantity of an item from a cart
        public IActionResult UpdateQuantityInCart([Bind("ProductId,Name,ProductNumber,Color,ListPrice,Size,Weight,ProductCategoryId,ProductModelId,SellStartDate,SellEndDate,DiscontinuedDate,ThumbNailPhoto,ThumbnailPhotoFileName,Rowguid,ModifiedDate")] Product product, int Quantity)
        {
            //the dictionary of products in the cart links product id to quantities
            Dictionary<int, int> cartProductId = new Dictionary<int, int>();

            //get cookie
            String cart = HttpContext.Request.Cookies[CART_COOKIE]; //get cart as json-formatted string
            //read json
            cartProductId = JsonConvert.DeserializeObject<Dictionary<int, int>>(cart);
            //remove one of product
            cartProductId[product.ProductId] = Quantity;

            //check if quanity of product is zero or fewer
            if (cartProductId[product.ProductId] <= 0)
            {
                //remove this from dictionary completly
                cartProductId.Remove(product.ProductId);

                //if cart is empty, remove cookie
                if (cartProductId.Count == 0)
                {
                    HttpContext.Response.Cookies.Delete(CART_COOKIE);
                    return RedirectToAction("Cart");
                }
            }

            //serialize cartProducts to json
            String cartJson = JsonConvert.SerializeObject(cartProductId);
            //set cookie
            HttpContext.Response.Cookies.Append(CART_COOKIE, cartJson);

            //go back to cart
            return RedirectToAction("Cart");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// FRENCH VERSION
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public IActionResult IndexFR()
        {
            var bikeList = from bikes in db.ProductCategory
                           where bikes.ParentProductCategoryId == 1
                           select bikes;

            var salesList = (from pd in db.VProductAndDescription
                             join p in db.Product
                             on pd.ProductModelId equals p.ProductModelId
                             where pd.Culture == "en" &&
                             pd.SellEndDate == null &&
                             p.SalePrice != null &&
                             DateTime.Compare((DateTime)p.SaleEndDate, DateTime.Now) > 0
                             select new SaleProduct
                             {
                                 ProductModel = pd.ProductModel,
                                 ProductCategory = p.ProductCategory,
                                 Description = pd.Description,
                                 ProductModelID = pd.ProductModelId,
                                 Price = p.ListPrice,
                                 Photo = p.ThumbNailPhoto,
                                 StartSellDate = p.SellStartDate,
                                 SalePrice = p.SalePrice,
                                 SaleStartDate = p.SaleStartDate,
                                 SaleEndDate = p.SaleEndDate
                             }).Distinct();

            /*salesList.GroupBy(i => i.ProductModelID);*/


            ViewData["SalesList"] = salesList.ToList();

            return View(bikeList.ToList());
        }

        [Obsolete]
        public async Task<IActionResult> SalesFR(string sortOrder, int page = 1)
        {
            ViewBag.NameSortDParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.NameSortAParm = String.IsNullOrEmpty(sortOrder) ? "name_asc" : "";
            ViewBag.PriceSortDParm = String.IsNullOrEmpty(sortOrder) ? "price_desc" : "";
            ViewBag.PriceSortAParm = String.IsNullOrEmpty(sortOrder) ? "price_asc" : "";
            ViewBag.SellDateSortDParm = String.IsNullOrEmpty(sortOrder) ? "sellDate_desc" : "";
            ViewBag.SellDateSortAParm = String.IsNullOrEmpty(sortOrder) ? "sellDate_asc" : "";

            var salesList = (from pd in db.VProductAndDescription
                             join p in db.Product
                             on pd.ProductModelId equals p.ProductModelId
                             where pd.Culture == "fr" &&
                             pd.SellEndDate == null &&
                             p.SalePrice != null &&
                             DateTime.Compare((DateTime)p.SaleEndDate, DateTime.Now) > 0
                             select new SaleProduct
                             {
                                 ProductModel = pd.ProductModel,
                                 ProductCategory = p.ProductCategory,
                                 Description = pd.Description,
                                 ProductModelID = pd.ProductModelId,
                                 Price = p.ListPrice,
                                 Photo = p.ThumbNailPhoto,
                                 StartSellDate = p.SellStartDate,
                                 SalePrice = p.SalePrice,
                                 SaleStartDate = p.SaleStartDate,
                                 SaleEndDate = p.SaleEndDate
                             }).Distinct();

            var qry = salesList.AsNoTracking().OrderBy(p => p.ProductModel);

            switch (sortOrder)
            {
                case "name_desc":
                    qry = salesList.AsNoTracking().OrderByDescending(p => p.ProductModel);
                    break;
                case "price_asc":
                    qry = salesList.AsNoTracking().OrderBy(p => p.SalePrice);
                    break;
                case "price_desc":
                    qry = salesList.AsNoTracking().OrderByDescending(p => p.SalePrice);
                    break;
                case "sellDate_asc":
                    qry = salesList.AsNoTracking().OrderBy(p => p.StartSellDate);
                    break;
                case "sellDate_desc":
                    qry = salesList.AsNoTracking().OrderByDescending(p => p.StartSellDate);
                    break;
                default:
                    qry = salesList.AsNoTracking().OrderBy(p => p.ProductModel);
                    break;
            }

            var model = await PagingList<SaleProduct>.CreateAsync(qry, 5, page);
            model.Action = "SalesFR";

            return View(model);
        }

        public IActionResult PrivacyFR()
        {
            return View();
        }

        public IActionResult AboutUsFR()
        {
            return View();
        }

        public IActionResult CareersFR()
        {
            return View();
        }

        public IActionResult CyclingSafetyFR()
        {
            return View();
        }

        public IActionResult ServicesFR()
        {
            return View();
        }

        public IActionResult ReturnsFR()
        {
            return View();
        }

        public IActionResult ContactUsFR()
        {
            return View();
        }

        public IActionResult CovidRulesFR()
        {
            return View();
        }

        public IActionResult LogInFR()
        {
            return View();
        }

        // GET: HomeFR/Details/5
        public async Task<IActionResult> DetailsFR(int? id)
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

        public IActionResult CartFR()
        {
            //Check if "cart" cookie exists
            if (HttpContext.Request.Cookies.ContainsKey(CART_COOKIE))
            {
                //get cookie
                String cartJson = HttpContext.Request.Cookies[CART_COOKIE]; //get cart as json-formatted string

                //read json into the dictionary of products in the cart that links product id to quantities
                Dictionary<int, int> cartProductId = JsonConvert.DeserializeObject<Dictionary<int, int>>(cartJson);

                //change this into a dictionary that stores the entire product model and quantity
                Dictionary<Product, int> cartProduct = new Dictionary<Product, int>();
                foreach (KeyValuePair<int, int> item in cartProductId)
                {
                    //get product from product id (key)
                    Product product = db.Product.First(products => products.ProductId == item.Key);

                    //add product to cartProduct
                    cartProduct.Add(product, item.Value);
                }

                return View(cartProduct);
            }

            //if no cart cookie is set, cart is empty
            return View();
        }

        // Add an Item To the cart in cookes
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddToCartFR([Bind("ProductId,Name,ProductNumber,Color,ListPrice,Size,Weight,ProductCategoryId,ProductModelId,SellStartDate,SellEndDate,DiscontinuedDate,ThumbNailPhoto,ThumbnailPhotoFileName,Rowguid,ModifiedDate")] Product product)
        {
            //get product from db based on values supplied by form
            Product selectedProduct = db.Product.First(products => products.ProductModelId == product.ProductModelId //product Model is the same
                                                                                                && (product.Size == null || products.Size == product.Size) //product size is the same or null (size does not exist)
                                                                                                && (product.Color == null || products.Color == product.Color)); //product color is the same or null (size does not exist)

            //the dictionary of products in the cart links product id to quantities
            Dictionary<int, int> cartProductId = new Dictionary<int, int>();

            //Check if "cart" cookie already exists
            if (HttpContext.Request.Cookies.ContainsKey(CART_COOKIE))
            {
                //get cookie
                String cart = HttpContext.Request.Cookies[CART_COOKIE]; //get cart as json-formatted string
                //read json
                cartProductId = JsonConvert.DeserializeObject<Dictionary<int, int>>(cart);
                //check if this product already in cart
                if (cartProductId.ContainsKey(selectedProduct.ProductId))
                    //increment selectedProduct quantity by 1
                    cartProductId[selectedProduct.ProductId]++;
                else
                    //add new entry to cartProductId
                    cartProductId.Add(selectedProduct.ProductId, 1);
            }
            else
                //add new entry to cartProductId
                cartProductId.Add(selectedProduct.ProductId, 1);

            //serialize cartProducts to json
            String cartJson = JsonConvert.SerializeObject(cartProductId);
            //set cookie
            HttpContext.Response.Cookies.Append(CART_COOKIE, cartJson);

            //get parent product category name so we can go back to controller
            ProductCategory productCategory = db.ProductCategory.First(ProductCategory => ProductCategory.ProductCategoryId == selectedProduct.ProductCategoryId);
            string parentProductCategoryName = db.ProductCategory.First(ProductCategory => ProductCategory.ProductCategoryId == productCategory.ParentProductCategoryId).Name;

            //go back to product page
            return RedirectToAction("Details", parentProductCategoryName, new { id = selectedProduct.ProductModelId });
        }

        // GET: HomeFR/Create
        public IActionResult CreateFR()
        {
            ViewData["ParentProductCategoryId"] = new SelectList(db.ProductCategory, "ProductCategoryId", "Name");
            return View();
        }

        // POST: HomeFR/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateFR([Bind("ProductCategoryId,ParentProductCategoryId,Name,Rowguid,ModifiedDate")] ProductCategory productCategory)
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

        // GET: HomeFR/Edit/5
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

        // POST: HomeFR/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditFR(int id, [Bind("ProductCategoryId,ParentProductCategoryId,Name,Rowguid,ModifiedDate")] ProductCategory productCategory)
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
                    if (!ProductCategoryExistsFR(productCategory.ProductCategoryId))
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

        // GET: HomeFR/Delete/5
        public async Task<IActionResult> DeleteFR(int? id)
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

        // POST: HomeFR/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmedFR(int id)
        {
            var productCategory = await db.ProductCategory.FindAsync(id);
            db.ProductCategory.Remove(productCategory);
            await db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductCategoryExistsFR(int id)
        {
            return db.ProductCategory.Any(e => e.ProductCategoryId == id);
        }
    }
}
