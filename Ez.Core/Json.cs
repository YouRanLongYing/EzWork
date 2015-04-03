using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Ez.Core.Attributes;
using System.Collections;

namespace Ez.Core
{
    /// <summary>
    /// 解决常见类型到Json的转换
    /// </summary>
    [Serializable]
    public class Json
    {
        public override string ToString()
        {
            IList<string> resultcoll = new List<string>();
            IList<string> jsonList = new List<string>();
            PropertyInfo[] propers = this.GetType().GetProperties();
            foreach (var proper in propers)
            {
                object[] objs = proper.GetCustomAttributes(typeof(JsonItemAttribute), false);
                if (objs == null || objs.Length == 0) continue;
                JsonItemAttribute jsonitem = objs[0] as JsonItemAttribute;
                if (jsonitem == null) continue;
                var obj = proper.GetValue(this, null);
                var type = obj.GetType();
                switch (Type.GetTypeCode(type))
                {
                    case TypeCode.Decimal:
                    case TypeCode.Int32:
                        if (type.BaseType.Name == "Enum")
                        {
                            jsonList.Add(string.Format("\"{0}\":{1}", jsonitem.Key, obj.GetHashCode()));
                        }
                        else
                        {
                            jsonList.Add(string.Format("\"{0}\":{1}", jsonitem.Key, obj));
                        }

                        break;
                    case TypeCode.String: jsonList.Add(string.Format("\"{0}\":\"{1}\"", jsonitem.Key, obj)); break;
                    case TypeCode.Object:
                        if (type.Name == "List`1")
                        {
                            IList<Object> inneritems = new List<Object>();
                            IList list = obj as IList;
                            foreach (var item in list)
                            {
                                inneritems.Add(item);
                            }
                            jsonList.Add(string.Format("\"{0}\":[\r\n{1}\r\n]", jsonitem.Key, string.Join(",\r\n", inneritems.ToArray())));
                        }
                        else
                        {
                            jsonList.Add(string.Format("\"{0}\":{1}", jsonitem.Key, obj));
                        }
                        break;
                }
            }
            return "{" + string.Join(",\r\n", jsonList.ToArray()) + "}";
        }
    }
}
