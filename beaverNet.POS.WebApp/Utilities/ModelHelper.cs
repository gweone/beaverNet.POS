using beaverNet.POS.WebApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace beaverNet.POS.WebApp.Utilities
{
    public static class ModelHelper
    {
        public static List<SearchColumn> GetSearchColumns<T>()
        {
            List<SearchColumn> results = new List<SearchColumn>();
            foreach (var item in typeof(T).GetProperties())
            {
                var attribute = item.GetCustomAttribute<SearchColumnAttribute>();
                if (attribute == null)
                    continue;

                string displayname = item.Name;
                var displayNameAttribute = item.GetCustomAttribute<DisplayNameAttribute>();
                if (displayNameAttribute != null)
                    displayname = displayNameAttribute.DisplayName;
                results.Add(new SearchColumn()
                {
                    name = item.Name,
                    displayname = displayname,
                    position = attribute.position,
                    searchable = attribute.searchable && item.PropertyType != typeof(int) && item.PropertyType != typeof(DateTimeOffset?),
                    orderable = attribute.orderable
                });
            }
            return results.OrderBy(x => x.position).ToList();
        }

        public static List<SearchColumn> GetSearchColumnsWithAction<T>()
        {
            return GetSearchColumns<T>().Union(new[] { new SearchColumn(){
                searchable = false,
                orderable = false,
                displayname = "Tindakan"
            }}).ToList();
        }
    }
}
