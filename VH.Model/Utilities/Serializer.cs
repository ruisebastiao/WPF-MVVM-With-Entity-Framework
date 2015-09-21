using System;
using System.Configuration;
using System.IO;
using System.Xml;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace VH.Model.Utilities
{
    public enum SerializationFormat
    {
        DataContract,
        JSON,
        XML
    }

    public static class Serializer
    {
        public static string SerializeToString(object instance)
        {
            return SerializeToString(instance, SerializationFormat.DataContract, instance.GetType());
        }
        public static string SerializeToString(object instance, Type type)
        {
            return SerializeToString(instance, SerializationFormat.DataContract, type);
        }
        public static string SerializeToString(object instance, SerializationFormat format)
        {
            return SerializeToString(instance, format, instance.GetType());
        }
        public static string SerializeToString(object instance, SerializationFormat format, Type type, params Type[] knownTypes)
        {
            string result = String.Empty;

            try
            {
                using (MemoryStream memoryStream = Serializer.SerializeToMemoryStream(instance, format, type, knownTypes))
                {
                    if (memoryStream.Length > 0)
                    {
                        memoryStream.Position = 0;
                        result = Encoding.ASCII.GetString(memoryStream.ToArray());
                    }
                }
            }
            catch (Exception ex)
            {
                string logStr = String.Format("Exception in SerializeToString [{0}]: {1}", instance.GetType().Name, ex.ToString());

                Debug.WriteLine(logStr);
                //Logger.Write(logStr, EnumResources.Exception);
            }

            return result;
        }

        public static MemoryStream SerializeToMemoryStream(object instance)
        {
            return SerializeToMemoryStream(instance, SerializationFormat.DataContract, instance.GetType());
        }
        public static MemoryStream SerializeToMemoryStream(object instance, Type type)
        {
            return SerializeToMemoryStream(instance, SerializationFormat.DataContract, type);
        }
        public static MemoryStream SerializeToMemoryStream(object instance, SerializationFormat format)
        {
            return SerializeToMemoryStream(instance, format, instance.GetType());
        }
        public static MemoryStream SerializeToMemoryStream(object instance, SerializationFormat format, Type type, params Type[] knownTypes)
        {
            MemoryStream outputMemoryStream = new MemoryStream();

            try
            {
                switch (format)
                {
                    case SerializationFormat.DataContract:
                        DataContractSerializer dataContractSerializer = new DataContractSerializer(type, knownTypes);
                        dataContractSerializer.WriteObject(outputMemoryStream, instance);
                        break;

                    case SerializationFormat.JSON:
                        DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(type, knownTypes);
                        dataContractJsonSerializer.WriteObject(outputMemoryStream, instance);
                        break;

                    case SerializationFormat.XML:
                        XmlSerializer xmlSerializer = new XmlSerializer(type, knownTypes);
                        var xmlTextWriter = new XmlTextWriter(outputMemoryStream, Encoding.ASCII);
                        xmlSerializer.Serialize(xmlTextWriter, instance);
                        break;
                }
            }
            catch (Exception ex)
            {
                string logStr = String.Format("Exception in SerializeToStream [{0}]: {1}", instance.GetType().Name, ex.ToString());

                Debug.WriteLine(logStr);
                //Logger.Write(logStr, EnumResources.Exception);
            }

            return outputMemoryStream;
        }

        public static bool SerializeToFile(object instance)
        {
            string fileName = instance.GetType().GetObjectName();
            return SerializeToFile(instance, fileName);
        }
        public static bool SerializeToFile(object instance, string fileName)
        {
            string outputDirectory = ConfigurationManager.AppSettings["UserDataPath"];
            return SerializeToFile(instance, fileName, outputDirectory);
        }
        public static bool SerializeToFile(object instance, string fileName, string outputDirectory)
        {
            return SerializeToFile(instance, fileName, outputDirectory, SerializationFormat.DataContract);
        }
        public static bool SerializeToFile(object instance, string fileName, string outputDirectory, SerializationFormat format, params Type[] knownTypes)
        {
            bool successful = false;

            try
            {
                #region Setup File
                string newFileName = AddFileExtension(fileName, format);
                string fullPath = Path.Combine(outputDirectory, newFileName);

                int maximumDeleteAttempts = 10;
                do
                {
                    maximumDeleteAttempts--;
                    try
                    {
                        if (!Directory.Exists(outputDirectory))
                            Directory.CreateDirectory(outputDirectory);

                        if (File.Exists(fullPath))
                            File.Delete(fullPath);

                        break;
                    }
                    catch (Exception error)
                    {
                        if (maximumDeleteAttempts == 0)
                            throw error;
                        Thread.Sleep(500);
                    }

                }
                while (true);
                #endregion

                using (FileStream outputFileStream = File.Open(fullPath, FileMode.CreateNew))
                {
                    switch (format)
                    {
                        case SerializationFormat.DataContract:
                            DataContractSerializer dataContractSerializer = new DataContractSerializer(instance.GetType(), knownTypes);
                            dataContractSerializer.WriteObject(outputFileStream, instance);
                            break;

                        case SerializationFormat.JSON:
                            DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(instance.GetType(), knownTypes);
                            dataContractJsonSerializer.WriteObject(outputFileStream, instance);
                            break;

                        case SerializationFormat.XML:
                            XmlSerializer xmlSerializer = new XmlSerializer(instance.GetType(), knownTypes);
                            var xmlTextWriter = new XmlTextWriter(outputFileStream, Encoding.ASCII);
                            xmlSerializer.Serialize(xmlTextWriter, instance);
                            break;
                    }

                    outputFileStream.Flush();
                    outputFileStream.Close();
                    successful = true;
                }
            }
            catch (Exception ex)
            {
                string logStr = String.Format("Exception in SerializeToFile [{0}]: {1}", instance.GetType().Name, ex.ToString());

                Debug.WriteLine(logStr);
                //Logger.Write(logStr, EnumResources.Exception);
            }

            return successful;
        }

        public static object DeserializeFromString(string stringContent, Type type)
        {
            return DeserializeFromString(stringContent, type, SerializationFormat.DataContract);
        }
        public static object DeserializeFromString(string stringContent, Type type, SerializationFormat format, params Type[] knownTypes)
        {
            object returnObject = null;

            try
            {
                byte[] bytes = Encoding.ASCII.GetBytes(stringContent);
                using (MemoryStream inputMemoryStream = new MemoryStream(bytes))
                {
                    returnObject = Serializer.DeserializeFromStream(inputMemoryStream, type, format, knownTypes);
                }
            }
            catch (Exception ex)
            {
                string logStr = String.Format("Exception in DeserializeFromString [{0}]: {1}", type.Name, ex.ToString());

                Debug.WriteLine(logStr);
                //Logger.Write(logStr, EnumResources.Exception);
            }

            return returnObject;
        }

        public static object DeserializeFromStream(Stream inputStream, Type type)
        {
            return DeserializeFromStream(inputStream, type, SerializationFormat.DataContract);
        }
        public static object DeserializeFromStream(Stream inputStream, Type type, SerializationFormat format, params Type[] knownTypes)
        {
            object returnObject = null;

            try
            {
                if (inputStream.Length >= 0)
                    inputStream.Position = 0;

                switch (format)
                {
                    case SerializationFormat.DataContract:
                        DataContractSerializer dataContractSerializer = new DataContractSerializer(type, knownTypes);
                        returnObject = dataContractSerializer.ReadObject(inputStream);
                        break;

                    case SerializationFormat.JSON:
                        DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(type, knownTypes);
                        returnObject = dataContractJsonSerializer.ReadObject(inputStream);
                        break;

                    case SerializationFormat.XML:
                        XmlSerializer xmlSerializer = new XmlSerializer(type, knownTypes);
                        inputStream.Position = 0;
                        returnObject = xmlSerializer.Deserialize(inputStream);
                        break;
                }

                if (returnObject is IDeserializationDefault)
                    ((IDeserializationDefault)returnObject).SetDeserializationDefault();
            }
            catch (Exception ex)
            {
                string logStr = String.Format("Exception in DeserializeFromStream [{0}]: {1}", type.Name, ex.ToString());

                Debug.WriteLine(logStr);
                //Logger.Write(logStr, EnumResources.Exception);
            }

            return returnObject;
        }

        public static T DeserializeFromString<T>(string stringContent)
        {
            return DeserializeFromString<T>(stringContent, SerializationFormat.DataContract);
        }
        public static T DeserializeFromString<T>(string stringContent, SerializationFormat format, params Type[] knownTypes)
        {
            T returnObject = default(T);

            try
            {
                byte[] bytes = Encoding.ASCII.GetBytes(stringContent);
                using (MemoryStream inputMemoryStream = new MemoryStream(bytes))
                {
                    returnObject = Serializer.DeserializeFromStream<T>(inputMemoryStream, format, knownTypes);
                }
            }
            catch (Exception ex)
            {
                string logStr = String.Format("Exception in DeserializeFromString<{0}>: {1}", typeof(T).Name, ex.ToString());

                Debug.WriteLine(logStr);
                //Logger.Write(logStr, EnumResources.Exception);
            }

            return returnObject;
        }

        public static T DeserializeFromStream<T>(Stream inputStream)
        {
            return DeserializeFromStream<T>(inputStream, SerializationFormat.DataContract);
        }
        public static T DeserializeFromStream<T>(Stream inputStream, SerializationFormat format, params Type[] knownTypes)
        {
            T returnObject = default(T);

            try
            {
                if (inputStream.Length >= 0)
                    inputStream.Position = 0;

                switch (format)
                {
                    case SerializationFormat.DataContract:
                        DataContractSerializer dataContractSerializer = new DataContractSerializer(typeof(T), knownTypes);
                        returnObject = (T)dataContractSerializer.ReadObject(inputStream);
                        break;

                    case SerializationFormat.JSON:
                        DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(T), knownTypes);
                        returnObject = (T)dataContractJsonSerializer.ReadObject(inputStream);
                        break;

                    case SerializationFormat.XML:
                        XmlSerializer xmlSerializer = new XmlSerializer(typeof(T), knownTypes);
                        inputStream.Position = 0;
                        returnObject = (T)xmlSerializer.Deserialize(inputStream);
                        break;
                }

                if (returnObject is IDeserializationDefault)
                    ((IDeserializationDefault)returnObject).SetDeserializationDefault();
            }
            catch (Exception ex)
            {
                string logStr = String.Format("Exception in DeserializeFromStream<{0}>: {1}", typeof(T).Name, ex.ToString());

                Debug.WriteLine(logStr);
                //Logger.Write(logStr, EnumResources.Exception);
            }

            return returnObject;
        }

        public static T DeserializeFromFile<T>()
        {
            string fileName = typeof(T).GetObjectName();
            return DeserializeFromFile<T>(fileName);
        }
        public static T DeserializeFromFile<T>(string fileName)
        {
            string inputDirectory = ConfigurationManager.AppSettings["UserDataPath"];
            return DeserializeFromFile<T>(fileName, inputDirectory);
        }
        public static T DeserializeFromFile<T>(string fileName, string inputDirectory)
        {
            return DeserializeFromFile<T>(fileName, inputDirectory, SerializationFormat.DataContract);
        }
        public static T DeserializeFromFile<T>(string fileName, string inputDirectory, SerializationFormat format, params Type[] knownTypes)
        {
            T returnObject = default(T);

            try
            {
                string newFileName = AddFileExtension(fileName, format);
                string fullPath = Path.Combine(inputDirectory, newFileName);

                if (File.Exists(fullPath))
                {
                    using (FileStream inputFileStream = File.Open(fullPath, FileMode.Open, FileAccess.Read))
                    {
                        switch (format)
                        {
                            case SerializationFormat.DataContract:
                                DataContractSerializer dataContractSerializer = new DataContractSerializer(typeof(T), knownTypes);
                                returnObject = (T)dataContractSerializer.ReadObject(inputFileStream);
                                break;

                            case SerializationFormat.JSON:
                                DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(T), knownTypes);
                                returnObject = (T)dataContractJsonSerializer.ReadObject(inputFileStream);
                                break;

                            case SerializationFormat.XML:
                                XmlSerializer xmlSerializer = new XmlSerializer(typeof(T), knownTypes);
                                inputFileStream.Position = 0;
                                returnObject = (T)xmlSerializer.Deserialize(inputFileStream);
                                break;
                        }
                    }
                }

                if (returnObject is IDeserializationDefault)
                    ((IDeserializationDefault)returnObject).SetDeserializationDefault();
            }
            catch (Exception ex)
            {
                string logStr = String.Format("Exception in DeserializeFromFile<{0}>: {1}", typeof(T).Name, ex.ToString());

                Debug.WriteLine(logStr);
                //Logger.Write(logStr, EnumResources.Exception);
            }

            return returnObject;
        }

        private static string AddFileExtension(string fileName, SerializationFormat format)
        {
            string newFileName = fileName;
            string fileExt = String.Empty;

            switch (format)
            {
                case SerializationFormat.DataContract:
                    {
                        fileExt = ".xml";

                        if (!fileName.EndsWith(fileExt, StringComparison.CurrentCultureIgnoreCase))
                            newFileName = String.Format("{0}{1}", fileName, fileExt);

                        break;
                    }

                case SerializationFormat.JSON:
                    {
                        fileExt = ".xml";

                        if (!fileName.EndsWith(fileExt, StringComparison.CurrentCultureIgnoreCase))
                            newFileName = String.Format("{0}{1}", fileName, fileExt);

                        break;
                    }

                case SerializationFormat.XML:
                    {
                        fileExt = ".xml";

                        if (!fileName.EndsWith(fileExt, StringComparison.CurrentCultureIgnoreCase))
                            newFileName = String.Format("{0}{1}", fileName, fileExt);

                        break;
                    }
            }

            return newFileName;
        }
    }
}