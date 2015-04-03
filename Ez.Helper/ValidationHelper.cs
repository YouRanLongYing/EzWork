using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Ez.Helper
{
    public class ValidationHelper
    {
        #region 检测对象是否为空 ->RequiredAttribute

        #region 重载1
        /// <summary>
        /// 检测对象是否为空，为空返回true ->RequiredAttribute
        /// </summary>
        /// <typeparam name="T">要验证的对象的类型</typeparam>
        /// <param name="data">要验证的对象</param>        
        public static bool IsNullOrEmpty<T>(T data)
        {
            //如果为null
            if (data == null)
            {
                return true;
            }

            //如果为""
            if (data.GetType() == typeof(String))
            {
                if (string.IsNullOrEmpty(data.ToString().Trim()))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            //如果为DBNull
            if (data.GetType() == typeof(DBNull))
            {
                return true;
            }

            //不为空
            return false;
        }
        #endregion

        #region 重载2
        /// <summary>
        /// 检测对象是否为空，为空返回true ->Required
        /// </summary>
        /// <param name="data">要验证的对象</param>
        public static bool IsNullOrEmpty(object data)
        {
            return IsNullOrEmpty<object>(data);
        }
        #endregion

        #region 重载3
        /// <summary>
        /// 检测字符串是否为空，为空返回true ->Required
        /// </summary>
        /// <param name="text">要检测的字符串</param>
        public static bool IsNullOrEmpty(string text)
        {
            //检测是否为null
            if (text == null)
            {
                return true;
            }

            //检测字符串空值
            if (string.IsNullOrEmpty(text.ToString().Trim()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #endregion

        #region 验证IP地址是否合法 ->IPAttribute
        /// <summary>
        /// 验证IP地址是否合法->IPAttribute
        /// </summary>
        /// <param name="ip">要验证的IP地址</param>        
        public static bool IsIP(string ip)
        {
            //如果为空，认为验证合格
            if (IsNullOrEmpty(ip))
            {
                return true;
            }

            //清除要验证字符串中的空格
            ip = ip.Trim();

            //模式字符串
            string pattern = @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$";

            //验证
            return IsMatch(ip, pattern);
        }
        #endregion

        #region  验证EMail是否合法 ->EmailAttribute
        /// <summary>
        /// 验证EMail是否合法 ->EmailAttribute
        /// </summary>
        /// <param name="email">要验证的Email</param>
        public static bool IsEmail(string email)
        {
            //如果为空，认为验证不合格
            if (IsNullOrEmpty(email))
            {
                return false;
            }

            //清除要验证字符串中的空格
            email = email.Trim();

            //模式字符串
            string pattern = @"^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$";

            //验证
            return IsMatch(email, pattern);
        }
        #endregion

        #region 验证是否为整数 ->InputTypeAttribute
        /// <summary>
        /// 验证是否为整数  ->InputTypeAttribute
        /// </summary>
        /// <param name="number">要验证的整数</param>        
        public static bool IsInt(string number)
        {
            //如果为空，认为验证不合格
            if (IsNullOrEmpty(number))
            {
                return false;
            }

            //清除要验证字符串中的空格
            number = number.Trim();

            //模式字符串
            string pattern = @"^[1-9]+[0-9]*$";

            //验证
            return IsMatch(number, pattern);
        }
        #endregion

        #region 验证是否为数字  ->InputTypeAttribute
        /// <summary>
        /// 验证是否为数字(为空为验证不合格)  ->InputTypeAttribute
        /// </summary>
        /// <param name="number">要验证的数字</param>        
        public static bool IsNumber(string number)
        {
            //如果为空，认为验证不合格
            if (IsNullOrEmpty(number))
            {
                return false;
            }

            //清除要验证字符串中的空格
            number = number.Trim();

            //模式字符串
            string pattern = @"^[1-9]+[0-9]*[.]?[0-9]*$";

            //验证
            return IsMatch(number, pattern);
        }
        /// <summary>
        /// 验证是否为数字(为空为验证不合格)
        /// </summary>
        /// <param name="number">要验证的数字</param>        
        public static bool IsNumberNotNull(string number)
        {
            //如果为空，认为不合格
            if (IsNullOrEmpty(number))
            {
                return false;
            }

            //清除要验证字符串中的空格
            number = number.Trim();

            //模式字符串
            string pattern = @"^[1-9]+[0-9]*[.]?[0-9]*$";

            //验证
            return IsMatch(number, pattern);
        }
        #endregion

        #region 验证日期是否合法
        public static bool IsDate(string date)
        {
            //如果为空，认为验证不合格
            DateTime time;
            if (DateTime.TryParse(date, out time))
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 验证日期是否合法,对不规则的作了简单处理
        /// </summary>
        /// <param name="date">日期</param>
        public static bool IsDate(ref string date)
        {
            //如果为空，认为验证合格
            if (IsNullOrEmpty(date))
            {
                return true;
            }

            //清除要验证字符串中的空格
            date = date.Trim();

            //替换\
            date = date.Replace(@"\", "-");
            //替换/
            date = date.Replace(@"/", "-");

            //如果查找到汉字"今",则认为是当前日期
            if (date.IndexOf("今") != -1)
            {
                date = DateTime.Now.ToString();
            }

            try
            {
                //用转换测试是否为规则的日期字符
                date = Convert.ToDateTime(date).ToString("d");
                return true;
            }
            catch
            {
                //如果日期字符串中存在非数字，则返回false
                if (!IsInt(date))
                {
                    return false;
                }

                #region 对纯数字进行解析
                //对8位纯数字进行解析
                if (date.Length == 8)
                {
                    //获取年月日
                    string year = date.Substring(0, 4);
                    string month = date.Substring(4, 2);
                    string day = date.Substring(6, 2);

                    //验证合法性
                    if (Convert.ToInt32(year) < 1900 || Convert.ToInt32(year) > 2100)
                    {
                        return false;
                    }
                    if (Convert.ToInt32(month) > 12 || Convert.ToInt32(day) > 31)
                    {
                        return false;
                    }

                    //拼接日期
                    date = Convert.ToDateTime(year + "-" + month + "-" + day).ToString("d");
                    return true;
                }

                //对6位纯数字进行解析
                if (date.Length == 6)
                {
                    //获取年月
                    string year = date.Substring(0, 4);
                    string month = date.Substring(4, 2);

                    //验证合法性
                    if (Convert.ToInt32(year) < 1900 || Convert.ToInt32(year) > 2100)
                    {
                        return false;
                    }
                    if (Convert.ToInt32(month) > 12)
                    {
                        return false;
                    }

                    //拼接日期
                    date = Convert.ToDateTime(year + "-" + month).ToString("d");
                    return true;
                }

                //对5位纯数字进行解析
                if (date.Length == 5)
                {
                    //获取年月
                    string year = date.Substring(0, 4);
                    string month = date.Substring(4, 1);

                    //验证合法性
                    if (Convert.ToInt32(year) < 1900 || Convert.ToInt32(year) > 2100)
                    {
                        return false;
                    }

                    //拼接日期
                    date = year + "-" + month;
                    return true;
                }

                //对4位纯数字进行解析
                if (date.Length == 4)
                {
                    //获取年
                    string year = date.Substring(0, 4);

                    //验证合法性
                    if (Convert.ToInt32(year) < 1900 || Convert.ToInt32(year) > 2100)
                    {
                        return false;
                    }

                    //拼接日期
                    date = Convert.ToDateTime(year).ToString("d");
                    return true;
                }
                #endregion

                return false;
            }
        }
        #endregion

        #region 验证身份证是否合法 -> idcard
        /// <summary>
        /// 拆分身份证
        /// </summary>
        /// <param name="Id">身份证号</param>
        /// <returns></returns>
        private static List<string> splitID(string Id)
        {
            List<string> rtn = new List<string>();
            rtn.Add(Id.Substring(0, 6));
            if (Id.Length == 15)
            {
                //年
                rtn.Add(Id.Substring(6, 2));
                rtn.Add(Id.Substring(8, 2));
                rtn.Add(Id.Substring(10, 2));
                rtn.Add(Id.Substring(12, 3));
            }
            else if (Id.Length == 18)
            {
                rtn.Add(Id.Substring(6, 4));
                rtn.Add(Id.Substring(10, 2));
                rtn.Add(Id.Substring(12, 2));
                rtn.Add(Id.Substring(14, 3));
                rtn.Add(Id.Substring(17, 1));
            }
            return rtn;
        }
        /// <summary>
        /// 验证身份证是否合法 -> idcard
        /// </summary>
        /// <param name="idCard">身份证号</param>
        /// <param name="checkEmpty">是否检测空值</param>
        /// <returns>true成功，false失败</returns>
        public static bool IsIdCard(string idCard,bool checkEmpty=false)
        {
            if (!checkEmpty && string.IsNullOrEmpty(idCard))
            {
                return true;
            }

            string num = idCard.ToUpper();
            //身份证号码为15位或者18位，15位时全为数字，18位前17位为数字，最后一位是校验位，可能为数字或字符X。   
            if (!(Regex.Match(num, @"^\d{17}([0-9]|X)$", RegexOptions.IgnoreCase).Success))//if (!(Regex.Match(num, @"(^\d{15}$)|(^\d{17}([0-9]|X)$)", RegexOptions.IgnoreCase).Success))
            {
                //MessageBox.Show("输入的身份证号长度不对，或者号码不符合规定！\n15位号码应全为数字，18位号码末位可以为数字或X。");
                return false;
            }
            //校验位按照ISO 7064:1983.MOD 11-2的规定生成，X可以认为是数字10。 
            //下面分别分析出生日期和校验位 
            int len;//, re;
            len = num.Length;
            var arrInt = new int[] { 7, 9, 10, 5, 8, 4, 2, 1, 6, 3, 7, 9, 10, 5, 8, 4, 2 };
            var arrCh = new string[] { "1", "0", "X", "9", "8", "7", "6", "5", "4", "3", "2" };
            //if (len == 15)
            //{
            //    List<string> arrSplit = splitID(idCard);

            //    //检查生日日期是否正确 
            //    DateTime dtmBirth = DateTime.Parse("19" + arrSplit[2].ToString() + "/" + arrSplit[3].ToString() + "/" + arrSplit[4].ToString());

            //    if (dtmBirth == null)
            //    {
            //        //MessageBox.Show("输入的身份证号里出生日期不对！");
            //        return false;
            //    }
            //    else
            //    {
            //        //将15位身份证转成18位 
            //        //校验位按照ISO 7064:1983.MOD 11-2的规定生成，X可以认为是数字10。 

            //        var nTemp = 0;
            //        num = num.Substring(0, 6) + "19" + num.Substring(6, num.Length - 6);
            //        for (int i = 0; i < 17; i++)
            //        {
            //            nTemp += int.Parse(num.Substring(i, 1)) * arrInt[i];
            //        }
            //        num += arrCh[nTemp % 11];
            //        return true;
            //    }
            //}
            if (len == 18)
            {
                List<string> arrSplit = splitID(idCard);

                //检查生日日期是否正确 
                DateTime brDay;
                if (!DateTime.TryParse(arrSplit[2].ToString() + "/" + arrSplit[3].ToString() + "/" + arrSplit[1].ToString(), out brDay))
                {
                    //MessageBox.Show("输入的身份证号里出生日期不对！");
                    return false;
                }
                else
                {
                    //检验18位身份证的校验码是否正确。 
                    //校验位按照ISO 7064:1983.MOD 11-2的规定生成，X可以认为是数字10。 
                    int nTemp = 0;
                    for (int i = 0; i < 17; i++)
                    {
                        nTemp += int.Parse(num.Substring(i, 1)) * arrInt[i];
                    }
                    string valnum = arrCh[nTemp % 11];
                    if (valnum != num.Substring(17, 1))
                    {
                        //MessageBox.Show("18位身份证的校验码不正确！应该为：" + valnum);
                        return false;
                    }
                    return true;
                }
            }
            return false;
        }
        #endregion

        #region 检测客户的输入中是否有危险字符串,检测客户输入的字符串是否有效,并将原始字符串修改为有效字符串或空字符串
        /// <summary>
        /// 检测客户输入的字符串是否有效,并将原始字符串修改为有效字符串或空字符串。
        /// 当检测到客户的输入中有攻击性危险字符串,则返回false,有效返回true。
        /// </summary>
        /// <param name="input">要检测的字符串</param>
        public static bool IsValidInput(ref string input)
        {
            #region
            try
            {
                if (IsNullOrEmpty(input))
                {
                    //如果是空值,则跳出
                    return true;
                }
                else
                {
                    //替换单引号
                    input = input.Replace("'", "''").Trim();

                    //检测攻击性危险字符串
                    string testString = "and |or |exec |insert |select |delete |update |count |chr |mid |master |truncate |char |declare ";
                    string[] testArray = testString.Split('|');
                    foreach (string testStr in testArray)
                    {
                        if (input.ToLower().IndexOf(testStr) != -1)
                        {
                            //检测到攻击字符串,清空传入的值
                            input = string.Empty;
                            return false;
                        }
                    }

                    //未检测到攻击字符串
                    return true;
                }
            }
            catch
            {
                return false;
            }
            #endregion
        }
        #endregion

        #region 验证输入字符串是否与模式字符串匹配,验证输入字符串是否与模式字符串匹配，匹配返回true
        /// <summary>
        /// 验证输入字符串是否与模式字符串匹配，匹配返回true
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <param name="pattern">模式字符串</param>        
        public static bool IsMatch(string input, string pattern)
        {
            return IsMatch(input, pattern, RegexOptions.IgnoreCase);
        }
        #endregion

        #region 验证输入字符串是否与模式字符串匹配,验证输入字符串是否与模式字符串匹配，匹配返回true
        /// <summary>
        /// 验证输入字符串是否与模式字符串匹配，匹配返回true
        /// </summary>
        /// <param name="input">输入的字符串</param>
        /// <param name="pattern">模式字符串</param>
        /// <param name="options">筛选条件,比如是否忽略大小写</param>
        public static bool IsMatch(string input, string pattern, RegexOptions options)
        {
            return Regex.IsMatch(input, pattern, options);
        }
        #endregion

        #region 检测是否有中文字符

        /// <summary>
        /// 检测是否有中文字符
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public static bool IsHasCHZN(string inputData)
        {
            Match m = new Regex("[\u4e00-\u9fa5]").Match(inputData);
            return m.Success;
        }

        #endregion

        #region 检测是否符合电话格式 ->PhoneNumberAttribute
        /// <summary>
        /// 检测是否符合电话格式(为空返回false) ->PhoneNumberAttribute
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public static bool IsPhoneNumber(string phoneNumber)
        {
            return Regex.IsMatch(phoneNumber, @"(^[0-9]{3,4}\-[0-9]{3,8}$)|(^[0-9]{3,8}$)|(^\([0-9]{3,4}\)[0-9]{3,8}$)");
        }
        /// <summary>
        /// 检测是否符合电话格式(为空返回true)
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public static bool IsPhoneNumberNotNull(string phoneNumber)
        {
            #region
            if (phoneNumber.Length <= 0)
            {
                return true;
            }
            else
            {
                //return Regex.IsMatch(phoneNumber, @"(^[0-9]{3,4}\-[0-9]{3,8}$)|(^[0-9]{3,8}$)|(^\([0-9]{3,4}\)[0-9]{3,8}$)");
                return Regex.IsMatch(phoneNumber, @"([1-9][0-9]*|0)(\-[0-9]+)?");
            }
            #endregion
        }
        #endregion

        #region 检测是否手机号码格式 ->MobileNumberAttribute
        /// <summary>
        /// 检测是否手机号码格式(为空返回true)  ->MobileNumberAttribute
        /// </summary>
        /// <param name="phoneNumber">手机号</param>
        /// <returns></returns>
        public static bool IsMobileNumber(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber)) return true;
            return Regex.IsMatch(phoneNumber, "^1[3,5,8][0-9]{1}[0-9]{8}$");
        }
        #endregion

        #region 检测是否符合url格式,前面必需含有http://  ->UrlAttribute
        /// <summary>
        /// 检测是否符合url格式,前面必需含有http://  ->UrlAttribute
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool IsURL(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return false;
            }
            return Regex.IsMatch(url, @"^http(s):\/\/([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?$");
        }
        #endregion

        #region 检测是否符合时间格式
        /// <summary>
        /// 检测是否符合时间格式
        /// </summary>
        /// <returns></returns>
        public static bool IsTime(string timeval)
        {
            return Regex.IsMatch(timeval, @"((^((1[8-9]\d{2})|([2-9]\d{3}))([-\/\._])(10|12|0?[13578])([-\/\._])(3[01]|[12][0-9]|0?[1-9])$)|(^((1[8-9]\d{2})|([2-9]\d{3}))([-\/\._])(11|0?[469])([-\/\._])(30|[12][0-9]|0?[1-9])$)|(^((1[8-9]\d{2})|([2-9]\d{3}))([-\/\._])(0?2)([-\/\._])(2[0-8]|1[0-9]|0?[1-9])$)|(^([2468][048]00)([-\/\._])(0?2)([-\/\._])(29)$)|(^([3579][26]00)([-\/\._])(0?2)([-\/\._])(29)$)|(^([1][89][0][48])([-\/\._])(0?2)([-\/\._])(29)$)|(^([2-9][0-9][0][48])([-\/\._])(0?2)([-\/\._])(29)$)|(^([1][89][2468][048])([-\/\._])(0?2)([-\/\._])(29)$)|(^([2-9][0-9][2468][048])([-\/\._])(0?2)([-\/\._])(29)$)|(^([1][89][13579][26])([-\/\._])(0?2)([-\/\._])(29)$)|(^([2-9][0-9][13579][26])([-\/\._])(0?2)([-\/\._])(29)$))");
        }

        #endregion

        #region 检测是否符合邮编格式 ->PostCodeAttribute
        /// <summary>
        /// 检测是否符合邮编格式->PostCodeAttribute
        /// </summary>
        /// <param name="postCode"></param>
        /// <returns></returns>
        public static bool IsPostCode(string postCode)
        {
            return Regex.IsMatch(postCode, @"^\d{6}$");
        }
        #endregion

        #region 验证是否为汉字,拼音数字
        /// <summary>
        ///  验证是否为汉字,拼音数字
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsNTS(string input)
        {
            return Regex.IsMatch(input, "^[a-zA-Z0-9\\u4e00-\\u9fa5]+$");
        }
        #endregion

        #region 看字符串的长度是不是在限定数之间 一个中文为两个字符 ->RangeLenAttribute
        /// <summary>
        /// 看字符串的长度是不是在限定数之间 一个中文为两个字符->RangeLenAttribute
        /// </summary>
        /// <param name="source">字符串</param>
        /// <param name="begin">大于等于</param>
        /// <param name="end">小于等于</param>
        /// <returns></returns>
        public static bool IsLengthStr(string source, int begin, int end, bool eqmin = true, bool eqmax = true)
        {
            if (string.IsNullOrEmpty(source)) return false;
            int length = source.Length;
            if (eqmin && eqmax)
            {
                return (length <= begin) || (length >= end);
            }
            else if (!eqmin && eqmax)
            {
                return (length < begin) || (length >= end);
            }
            else if (eqmin && !eqmax)
            {
                return (length <= begin) || (length > end);
            }
            else
            {
                return (length < begin) || (length > end);
            }
        }
        #endregion

        #region 检测字符串长度是否小于指定长度 ->LessThanLenAttribute
        /// <summary>
        /// 检测字符串长度是否小于等于指定长度 ->LessThanLenAttribute
        /// </summary>
        /// <param name="source">要检测的字符串</param>
        /// <param name="maxLength">最大长度</param>
        /// <returns></returns>
        public static bool IsLessThanStr(string source, int maxLength, bool caneq = true)
        {
            #region
            if (source != null)
            {
                int temp = source.Length;
                if (caneq)
                {
                    return temp <= maxLength;
                }
                else
                {
                    return temp < maxLength;
                }
            }
            else
            {
                return true;
            }
            #endregion
        }
        #endregion

        #region 检测字符串长度是否大于等于指定长度
        /// <summary>
        /// 检测字符串长度是否大于等于指定长度
        /// </summary>
        /// <param name="source">要检测的字符串</param>
        /// <param name="lessLength">最小长度</param>
        /// <returns></returns>
        public static bool IsGreaterThanStr(string source, int lessLength, bool caneq = true)
        {
            #region
            int len = source.Length;
            if (caneq)
            {
                return len >= lessLength;
            }
            else
            {
                return len > lessLength;
            }
            #endregion
        }
        #endregion


        #region 验证是不是正常字符 字母，数字，下划线的组合
        /// <summary>
        /// 验证是不是正常字符 字母，数字，下划线的组合
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsNormalChar(string source)
        {
            return Regex.IsMatch(source, @"[\w\d_]+", RegexOptions.IgnoreCase);
        }
        #endregion

        #region 移除中文中的空格
        /// <summary>
        /// 移除中文中的空格
        /// </summary>
        /// <param name="input">替换字符</param>
        public static string RemoveSofChinese(string input)
        {
            #region
            if (string.IsNullOrEmpty(input))
                return string.Empty;
            else
            {
                string[] arr = Regex.Replace(input, @"\s+", " ").Split('|');
                string returnstring = arr[0];
                for (int i = 1; i < arr.Length; i++)
                {
                    if (isnum(arr[i]))
                        returnstring += "|" + arr[i];
                    else
                        if (isnum(arr[i - 1]))
                        {
                            returnstring += "\n" + arr[i];
                        }
                        else
                        {
                            returnstring += " " + arr[i];
                        }
                }
                return returnstring;
            }
            #endregion
        }
        #endregion

        #region 是否是数字或百分比,如123，12.34，12.34%
        /// <summary>
        /// 是否是数字或百分比,如123，12.34，12.34%
        /// </summary>
        /// <param name="stringtext">校验字符</param>
        public static bool isnum(string stringtext)
        {
            Regex reg = new Regex("^[-]?[0-9]*[.]?[0-9]*[%]?$");
            if (reg.IsMatch(stringtext))
                return true;
            else
                return false;
        }
        #endregion
    }
}
