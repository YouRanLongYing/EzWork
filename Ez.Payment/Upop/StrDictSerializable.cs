using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml.Schema;

namespace Ez.Payment.Upop
{
    /// <summary>
    /// 可序列化的Dictionary(Of String,String)
    /// </summary>
    /// <remarks></remarks>
    public class StrDictSerializable : Dictionary<string, string>, IXmlSerializable
    {
        public System.Xml.Schema.XmlSchema Getschema()
        {
            throw new NotImplementedException();
        }
        XmlSchema IXmlSerializable.GetSchema()
        {
            return Getschema();
        }
        public void ReadXml(System.Xml.XmlReader reader)
        {
            if (reader.IsEmptyElement)
            {
                return;
            }

            reader.Read();
            try
            {
                while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
                {
                    if (reader.NodeType == System.Xml.XmlNodeType.Comment)
                    {
                        reader.Skip();
                        continue;
                    }

                    string k = reader.Name;
                    if (reader.IsEmptyElement)
                    {
                        this[k] = "";
                        reader.Read();
                    }
                    else
                    {
                        this[k] = reader.ReadElementString();
                    }
                }
                reader.Read();
            }
            catch (Exception ex)
            {
                //Interaction.MsgBox(ex.Message);
            }
        }
        public void WriteXml(System.Xml.XmlWriter writer)
        {
            foreach (string k_loopVariable in Keys)
            {
                var k = k_loopVariable;
                writer.WriteElementString(k, this[k]);
            }
        }
    }
}
