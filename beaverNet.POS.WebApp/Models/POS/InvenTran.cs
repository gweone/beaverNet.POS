﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace beaverNet.POS.WebApp.Models.POS
{
    public class InvenTran
    {
        public Guid InvenTranId { get; set; }
        [DisplayName("No. Transaksi")]
        public string Number { get; set; }
        public string Description { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public Guid TranSourceId { get; set; }
        public string TranSourceNumber { get; set; }
        public string TranSourceType { get; set; }
        public int Quantity { get; set; }
        public DateTimeOffset? InvenTranDate { get; set; } = DateTime.Now;
    }
}
