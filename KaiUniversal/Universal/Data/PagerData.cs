using System;
using System.Collections.Generic;

namespace Kai.Universal.Data {
    public class PagerData<T> where T : new() {

        public int PageNumber { get; set; }
        public int EachPageSize { get; set; }
        public int TotalCount { get; set; }

        public List<T> Datas { get; set; }

        public bool IsLastPage() {
            return (PageNumber + 1) * EachPageSize >= TotalCount;
        }

        public int GetTotalPage() {
            return (int)Math.Ceiling((double)TotalCount / EachPageSize);
        }

    }

}
