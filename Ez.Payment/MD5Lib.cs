using System;
using System.Security.Cryptography;
using System.Text;
namespace Ez.Payment
{
	public sealed class MD5Lib
	{
		public static string Sign(string prestr, string key, string _input_charset)
		{
			StringBuilder stringBuilder = new StringBuilder(32);
			prestr += key;
			MD5 mD = new MD5CryptoServiceProvider();
			byte[] array = mD.ComputeHash(Encoding.GetEncoding(_input_charset).GetBytes(prestr));
			for (int i = 0; i < array.Length; i++)
			{
				stringBuilder.Append(array[i].ToString("x").PadLeft(2, '0'));
			}
			return stringBuilder.ToString();
		}
		public static bool Verify(string prestr, string sign, string key, string _input_charset)
		{
			string a = MD5Lib.Sign(prestr, key, _input_charset);
			return a == sign;
		}
	}
}
