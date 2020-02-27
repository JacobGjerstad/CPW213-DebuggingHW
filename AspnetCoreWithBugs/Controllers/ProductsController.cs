using AspnetCoreWithBugs.Data;
using AspnetCoreWithBugs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspnetCoreWithBugs.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ProductContext _context;

        public ProductsController(ProductContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? page)
        {
            const int PageSize = 2;

            int pageNumber = page ?? 1;
            ViewData["CurrentPage"] = pageNumber;

            int maxPage = await GetMaxPage(PageSize);

            ViewData["MaxPage"] = maxPage;

            List<Product> products = await ProductDb.GetProductByPage(_context, pageNum: pageNumber, pageSize: PageSize);

            return View(products);
        }

        private async Task<int> GetMaxPage(int PageSize)
        {
            int numProducts = await ProductDb.GetNumProduct(_context);

            int maxPage = Convert.ToInt32(Math.Ceiling((double)numProducts / PageSize));

            return maxPage;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                await _context.AddAsync(product);
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            Product product = await ProductDb.GetProductById(id, _context);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                await ProductDb.Update(product, _context);
 
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            Product product = await ProductDb.GetProductById(id, _context);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            Product product = await ProductDb.GetProductById(id, _context);
            await ProductDb.Delete(product, _context);
            return RedirectToAction(nameof(Index));
        }
    }
}
