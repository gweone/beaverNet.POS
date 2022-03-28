using beaverNet.POS.WebApp.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace beaverNet.POS.WebApp.Models.POS
{
    public class MedicalRecord
    {
        public Guid MedicalRecordId { get; set; }
        [DisplayName("Tanggal Konsultasi")]
        [SearchColumn(0)]
        public DateTimeOffset? RecordDate { get; set; }

        [DisplayName("Pemeriksaan Fisik")]
        [SearchColumn(1)]
        public string PyshicalCheck { get; set; }

        [DisplayName("Diagnosa")]
        [SearchColumn(2)]
        public string Diagnosis { get; set; }

        [DisplayName("Therapy")]
        [SearchColumn(3)]
        public string Therapy { get; set; }
        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; }
        public Guid SalesOrderId { get; set; }
        public SalesOrder SalesOrder { get; set; }
    }
}
