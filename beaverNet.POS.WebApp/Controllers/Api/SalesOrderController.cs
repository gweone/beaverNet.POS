﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using beaverNet.POS.WebApp.Data;
using beaverNet.POS.WebApp.Models.POS;
using beaverNet.POS.WebApp.Models;
using System.Linq.Expressions;
using Microsoft.Extensions.Configuration;

namespace beaverNet.POS.WebApp.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesOrderController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly Services.POS.IRepository _pos;
        private readonly IConfiguration _configuration;

        public SalesOrderController(ApplicationDbContext context, Services.POS.IRepository pos, IConfiguration config)
        {
            _context = context;
            _pos = pos;
            _configuration = config;
        }

        // GET: api/SalesOrder
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SalesOrder>>> GetSalesOrder([FromQuery] SearchCriteria criteria)
        {
            if (criteria == null || criteria.columns == null)
                return await _context.SalesOrder.ToListAsync();
            return Ok(criteria.Apply<SalesOrder>(_context, new Expression<Func<SalesOrder, object>>[] { x => x.Customer }, "SalesOrderId"));
        }

        // GET: api/SalesOrder/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SalesOrder>> GetSalesOrder(Guid id)
        {
            var salesOrder = await _context.SalesOrder.FindAsync(id);

            if (salesOrder == null)
            {
                return NotFound();
            }

            return salesOrder;
        }

        // PUT: api/SalesOrder/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSalesOrder(Guid id, SalesOrder salesOrder)
        {
            if (id != salesOrder.SalesOrderId)
            {
                return BadRequest();
            }

            _context.Entry(salesOrder).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SalesOrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        //POST: api/SalesOrder/InitiateNewPOSTrans
        [HttpPost("[action]")]
        public async Task<IActionResult> InitiateNewPOSTrans()
        {
            SalesOrder salesOrder = new SalesOrder();
            salesOrder.Number = _pos.GenerateSONumber();
            salesOrder.SalesOrderDate = DateTime.Now;
            
            //random customer
            Customer cust = new Customer();
            cust = await _context.Customer.FirstOrDefaultAsync(x => x.Name == _configuration.GetValue<string>("PoS:Customer"));
            if (cust != null)
            {
                salesOrder.Description = cust.Name;
                salesOrder.CustomerId = cust.CustomerId;
            }

            _context.SalesOrder.Add(salesOrder);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSalesOrder", new { id = salesOrder.SalesOrderId }, salesOrder);
        }

        // POST: api/SalesOrder
        [HttpPost]
        public async Task<ActionResult<SalesOrder>> PostSalesOrder(SalesOrder salesOrder)
        {
            _context.SalesOrder.Add(salesOrder);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSalesOrder", new { id = salesOrder.SalesOrderId }, salesOrder);
        }

        // DELETE: api/SalesOrder/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<SalesOrder>> DeleteSalesOrder(Guid id)
        {
            var salesOrder = await _context.SalesOrder.FindAsync(id);
            if (salesOrder == null)
            {
                return NotFound();
            }

            _context.SalesOrder.Remove(salesOrder);
            await _context.SaveChangesAsync();

            return salesOrder;
        }

        private bool SalesOrderExists(Guid id)
        {
            return _context.SalesOrder.Any(e => e.SalesOrderId == id);
        }
    }
}
