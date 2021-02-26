using System;
using System.Collections.Generic;

namespace VMS.Core.Helpers
{
    [Serializable]
    public class ItemList<T> where T : class
    {
        public int Total { get; set; }

        public int Page { get; set; }

        public int PageSize { get; set; }

        public List<T> Items { get; set; }
    }
}