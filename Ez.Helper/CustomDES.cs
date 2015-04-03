using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Xml;

namespace Ez.Helper
{
    /// <summary>
    /// DES 加密
    /// </summary>
    public class CustomDES
    {
        /// <summary>
        /// 默认密钥向量
        /// </summary>
        private static byte[] DefaultIV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

        public static byte[] key = null;

        public static byte[] iv = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

        private static byte[] GetBytes(string input, int len)
        {
            if (input == null || input.Trim() == string.Empty)
                throw new ArgumentNullException();
            string[] s = input.Split(',');
            int n = s.Length;
            byte[] b = new byte[len];
            for (int i = 0; i < len; i++)
            {
                if (i > n)
                    b[i] = byte.Parse(" ", System.Globalization.NumberStyles.HexNumber);
                else
                    b[i] = byte.Parse(s[i].Trim(), System.Globalization.NumberStyles.HexNumber);
            }
            return b;
        }

        static CustomDES()
        {
            XmlDocument xdoc = new XmlDocument();
            var path = Tools.GetMapPath("/Config/Secret.config");
            if (File.Exists(path))
            {
                xdoc.Load(path);
                XmlNode secretKey = xdoc.SelectSingleNode("secret/key");
                XmlNode secretIV = xdoc.SelectSingleNode("secret/iv");
                if (secretIV != null && secretIV != null)
                {
                    string _key = secretKey.InnerText;
                    string _iv = secretIV.InnerText;

                    key = GetBytes(_key, 8);
                    iv = GetBytes(_iv, 8);
                }
                else
                {
                    throw new NullReferenceException("请检查在/Config/Secret.config的节点只是否正确配置！");
                }
            }
            else
            {
                throw new NullReferenceException("请检查您的配置文件/Config/Secret.config是否存在！");
            }
        }

        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="EncryptString">待加密的字符串</param>
        /// <param name="Key">加密密钥</param>
        /// <param name="IV">密钥向量</param>
        /// <returns></returns>
        public static string Encrypt(string EncryptString, byte[] Key, byte[] IV)
        {

            byte[] inputByteArray = Encoding.UTF8.GetBytes(EncryptString);
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, des.CreateEncryptor(Key, IV), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            return Convert.ToBase64String(mStream.ToArray());
        }
        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="DecryptString">待解密的字符串</param>
        /// <param name="Key">解密密钥,要求为8位,和加密密钥相同</param>
        /// <param name="IV">密钥向量</param>
        /// <returns>解密成功返回解密后的字符串,失败返源串</returns>
        public static string Decrypt(string DecryptString, byte[] Key, byte[] IV)
        {
            try
            {

                byte[] inputByteArray = Convert.FromBase64String(DecryptString);
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, des.CreateDecryptor(Key, IV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch
            {
                return "";
            }
        }

        /// <summary>
        /// DES加密字符串
        /// </summary>
        /// <param name="encryptString">待加密的字符串</param>
        /// <returns>加密成功返回加密后的字符串,失败返回源串</returns>
        public static string Encode(string encryptString)
        {
            return Encrypt(encryptString, key, iv);
        }
        /// <summary>
        /// DES解密字符串
        /// </summary>
        /// <param name="decryptString">待解密的字符串</param>
        /// <returns>解密成功返回解密后的字符串,失败返源串</returns>
        public static string Decode(string decryptString)
        {
            try
            {
                return Decrypt(decryptString, key, iv);
            }
            catch
            {
                return "";
            }
        }
        /// <summary>
        /// DES加密字符串
        /// </summary>
        /// <param name="encryptString">待加密的字符串</param>
        /// <param name="encryptKey">加密密钥,要求为8位</param>
        /// <returns>加密成功返回加密后的字符串,失败返回源串</returns>
        public static string Encode(string encryptString, string encryptKey)
        {
            encryptKey = Tools.GetSubString(encryptKey, 8, "");
            encryptKey = encryptKey.PadRight(8, ' ');
            byte[] rgbKey = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));
            byte[] rgbIV = DefaultIV;
            return Encrypt(encryptString, rgbKey, rgbIV);
        }
        /// <summary>
        /// DES解密字符串
        /// </summary>
        /// <param name="decryptString">待解密的字符串</param>
        /// <param name="decryptKey">解密密钥,要求为8位,和加密密钥相同</param>
        /// <returns>解密成功返回解密后的字符串,失败返源串</returns>
        public static string Decode(string decryptString, string decryptKey)
        {
            try
            {
                decryptKey = Tools.GetSubString(decryptKey, 8, "");
                decryptKey = decryptKey.PadRight(8, ' ');
                byte[] rgbKey = Encoding.UTF8.GetBytes(decryptKey);
                byte[] rgbIV = DefaultIV;
                return Decrypt(decryptString, rgbKey, rgbIV);
            }
            catch
            {
                return "";
            }
        }

    }
}
