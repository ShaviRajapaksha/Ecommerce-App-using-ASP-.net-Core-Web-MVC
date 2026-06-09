using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyEcommerceMvc.Data;
using MyEcommerceMvc.Models;

namespace MyEcommerceMvc.Controllers;

public class ProductsController : Controller
{
    private readonly ApplicationDbContext _context;
    
    public ProductsController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    // GET: Products
    public async Task<IActionResult> Index(string searchString, int? categoryId)
    {
        var products = _context.Products
            .Include(p => p.Category)
            .AsQueryable();
        
        if (!string.IsNullOrEmpty(searchString))
        {
            products = products.Where(p => p.Name.Contains(searchString) || 
                                          (p.Description != null && p.Description.Contains(searchString)));
            ViewBag.SearchString = searchString;
        }
        
        if (categoryId.HasValue && categoryId > 0)
        {
            products = products.Where(p => p.CategoryId == categoryId);
            ViewBag.SelectedCategory = categoryId;
        }
        
        ViewBag.Categories = await _context.Categories.ToListAsync();
        
        return View(await products.ToListAsync());
    }
    
    // GET: Products/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
            return NotFound();
        
        var product = await _context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);
        
        if (product == null)
            return NotFound();
        
        return View(product);
    }
    
    // GET: Products/Create
    public async Task<IActionResult> Create()
    {
        ViewBag.Categories = await _context.Categories.ToListAsync();
        return View();
    }
    
    // POST: Products/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name,Description,Price,StockQuantity,CategoryId")] Product product)
    {
        if (ModelState.IsValid)
        {
            _context.Add(product);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Product created successfully!";
            return RedirectToAction(nameof(Index));
        }
        
        ViewBag.Categories = await _context.Categories.ToListAsync();
        return View(product);
    }
    
    // GET: Products/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
            return NotFound();
        
        var product = await _context.Products.FindAsync(id);
        if (product == null)
            return NotFound();
        
        ViewBag.Categories = await _context.Categories.ToListAsync();
        return View(product);
    }
    
    // POST: Products/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Price,StockQuantity,CategoryId")] Product product)
    {
        if (id != product.Id)
            return NotFound();
        
        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(product);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Product updated successfully!";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(product.Id))
                    return NotFound();
                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        
        ViewBag.Categories = await _context.Categories.ToListAsync();
        return View(product);
    }
    
    // GET: Products/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
            return NotFound();
        
        var product = await _context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);
        
        if (product == null)
            return NotFound();
        
        return View(product);
    }
    
    // POST: Products/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product != null)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Product deleted successfully!";
        }
        
        return RedirectToAction(nameof(Index));
    }
    
    private bool ProductExists(int id)
    {
        return _context.Products.Any(e => e.Id == id);
    }
}