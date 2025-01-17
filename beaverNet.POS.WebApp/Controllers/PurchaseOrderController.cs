﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using beaverNet.POS.WebApp.Data;
using beaverNet.POS.WebApp.Models.POS;
using beaverNet.POS.WebApp.Services.POS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using beaverNet.POS.WebApp.Utilities;

namespace beaverNet.POS.WebApp.Controllers
{
    [Authorize]
    public class PurchaseOrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IRepository _pos;
        private readonly IConfiguration _configuration;

        public PurchaseOrderController(ApplicationDbContext context, Services.POS.IRepository pos, IConfiguration configuration)
        {
            _context = context;
            _pos = pos;
            _configuration = configuration;
        }

        // GET: PurchaseOrder
        public Task<IActionResult> Index()
        {
            return Task.FromResult(View() as IActionResult);
        }

        // GET: PurchaseOrder/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchaseOrder = await _context.PurchaseOrder
                .Include(p => p.Vendor)
                .FirstOrDefaultAsync(m => m.PurchaseOrderId == id);
            if (purchaseOrder == null)
            {
                return NotFound();
            }

            return View(purchaseOrder);
        }

        // GET: PurchaseOrder/Create
        public IActionResult Create()
        {
            ViewData["VendorId"] = new SelectList(_context.Vendor, "VendorId", "Name");
            ViewData["Number"] = _pos.GeneratePONumber();
            return View(new PurchaseOrder() { PurchaseOrderDate = DateTime.Now });
        }

        // POST: PurchaseOrder/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PurchaseOrderId,Number,Description,PurchaseOrderDate,VendorId")] PurchaseOrder purchaseOrder)
        {
            if (ModelState.IsValid)
            {
                purchaseOrder.PurchaseOrderId = Guid.NewGuid();
                _context.Add(purchaseOrder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Edit), new { id = purchaseOrder.PurchaseOrderId });
            }
            ViewData["VendorId"] = new SelectList(_context.Vendor, "VendorId", "Name", purchaseOrder.VendorId);
            return View(purchaseOrder);
        }

        // GET: PurchaseOrder/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchaseOrder = await _context.PurchaseOrder.FindAsync(id);
            if (purchaseOrder == null)
            {
                return NotFound();
            }

            ViewData["VendorId"] = new SelectList(_context.Vendor, "VendorId", "Name", purchaseOrder.VendorId);
            ViewData["ProductId"] = new SelectList(_context.Product, "ProductId", "Name");
            return View(purchaseOrder);
        }

        // POST: PurchaseOrder/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("PurchaseOrderId,Number,Description,PurchaseOrderDate,VendorId")] PurchaseOrder purchaseOrder)
        {
            if (id != purchaseOrder.PurchaseOrderId)
            {
                return NotFound();
            }

            if (!HttpContext.User.IsInRole(SD.RoleAdmin) && !HttpContext.User.IsInRole(SD.RoleSuperUser))
            {
                var backdate = _configuration.GetValue<int>("PoS.Backdated", 7);
                if ((DateTimeOffset.Now - purchaseOrder.PurchaseOrderDate).Value.TotalDays > 7)
                {
                    ModelState.AddModelError("Error", $"Anda tidak bisa mengubah data yang telah di input lebih dari {backdate} hari");
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(purchaseOrder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PurchaseOrderExists(purchaseOrder.PurchaseOrderId))
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
            ViewData["VendorId"] = new SelectList(_context.Vendor, "VendorId", "Name", purchaseOrder.VendorId);
            return View(purchaseOrder);
        }

        // GET: PurchaseOrder/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var purchaseOrder = await _context.PurchaseOrder
                .Include(p => p.Vendor)
                .FirstOrDefaultAsync(m => m.PurchaseOrderId == id);
            if (purchaseOrder == null)
            {
                return NotFound();
            }

            return View(purchaseOrder);
        }

        // POST: PurchaseOrder/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var purchaseOrder = await _context.PurchaseOrder.FindAsync(id);
            if (!HttpContext.User.IsInRole(SD.RoleAdmin) && !HttpContext.User.IsInRole(SD.RoleSuperUser))
            {
                var backdate = _configuration.GetValue<int>("PoS.Backdated", 7);
                if ((DateTimeOffset.Now - purchaseOrder.PurchaseOrderDate).Value.TotalDays > 7)
                {
                    ModelState.AddModelError("Error", $"Anda tidak bisa mengubah data yang telah di input lebih dari {backdate} hari");
                    return View(nameof(Delete), purchaseOrder);
                }
            }
            List<PurchaseOrderLine> line = await _context.PurchaseOrderLine.Where(x => x.PurchaseOrderId.Equals(id)).ToListAsync();
            _context.PurchaseOrderLine.RemoveRange(line);
            _context.PurchaseOrder.Remove(purchaseOrder);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PurchaseOrderExists(Guid id)
        {
            return _context.PurchaseOrder.Any(e => e.PurchaseOrderId == id);
        }
    }
}
