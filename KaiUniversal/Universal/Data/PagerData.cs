using System;
using System.Collections.Generic;

namespace Kai.Universal.Data {
    /// <summary>
    /// PageData object
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagerData<T> where T : new() {

        /// <summary>
        /// the page number
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// each page size
        /// </summary>
        public int EachPageSize { get; set; }

        /// <summary>
        /// total count
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// result data
        /// </summary>
        public List<T> Datas { get; set; }

        /// <summary>
        /// check is last page
        /// </summary>
        /// <returns></returns>
        public bool IsLastPage() {
            return (PageNumber + 1) * EachPageSize >= TotalCount;
        }

        /// <summary>
        /// get total page number
        /// </summary>
        /// <returns></returns>
        public int GetTotalPage() {
            return (int)Math.Ceiling((double)TotalCount / EachPageSize);
        }

    }

}
