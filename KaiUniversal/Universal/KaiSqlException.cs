using System;
using System.Runtime.Serialization;

namespace Kai.Universal {
    /// <summary>
    /// KaiSqlException
    /// </summary>
    [Serializable]
    public class KaiSqlException : Exception {

        /// <summary>
        /// code
        /// </summary>
        public virtual byte? Code { get; set; }

        /// <summary>
        /// constructor
        /// </summary>
        public KaiSqlException() {
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="message"></param>
        /// <param name="Code"></param>
        public KaiSqlException(string message, byte? Code)
            : base(message) {
            this.Code = Code;
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="message"></param>
        public KaiSqlException(string message)
            : base(message) {
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public KaiSqlException(string message, Exception inner)
            : base(message, inner) {
        }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected KaiSqlException(SerializationInfo info, StreamingContext context)
            : base(info, context) {
        }

        /// <summary>
        /// constructor
        /// </summary>
        public override string Message {
            get {
                if (this.Code == null) {
                    return base.Message;
                } else {
                    return string.Format("Code:{0}, Message:{1}", this.Code, base.Message);
                }
            }
        }
    }

}