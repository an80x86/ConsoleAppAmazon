using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppAmazon
{
    class Program
    {
        static string[] countries = {".com", ".de", ".co.uk", ".fr", ".it", ".es"};

        static string[] products =
        {
            "3922028004", "3596214629",
            "3577145552", "3442362644",
            "3442458366", "3577143045",
            "3802516427", "3464410048",
            "3596215080", "3815566355",
            "3981455533", "3426652684",
            "3898930696", "3811839217",
            "3442720001", "3596292301"
        };

        static void Main(string[] args)
        {
            try
            {
                var lines = new List<string>();
                foreach (var country in countries)
                {
                    foreach (var product in products)
                    {
                        var rets = UrunDetay(country, product).GetAwaiter().GetResult();
                        if (rets != null)
                        {
                            string data = getBetween(rets, "<span class=\"a-color-base\">", "<");
                            if (!string.IsNullOrWhiteSpace(data)) data = data.Replace("\n", "").Trim();

                            var line = string.Format("{0} {1} {2}", country, product, data);
                            lines.Add(line);
                            Console.WriteLine(line);
                        }
                    }
                }

                if (lines.Count > 0)
                {
                    File.WriteAllLines("liste.txt",lines);
                }
            }
            catch (Exception ex)
            {
                string err = ex.Message;
            }

            Console.WriteLine("Hello World!");
        }

        private static async Task<string> UrunDetay(string country, string product)
        {
            var url = string.Format("https://www.amazon{1}/dp/{0}", product, country);

            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("User-Agent",
                        //"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/57.0.2987.133 Safari/537.36"
                        "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)"
                        );
                    //client.DefaultRequestHeaders.Add("Authorization", "Bearer " + FaceBookToken);
                    //client.DefaultRequestHeaders.Add("fields", "created_time,id,ad_id,form_id,field_data");//"created_time,id,email,full_name,phone_number,company_name,form_name");
                    var byteArray = await client.GetByteArrayAsync(url);// .GetStringAsync(url);

                    try
                    {
                        byteArray = Decompress(byteArray);
                        var ret = System.Text.Encoding.UTF8.GetString(byteArray);

                        return ret;
                    }
                    catch
                    {
                        var ret = System.Text.Encoding.UTF8.GetString(byteArray);

                        return ret;
                    }
                }
            }
            catch (Exception ex)
            {
                string ss = ex.Message;
            }

            return null;
        }

        public static string getBetween(string strSource, string strStart, string strEnd)
        {
            int Start, End;
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                return strSource.Substring(Start, End - Start);
            }
            else
            {
                return "";
            }
        }

        static byte[] Compress(byte[] data)
        {
            using (var compressedStream = new MemoryStream())
            using (var zipStream = new GZipStream(compressedStream, CompressionMode.Compress))
            {
                zipStream.Write(data, 0, data.Length);
                zipStream.Close();
                return compressedStream.ToArray();
            }
        }

        static byte[] Decompress(byte[] data)
        {
            using (var compressedStream = new MemoryStream(data))
            using (var zipStream = new GZipStream(compressedStream, CompressionMode.Decompress))
            using (var resultStream = new MemoryStream())
            {
                zipStream.CopyTo(resultStream);
                return resultStream.ToArray();
            }
        }
    }
}
