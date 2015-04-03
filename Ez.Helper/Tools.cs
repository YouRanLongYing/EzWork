using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using System.IO;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.Configuration;
using System.Drawing;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.Win32;
using Ez.Lang;

namespace Ez.Helper
{
    /// <summary>
    /// 通用工具类
    /// </summary>
    public class Tools
    {
        /// <summary>
        /// 获得当前绝对路径
        /// </summary>
        /// <param name="p_strPath">指定的路径</param>
        /// <returns>绝对路径</returns>
        public static string GetMapPath(string p_strPath)
        {
            if (HttpContext.Current != null)
            {
                return HttpContext.Current.Server.MapPath(p_strPath);
            }
            else //非web程序引用
            {
                p_strPath = p_strPath.TrimStart('~','/');
                p_strPath = p_strPath.Replace("/", "\\");
                if (p_strPath.StartsWith("\\"))
                {
                    p_strPath = p_strPath.Substring(p_strPath.IndexOf('\\', 1)).TrimStart('\\');
                }
                return System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, p_strPath);
            }
        }

        /// <summary>
        /// 读取配置文件指定节点值
        /// </summary>
        /// <param name="target">目标节点</param>
        /// <param name="xmlPath">文件路径</param>
        /// <param name="cdata">是否为cdata</param>
        /// <returns></returns>
        public static string GetConfigValue(string target, string xmlPath, bool cdata)
        {
            System.Xml.XmlDocument xdoc = new XmlDocument();
            xdoc.Load(xmlPath);
            XmlElement root = xdoc.DocumentElement;
            XmlNode node = root.SelectSingleNode(target);
            if (node != null)
            {
                if (cdata)
                    return node.InnerText;
                else
                    return node.InnerXml;
            }
            else
            {
                return string.Empty;
            }
        }
        /// <summary>
        /// 获取Web类型站点下Web.Config AppSet节点的值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public static string GetAppSetString(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }
        /// <summary>
        /// 获取某个配置文件节点的属性值
        /// </summary>
        /// <param name="target">目标节点</param>
        /// <param name="xmlPath">文件路径</param>
        /// <param name="attrname">属性名称</param>
        /// <returns></returns>
        public static string GetConfigNodeAttribut(string target, string xmlPath, string attrname)
        {
            System.Xml.XmlDocument xdoc = new XmlDocument();
            xdoc.Load(xmlPath);
            XmlElement root = xdoc.DocumentElement;
            XmlNode node = root.SelectSingleNode(target);
            if (node != null)
            {
                XmlAttribute attr = node.Attributes[attrname];
                if (attr != null)
                {
                    return attr.Value;
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取当前的会话信息
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        public static object GetSessionValue(string key)
        {
            if (HttpContext.Current != null)
            {
                return HttpContext.Current.Session[key];
            }
            else
            {
                return null;//doing  
            }
        }
        /// <summary>
        /// 获取资源文件的字符串值
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>值</returns>
        public static string GetResourceString(string key)
        {
            return EzLanguage.ResourceManager.GetString(key);
        }
        /// <summary>
        /// 获取资源文件的字符串值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="defaultstring">为获取到值的给定值</param>
        /// <returns>资源值</returns>
        public static string GetResourceString(string key, string defaultstring)
        {
            string value = defaultstring;
            if (!string.IsNullOrEmpty(key))
                value = Tools.GetResourceString(key);
            if (string.IsNullOrEmpty(value))
            {
                if (!string.IsNullOrEmpty(key)) return "<i title='请检查是否设置了此键对应的资源！'>" + key + "</i>";
                //throw new Exception("无法获取用于UI显示的资源信息！");
                else
                    return "<i title='无法获取用于UI显示的资源信息！'>?</i>";
            }
            return value;
        }

        /// <summary>
        /// 获取Request值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Request(string key)
        {
            return HttpContext.Current.Request[key];
        }
        /// <summary>
        /// 获取Request 的Form值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string RequestForm(string key)
        {
            return HttpContext.Current.Request.Form[key];
        }
        /// <summary>
        /// 获取Request 的Query值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string RequestQuery(string key)
        {
            return HttpContext.Current.Request.QueryString[key];
        }


        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>创建是否成功</returns>
        [DllImport("dbgHelp", SetLastError = true)]
        private static extern bool MakeSureDirectoryPathExists(string name);

        /// <summary>
        /// 建立文件夹
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool CreateDir(string name)
        {
            return MakeSureDirectoryPathExists(name);
        }
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="path">文件路径</param>
        public static void DelFile(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
            catch (IOException e) { throw new Exception(e.Message); }
        }
        /// <summary>
        /// 删除文件夹
        /// </summary>
        /// <param name="path">文件夹路径</param>
        public static void DelDir(string path)
        {
            try
            {
                if (Directory.Exists(path))
                {
                    Directory.Delete(path);
                }
            }
            catch (IOException e) { throw new Exception(e.Message); }
        }
        /// <summary>
        /// 读取并返回一个文本文件的内容
        /// </summary>
        /// <param name="filePath">文件的物理路径</param>
        /// <returns></returns>
        public static string GetTextFileContent(string filePath)
        {
            string result = string.Empty;
            if (File.Exists(filePath))
            {
                try
                {
                    using (StreamReader sr = new StreamReader(filePath))
                    {
                        result = sr.ReadToEnd();
                    }
                }
                catch
                { }
            }
            return result;
        }
        /// <summary>
        /// 取得用户客户端IP(穿过代理服务器取远程用户真实IP地址)
        /// </summary>
        public static string GetClientIp()
        {
            if (HttpContext.Current == null) return "";
            if (HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
            {
                return HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            }
            else
            {
                return HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
            }
        }
        /// <summary>
        /// 取得前一个（上次提交或链接进来的）网页的URL
        /// </summary>
        /// <returns></returns>
        public static string GetReferrerUrl()
        {
            Uri uri = HttpContext.Current.Request.UrlReferrer;
            if (uri != null)
            {
                return uri.ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 取指定长度的字符串
        /// </summary>
        /// <param name="p_SrcString">要检查的字符串</param>
        /// <param name="p_StartIndex">起始位置</param>
        /// <param name="p_Length">指定长度</param>
        /// <param name="p_TailString">用于替换的字符串</param>
        /// <returns>截取后的字符串</returns>
        public static string GetSubString(string p_SrcString, int p_StartIndex, int p_Length, string p_TailString)
        {
            string myResult = p_SrcString;

            Byte[] bComments = Encoding.UTF8.GetBytes(p_SrcString);
            foreach (char c in Encoding.UTF8.GetChars(bComments))
            {    //当是日文或韩文时(注:中文的范围:\u4e00 - \u9fa5, 日文在\u0800 - \u4e00, 韩文为\xAC00-\xD7A3)
                if ((c > '\u0800' && c < '\u4e00') || (c > '\xAC00' && c < '\xD7A3'))
                {
                    //if (System.Text.RegularExpressions.Regex.IsMatch(p_SrcString, "[\u0800-\u4e00]+") || System.Text.RegularExpressions.Regex.IsMatch(p_SrcString, "[\xAC00-\xD7A3]+"))
                    //当截取的起始位置超出字段串长度时
                    if (p_StartIndex >= p_SrcString.Length)
                        return "";
                    else
                        return p_SrcString.Substring(p_StartIndex,
                                                       ((p_Length + p_StartIndex) > p_SrcString.Length) ? (p_SrcString.Length - p_StartIndex) : p_Length);
                }
            }

            if (p_Length >= 0)
            {
                byte[] bsSrcString = Encoding.Default.GetBytes(p_SrcString);

                //当字符串长度大于起始位置
                if (bsSrcString.Length > p_StartIndex)
                {
                    int p_EndIndex = bsSrcString.Length;

                    //当要截取的长度在字符串的有效长度范围内
                    if (bsSrcString.Length > (p_StartIndex + p_Length))
                    {
                        p_EndIndex = p_Length + p_StartIndex;
                    }
                    else
                    {   //当不在有效范围内时,只取到字符串的结尾

                        p_Length = bsSrcString.Length - p_StartIndex;
                        p_TailString = "";
                    }

                    int nRealLength = p_Length;
                    int[] anResultFlag = new int[p_Length];
                    byte[] bsResult = null;

                    int nFlag = 0;
                    for (int i = p_StartIndex; i < p_EndIndex; i++)
                    {
                        if (bsSrcString[i] > 127)
                        {
                            nFlag++;
                            if (nFlag == 3)
                                nFlag = 1;
                        }
                        else
                            nFlag = 0;

                        anResultFlag[i] = nFlag;
                    }

                    if ((bsSrcString[p_EndIndex - 1] > 127) && (anResultFlag[p_Length - 1] == 1))
                        nRealLength = p_Length + 1;

                    bsResult = new byte[nRealLength];

                    Array.Copy(bsSrcString, p_StartIndex, bsResult, 0, nRealLength);

                    myResult = Encoding.Default.GetString(bsResult);
                    myResult = myResult + p_TailString;
                }
            }

            return myResult;
        }
        /// <summary>
        /// 字符串如果操过指定长度则将超出的部分用指定字符串代替
        /// </summary>
        /// <param name="p_SrcString">要检查的字符串</param>
        /// <param name="p_Length">指定长度</param>
        /// <param name="p_TailString">用于替换的字符串</param>
        /// <returns>截取后的字符串</returns>
        public static string GetSubString(string p_SrcString, int p_Length, string p_TailString)
        {
            return GetSubString(p_SrcString, 0, p_Length, p_TailString);
        }
        /// <summary>
        /// 获取站点根目录URL
        /// </summary>
        /// <returns></returns>
        public static string GetRootUrl(string GetWebPath)
        {
            int port = HttpContext.Current.Request.Url.Port;
            return string.Format("{0}://{1}{2}{3}",
                                 HttpContext.Current.Request.Url.Scheme,
                                 HttpContext.Current.Request.Url.Host.ToString(),
                                 (port == 80 || port == 0) ? "" : ":" + port,
                                 GetWebPath);
        }

        /// <summary>
        /// 生成条形码
        /// </summary>
        /// <param name="text">待编码文本</param>
        /// <param name="ecode">编码方式</param>
        /// <returns></returns>
        public static Bitmap GetBarCodeImage(string text, Ez.Helper.BarCode.Code128.Encode ecode)
        {
            BarCode.Code128 c128 = new BarCode.Code128();
            return c128.GetCodeImage(text, ecode);
        }
        /// <summary>
        /// 按照Code128B方式编码 获取条形码
        /// </summary>
        /// <param name="text">待编码文本</param>
        /// <returns></returns>
        public static Bitmap GetBarCodeImage(string text)
        {
            return GetBarCodeImage(text, BarCode.Code128.Encode.Code128B);
        }

        /// <summary>
        /// 创建Zip文件
        /// </summary>
        /// <param name="filesPath">文件所在目录</param>
        /// <param name="zipFilePath">输出目录</param>
        /// <param name="password">压缩密码为空时不设置</param>
        /// <returns>执行的错误信息，若大于0说明执行有错误产生</returns>
        public static IList<string> CreateZipFile(string filesPath, string zipFilePath, string password = "")
        {
            IList<string> errors = new List<string>();
            if (!Directory.Exists(filesPath))
            {
                errors.Add(string.Format("Cannot find directory '{0}'", filesPath));
            }
            else
            {
                string[] filenames = Directory.GetFiles(filesPath);
                CreateZipFile(Directory.GetFiles(filesPath), zipFilePath, password);
            }
            return errors;
        }
        /// <summary>
        /// 创建Zip文件
        /// </summary>
        /// <param name="filenames">文件路径列表</param>
        /// <param name="zipFilePath">输出目录</param>
        /// <param name="password">压缩密码为空时不设置</param>
        /// <returns>执行的错误信息，若大于0说明执行有错误产生</returns>
        public static IList<string> CreateZipFile(string[] filenames, string zipFilePath, string password = "")
        {
            IList<string> errors = new List<string>();
            try
            {
                using (ZipOutputStream s = new ZipOutputStream(File.Create(zipFilePath)))
                {
                    s.SetLevel(9); // 压缩级别 0-9
                    if (!string.IsNullOrEmpty(password))
                    {
                        s.Password = password; //Zip压缩文件密码
                    }
                    byte[] buffer = new byte[4096]; //缓冲区大小
                    foreach (string file in filenames)
                    {
                        if (File.Exists(file))
                        {
                            ZipEntry entry = new ZipEntry(Path.GetFileName(file));
                            entry.DateTime = DateTime.Now;
                            s.PutNextEntry(entry);
                            using (FileStream fs = File.OpenRead(file))
                            {
                                int sourceBytes;
                                do
                                {
                                    sourceBytes = fs.Read(buffer, 0, buffer.Length);
                                    s.Write(buffer, 0, sourceBytes);
                                } while (sourceBytes > 0);
                            }
                        }
                        else
                        {
                            errors.Add(string.Format("NotExitsFile:{0}", file));
                        }
                    }
                    s.Finish();
                    s.Close();
                }
            }
            catch (Exception ex)
            {
                errors.Add(string.Format("Exception during processing {0}", ex));
            }
            return errors;
        }
        /// <summary>
        /// 解压ZIP文件
        /// </summary>
        /// <param name="zipFilePath">ZIP文件路径</param>
        /// <param name="password">解压密码</param>
        public static string UnZipFile(string zipFilePath, string password = "")
        {
            if (!File.Exists(zipFilePath))
            {
                return string.Format("Cannot find file '{0}'", zipFilePath);
            }
            using (ZipInputStream s = new ZipInputStream(File.OpenRead(zipFilePath)))
            {
                ZipEntry theEntry;
                while ((theEntry = s.GetNextEntry()) != null)
                {
                    if (!string.IsNullOrEmpty(password))
                    {
                        s.Password = password; //Zip压缩文件密码
                    }
                    Console.WriteLine(theEntry.Name);
                    string directoryName = Path.GetDirectoryName(theEntry.Name);
                    string fileName = Path.GetFileName(theEntry.Name);

                    // create directory
                    if (directoryName.Length > 0)
                    {
                        Directory.CreateDirectory(directoryName);
                    }

                    if (fileName != String.Empty)
                    {
                        using (FileStream streamWriter = File.Create(theEntry.Name))
                        {
                            int size = 2048;
                            byte[] data = new byte[2048];
                            while (true)
                            {
                                size = s.Read(data, 0, data.Length);
                                if (size > 0)
                                {
                                    streamWriter.Write(data, 0, size);
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            return null;
        }
        /// <summary>
        /// 生成短Url
        /// </summary>
        /// <param name="url">原始Url地址</param>
        /// <returns>短地址</returns>
        public static string[] ShortUrl(string url)
        {
            //可以自定义生成MD5加密字符传前的混合KEY  
            //要使用生成URL的字符  
            string[] chars = new string[]{  
                "a","b","c","d","e","f","g","h",  
                "i","j","k","l","m","n","o","p",  
                "q","r","s","t","u","v","w","x",  
                "y","z","0","1","2","3","4","5",  
                "6","7","8","9","A","B","C","D",  
                "E","F","G","H","I","J","K","L",  
                "M","N","O","P","Q","R","S","T",  
                "U","V","W","X","Y","Z"  
            };
            //对传入网址进行MD5加密  
            string hex = CustomMD5.Powered(url);
            string[] resUrl = new string[4];
            for (int i = 0; i < 4; i++)
            {
                //把加密字符按照8位一组16进制与0x3FFFFFFF进行位与运算  
                int hexint = 0x3FFFFFFF & Convert.ToInt32("0x" + hex.Substring(i * 8, 8), 16);
                string outChars = string.Empty;
                for (int j = 0; j < 6; j++)
                {
                    //把得到的值与0x0000003D进行位与运算，取得字符数组chars索引  
                    int index = 0x0000003D & hexint;
                    //把取得的字符相加  
                    outChars += chars[index];
                    //每次循环按位右移5位  
                    hexint = hexint >> 5;
                }
                //把字符串存入对应索引的输出数组  
                resUrl[i] = outChars;
            }
            return resUrl;
        }

        /// <summary>
        /// 随机数 
        /// </summary>
        private static char[] chars = 
        { 
            '0','1','2','3','4','5','6','7','8','9',  
            'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z',
            'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T','U','V','W','X','Y','Z',
            '_'
        };
        /// <summary>
        /// 生成随机码
        /// </summary>
        /// <param name="len">随机码长度</param>
        /// <returns>随机码</returns>
        public static string RandomChars(int len)
        {
            StringBuilder str = new StringBuilder();
            for (int i = 0; i < len; i++)
            {
                str.Append(chars[new Random(DateTime.Now.Millisecond + i).Next(0, chars.Length - 1)]);
            }
            return str.ToString();
        }
        /// <summary>
        /// 获取文件Content Type
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <returns></returns>
        public static string GetFileContentType(string filename)
        {
            string[] array = filename.Split('.');
            string result = string.Empty;
            string suffix = "." + array[array.Length - 1];
            RegistryKey rg = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(suffix);
            object obj = rg.GetValue("Content Type");
            result = obj != null ? obj.ToString() : string.Empty;
            rg.Close();
            return result;
        }

        /// <summary>
        /// 清除html标签
        /// </summary>
        /// <param name="html">标签内容</param>
        /// <param name="length">截取长度</param>
        /// <returns></returns>
        public static string ReplaceHtmlTag(string html, int length = 0)
        {
            string strText = System.Text.RegularExpressions.Regex.Replace(html, "<[^>]+>", "");
            strText = System.Text.RegularExpressions.Regex.Replace(strText, "&[^;]+;", "");

            if (length > 0 && strText.Length > length)
                return strText.Substring(0, length);

            return strText;
        }
    }
}
