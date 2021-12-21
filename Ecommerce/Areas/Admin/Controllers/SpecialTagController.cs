using Ecommerce.Data;
using Ecommerce.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Areas.Admin.Controllers
{
    public class SpecialTagController : Controller
    {

        private ApplicationDbContext _db;

        public SpecialTagController(ApplicationDbContext db)
        {
            _db = db;
        }

        [Area("Admin")]
        public IActionResult Index()
        {
            return View(_db.SpecialTags.ToList());
        }


        [Area("Admin")]
        public ActionResult Create()
        {
            return View();
        }

        [Area("Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SpecialTags specialTags)
        {
            if (ModelState.IsValid)
            {
                _db.SpecialTags.Add(specialTags);
                await _db.SaveChangesAsync();
                return RedirectToAction(actionName: nameof(Index));
            }

            return View(specialTags);
        }

        [Area("Admin")]
        public IActionResult Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var specialTags = _db.SpecialTags.Find(id);
            if (specialTags == null)
            {
                return NotFound();
            }
            return View(specialTags);
        }

        [Area("Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(SpecialTags specialTags)
        {
            if (ModelState.IsValid)
            {
                _db.Update(specialTags);
                await _db.SaveChangesAsync();
                return RedirectToAction(actionName: nameof(Index));
            }

            return View(specialTags);
        }

        [Area("Admin")]
        public IActionResult Details(int? id)
        {
            if (id == null)
                return NotFound();

            var specialTags = _db.SpecialTags.Find(id);
            if (specialTags == null)
            {
                return NotFound();
            }
            return View(specialTags);
        }

        [Area("Admin")]
        [HttpPost]
        public IActionResult Details(SpecialTags specialTags)
        {
            return RedirectToAction(actionName: nameof(Index));
        }

        [Area("Admin")]
        public IActionResult Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var specialtags = _db.SpecialTags.Find(id);
            if (specialtags == null)
            {
                return NotFound();
            }
            return View(specialtags);
        }

        [Area("Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id, SpecialTags specialTags)
        {

            if (id == null)
            {
                return NotFound();
            }

            if (id != specialTags.SpecialTagId)
            {
                return NotFound();
            }

            var special = _db.SpecialTags.Find(id);
            if (special == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                _db.Remove(special);
                await _db.SaveChangesAsync();
                return RedirectToAction(actionName: nameof(Index));
            }

            return View(special);
        }



    }
}
