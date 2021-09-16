using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
public class MD5
{
    public static string CreateMD5(string str)
    {
        MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
        byte[] encryptedBytes = md5.ComputeHash(Encoding.ASCII.GetBytes(str));
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < encryptedBytes.Length; i++)
        {
            sb.AppendFormat("{0:x2}", encryptedBytes[i]);
        }
        return sb.ToString();
    }

    #region DES 加密/解密

    private static byte[] key = ASCIIEncoding.ASCII.GetBytes("WikeSoft");
    private static byte[] iv = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };

    /// <summary>
    /// DES加密。
    /// </summary>
    /// <param name="inputString">输入字符串。</param>
    /// <returns>加密后的字符串。</returns>
    public static string DESEncrypt(string inputString)
    {
        MemoryStream ms = null;
        CryptoStream cs = null;
        StreamWriter sw = null;
        DESCryptoServiceProvider des = new DESCryptoServiceProvider();
        try
        {
            ms = new MemoryStream();
            cs = new CryptoStream(ms, des.CreateEncryptor(key, iv), CryptoStreamMode.Write);
           
            sw = new StreamWriter(cs);
            sw.Write(inputString);
            sw.Flush();
            cs.FlushFinalBlock();
            return Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length);
        }
        finally
        {
            if (sw != null) sw.Close();
            if (cs != null) cs.Close();
            if (ms != null) ms.Close();
        }
    }

    /// <summary>
    /// DES解密。
    /// </summary>
    /// <param name="inputString">输入字符串。</param>
    /// <returns>解密后的字符串。</returns>
    public static string DESDecrypt(string inputString)
    {
        MemoryStream ms = null;
        CryptoStream cs = null;
        StreamReader sr = null;
        DESCryptoServiceProvider des = new DESCryptoServiceProvider();
        try
        {
            if (string.IsNullOrEmpty(inputString))
            {
                return null;
            }
            else
            {
                ms = new MemoryStream(Convert.FromBase64String(inputString));
                cs = new CryptoStream(ms, des.CreateDecryptor(key, iv), CryptoStreamMode.Read);
             //   cs = new CryptoStream(ms, des.CreateDecryptor(key, iv), CryptoStreamMode.Read);
                sr = new StreamReader(cs);
                return sr.ReadToEnd();
            }
        }
        finally
        {
            if (sr != null) sr.Close();
            if (cs != null) cs.Close();
            if (ms != null) ms.Close();
        }
    }

    #endregion
}
