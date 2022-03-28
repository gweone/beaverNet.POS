using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace beaverNet.POS.WebApp.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<beaverNet.POS.WebApp.Models.POS.Customer> Customer { get; set; }
        public DbSet<beaverNet.POS.WebApp.Models.POS.Vendor> Vendor { get; set; }
        public DbSet<beaverNet.POS.WebApp.Models.POS.Product> Product { get; set; }
        public DbSet<beaverNet.POS.WebApp.Models.POS.PurchaseOrder> PurchaseOrder { get; set; }
        public DbSet<beaverNet.POS.WebApp.Models.POS.SalesOrder> SalesOrder { get; set; }
        public DbSet<beaverNet.POS.WebApp.Models.POS.GoodsReceive> GoodsReceive { get; set; }
        public DbSet<beaverNet.POS.WebApp.Models.POS.InvenTran> InvenTran { get; set; }
        public DbSet<beaverNet.POS.WebApp.Models.POS.PurchaseOrderLine> PurchaseOrderLine { get; set; }
        public DbSet<beaverNet.POS.WebApp.Models.POS.SalesOrderLine> SalesOrderLine { get; set; }
        public DbSet<beaverNet.POS.WebApp.Models.POS.GoodsReceiveLine> GoodsReceiveLine { get; set; }
        public DbSet<beaverNet.POS.WebApp.Models.POS.MedicalRecord> MedicalRecord { get; set; }

        public string DatePart(string datePartArg, DateTimeOffset? arg1 ) => throw new InvalidOperationException($"{nameof(DatePart)} cannot be called client side.");

        public double? Date(DateTimeOffset? arg1) => throw new InvalidOperationException($"{nameof(Date)} cannot be called client side.");

        protected override void OnModelCreating(ModelBuilder builder)
        {
            var datePartMethodInfo = typeof(ApplicationDbContext)
                .GetMethod(nameof(ApplicationDbContext.DatePart));
            builder.HasDbFunction(datePartMethodInfo)
               .HasTranslation(args => SqlFunctionExpression.Create("strftime",
                            new[]
                            {
                            args.ToArray()[0],
                            args.ToArray()[1]
                            },
                            typeof(string),
                            null
                        )
                    );

            var dateMethodInfo = typeof(ApplicationDbContext)
                .GetMethod(nameof(ApplicationDbContext.Date));
            builder.HasDbFunction(dateMethodInfo)
               .HasTranslation(args => SqlFunctionExpression.Create("julianday",
                            new[]
                            {
                            args.ToArray()[0]
                            },
                            typeof(double?),
                            null
                        )
                    );

            base.OnModelCreating(builder);
        }

    }
}
