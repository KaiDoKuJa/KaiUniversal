using Kai.Universal.Data;

namespace Kai.Universal.DataModel {
    public class DmlInfoExtension : DmlInfo {

        public string DmlId { get; set; }  // this info id
        public string GroupId { get; set; } // this info id of group

        public string CriteriaId { get; set; }
        public string ConnectionId { get; set; }
        public string Descript { get; set; }

    }
}
