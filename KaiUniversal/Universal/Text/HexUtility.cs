using System;
using System.Text;

namespace Kai.Universal.Text {

    /// <summary>
    /// the Hex Utility
    /// </summary>
    public class HexUtility {

        private HexUtility() { }

        /// <summary>
        /// KaiJava version code :
        /// </summary>
        static readonly char[] HEXES = "0123456789ABCDEF".ToCharArray();

        /// <summary>
        /// KaiJava version code : 
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string BytesToHexJ(byte[] bytes) {
            if (bytes == null) {
                return null;
            }
            char[] hexChars = new char[bytes.Length * 2];
            for (int j = 0; j < bytes.Length; j++) {
                int v = bytes[j] & 0xFF;
                if (v >= 0) {
                    hexChars[j * 2] = HEXES[v >> 4];
                } else {
                    hexChars[j * 2] = HEXES[(v >> 4) + (2 << ~4)];
                }
                hexChars[j * 2 + 1] = HEXES[v & 0x0F];
            }
            return new string(hexChars);
        }

        /// <summary>
        /// bytes to hex
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string BytesToHex(byte[] bytes) {
            if (bytes == null) {
                return null;
            }
            StringBuilder sb = new StringBuilder(bytes.Length * 2);
            for (int j = 0; j < bytes.Length; j++) {
                sb.AppendFormat("{0:X2}", bytes[j]);
            }
            return sb.ToString();
        }

        private static int HexToBin(char ch) {
            if ('0' <= ch && ch <= '9')
                return ch - '0';
            if ('A' <= ch && ch <= 'F')
                return ch - 'A' + 10;
            if ('a' <= ch && ch <= 'f')
                return ch - 'a' + 10;
            return -1;
        }

        /// <summary>
        /// hex to bytes
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        public static byte[] HexToBytes(string hexString) {
            int len = hexString.Length;

            // "111" is not a valid hex encoding.
            if (len % 2 != 0) {
                throw new ArgumentException("hexBinary needs to be even-length: " + hexString);
            }

            byte[] bytes = new byte[len / 2];

            for (int i = 0; i < len; i += 2) {
                int h = HexToBin(hexString[i]);
                int l = HexToBin(hexString[i + 1]);
                if (h == -1 || l == -1) {
                    throw new ArgumentException("contains illegal character for hexBinary: " + hexString);
                }

                bytes[i / 2] = (byte)(h * 16 + l);
            }

            return bytes;
        }

        /// <summary>
        /// int to binary array 
        /// (沒有任何地方用到)
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static int[] ToBinaryArray(int val) {
            int i = 0;
            int t1 = val;
            int remainder = 0;
            while (t1 > 0) {
                remainder = t1 % 2;
                t1 = t1 / 2;
                if (remainder == 1) {
                    i++;
                }
            }
            // 宣告數量
            int[] result = new int[i];
            i = 0;
            int t2 = val;
            int number = 1;
            while (t2 > 0) {
                remainder = t2 % 2;
                t2 = t2 / 2;
                if (remainder == 1) {
                    result[i] = number;
                    i++;
                }
                number = number * 2;
            }
            return result;
        }
    }
}