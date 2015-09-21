using System;
using System.IO;
using System.Windows;
using System.Xml;
using System.Xml.Serialization;

namespace Widget.Base
{
    public class XmlSerializable
    {
        public virtual void Save(string path)
        {
            //var w = new StreamWriter(path);
            if (!Directory.Exists(Path.GetDirectoryName(path)))
                return;
            var w = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Read);
            var s = new XmlSerializer(this.GetType());
            s.Serialize(w, this);
            w.Close();
        }

        public static object Load(Type t, string path)
        {
            if (File.Exists(path))
            {
                var sr = new StreamReader(path);
                var xr = new XmlTextReader(sr);
                var xs = new XmlSerializer(t);
                object result = null;
                try
                {
                    result = xs.Deserialize(xr);
                }
                catch (Exception ex)
                {
                    //logger.Error("Can't read xml file: " + path + ". Deserialization failed.\n" + ex);
                    MessageBox.Show("Can't read xml file: " + path + ". See details in log.");
                }
                xr.Close();
                sr.Close();
                return result;
            }
            return null;
        }
        public static object Load(Type t, Stream sr)
        {
           // if (File.Exists(path))
            {
                //var sr = new StreamReader(path);
                var xr = new XmlTextReader(sr);
                var xs = new XmlSerializer(t);
                object result = null;
                try
                {
                    result = xs.Deserialize(xr);
                }
                catch (Exception ex)
                {
                    //logger.Error("Can't read xml file: " + path + ". Deserialization failed.\n" + ex);
                   // MessageBox.Show("Can't read xml file: " + path + ". See details in log.");
                }
                xr.Close();
                sr.Close();
                return result;
            }
            return null;
        }
    } 
    }
