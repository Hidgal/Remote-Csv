using System;
using System.Security.Cryptography;
using System.Text;

namespace RemoteCsv.Internal.Extensions
{
    public static class FileExtensions
    {
        private static string[] _sizeKeys = { "Bytes", "Kb", "Mb", "Gb", "Tb" };

        public static string GetSizeString(ulong bytesLength)
        {
            if (bytesLength == 0)
                return "0 Bytes";

            int i = (int)(Math.Floor(Math.Log(bytesLength) / Math.Log(1024)));
            return Math.Round(bytesLength / Math.Pow(1024, i), 2) + " " + _sizeKeys[i];
        }

        // from https://github.com/MartinSchultz/unity3d/blob/master/CryptographyHelper.cs
        public static string GetHash(string strToEncrypt)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            byte[] bytes = encoding.GetBytes(strToEncrypt);

            return GetHash(bytes);
        }

        // from https://github.com/MartinSchultz/unity3d/blob/master/CryptographyHelper.cs
        public static string GetHash(byte[] bytes)
        {
            if (bytes == null)
                return string.Empty;

            // encrypt bytes
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] hashBytes = md5.ComputeHash(bytes);

            // Convert the encrypted bytes back to a string (base 16)
            string hashString = "";
            for (int i = 0; i < hashBytes.Length; i++)
            {
                hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, "0"[0]);
            }
            return hashString.PadLeft(32, "0"[0]);
        }
    }
}
