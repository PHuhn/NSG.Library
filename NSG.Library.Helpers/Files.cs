using System;
using System.IO;
//
namespace NSG.Library.Helpers
{
    /// <summary>
    /// Static helpers for handling files.
    /// </summary>
    public static partial class Files
    {
        //
        /// <summary>
        /// Create a temporary file name.
        /// </summary>
        /// <param name="prefix">first part of file name
        ///  <example>file 'prefix' examples:
        ///   <list type="bullet">
        ///    <item><description>tmp</description></item>
        ///    <item><description>ni (as in app initials)</description></item>
        ///    <item><description>nsg (as in company initials)</description></item>
        ///   </list>
        ///  </example>
        /// </param>
        /// <param name="extent">the temporary file name extension:
        ///  <example>file 'extent' examples:
        ///   <list type="bullet">
        ///    <item><description>txt</description></item>
        ///    <item><description>tmp</description></item>
        ///    <item><description>log</description></item>
        ///   </list>
        ///  </example>
        /// </param>
        /// <returns>string of prefix followed by GUID and extent</returns>
        public static string TempFileName(string prefix = "tmp", string extent = "txt")
        {
            if ((!(string.IsNullOrEmpty(extent))))
            {
                if (extent.Substring(0, 1) != ".")
                    extent = "." + extent;
            }
            //
            return prefix + Guid.NewGuid().ToString() + extent;
        }
        //
        /// <summary>
        /// Delete the file
        /// </summary>
        /// <param name="fullFilePathAndName">full path and file name</param>
        public static void DeleteFile(string fullFilePathAndName)
        {
            if (File.Exists(fullFilePathAndName))
            {
                try
                {
                    File.Delete(fullFilePathAndName);
                }
                catch
                {
                }
            }
        }
        //
        /// <summary>
        /// Make a directory
        /// </summary>
        /// <param name="directoryName">name of the new direcory</param>
        public static void MakeDirectory(string directoryName)
        {
            if (!Directory.Exists(directoryName))
            {
                try
                {
                    Directory.CreateDirectory(directoryName);
                }
                catch
                {
                }
                if (!Directory.Exists(directoryName))
                {
                    throw new ApplicationException("Helpers.MakeDirectory, failed to make: " + directoryName);
                }
            }
        }
        //
        /// <summary>
        /// Read a file into a in-memory byte array.
        /// </summary>
        /// <param name="filePath">full path and file name</param>
        /// <returns>byte array</returns>
        /// <remarks>The opposite of ByteArrayToFile</remarks>
        public static byte[] FileToByteArray(string filePath)
        {
            if (File.Exists(filePath))
            {
                using (FileStream _fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    int _len = (int)_fs.Length;
                    byte[] _buffer = new byte[_len - 1 + 1];
                    _fs.Read(_buffer, 0, _len);
                    _fs.Close();
                    return _buffer;
                }
            }
            return null;
        }
        //
        /// <summary>
        /// Write a file from an in-memory byte array.
        /// </summary>
        /// <param name="file">byte array</param>
        /// <param name="fullFileName">full path and file name</param>
        /// <remarks>The opposite of FileToByteArray</remarks>
        public static void ByteArrayToFile(byte[] file, string fullFileName)
        {
            DeleteFile(fullFileName);
            using (FileStream _fs = new FileStream(fullFileName, FileMode.CreateNew))
            {
                using (BinaryWriter _bWriter = new BinaryWriter(_fs))
                {
                    _bWriter.Write(file);
                    _bWriter.Flush();
                    _bWriter.Close();
                }
                _fs.Close();
            }
        }
        //
        /// <summary>
        /// Remove the following reserved characters and replace with a space.
        ///   (less than)
        /// > (greater than)
        /// : (colon)
        /// " (double quote)
        /// / (forward slash)
        /// \ (backslash)
        /// | (vertical bar or pipe)
        /// ? (question mark)
        /// * (asterisk)
        /// , (comma)
        ///   (amperstand)
        /// </summary>
        /// <param name="fileName">file name with out path</param>
        /// <returns>
        /// A clean file name
        /// </returns>
        public static string FileNameCleaner(string fileName)
        {
            string _name = fileName.Replace("<", " ")
                .Replace(">", " ")
                .Replace(":", " ")
                .Replace("\"", " ")
                .Replace("/", " ")
                .Replace("\\", " ")
                .Replace("|", " ")
                .Replace("?", " ")
                .Replace("*", " ")
                .Replace(",", " ")
                .Replace("&", " ");
            return _name;
        }
    }
}
//