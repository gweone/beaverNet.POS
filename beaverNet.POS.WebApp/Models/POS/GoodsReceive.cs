using beaverNet.POS.WebApp.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace beaverNet.POS.WebApp.Models.POS
{
    public class GoodsReceive
    {
        public Guid GoodsReceiveId { get; set; }
        [Required]
        [SearchColumn(0)]
        [DisplayName("No. Transaksi")]
        public string Number { get; set; }
        [SearchColumn(1)]
        public string Description { get; set; }
        [SearchColumn(2)]
        public DateTimeOffset? GoodsReceiveDate { get; set; } = DateTime.Now;
        [DisplayName("Purchase Order")]
        public Guid PurchaseOrderId { get; set; }
        public PurchaseOrder PurchaseOrder { get; set; }
    }

    public class GoodsReceiveLine
    {
        public Guid GoodsReceiveLineId { get; set; }
        public Guid GoodsReceiveId { get; set; }
        [JsonIgnore]
        public GoodsReceive GoodsReceive { get; set; }
        public Guid PurchaseOrderLineId { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public int QtyPurchase { get; set; }
        public int QtyReceive { get; set; }
        public int QtyReceived { get; set; }
    }
}
