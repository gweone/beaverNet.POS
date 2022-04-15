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
using beaverNet.POS.WebApp.Utilities;
using Microsoft.Extensions.Configuration;

namespace beaverNet.POS.WebApp.Controllers
{
    [Authorize]
    public class SalesOrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly Services.POS.IRepository _pos;
        private readonly IConfiguration _configuration;

        public SalesOrderController(ApplicationDbContext context, Services.POS.IRepository pos, IConfiguration configuration)
        {
            _context = context;
            _pos = pos;
            _configuration = configuration;
        }

        // GET: SalesOrder/POS
        public IActionResult POS()
        {
            return View();
        }

        // GET: SalesOrder
        public Task<IActionResult> Index()
        {
            return Task.FromResult(View() as IActionResult);
        }

        // GET: SalesOrder/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salesOrder = await _context.SalesOrder
                .Include(s => s.Customer)
                .FirstOrDefaultAsync(m => m.SalesOrderId == id);
            if (salesOrder == null)
            {
                return NotFound();
            }

            return View(salesOrder);
        }

        // GET: SalesOrder/Create
        public IActionResult Create()
        {
            ViewData["CustomerId"] = new SelectList(_context.Customer, "CustomerId", "Name");
            ViewData["Number"] = _pos.GenerateSONumber();
            return View(new SalesOrder() { SalesOrderDate = DateTime.Now });
        }

        // POST: SalesOrder/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SalesOrderId,Number,Description,SalesOrderDate,CustomerId")] SalesOrder salesOrder, MedicalRecord medicalRecord)
        {
            if (ModelState.IsValid)
            {
                salesOrder.SalesOrderId = Guid.NewGuid();
                salesOrder.Description = medicalRecord.Diagnosis;
                _context.Add(salesOrder);

                medicalRecord.MedicalRecordId = Guid.NewGuid();
                medicalRecord.CustomerId = salesOrder.CustomerId;
                medicalRecord.SalesOrderId = salesOrder.SalesOrderId;
                medicalRecord.RecordDate = DateTimeOffset.Now;
                _context.Add(medicalRecord);

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Edit), new { id = salesOrder.SalesOrderId });
            }
            ViewData["CustomerId"] = new SelectList(_context.Customer, "CustomerId", "Name", salesOrder.CustomerId);
            return View(salesOrder);
        }

        // GET: SalesOrder/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salesOrder = await _context.SalesOrder.FindAsync(id);
            if (salesOrder == null)
            {
                return NotFound();
            }
            else if(salesOrder.PaidStatus)
            {
                if (!HttpContext.User.IsInRole(SD.RoleAdmin) && !HttpContext.User.IsInRole(SD.RoleSuperUser))
                    return Forbid();
            }

            ViewData["CustomerId"] = new SelectList(_context.Customer, "CustomerId", "Name", salesOrder.CustomerId);
            ViewData["medicalRecord"] = await _context.MedicalRecord.FirstOrDefaultAsync(x => x.SalesOrderId == salesOrder.SalesOrderId);

            return View(salesOrder);
        }

        // POST: SalesOrder/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("SalesOrderId,Number,Description,SalesOrderDate,CustomerId")] SalesOrder salesOrder, [Bind(Prefix = "record.")] MedicalRecord medicalRecord)
        {
            if (id != salesOrder.SalesOrderId)
            {
                return NotFound();
            }

            if (!HttpContext.User.IsInRole(SD.RoleAdmin) && !HttpContext.User.IsInRole(SD.RoleSuperUser))
            {
                var backdate = _configuration.GetValue<int>("PoS.Backdated", 7);
                if ((DateTimeOffset.Now - salesOrder.SalesOrderDate).Value.TotalDays > 7)
                {
                    ModelState.AddModelError("Error", $"Anda tidak bisa mengubah data yang telah di input lebih dari {backdate} hari");
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if(medicalRecord != null && medicalRecord.MedicalRecordId != Guid.Empty)
                    {
                        salesOrder.Description = medicalRecord.Diagnosis;
                        _context.Update(medicalRecord);
                    }
                    _context.Update(salesOrder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SalesOrderExists(salesOrder.SalesOrderId))
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
            ViewData["CustomerId"] = new SelectList(_context.Customer, "CustomerId", "Name", salesOrder.CustomerId);
            return View(salesOrder);
        }

        // GET: SalesOrder/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salesOrder = await _context.SalesOrder
                .Include(s => s.Customer)
                .FirstOrDefaultAsync(m => m.SalesOrderId == id);
            if (salesOrder == null)
            {
                return NotFound();
            }

            return View(salesOrder);
        }

        // POST: SalesOrder/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var salesOrder = await _context.SalesOrder.FindAsync(id);

            if (!HttpContext.User.IsInRole(SD.RoleAdmin) && !HttpContext.User.IsInRole(SD.RoleSuperUser))
            {
                var backdate = _configuration.GetValue<int>("PoS.Backdated", 7);
                if ((DateTimeOffset.Now - salesOrder.SalesOrderDate).Value.TotalDays > 7)
                {
                    ModelState.AddModelError("Error", $"Anda tidak bisa mengubah data yang telah di input lebih dari {backdate} hari");
                    return View(nameof(Delete), salesOrder);
                }
            }

            List<SalesOrderLine> line = await _context.SalesOrderLine.Where(x => x.SalesOrderId.Equals(id)).ToListAsync();
            List<InvenTran> tran = await _context.InvenTran.Where(x => x.TranSourceNumber.Equals(salesOrder.Number)).ToListAsync();
            _context.InvenTran.RemoveRange(tran);
            _context.SalesOrderLine.RemoveRange(line);
            _context.SalesOrder.Remove(salesOrder);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Payment(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var salesOrder = await _context.SalesOrder
                .Include(s => s.Customer)
                .FirstOrDefaultAsync(m => m.SalesOrderId == id);
            if (salesOrder == null)
            {
                return NotFound();
            }
            salesOrder.PaidStatus = true;
            _context.SalesOrder.Update(salesOrder);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool SalesOrderExists(Guid id)
        {
            return _context.SalesOrder.Any(e => e.SalesOrderId == id);
        }
    }
}
