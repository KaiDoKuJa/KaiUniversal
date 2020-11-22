
namespace Kai.Universal.Util {
    /// <summary>
    /// DateTime utlity
    /// </summary>
    public class DateTimeUtility {
        /// <summary>
        /// yyyy-MM-dd\THH:mm:ss\Z
        /// </summary>
        public static readonly string ISO8601 = @"yyyy-MM-dd\THH:mm:ss\Z";

        /// <summary>
        /// yyyy-MM-dd HH:mm:ss
        /// </summary>
        public static readonly string DTTM = "yyyy-MM-dd HH:mm:ss";
        /// <summary>
        /// yyyy-MM-dd
        /// </summary>
        public static readonly string DATE_WITH_HYPHEN = "yyyy-MM-dd";
        /// <summary>
        /// HH:mm:ss
        /// </summary>
        public static readonly string TIME_WITH_COLON = "HH:mm:ss";
        /// <summary>
        /// yyyyMMdd
        /// </summary>
        public static readonly string YYYYMMDD = "yyyyMMdd";
        /// <summary>
        /// yyyyMMddHHmmss
        /// </summary>
        public static readonly string YYYYMMDDHHMMSS = "yyyyMMddHHmmss";
        /// <summary>
        /// HHmmss
        /// </summary>
        public static readonly string HHMMSS = "HHmmss";

        private DateTimeUtility() { }

    }
}