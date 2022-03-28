using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using beaverNet.POS.WebApp.Data;
using beaverNet.POS.WebApp.Models.POS;
using Microsoft.AspNetCore.Authorization;

namespace beaverNet.POS.WebApp.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Product
        public Task<IActionResult> Index()
        {
            return Task.FromResult(View() as IActionResult);
        }

        // GET: Product/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }


        // GET: Product/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Product/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,Name,Description,PriceSell,PricePurchase")] Product product)
        {
            if (ModelState.IsValid)
            {
                product.ProductId = Guid.NewGuid();
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Edit), new { id = product.ProductId });
            }
            return View(product);
        }

        // GET: Product/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Product/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ProductId,Name,Description,PriceSell,PricePurchase")] Product product)
        {
            if (id != product.ProductId)
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
                    if (!ProductExists(product.ProductId))
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
            return View(product);
        }

        // GET: Product/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var product = await _context.Product.FindAsync(id);
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Product/PriceCalculation/5
        public async Task<IActionResult> PriceCalculation(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var goodsReceiveQueryAble = from goodsReceive in _context.GoodsReceive
                                        join purchase in _context.PurchaseOrder on goodsReceive.PurchaseOrderId equals purchase.PurchaseOrderId
                                        join vendor in _context.Vendor on purchase.VendorId equals vendor.VendorId
                                        join pline in _context.GoodsReceiveLine on goodsReceive.GoodsReceiveId equals pline.GoodsReceiveId
                                        join pcline in _context.PurchaseOrderLine on new { goodsReceive.PurchaseOrderId, pline.PurchaseOrderLineId }
                                                                                    equals new { pcline.PurchaseOrderId, pcline.PurchaseOrderLineId }
                                        select new
                                        {
                                            pline.ProductId,
                                            purchase.Number,
                                            purchase.PurchaseOrderDate,
                                            goodsReceive.GoodsReceiveDate,
                                            pline.QtyReceive,
                                            vendor.VendorId,
                                            Price = (int)pcline.Price / pcline.Quantity * pline.QtyReceive
                                        };
            if (id.HasValue && id.Value != Guid.Empty)
                goodsReceiveQueryAble = goodsReceiveQueryAble.Where(x => x.ProductId == id.Value);
            var gpGoodReceive = goodsReceiveQueryAble.GroupBy(x => x.ProductId)
                                              .Select(x => new
                                              {
                                                  ProductId = x.Key,
                                                  QtyReceive = x.Sum(y => y.QtyReceive),
                                                  Amount = x.Sum(y => y.Price)
                                              });
            var queryable = (from p in _context.Product
                             join receive in gpGoodReceive on p.ProductId equals receive.ProductId
                             select new Product
                             {
                                 Name = p.Name,
                                 Description = p.Description,
                                 ProductId = p.ProductId,
                                 PriceSell = p.PriceSell,
                                 PricePurchase = receive.Amount / receive.QtyReceive
                             });

            _context.UpdateRange(queryable);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(Guid id)
        {
            return _context.Product.Any(e => e.ProductId == id);
        }
    }
}
