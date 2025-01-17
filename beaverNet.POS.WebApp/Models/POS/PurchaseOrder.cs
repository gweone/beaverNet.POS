﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace beaverNet.POS.WebApp.Models.POS
{
    public class PurchaseOrder
    {
        public Guid PurchaseOrderId { get; set; }
        [Required]
        [DisplayName("No. Transaksi")]
        public string Number { get; set; }
        public string Description { get; set; }

        [DisplayName("Tanggal Pembelian")]
        public DateTimeOffset? PurchaseOrderDate { get; set; } = DateTime.Now;
        [DisplayName("Supplier")] 
        public Guid VendorId { get; set; }
        public Vendor Vendor { get; set; }
        public virtual List<PurchaseOrderLine> PurchaseOrderLine { get; set; } = new List<PurchaseOrderLine>();
    }

    public class PurchaseOrderLine
    {
        public Guid PurchaseOrderLineId { get; set; }
        public Guid PurchaseOrderId { get; set; }
        [JsonIgnore]
        public PurchaseOrder PurchaseOrder { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; }
    }
}
