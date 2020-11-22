using System.Collections.Generic;
using System.Text;

namespace Kai.Universal.Sql.Where {

    /// <summary>
    /// the criteria pool
    /// </summary>
    public class CriteriaPool {
        private readonly List<Criteria> criterias = new List<Criteria>();
        private bool isCompleted = false; // 可增加重設屬性
        private StringBuilder sb = new StringBuilder();

        /// <summary>
        /// is empty pool
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty() {
            return criterias == null || criterias.Count == 0;
        }

        /// <summary>
        /// append criteria
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public CriteriaPool Append(Criteria c) {
            if (c != null) {
                if (isCompleted) {
                    isCompleted = false;
                }
                criterias.Add(c);
            }
            return this;
        }
    
        /// <summary>
        /// add criteria
        /// </summary>
        /// <param name="c"></param>
        public void AddCriteria(Criteria c) {
            if (c != null) {
                if (isCompleted) {
                    isCompleted = false;
                }
                criterias.Add(c);
            }
        }

        /// <summary>
        /// to generate where sql
        /// </summary>
        /// <returns></returns>
        public string GetWhereSql() {
            if (isCompleted) return sb.ToString();
            isCompleted = true;
            sb = new StringBuilder();
            for (int i = 0; i < criterias.Count; i++) {
                if (i > 0) {
                    sb.Append("and ");
                }
                sb.Append(criterias[i].GetSql());
            }
            return sb.ToString();
        }

    }
}