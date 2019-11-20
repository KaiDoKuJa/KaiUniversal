using System;
using System.IO;

namespace Kai.Universal.Text {
    /// <summary>
    /// Description of FileUtility.
    /// </summary>
    [KaiOnlyApi]
    public class FileUtility {
        public static string GetFileOrDirName(string fileName) {
            if (Directory.Exists(fileName)) {
                return new DirectoryInfo(fileName).Name;
            } else if (File.Exists(fileName)) {
                return Path.GetFileName(fileName);
            } else {
                return "";
            }
//            FileAttributes attr = File.GetAttributes(fileName);
//            if (attr.HasFlag(FileAttributes.Directory)) {
//                return new DirectoryInfo(fileName).Name;
//            } else {
//                // TODO : maybe not file?
//                return Path.GetFileName(fileName);
//            }
        }
        
        public static bool IsFileOrDir(string fileName) {
             if (Directory.Exists(fileName)) {
                return true;
            } else if (File.Exists(fileName)) {
                return true;
            } else {
                return false;
            }
        }
    }
}