using beaverNet.POS.WebApp.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace beaverNet.POS.WebApp.Models.POS
{
    public class Customer
    {
        public Guid CustomerId { get; set; }
        [Required]
        [DisplayName("Nama")]
        [SearchColumn(0)]
        public string Name { get; set; }

        [DisplayName("Medical Record")]
        [SearchColumn(1)]
        public string Email { get; set; }

        [DisplayName("Alergi Obat")]
        [SearchColumn(2)]
        public string Description { get; set; }
        
        [SearchColumn(3)]
        public string Phone { get; set; }

        [DisplayName("Alamat")]
        [SearchColumn(4)]
        public string Address { get; set; }

        [DisplayName("Jenis Kelamin")]
        [SearchColumn(5)]
        public string Address2 { get; set; }

        [DisplayName("Tanggal Lahir")]
        [SearchColumn(6, searchable = false)]
        public DateTimeOffset? DateofBirthday { get; set; }

        [DisplayName("Usia")]
        [NotMapped]
        public int Age { get { return (int)(DateTime.Now - DateofBirthday.Value).TotalDays / 365; } }
    }
}
