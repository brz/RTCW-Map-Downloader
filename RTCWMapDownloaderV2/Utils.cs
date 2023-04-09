using System.Text;
using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace RTCWMapDownloader
{
    public class Utils
    {
        public static string CalculateMd5Checksum(string fileName)
        {
            var file = new FileStream(fileName, FileMode.Open);
            MD5 md5 = new MD5CryptoServiceProvider();
            var retVal = md5.ComputeHash(file);
            file.Close();

            var sb = new StringBuilder();
            for (var i = 0; i < retVal.Length; i++)
            {
                sb.Append(retVal[i].ToString("X2"));
            }
            return sb.ToString();
        }

        public static string FilterIp(string text)
        {
            var regex = @"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\:\d{1,5}";
            if (Regex.IsMatch(text, regex))
            {
                return Regex.Match(text, regex).ToString();
            }
            else
            {
                return null;
            }
        }

        public static double BytesToMegabytes(double bytesValue)
        {
            return (bytesValue / 1024f) / 1024f;
        }

        public static double BytesToKilobytes(double bytesValue)
        {
            return (bytesValue / 1024f);
        }

        public static void DeleteFile(string path)
        {
            try
            {
                File.Delete(path);
            }
            catch
            {
                //Wait 1 sec & try again
                System.Threading.Thread.Sleep(1000);
                try
                {
                    File.Delete(path);
                }
                catch
                {
                    //Wait 1 sec & try again
                    System.Threading.Thread.Sleep(1000);
                    try
                    {
                        File.Delete(path);
                    }
                    catch
                    {
                        //Do nothing
                    }
                }
            }
        }
    }
}
