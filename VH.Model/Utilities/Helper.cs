using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Reflection;
using System.Diagnostics;
using System.Globalization;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace VH.Model.Utilities
{
    public static class Helper
    {
        public static List<T> GetListOf<T>(params T[] objects)
        {
            return objects.ToList();
        }
        public static List<string> GetListStringVersionOf<T>(List<T> objectList)
        {
            var query = from obj in objectList
                        select obj.ToString();

            return query.ToList();
        }
        public static string CreateUniqueFileName(string subFolder, string fileExtension)
        {
            string folderPath;
            if (String.IsNullOrEmpty(subFolder))
                folderPath = Environment.CurrentDirectory;
            else
            {
                folderPath = Path.Combine(Environment.CurrentDirectory, subFolder);

                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);
            }
            string fileName = String.Format("{0}.{1}", Guid.NewGuid(), fileExtension);
            return Path.Combine(folderPath, fileName);
        }
        public static string GetUniqueId()
        {
            return DateTime.UtcNow.ToString("yyyyMMddHHmmssffff") + (Guid.NewGuid().ToString("N"));
        }
        public static void DeleteFileName(string fileName)
        {
            if ((!String.IsNullOrEmpty(fileName)) &&
                (File.Exists(fileName)))
                File.Delete(fileName);
        }
        public static byte[] ReadFile(string fileName)
        {
            if (String.IsNullOrEmpty(fileName) || !File.Exists(fileName))
                return null;


            byte[] buffer;

            using (var filestream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {

                buffer = new byte[filestream.Length];
                filestream.Read(buffer, 0, Convert.ToInt32(filestream.Length));
            }

            return buffer;

        }

        /// <summary>
        /// Return numeric value as a string of the Actual Enum value
        /// </summary>
        public static string ConvertToDbString(Enum value)
        {
            return Enum.Format(value.GetType(), value, "d");
        }
        public static List<string> EnumItemsToList(Type enumType)
        {
            var result = new List<string>();

            if (enumType.IsEnum)
            {
                string[] enumItems = Enum.GetNames(enumType);
                result.AddRange(enumItems.AsEnumerable());
            }

            return result;
        }

        /// <summary>
        /// Constructs a string key=value - if entry parameters are not null or String.Empty
        /// </summary>
        public static string ConstructEqualString(string key, string value)
        {
            return !String.IsNullOrEmpty(key) && !String.IsNullOrEmpty(value)
                       ? String.Format("{0}={1};", key, value)
                       : String.Empty;
        }

        /// <summary>
        /// Concatenates all of the parameters into a single string - no spaces
        /// If left parameter is null or String.Empty - then returns just the right
        /// If right parameter is null or String.Empty - then returns just the left
        /// If separator parameter is null or String.Empty - then returns concatenates the left and right
        /// </summary>
        public static string ConcatenateStrings(string left, string right, string separator)
        {
            // If All Parameters are: Not NULL and Not String.Empty
            if (!String.IsNullOrEmpty(left) && !String.IsNullOrEmpty(right) && !String.IsNullOrEmpty(separator))
                return String.Format("{0}{1}{2}", left, separator, right);

            // If left and right are: Not NULL and Not String.Empty
            if (String.IsNullOrEmpty(left) && !String.IsNullOrEmpty(separator))
                return String.Format("{0}{1}", left, right);

            // If left is: Not NULL
            if (left != null)
                return left;

            // If right is: Not NULL
            if (right != null)
                return right;

            // Return String.Empty
            return String.Empty;
        }

        /// <summary>
        /// WARNING :: This might not work properly in Release/Optimized build since code might be re-arranged.
        /// </summary>
        public static string GetCurrentPropertyName()
        {
            string methodName = GetMethodNameFromStack(2);
            return GetPropertyFromMethodName(methodName);
        }
        /// <summary>
        /// WARNING :: This might not work properly in Release/Optimized build since code might be re-arranged.
        /// </summary>
        public static string GetPreviousPropertyName()
        {
            string methodName = GetMethodNameFromStack(3);
            return GetPropertyFromMethodName(methodName);
        }
        /// <summary>
        /// WARNING :: This might not work properly in Release/Optimized build since code might be re-arranged.
        /// </summary>
        public static string GetPropertyFromMethodName(string methodName)
        {
            string propertyName = String.Empty;
            if (!String.IsNullOrEmpty(methodName))
            {
                int splitIndex = methodName.IndexOf("_");
                propertyName = splitIndex >= 0 ? methodName.Substring(splitIndex + 1) : methodName;
            }
            return propertyName;
        }

        /// <summary>
        /// WARNING :: This might not work properly in Release/Optimized build since code might be re-arranged.
        /// </summary>
        public static string GetCurrentMethodName()
        {
            return GetMethodNameFromStack(2);
        }
        /// <summary>
        /// WARNING :: This might not work properly in Release/Optimized build since code might be re-arranged.
        /// </summary>
        public static string GetPreviousMethodName()
        {
            return GetMethodNameFromStack(3);
        }
        /// <summary>
        /// WARNING :: This might not work properly in Release/Optimized build since code might be re-arranged.
        /// </summary>
        public static string GetMethodNameFromStack(int index)
        {
            var strackTrace = new StackTrace();
            StackFrame frame;
            MethodBase method;
            string methodName = "UNKNOWN-METHOD-NAME";

            if (strackTrace.FrameCount >= index)
            {
                frame = strackTrace.GetFrame(index);
                if (frame != null)
                {
                    method = frame.GetMethod();
                    if (method != null)
                    {
                       methodName = String.Format("{0}.{1}",
                            (method.DeclaringType != null) ? method.DeclaringType.Name : "UNKNOWN-METHOD-TYPE",
                            method.Name);
                    }
                }
            }

            return methodName;
        }

        public static int CalculateCheckSum(int value)
        {
            //Multiply by 10
            value = value * 10;
            //pad number with leading 0s - limit 10 digits
            string strIVRSEQ = value.ToString();
            strIVRSEQ = strIVRSEQ.PadLeft(10, '0');
            char[] chars = strIVRSEQ.ToCharArray();
            //multiply each digit by mask '2121212121'
            int temp;
            int xfoot = 0;
            for (int i = 0; i < 9; i++)
            {
                temp = Convert.ToInt32(chars[i].ToString());
                if (i % 2 == 0)
                    temp = temp << 1;

                xfoot += temp;
            }
            //divide results by 10
            temp = xfoot % 10;
            //subtract remainder from 10
            temp = 10 - temp;
            //add temp to original number
            temp = value + temp;

            return temp;
        }
        public static string AppendStringValue(string separator, params string[] stringValues)
        {
            var resultStr = new StringBuilder();

            if ((stringValues != null) && (stringValues.Length > 0))
            {
                for (int index = 0; index < stringValues.Length; index++)
                {
                    string stringValue = stringValues[index];

                    resultStr.AppendFormat("{0}{1}",
                        (!separator.IsNullOrEmpty() && (index != 0)) ? separator : String.Empty,
                        stringValue);
                }
            }

            return resultStr.ToString();
        }
       
        #region System.IO.Path
        /// <summary>
        /// Replaces invalid file name characters in <paramref name="fileName"/> with the 
        /// replacement value <paramref name="replaceWith"/>.
        /// </summary>
        public static string ReplaceInvalidFileNameChars(string fileName, char replaceWith)
        {
            if (fileName == null)
                throw new ArgumentNullException("fileName");

            char[] invalidFileNameChars = Path.GetInvalidFileNameChars();

            return fileName.Replace(invalidFileNameChars, replaceWith);
        }
        /// <summary>
        /// Replaces invalid path characters in <paramref name="path"/> with the 
        /// replacement value <paramref name="replaceWith"/>.
        /// </summary>
        public static string ReplaceInvalidPathChars(string path, char replaceWith)
        {
            if (path == null)
                throw new ArgumentNullException("path");

            char[] invalidPathChars = Path.GetInvalidPathChars();

            return path.Replace(invalidPathChars, replaceWith);
        }
        /// <summary>
        /// Builds a file name path after replacing any invalid characters with <param name="replaceInvalidCharWith"/>. 
        /// </summary>
        public static string BuildFileNamePath(String path, String fileName, char replaceInvalidCharWith)
        {
            return Helper.BuildFileNamePath(path, fileName, replaceInvalidCharWith, replaceInvalidCharWith);
        }
        /// <summary>
        /// Builds a file name path after replacing any invalid characters in <param name="path"/> with 
        /// <param name="replaceInvalidPathCharWith"/> and in <param name="fileName"/> with 
        /// <param name="replaceInvalidFileNameCharWith"/>. 
        /// </summary>
        public static string BuildFileNamePath(String path, String fileName, char replaceInvalidPathCharWith, char replaceInvalidFileNameCharWith)
        {
            string updatedPath = Helper.ReplaceInvalidPathChars(path, replaceInvalidPathCharWith);
            string updatedFileName = Helper.ReplaceInvalidFileNameChars(fileName, replaceInvalidFileNameCharWith);
            string finalPath = Path.Combine(updatedPath, updatedFileName);

            return finalPath;
        }
        #endregion

        #region Date Functions

        public static long DateDiffMonths(DateTime date1, DateTime date2)
        {
            Calendar currentCalendar = CurrentCalendar;

            return Convert.ToInt64(((((currentCalendar.GetYear(date2) - currentCalendar.GetYear(date1)) * 12) + currentCalendar.GetMonth(date2)) - currentCalendar.GetMonth(date1)));
        }

        private static Calendar CurrentCalendar
        {
            get { return Thread.CurrentThread.CurrentCulture.Calendar; }
        }

        #endregion

        #region Parse Arguments
        /// <summary>
        /// DEPRECATED :: Image arguments are parsed to filters the non available place holders.
        /// </summary>
        /// <param name="stringArrayList">List of Strings</param>
        /// <param name="argumentString">Argument String Format</param>        
        /// <returns>string</returns>
        public static string ParseImageArguments(List<string> stringArrayList, string argumentString)
        {
            var scChar = new[] { '~', '`', '!', '@', '#', '$', '%', '^', '&', '*', '(', ')', ')', '_', '+', '=', '|', '/', '\"' }; //List of possible delimiters.
            string[] wordArray = Regex.Split(argumentString, @"\W+"); // Gets the available word in the arument String
            string strResult = string.Empty;

            if (!string.IsNullOrEmpty(argumentString))
            {
                stringArrayList = BuildStringSequenceCollection(stringArrayList, argumentString.Split(scChar, StringSplitOptions.RemoveEmptyEntries));
                //if (wordArray[1] == null)
                if (wordArray.Length == 1)//Issue fix :9360               
                    return string.Empty;
                strResult = argumentString.Contains(wordArray[1]) ? wordArray[1] : argumentString.Substring(0, wordArray[1].Length + 1);

                IDictionary<int, int> htEndIndexLength = BuildIndexCollection();
                foreach (string str in stringArrayList)
                {
                    int index;
                    int endIndex;
                    string endChar = strResult.Substring(strResult.Length - 1, 1);
                    if (scChar.Contains(Char.Parse(endChar)))
                    {
                        index = argumentString.IndexOf(str);
                        endIndex = 4;
                    }
                    else
                    {
                        index = argumentString.IndexOf(str) - 1;
                        endIndex = 5;
                    }

                    if (!(index < 0))
                    {
                        int length = argumentString.Length;

                        if ((index + endIndex) > length)
                            endIndex = length - index;
                        if (str.Length > 3)
                        {
                            var addLength = htEndIndexLength[str.Length];
                            endIndex += addLength;
                        }

                        strResult += argumentString.Substring(index, endIndex);
                    }
                }

                string endStrChar = strResult.Substring(strResult.Length - 1, 1);
                if (scChar.Contains(Char.Parse(endStrChar)))
                {
                    strResult = strResult.Substring(0, strResult.Length - 1);
                }
            }

            return strResult;
        }


        /// <summary>
        /// Builds the imaging interface assembly argument string.
        /// Finds and replaces tokens in the interface assembly argument string with the assigned image identifiers values.
        /// </summary>
        /// <param name="identifiers">Image idenfiers Token/Value dictionary</param>
        /// <param name="argumentString">Imaging interface assembly argument string</param>
        /// <returns></returns>
        public static string BuildImageArgumentString(Dictionary<string, string> identifiers, string argumentString)
        {
            const string regExPattern = @"({\w+})";
            var regEx = new Regex(regExPattern);

            List<string> argTokens = regEx.Split(argumentString).ToList();

            argTokens.RemoveAll(token => token == string.Empty || !token.Contains('{'));

            foreach (string token in argTokens)
            {
                if (!identifiers.ContainsKey(token))
                    identifiers.Add(token, " ");
            }


            var arguments = new StringBuilder(argumentString);
            foreach (KeyValuePair<string, string> identifier in identifiers)
            {
                arguments.Replace(identifier.Key, identifier.Value);
            }

            return arguments.ToString();
        }

        /// <summary>
        /// Gives the length to which endIndex will be added to get the String Result.
        /// </summary>
        /// <returns>Hashtable</returns>
        private static IDictionary<int, int> BuildIndexCollection()
        {
            return new Dictionary<int, int> { { 4, 1 }, { 5, 2 }, { 6, 3 }, { 7, 4 }, { 8, 5 }, { 9, 6 } };
        }

        /// <summary>
        /// Rearranges the list of string values as per string array passed
        /// </summary>
        /// <param name="arrayList">List of String Value</param>
        /// <param name="stringArray">Original Sequence String</param>
        /// <returns></returns>
        private static List<String> BuildStringSequenceCollection(List<String> arrayList, IEnumerable<string> stringArray)
        {
            return stringArray.Where(arrayList.Contains).ToList();
        }

        public static List<int> BuildBatterySize()
        {
            return new List<int>() {675, 13, 312, 10};
        }

        #endregion

        
    }
}
