using System;
using System.Globalization;
using System.IO;
using System.Resources;
using System.Xml;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace VH.Model.Utilities
{
    public static class Extensions
    {
        #region Type
        public static List<Type> GetTypeByCustomAttribute(this Type type, Type attributeType)
        {
            List<Type> resultList = new List<Type>();

            #region Search At Interface Level
            Type[] interfaceTypes = type.GetInterfaces();
            if ((interfaceTypes != null) && (interfaceTypes.Length > 0))
            {
                object[] interfaceAttributes = null;
                foreach (Type interfaceType in interfaceTypes)
                {
                    interfaceAttributes = interfaceType.GetCustomAttributes(attributeType, false);
                    if ((interfaceAttributes != null) && (interfaceAttributes.Length > 0))
                        resultList.Add(interfaceType);
                }
            }
            #endregion

            #region Search At Class Level
            if (!type.IsInterface)
            {
                object[] classAttributes = type.GetCustomAttributes(attributeType, true);
                if ((classAttributes != null) && (classAttributes.Length > 0))
                    resultList.Add(type);
            }
            #endregion

            return resultList;
        }

        public static List<PropertyInfo> GetAllLevelPropertiesByCustomAttribute(this Type type, Type attributeType)
        {
            List<PropertyInfo> resultList = new List<PropertyInfo>();

            if (type != null)
            {
                // Retrieve properties from all applied interfaces
                Type[] innerInterfaceTypes = type.GetInterfaces();
                if ((innerInterfaceTypes != null) && (innerInterfaceTypes.Length > 0))
                {
                    for (int index = innerInterfaceTypes.Length - 1; index >= 0; index--)
                    {
                        PropertyInfo[] innerInterfaceProps = innerInterfaceTypes[index].GetProperties();
                        foreach (PropertyInfo prop in innerInterfaceProps)
                        {
                            if (prop.CanRead && !prop.CanWrite)
                            {
                                object[] propAttributes = prop.GetCustomAttributes(attributeType, true);
                                if ((propAttributes != null) && (propAttributes.Length > 0))
                                {
                                    if (!resultList.Exists(result => result.Name == prop.Name))
                                        resultList.Add(prop);
                                }
                            }
                        }
                    }
                }

                // Retrieve properties from type
                List<PropertyInfo> typeProps = GetPropertiesByCustomAttribute(type, attributeType);

                var query = from typeProp in typeProps
                            join resultProp in resultList
                            on typeProp.Name equals resultProp.Name
                            select typeProp;

                int numberOfRemoved = typeProps.RemoveAll(duplicateProp => query.Contains(duplicateProp));

                resultList.AddRange(typeProps.AsEnumerable());
            }

            return resultList;
        }
        public static List<PropertyInfo> GetInterfacePropertiesAtAllLevelByCustomAttribute(this Type interfaceType, Type attributeType)
        {
            List<PropertyInfo> resultList = new List<PropertyInfo>();

            if ((interfaceType != null) && (interfaceType.IsInterface))
            {
                // Retrieve properties from all inner interfaces
                Type[] innerInterfaceTypes = interfaceType.GetInterfaces();
                if ((innerInterfaceTypes != null) && (innerInterfaceTypes.Length > 0))
                {
                    for (int index = innerInterfaceTypes.Length - 1; index >= 0; index--)
                    {
                        PropertyInfo[] innerInterfaceProps = innerInterfaceTypes[index].GetProperties();
                        foreach (PropertyInfo prop in innerInterfaceProps)
                        {
                            if (prop.CanRead && !prop.CanWrite)
                            {
                                object[] propAttributes = prop.GetCustomAttributes(attributeType, true);
                                if ((propAttributes != null) && (propAttributes.Length > 0))
                                {
                                    if (!resultList.Exists(result => result.Name == prop.Name))
                                        resultList.Add(prop);
                                }
                            }
                        }
                    }
                }

                // Retrieve properties from interface
                List<PropertyInfo> interfaceProps = GetPropertiesByCustomAttribute(interfaceType, attributeType);

                var query = from interfaceProp in interfaceProps
                            join resultProp in resultList
                            on interfaceProp.Name equals resultProp.Name
                            select interfaceProp;

                int numberOfRemoved = interfaceProps.RemoveAll(duplicateProp => query.Contains(duplicateProp));
                resultList.AddRange(interfaceProps.AsEnumerable());
            }

            return resultList;
        }
        public static List<PropertyInfo> GetPropertiesByCustomAttribute(this Type type, Type attributeType)
        {
            List<PropertyInfo> resultList = new List<PropertyInfo>();

            if (type != null)
            {
                PropertyInfo[] interfaceProps = type.GetProperties();
                foreach (PropertyInfo prop in interfaceProps)
                {
                    if (prop.CanRead && !prop.CanWrite)
                    {
                        object[] propAttributes = prop.GetCustomAttributes(attributeType, true);
                        if ((propAttributes != null) && (propAttributes.Length > 0))
                        {
                            if (!resultList.Exists(result => result.Name == prop.Name))
                                resultList.Add(prop);
                        }
                    }
                }
            }

            return resultList;
        }

       
        public static string GetObjectName(this Type objectType)
        {
            if (objectType == null)
                throw new ArgumentNullException("objectType");

            if (!objectType.IsGenericType)
                return objectType.FullName;

            int accentIndex = objectType.FullName.IndexOf('`');
            return objectType.FullName.Substring(0, accentIndex);
        }

      
        public static string GetObjectNameOfDerivedT(this Type objectType)
        {
            if (objectType == null)
                throw new ArgumentNullException("objectType");

            if (!objectType.IsGenericType)
                return string.Empty;

            Type firstGenericArgument = objectType.GetGenericArguments()[0];
            return firstGenericArgument.FullName;
        }
        #endregion

        #region String
   
        public static Type GetObjectType(this string typeName)
        {
            Type result = null;

            string assemblyName = String.Empty;
            string assemblyFullPath = String.Empty;
            Assembly assembly = null;

            int entitiesPeriodPos = typeName.IndexOf("Entities", 0) + "Entities".Length;
            if (entitiesPeriodPos > -1)
                assemblyName = String.Format("{0}.dll", typeName.Substring(0, entitiesPeriodPos));

            Assembly eAssembly = Assembly.GetEntryAssembly();
            if (eAssembly == null)
                eAssembly = Assembly.GetExecutingAssembly();

            assemblyFullPath = string.Format("{0}\\{1}", System.IO.Path.GetDirectoryName(eAssembly.Location), assemblyName);
            assembly = Assembly.LoadFrom(assemblyFullPath);
            if (assembly != null)
                result = assembly.GetType(typeName);

            return result;
        }

        /// <summary>
        /// Replaces all instances of the characters in <paramref name="toReplaceList"/> with the
        /// character <paramref name="replaceWith"/>.  If no characters are found for replacement, the
        /// original string is returned.
        /// </summary>
        /// <param name="value">string to scan characters for replacement</param>
        /// <param name="toReplaceList">set of characters to replace</param>
        /// <param name="replaceWith">replacement value</param>
        /// <returns>string with replaced characters</returns>
        public static string Replace(this string value, char[] toReplaceList, char replaceWith)
        {
            string updatedValue = value;

            foreach (char toReplace in toReplaceList)
            {
                if (updatedValue.Contains(toReplace))
                    updatedValue = updatedValue.Replace(toReplace, replaceWith);
            }
            return updatedValue;
        }

        
        public static string MaskConnectionStringPassword(this string connectionString)
        {
            string result = connectionString;

            if (!result.IsNullOrEmpty())
            {
                Regex regEx = new Regex(@"(Password=)([\w\d:#@%/;$\(\)~_?\+-=\\\.&]*)(;)");
                if (regEx.IsMatch(result))
                {
                    Match match = regEx.Match(result);
                    if (!match.Groups[2].Value.IsNullOrEmpty())
                        result = result.Replace(match.Groups[2].Value, "******");
                }
            }

            return result;
        }
        public static byte[] ConvertToASCIIBytes(this string valueStr)
        {
            byte[] result = null;

            ASCIIEncoding encoding = new ASCIIEncoding();
            result = encoding.GetBytes(valueStr);

            return result;
        }
       

        public static string TrimStartWithTrimCount(this String str, char trimChar, out int count)
        {
            count = 0;

            foreach (char c in str)
            {
                if (c == trimChar)
                    count++;
                else
                    break;
            }

            return str.TrimStart(trimChar);
        }
        public static string TrimEndWithTrimCount(this String str, char trimChar, out int count)
        {
            count = 0;
            int strLen = str.Length;

            for (int i = strLen - 1; i >= 0; i--)
            {
                if (str[i] == trimChar)
                    count++;
                else
                    break;
            }

            return str.TrimEnd(trimChar);
        }
        public static string RemoveExtraWhiteSpaces(this String str)
        {
            StringBuilder sb = new StringBuilder();

            var ca = str.ToCharArray();
            int strLen = ca.Length;

            for (int i = 0; i < strLen; i++)
            {
                if (ca[i] != ' ')
                    sb.Append(ca[i]);
                else if (i != 0 && i < strLen - 1)
                {
                    if (ca[i - 1] != ' ' && ca[i + 1] != ' ')
                        sb.Append(ca[i]);
                    else
                        ca[i] = 'x';
                }
            }

            return sb.ToString().Trim();
        }

        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }
        public static string AddWordFileExtension(this string value)
        {
            string result = value;

            if (!result.Contains(".docx"))
                result = String.Format("{0}.docx", result);

            return result;
        }
        public static XmlDocument AsXmlDocument(this string xml)
        {
            if (xml == null)
                throw new ArgumentNullException("xml");
            if (xml.Length == 0)
                throw new ArgumentException("Should be valid XML string", "xml");

            try
            {
                var xdocument = new XmlDocument();
                xdocument.LoadXml(xml);
                return xdocument;
            }
            catch (XmlException)
            {
                return null;
            }
        }

        /// <summary>
        /// Replace string along with handling the OldChar/newChar if NULL or Empty String
        /// </summary>
        /// <param name="_string"></param>
        /// <param name="oldChar"></param>
        /// <param name="newChar"></param>
        /// <param name="handleIsEmptyAndNull">On True Handles the OldChar/newChar if NULL or Empty String</param>
        /// <returns></returns>
        public static string Replace(this string _string, string oldChar, string newChar, bool handleIsEmptyAndNull)
        {
            string result;
            if (handleIsEmptyAndNull)
            {
                if (newChar == null)
                    newChar = string.Empty;

                result = string.IsNullOrEmpty(oldChar) ? _string : _string.Replace(oldChar, newChar);
            }
            else
            {
                result = _string.Replace(oldChar, newChar);
            }
            return result;
        }
        public static string AppendSpace(this string inputText)
        {
            if (string.IsNullOrEmpty(inputText))
                return string.Empty;

            string result = inputText.Trim();
            return result.Length > 0 ? string.Concat(inputText, " ") : inputText;
        }

        public static bool ToBoolean(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;

            return (value.Equals("1") ||
                    value.Equals("Y", StringComparison.OrdinalIgnoreCase) ||
                    value.Equals("YES", StringComparison.OrdinalIgnoreCase) ||
                    value.Equals("T", StringComparison.OrdinalIgnoreCase) ||
                    value.Equals("TRUE", StringComparison.OrdinalIgnoreCase));
        }
        
        #endregion String

        #region StringBuilder
        /// <summary>
        /// Appends to the StringBuilder a New Line + value - If value is not Null or an EmptyString
        /// </summary>
        public static void AppendNewFormatedLine(this StringBuilder stringBuilder, string value)
        {
            if (!String.IsNullOrEmpty(value))
                stringBuilder.AppendFormat("{0}{1}", Environment.NewLine, value);
        }
        /// <summary>
        /// Appends to the StringBuilder a New Line + separator + value - If value is not Null or an EmptyString
        /// </summary>
        public static void AppendNewFormatedLine(this StringBuilder stringBuilder, string label, string separator, string value)
        {
            if (!String.IsNullOrEmpty(value))
                stringBuilder.AppendFormat("{0}{1}{2}{3}", Environment.NewLine, label, separator, value);
        }
        /// <summary>
        /// Appends to the StringBuilder a New Line + separator + value - If value is not Null
        /// </summary>
        public static void AppendNewFormatedLine(this StringBuilder stringBuilder, string label, string separator, int? value)
        {
            if (value.HasValue)
                stringBuilder.AppendNewFormatedLine(label, separator, value.Value.ToString());
        }
        /// <summary>
        /// Appends to the StringBuilder a New Line + separator + value - If value is not Null
        /// </summary>
        public static void AppendNewFormatedLine(this StringBuilder stringBuilder, string label, string separator, decimal? value)
        {
            if (value.HasValue)
                stringBuilder.AppendNewFormatedLine(label, separator, value.Value.ToString());
        }
        /// <summary>
        /// Appends to the StringBuilder a New Line + separator + value - If value is not Null
        /// </summary>
        public static void AppendNewFormatedLine(this StringBuilder stringBuilder, string label, string separator, DateTime? value)
        {
            if (value.HasValue)
                stringBuilder.AppendNewFormatedLine(label, separator, value.Value.ToString());
        }
        #endregion

        #region SID Conversion
        [DllImport("advapi32.dll", EntryPoint = "ConvertStringSidToSid", SetLastError = true)]
        private static extern bool ConvertStringSidToSidAPI(string StringSid, out IntPtr ptrSid);

        [DllImport("advapi32.dll", EntryPoint = "GetLengthSid", CharSet = CharSet.Auto)]
        private static extern int GetLengthSidAPI(IntPtr pSID);

        [DllImport("advapi32", EntryPoint = "ConvertSidToStringSid", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool ConvertSidToStringSidAPI([MarshalAs(UnmanagedType.LPArray)] byte[] pSID, out IntPtr ptrSid);

        public static byte[] ConvertToByteSid(this string sid)
        {
            IntPtr sidPtr = IntPtr.Zero;
            if (Extensions.ConvertStringSidToSidAPI(sid, out sidPtr))
            {
                int size = (int)Extensions.GetLengthSidAPI(sidPtr);
                byte[] btArray = new byte[size];
                Marshal.Copy(sidPtr, btArray, 0, size);
                Marshal.FreeHGlobal(sidPtr);

                return btArray;
            }

            return null;
        }
        public static string ConvertToStringSid(this byte[] sidBytes)
        {
            string sid = string.Empty;
            IntPtr sidPtr = IntPtr.Zero;
            if (Extensions.ConvertSidToStringSidAPI(sidBytes, out sidPtr))
            {
                sid = Marshal.PtrToStringAuto(sidPtr);
                Marshal.FreeHGlobal(sidPtr);
            }

            return sid;
        }
        #endregion

        #region Decimal
        public static decimal SetScale(this decimal value, int scale)
        {
            if (scale < 0)
                throw new ArgumentException("decimal scale value cannot be negative");

            string valueStr = value.ToString();
            // Check for a decimal point...  If one exists, we need to check the scale.  CR allows a 
            // scale of 6, IFAS only 5...
            int decimalPointIndex = valueStr.IndexOf('.');
            if (decimalPointIndex > -1)
            {
                // if starts with a decimal point, prepend a 0 so split will always give us an array of length two...
                if (decimalPointIndex == 0)
                    valueStr = '0' + valueStr;
                string[] numberAndFraction = valueStr.Split('.');
                // if fraction portion is greater than five, we need to truncate it to five (essentially, set scale to 5)
                if (numberAndFraction[1].Length > scale)
                    valueStr = numberAndFraction[0] + '.' + numberAndFraction[1].Substring(0, scale);
            }
            decimal scaledValue = Decimal.Parse(valueStr);

            return scaledValue;
        }
        #endregion Decimal

        #region NameValueCollection
        public static string GetValue(this NameValueCollection nameValueCollection, string valueName)
        {
            string result = String.Empty;

            if ((nameValueCollection != null) && (nameValueCollection[valueName] != null))
                result = nameValueCollection[valueName];

            return result;
        }
        #endregion

        #region Check Program for program in system
        /// <summary>
        /// Check for the existance of a program in the system
        /// </summary>
        /// <param name="assemblyName">Assembly name (Environment Variables will be used to retrieve the path) OR full path assembly name.</param>
        /// <returns>bool</returns>
        public static bool CheckProgram(string assemblyName)
        {
            string target = string.Empty;
            bool assemblyFlag = false;

            if (assemblyName != string.Empty)
            {
                assemblyFlag = ((File.Exists(assemblyName)) ? true : false);
                if (assemblyFlag == false)
                {
                    string EnvironmentPath = Environment.GetEnvironmentVariable("Path");
                    List<string> paths = new List<string>(EnvironmentPath.Split(';'));
                    foreach (string path in paths)
                    {
                        target = Path.Combine(path, assemblyName);
                        if (File.Exists(target))
                        {
                            assemblyFlag = true;
                            break;
                        }
                    }
                }
            }
            return assemblyFlag;
        }
        #endregion

        #region Arrays
        public static string ToPrintableString(this string[] stringArray)
        {
            StringBuilder resultStr = new StringBuilder();

            if ((stringArray != null) && (stringArray.Length > 0))
            {
                foreach (string item in stringArray)
                    resultStr.AppendFormat("{0},", item);

                if (resultStr.Length > 0)
                    resultStr.Remove(resultStr.Length - 1, 1);
            }

            return resultStr.ToString();
        }
        public static string ConvertToASCIIString(this byte[] byteArray)
        {
            string result = String.Empty;

            ASCIIEncoding encoding = new ASCIIEncoding();
            result = encoding.GetString(byteArray);

            return result;
        }
        #endregion

        #region IEnumerable
        public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable enumerableList)
        {
            if (enumerableList != null)
            {
                //create an emtpy observable collection object 
                var observableCollection = new ObservableCollection<T>();

                //loop through all the records and add to observable collection object 
                foreach (T item in enumerableList)
                    observableCollection.Add((T)item);

                //return the populated observable collection 
                return observableCollection;
            }
            return null;

        }
        #endregion IEnumerable

        #region DateTime
        /// <summary>
        /// Calculates the occurrence (first, second, third, etc) of a particular day of the week (Monday, Tuesday, etc) for a given <paramref name="date"/>
        /// </summary>
        /// <param name="date">date to determine the occurrence of that particular day of the week</param>
        /// <returns>numeric representation of the occurrence</returns>
        public static int OccurrenceOfDayInMonth(this DateTime date)
        {
            date = date.Date;

            System.Globalization.Calendar calendar = new System.Globalization.GregorianCalendar();
            DateTime firstOfMonth = new DateTime(date.Year, date.Month, 1);

            TimeSpan daysFromFirstOfMonth = date - firstOfMonth;

            int occurrence = daysFromFirstOfMonth.Days / 7;

            if ((daysFromFirstOfMonth.Days % 7 != 0) || (firstOfMonth.DayOfWeek == date.DayOfWeek))
                ++occurrence;

            return occurrence;
        }
        /// <summary>
        /// Query if <paramref name="date"/> is the last occurrence of that particulay day of the week for the month that <paramref name="date"/>
        /// occurs.
        /// </summary>
        /// <param name="date">date</param>
        /// <returns>true if last occurrence, else false</returns>
        public static bool IsLastOccurrenceOfDayInMonth(this DateTime date)
        {
            date = date.Date;

            System.Globalization.Calendar calendar = new System.Globalization.GregorianCalendar();
            DateTime lastOfMonth = new DateTime(date.Year, date.Month, calendar.GetDaysInMonth(date.Year, date.Month));

            TimeSpan daysFromLastOfMonth = lastOfMonth - date;

            return daysFromLastOfMonth.Days < 7;
        }
        public static string TimeStampFormat(this DateTime dt)
        {
            return dt.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss:ff");
        }

        /// <summary>
        /// Return string formatted as YYYY-MM-DD HH:MM:SS:MMMMMM
        /// </summary>
        public static string ConvertToDbString(this DateTime value)
        {
            return value.ToString("yyyy-MM-dd HH:mm:ss.fff");
        }
        public static DateTime ConvertToEndDateTime(this DateTime value)
        {
            return Convert.ToDateTime(value.ToShortDateString() + " 23:59:59");
        }
        public static DateTime ConvertToMinimumDate(this DateTime value)
        {
            return new DateTime(1753, 1, 1, value.Hour, value.Minute, value.Second);
        }
        public static int MinutesIn24HourDay(this DateTime value)
        {
            return 24 * 60;
        }

        public static bool IsBetween(this DateTime value, DateTime left, DateTime right)
        {
            return (value > left && value < right) || (value < left && value > right);
        }
        #endregion

        #region Boolean
        public static string ToYNFlag(this bool value)
        {
            return (value ? "Y" : "N");
        }
        public static string ToYesNoFlag(this bool value)
        {
            return (value ? "Yes" : "No");
        }
        #endregion

        #region List<T>
        public static string ConvertToDeliminated<T>(this List<T> listCollection)
        {
            return listCollection.ConvertToDeliminated(",");
        }
        public static string ConvertToDeliminated<T>(this List<T> listCollection, string deliminator)
        {
            var resultStr = new StringBuilder();

            for (int i = 0; i < listCollection.Count; i++)
            {
                resultStr.Append(listCollection[i].ToString());

                if (i < listCollection.Count - 1)
                    resultStr.Append(deliminator);
            }

            return resultStr.ToString();
        }

        public static string[] ConvertToStringArray<T>(this List<T> listCollection)
        {
            var query = from item in listCollection
                        select item.ToString();

            return query.ToArray();
        }
        #endregion

        #region Object
        public static bool ToBoolean(this object value)
        {
            return ToBoolean(value.ToString());
        }
        public static string ToYNFlag(this object value)
        {
            bool boolValue = value.ToBoolean();
            return boolValue.ToYNFlag();
        }
        public static string ToYesNoFlag(this object value)
        {
            bool boolValue = value.ToBoolean();
            return boolValue.ToYesNoFlag();
        }
        public static T ConvertToEnum<T>(this object value)
        {
            T result = default(T);
            if (value != null)
            {
                object parsedValue = Enum.Parse(typeof(T), value.ToString(), true);
                if (Enum.IsDefined(typeof(T), parsedValue))
                    result = (T)parsedValue;
                else
                    throw new ArgumentException(String.Format("Requested value '{0}' was not found in {1}", value, typeof(T).FullName));
            }
            return result;
        }
        #endregion

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (action == null) throw new ArgumentNullException("action");

            foreach (T item in source)
            {
                action(item);
            }
        }

        public static string GetResourceValueForEnum(this Enum enumValue)
        {
            return Resources.EnumResources.ResourceManager.GetString(enumValue.GetType().Name + "_" + enumValue.ToString(), CultureInfo.CurrentCulture);
        }
    }
}