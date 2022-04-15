using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace beaverNet.POS.WebApp.Models
{
    public class SearchModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string ApiUrl { get; set; }
        public IEnumerable<SearchColumn> Columns { get; set; } = Enumerable.Empty<SearchColumn>();
        public string ColumnDefinition { get; set; }

        public string AjaxData { get; set; }

        public string FieldLabel { get; set; }
        public string FieldName { get; set; }
        public string FieldValue { get; set; }
        public string FieldDisplayName { get; set; }
        public int FieldIndex { get; set;}

        public string OnSelected { get; set; }
    }
}
