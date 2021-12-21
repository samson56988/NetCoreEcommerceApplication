using Ecommerce.Data;
using Ecommerce.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Areas.Admin.Controllers
{
    public class ProductTypesController : Controller
    {
        private ApplicationDbContext _db;

        public ProductTypesController(ApplicationDbContext db)
        {
            _db = db;
        }

        [Area("Admin")]
        public IActionResult Index()
        { 
            return View(_db.ProductTypes.ToList());
        }

        [Area("Admin")]
        public ActionResult Create()
        {
            return View();
        }

        [Area("Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductType productType)
        {
            if(ModelState.IsValid)
            {
                _db.ProductTypes.Add(productType);
                await _db.SaveChangesAsync();
                TempData["save"] = "Product has been saved successfully";
                return RedirectToAction(actionName: nameof(Index));
            }

            return View(productType);
        }


        [Area("Admin")]
        public IActionResult Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var productType = _db.ProductTypes.Find(id);
            if(productType == null)
            {
                return NotFound();
            }
            return View(productType);
        }

        [Area("Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(ProductType productType)
        {
            if (ModelState.IsValid)
            {
                _db.Update(productType);
                await _db.SaveChangesAsync();
                return RedirectToAction(actionName: nameof(Index));
            }

            return View(productType);
        }

        [Area("Admin")]
        public IActionResult Details(int? id)
        {
            if (id == null)
                return NotFound();

            var productType = _db.ProductTypes.Find(id);
            if (productType == null)
            {
                return NotFound();
            }
            return View(productType);
        }

        [Area("Admin")]
        [HttpPost]
        public IActionResult Details(ProductType productType)
        {
            return RedirectToAction(actionName: nameof(Index));
        }

        [Area("Admin")]
        public IActionResult Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var productType = _db.ProductTypes.Find(id);
            if (productType == null)
            {
                return NotFound();
            }
            return View(productType);
        }

        [Area("Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id,ProductType productType)
        {

            if(id == null)
            {
                return NotFound();
            }

            if(id!= productType.ProductId)
            {
                return NotFound();
            }

            var producttype = _db.ProductTypes.Find(id);
            if(producttype == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _db.Remove(producttype);
                await _db.SaveChangesAsync();
                return RedirectToAction(actionName: nameof(Index));
            }

            return View(productType);
        }

    }
}
