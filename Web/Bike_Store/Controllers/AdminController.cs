using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NoNameBikes.Models;
using NoNameBikes.Models;

namespace NoNameBikes.Controllers
{
    public class AdminController : Controller
    {
        private readonly AdventureWorksLT2017Context db;

        public AdminController(AdventureWorksLT2017Context context)
        {
            db = context;
        }

        // GET: AdminController
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Inventory()
        {
            var productList = from items in db.Product
                              orderby items.ProductCategory
                              select items;
            return View(productList);
        }

        //public async Task<IActionResult> Inventory(string searchString)
        //{
        //    var productList = from items in db.Product
        //                      select items;

        //    if (!String.IsNullOrEmpty(searchString))
        //    {
        //        productList = productList.Where(s => s.Name.Contains(searchString));
        //    }

        //    return View(await productList.ToListAsync());
        //}

        public ActionResult SalesReport()
        {
            return View();
        }

        // GET: AdminController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AdminController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AdminController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Name,Color,StandardCost,ListPrice,Size,Weight,ProductCategoryId,ProductModelId,SellStartDate,SellEndDate,DiscontinuedDate,ThumbnailPhoto,ThumbNailPhotoFileName,Rowguid,ModifiedDate, ProductCategory, ProductModel, SalesOrderDetail")] Product product)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            db.Product.Add(product);
        //            await db.SaveChangesAsync();
        //            return RedirectToAction("Inventory");
        //        }
        //    }
        //    catch (Exception e /* dex */)
        //    {
        //        //Log the error (uncomment dex variable name and add a line here to write a log.
        //        ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
        //    }
        //    return View(product);
        //}

        // GET: AdminController/Edit/5
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

            return View(product);

        }

        // POST: AdminController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,Name,ProductNumber,Color,StandardCost,ListPrice,Size,Weight,ProductCategoryId,ProductModelId,SellStartDate,SellEndDate,DiscontinuedDate,ThumbnailPhoto,ThumbNailPhotoFileName,Rowguid,ModifiedDate,ProductCategory,ProductModel,SalesOrderDetail")] Product product)
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
                    var newProduct = await db.Product.FindAsync(id);
                    if (newProduct == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Inventory");
            }

            return View(product);
        }

        // GET: AdminController/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await db.Product
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: AdminController/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await db.Product.FindAsync(id);
            db.Product.Remove(product);
            await db.SaveChangesAsync();
            return RedirectToAction("Inventory");
        }

        // POST: AdminController/Delete/5
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        
    }
}
