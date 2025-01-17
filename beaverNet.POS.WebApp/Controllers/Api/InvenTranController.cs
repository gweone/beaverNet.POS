﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using beaverNet.POS.WebApp.Data;
using beaverNet.POS.WebApp.Models.POS;
using System.Linq.Expressions;
using beaverNet.POS.WebApp.Models;

namespace beaverNet.POS.WebApp.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvenTranController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public InvenTranController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/InvenTran
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InvenTran>>> GetInvenTran([FromQuery] SearchCriteria criteria)
        {
            if (criteria == null || criteria.columns == null)
                return await _context.InvenTran.ToListAsync();
            return Ok(criteria.Apply<InvenTran>(_context, new Expression<Func<InvenTran, object>>[] { x => x.Product }, "InvenTranId"));
        }

        // GET: api/InvenTran/5
        [HttpGet("{id}")]
        public async Task<ActionResult<InvenTran>> GetInvenTran(Guid id)
        {
            var invenTran = await _context.InvenTran.FindAsync(id);

            if (invenTran == null)
            {
                return NotFound();
            }

            return invenTran;
        }

        // PUT: api/InvenTran/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInvenTran(Guid id, InvenTran invenTran)
        {
            if (id != invenTran.InvenTranId)
            {
                return BadRequest();
            }

            _context.Entry(invenTran).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InvenTranExists(id))
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

        // POST: api/InvenTran
        [HttpPost]
        public async Task<ActionResult<InvenTran>> PostInvenTran(InvenTran invenTran)
        {
            _context.InvenTran.Add(invenTran);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetInvenTran", new { id = invenTran.InvenTranId }, invenTran);
        }

        // DELETE: api/InvenTran/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<InvenTran>> DeleteInvenTran(Guid id)
        {
            var invenTran = await _context.InvenTran.FindAsync(id);
            if (invenTran == null)
            {
                return NotFound();
            }

            _context.InvenTran.Remove(invenTran);
            await _context.SaveChangesAsync();

            return invenTran;
        }

        private bool InvenTranExists(Guid id)
        {
            return _context.InvenTran.Any(e => e.InvenTranId == id);
        }
    }
}
