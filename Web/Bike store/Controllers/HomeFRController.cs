using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NoNameBikes.Models;

namespace NoNameBikes.Controllers
{
    public class HomeFRController : Controller
    {
        public const String CART_COOKIE = "cart"; //the string name of cookie that stores the cart
        private readonly AdventureWorksLT2017Context db;

        public HomeFRController(AdventureWorksLT2017Context context)
        {
            db = context;
        }

        // GET: HomeFR
        public IActionResult Index()
        {
            var bikeList = from bikes in db.ProductCategory
                           where bikes.ParentProductCategoryId == 1
                           select bikes;

            return View(bikeList.ToList());
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

        // GET: HomeFR/Details/5
        public async Task<IActionResult> Details(int? id)
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
        public IActionResult AddToCart([Bind("ProductId,Name,ProductNumber,Color,ListPrice,Size,Weight,ProductCategoryId,ProductModelId,SellStartDate,SellEndDate,DiscontinuedDate,ThumbNailPhoto,ThumbnailPhotoFileName,Rowguid,ModifiedDate")] Product product)
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
        public IActionResult Create()
        {
            ViewData["ParentProductCategoryId"] = new SelectList(db.ProductCategory, "ProductCategoryId", "Name");
            return View();
        }

        // POST: HomeFR/Create
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

        // GET: HomeFR/Delete/5
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

        // POST: HomeFR/Delete/5
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
