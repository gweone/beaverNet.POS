﻿using beaverNet.POS.WebApp.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace beaverNet.POS.WebApp.Models.POS
{
    public class SalesOrder
    {
        public Guid SalesOrderId { get; set; }
        [Required]
        [DisplayName("No. Transaksi")]

        [SearchColumn(0)]
        public string Number { get; set; }

        [SearchColumn(1)]
        public string Description { get; set; }
        [DisplayName("Tanggal Konsultasi")]

        [SearchColumn(2, searchable = false)]
        public DateTimeOffset? SalesOrderDate { get; set; } = DateTime.Now;
        [DisplayName("Pasien")]
        public Guid CustomerId { get; set; }

        [DisplayName("Pembayaran")]
        public bool PaidStatus { get; set; }

        public Customer Customer { get; set; }
        public virtual List<SalesOrderLine> SalesOrderLine { get; set; } = new List<SalesOrderLine>();
    }

    public class SalesOrderLine
    {
        public Guid SalesOrderLineId { get; set; }
        public Guid SalesOrderId { get; set; }
        [JsonIgnore]
        public SalesOrder SalesOrder { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; }
    }
}
