using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using ShopQuanAo.Models;

namespace ShopQuanAo.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class Products_AdminController : Controller
    {
        private readonly LTWEBContext _context;
        private IHostEnvironment _hostEnvironment;

        public Products_AdminController(LTWEBContext context, IHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

        // GET: Admin/Products_Admin
        public async Task<IActionResult> Index()
        {
            var lTWEBContext = _context.Products.Include(p => p.IdcategoryNavigation);
            return View(await lTWEBContext.ToListAsync());
        }

        // GET: Admin/Products_Admin/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.IdcategoryNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Admin/Products_Admin/Create
        public IActionResult Create()
        {
            ViewData["Idcategory"] = new SelectList(_context.Categories, "Id", "Id");
            return View();
        }

        // POST: Admin/Products_Admin/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,Quantity,Promationprice,Description,Image,Newproduct,Idcategory")] Product product)
        {
            if(product.Image != null)
            {
                var file = Path.Combine(_hostEnvironment.ContentRootPath, "wwwroot\\Uploads", product.Image.FileName);
                using (FileStream fileStream = new FileStream(file, FileMode.Create))
                {
                    await product.Image.CopyToAsync(fileStream);
                }
            }
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Idcategory"] = new SelectList(_context.Categories, "Id", "Id", product.Idcategory);
            return View(product);
        }

        // GET: Admin/Products_Admin/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["Idcategory"] = new SelectList(_context.Categories, "Id", "Id", product.Idcategory);
            return View(product);
        }

        // POST: Admin/Products_Admin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,Quantity,Promationprice,Description,Image,Newproduct,Idcategory")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
            ViewData["Idcategory"] = new SelectList(_context.Categories, "Id", "Id", product.Idcategory);
            return View(product);
        }

        // GET: Admin/Products_Admin/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.IdcategoryNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Admin/Products_Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'LTWEBContext.Products'  is null.");
            }
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
          return (_context.Products?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
