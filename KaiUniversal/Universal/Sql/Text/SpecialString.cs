namespace Kai.Universal.Sql.Text {
    /// <summary>
    /// the special string
    /// </summary>
    public class SpecialString {

        /// <summary>
        /// string value
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// special string constructor
        /// </summary>
        /// <param name="value"></param>
        public SpecialString(string value) {
            this.Value = value;
        }

        /// <summary>
        /// toString
        /// </summary>
        /// <returns></returns>
        public override string ToString() {
            return this.Value;
        }
    }
}
