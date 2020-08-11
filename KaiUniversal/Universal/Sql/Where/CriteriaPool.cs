using System.Collections.Generic;
using System.Text;

namespace Kai.Universal.Sql.Where {

    public class CriteriaPool {
        private readonly List<Criteria> criterias = new List<Criteria>();
        private bool isCompleted = false; // 可增加重設屬性
        private StringBuilder sb = new StringBuilder();

        public bool IsEmpty() {
            return criterias == null || criterias.Count == 0;
        }

        public CriteriaPool Append(Criteria c) {
            if (c != null) {
                if (isCompleted) {
                    isCompleted = false;
                }
                criterias.Add(c);
            }
            return this;
        }

        public void AddCriteria(Criteria c) {
            if (c != null) {
                if (isCompleted) {
                    isCompleted = false;
                }
                criterias.Add(c);
            }
        }
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