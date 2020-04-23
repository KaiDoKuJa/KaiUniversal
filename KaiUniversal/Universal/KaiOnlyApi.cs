using System;

namespace Kai.Universal {
    /// <summary>
    /// this is mean only KaiNet has this api
    /// only for kai.universal.db, sql, text
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class KaiOnlyApi : Attribute {
    }
}
