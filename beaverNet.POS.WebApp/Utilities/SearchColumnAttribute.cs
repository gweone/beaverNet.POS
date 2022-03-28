using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace beaverNet.POS.WebApp.Utilities
{
    public class SearchColumnAttribute : Attribute
    {
        public SearchColumnAttribute(int _position)
        {
            position = _position;
        }
        public int position { get; set; }
        public bool searchable { get; set; } = true;
        public bool orderable { get; set; } = true;
    }
}
