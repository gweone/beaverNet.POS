using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using beaverNet.POS.WebApp.Data;
using beaverNet.POS.WebApp.Models.POS;
using beaverNet.POS.WebApp.Models;

namespace beaverNet.POS.WebApp.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ReportController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Stocks([FromQuery] SearchCriteria criteria)
        {
            var queryable = (from p in _context.Product
                             join inv in _context.InvenTran on p.ProductId equals inv.ProductId into ginv
                             from inv in ginv.DefaultIfEmpty()
                             select new
                             {
                                 ProductId = p.ProductId,
                                 Product = p.Name,
                                 inv.Quantity
                             });
            var recordsTotal = queryable.GroupBy(x => x.ProductId)
                                        .Select(x => new
                                        {
                                            ProductId = x.Key,
                                            Product = x.Min(y => y.Product),
                                            Quantity = x.Sum(y => y.Quantity)
                                        })
                                        .Count();

            queryable = criteria.ApplyFilters(queryable);
            var groupQueryable = queryable.GroupBy(x => x.ProductId)
                .Select(x => new
                {
                    ProductId = x.Key,
                    Product = x.Min(y => y.Product),
                    Quantity = x.Sum(y => y.Quantity)
                });

            var recordsFiltered = recordsTotal;
            if (criteria.search != null || criteria.filters != null)
                recordsFiltered = groupQueryable.Count();
            groupQueryable = criteria.ApplySorting(groupQueryable);
            groupQueryable = criteria.ApplyPaging(groupQueryable);
            return Ok(new
            {
                criteria.draw,
                recordsTotal,
                recordsFiltered,
                data = (await groupQueryable.ToListAsync()).Select(x => criteria.GetValueOnly(x))
            });
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Purchases([FromQuery] SearchCriteria criteria)
        {
            var purchaseQueryAble = from purchase in _context.PurchaseOrder
                                    join vendor in _context.Vendor on purchase.VendorId equals vendor.VendorId
                                    join goodsReceive in _context.GoodsReceive on purchase.PurchaseOrderId equals goodsReceive.PurchaseOrderId into gl
                                    from goodsReceive in gl.DefaultIfEmpty()
                                    join pline in _context.PurchaseOrderLine on purchase.PurchaseOrderId equals pline.PurchaseOrderId
                                    select new
                                    {
                                        pline.ProductId,
                                        purchase.Number,
                                        purchase.PurchaseOrderDate,
                                        Price = (int)pline.Price,
                                        goodsReceive.GoodsReceiveDate,
                                        QtyPurchase = pline.Quantity,
                                        vendor.VendorId
                                    };

            purchaseQueryAble = criteria.ApplyFilters(purchaseQueryAble);

            var gpPurchase = purchaseQueryAble.GroupBy(x => x.ProductId)
                                              .Select(x => new
                                              {
                                                  ProductId = x.Key,
                                                  QtyPurchase = x.Sum(y => y.QtyPurchase)
                                              });


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
            goodsReceiveQueryAble = criteria.ApplyFilters(goodsReceiveQueryAble);
            var gpGoodReceive = goodsReceiveQueryAble.GroupBy(x => x.ProductId)
                                              .Select(x => new
                                              {
                                                  ProductId = x.Key,
                                                  QtyReceive = x.Sum(y => y.QtyReceive),
                                                  Amount = x.Sum(y => y.Price)
                                              });

            var queryable = (from p in _context.Product
                             join purchase in gpPurchase on p.ProductId equals purchase.ProductId
                             join receive in gpGoodReceive on p.ProductId equals receive.ProductId into rl
                             from receive in rl.DefaultIfEmpty()
                             select new
                             {
                                 ProductId = p.ProductId,
                                 Product = p.Name,
                                 Price = receive.Amount / receive.QtyReceive,
                                 purchase.QtyPurchase,
                                 receive.QtyReceive,
                                 receive.Amount
                             });

            var recordsTotal = _context.Product.Count();
            var recordsFiltered = recordsTotal;
            if (criteria.search != null || criteria.filters != null)
                recordsFiltered = queryable.Count();
            queryable = criteria.ApplySorting(queryable);
            queryable = criteria.ApplyPaging(queryable);
            return Ok(new
            {
                criteria.draw,
                recordsTotal,
                recordsFiltered,
                data = (await queryable.ToListAsync()).Select(x => criteria.GetValueOnly(x))
            });
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Sales([FromQuery] SearchCriteria criteria)
        {
            var salesQueryAble = from sales in _context.SalesOrder
                                 join customer in _context.Customer on sales.CustomerId equals customer.CustomerId
                                 join pline in _context.SalesOrderLine on sales.SalesOrderId equals pline.SalesOrderId
                                 where sales.PaidStatus
                                 select new
                                 {
                                     pline.ProductId,
                                     sales.Number,
                                     sales.SalesOrderDate,
                                     Quantity = pline.Quantity,
                                     Total = (int)pline.Total,
                                     customer.CustomerId
                                 };

            var recordsTotal = salesQueryAble.GroupBy(x => x.ProductId)
                                              .Select(x => x.Key).Count();

            salesQueryAble = criteria.ApplyFilters(salesQueryAble);

            var gpSales = salesQueryAble.GroupBy(x => x.ProductId)
                                              .Select(x => new
                                              {
                                                  ProductId = x.Key,
                                                  QtySales = x.Sum(y => y.Quantity),
                                                  TotalSales = x.Sum(y => y.Total)
                                              });

            var queryable = (from p in _context.Product
                             join sales in gpSales on p.ProductId equals sales.ProductId
                             select new
                             {
                                 ProductId = p.ProductId,
                                 Product = p.Name,
                                 sales.QtySales,
                                 sales.TotalSales
                             });

            var recordsFiltered = recordsTotal;
            if (criteria.search != null || criteria.filters != null)
                recordsFiltered = queryable.Count();
            queryable = criteria.ApplySorting(queryable);
            queryable = criteria.ApplyPaging(queryable);
            return Ok(new
            {
                criteria.draw,
                recordsTotal,
                recordsFiltered,
                data = (await queryable.ToListAsync()).Select(x => criteria.GetValueOnly(x))
            });
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> SalesByTime([FromQuery] SearchCriteria criteria, [FromQuery] string periode = "%Y-%m-%d")
        {
            var salesQueryAble = (from sales in _context.SalesOrder
                                  join customer in _context.Customer on sales.CustomerId equals customer.CustomerId
                                  join pline in _context.SalesOrderLine on sales.SalesOrderId equals pline.SalesOrderId
                                  where sales.PaidStatus
                                  select new
                                  {
                                      pline.ProductId,
                                      sales.Number,
                                      Grouped = sales.SalesOrderDate,
                                      Quantity = pline.Quantity,
                                      Total = (int)pline.Total,
                                      customer.CustomerId,
                                      SalesOrderDate = _context.DatePart(periode, sales.SalesOrderDate)
                                  });

            var recordsTotal = salesQueryAble.GroupBy(x => x.SalesOrderDate)
                                       .Select(x => x.Key).Count();

            salesQueryAble = criteria.ApplyFilters(salesQueryAble);
            var queryable = salesQueryAble.GroupBy(x => x.SalesOrderDate)
                                              .Select(x => new
                                              {
                                                  SalesOrderDate = x.Key,
                                                  TotalSales = x.Sum(y => y.Total)
                                              });
            var recordsFiltered = recordsTotal;
            if (criteria.search != null || criteria.filters != null)
                recordsFiltered = queryable.Count();
            queryable = criteria.ApplySorting(queryable);
            queryable = criteria.ApplyPaging(queryable);
            return Ok(new
            {
                criteria.draw,
                recordsTotal,
                recordsFiltered,
                data = (await queryable.ToListAsync()).Select(x => criteria.GetValueOnly(x))
            });
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> SalesByCustomer([FromQuery] SearchCriteria criteria)
        {
            var salesQueryAble = (from sales in _context.SalesOrder
                                  join customer in _context.Customer on sales.CustomerId equals customer.CustomerId
                                  join pline in _context.SalesOrderLine on sales.SalesOrderId equals pline.SalesOrderId
                                  where sales.PaidStatus
                                  select new
                                  {
                                      pline.ProductId,
                                      sales.Number,
                                      sales.SalesOrderDate,
                                      Quantity = pline.Quantity,
                                      Total = (int)pline.Total,
                                      customer.CustomerId,
                                      CustomerName = customer.Name,
                                  });
            var recordsTotal = salesQueryAble.GroupBy(x => x.SalesOrderDate)
                                       .Select(x => x.Key).Count();
            salesQueryAble = criteria.ApplyFilters(salesQueryAble);
            var queryable = salesQueryAble.GroupBy(x => x.CustomerName)
                                              .Select(x => new
                                              {
                                                  CustomerName = x.Key,
                                                  TotalSales = x.Sum(y => y.Total)
                                              });

            var recordsFiltered = recordsTotal;
            if (criteria.search != null || criteria.filters != null)
                recordsFiltered = queryable.Count();
            queryable = criteria.ApplySorting(queryable);
            queryable = criteria.ApplyPaging(queryable);
            return Ok(new
            {
                criteria.draw,
                recordsTotal,
                recordsFiltered,
                data = (await queryable.ToListAsync()).Select(x => criteria.GetValueOnly(x))
            });
        }
    }
}
