using beaverNet.POS.WebApp.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace beaverNet.POS.WebApp.Models.POS
{
    public class Vendor
    {
        public Guid VendorId { get; set; }
        [Required]
        [SearchColumn(0)]
        public string Name { get; set; }

        [SearchColumn(1)]
        public string Description { get; set; }

        [SearchColumn(2)]
        public string Phone { get; set; }

        [SearchColumn(3)]
        public string Email { get; set; }

        [SearchColumn(4)]
        public string Address { get; set; }
        
        [SearchColumn(5)] 
        public string Address2 { get; set; }
    }
}
