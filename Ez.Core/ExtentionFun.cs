using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
namespace Ez.Core
{
    public static class ExtentionFun
    {
        /// <summary>
        /// 将类型为int的Object转化为Int类型返回
        /// </summary>
        public static int ToSafeInt(this object obj,int defaultvalue=0,bool checkZero=false)
        {
            int cvtvalue=defaultvalue;
            if (obj != null)
            {
                int.TryParse(obj.ToString(), out cvtvalue);
                cvtvalue = checkZero && cvtvalue == 0 ? defaultvalue : cvtvalue;
            }
            return cvtvalue;
        }
        public static string ToSafeString(this object obj,bool isdateObj,string format="", string defaultvalue="")
        {
            string cvtvalue = defaultvalue;
            if (obj != null)
            {
                if (isdateObj)
                {
                    cvtvalue = ToSafeDateTime(obj).ToString(format);
                }
                else
                {
                    cvtvalue = obj.ToString();
                }
            }
            return cvtvalue;
        }
        public static string ToSafeString(this object obj,string defaultvalue = "")
        {
            return ToSafeString(obj, false, null, defaultvalue);
        }
        public static DateTime ToSafeDateTime(this object obj)
        {
            DateTime tmp;
            DateTime bi = DateTime.Parse("1970-01-01");
            if (DateTime.TryParse(obj.ToString(), out tmp) && tmp >= bi)
            {
                return tmp;
            }
            else
            {
                return bi;
            }
        }
        public static T InjectEntity<T>(this NameValueCollection nvc) where T:new()
        {

            T t = new T();
            PropertyInfo[] pinfos = t.GetType().GetProperties();
            foreach (string key in nvc.Keys)
            {
                var value = nvc[key];
                PropertyInfo propertyinfo =  pinfos.FirstOrDefault(p => p.Name.Equals(key));
                if (propertyinfo != null)
                {
                    TypeCode tcode = Type.GetTypeCode(propertyinfo.PropertyType);
   
                    if ((tcode == TypeCode.Int16 || tcode == TypeCode.Int32 || tcode == TypeCode.Int64) && Regex.IsMatch(value, @"^\d+$"))
                    {
                        int _value = 0;
                        if (int.TryParse(value, out _value))
                        {
                            propertyinfo.SetValue(t, _value, null);
                        }
                    }
                    else if ((tcode == TypeCode.Decimal) && Regex.IsMatch(value, @"^\d+\.\d+$]"))
                    {
                        double _value = 0;
                        if (double.TryParse(value, out _value))
                        {
                            propertyinfo.SetValue(t, _value, null);
                        }
                        else
                        {
                            decimal __value = 0;
                            if (decimal.TryParse(value, out __value))
                            {
                                propertyinfo.SetValue(t, __value, null);
                            }
                        }
                        
                    }
                    else if (tcode == TypeCode.String)
                    {
                            propertyinfo.SetValue(t, value, null);
                    }
                }
            }


            return t;
        }

        public static T TranslatorTo<T,S>(this S s) where T:new()
        {
           T t = new T();
           PropertyInfo[] target_pinfos = t.GetType().GetProperties();
           PropertyInfo[] source_pinfos = s.GetType().GetProperties();
           foreach (var property in target_pinfos)
           {
              PropertyInfo source_property = source_pinfos.FirstOrDefault(p => p.Name.ToLower().Equals(property.Name.ToLower()));
              if (source_property.PropertyType == property.PropertyType)
              {
                  var value = source_property.GetValue(s, null);
                  property.SetValue(t, value, null);
              }
           }
          return t;
        }
    }
}
