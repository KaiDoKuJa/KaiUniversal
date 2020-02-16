
namespace Kai.Universal.Util {
    public class DateTimeUtility {
        public static readonly string ISO8601 = @"yyyy-MM-dd\THH:mm:ss\Z";

        public static readonly string DTTM = "yyyy-MM-dd HH:mm:ss";
        public static readonly string DATE_WITH_HYPHEN = "yyyy-MM-dd";
        public static readonly string TIME_WITH_COLON = "HH:mm:ss";

        public static readonly string YYYYMMDD = "yyyyMMdd";
        public static readonly string YYYYMMDDHHMMSS = "yyyyMMddHHmmss";
        public static readonly string HHMMSS = "HHmmss";

        private DateTimeUtility() { }

        //TODO : java有其他api
    }
}