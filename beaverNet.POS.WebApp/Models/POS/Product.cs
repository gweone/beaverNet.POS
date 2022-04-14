using beaverNet.POS.WebApp.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace beaverNet.POS.WebApp.Models.POS
{
    public class Product
    {
        public Guid ProductId { get; set; }
        [Required]
        [SearchColumn(0)]
        public string Name { get; set; }
        
        [SearchColumn(1)]
        public string Description { get; set; }
        
        [Required]
        [DisplayName("Harga Jual")]
        [SearchColumn(2, searchable = false, orderable = false)]
        public decimal PriceSell { get; set; }

        [Required]
        [DisplayName("Harga Beli")]
        [SearchColumn(3, searchable = false, orderable = false)]
        public decimal PricePurchase { get; set; }
    }
}
