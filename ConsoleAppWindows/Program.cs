using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ConsoleAppWindows
{
    class Program
    {
        //To discuss automated access to Amazon data please contact api-services-support @amazon.com.
        //For information about migrating to our APIs refer to our Marketplace APIs at https://developer.amazonservices.com/ref=rm_c_sv, or our
        //Product Advertising API at https://affiliate-program.amazon.com/gp/advertising/api/detail/main.html/ref=rm_c_ac for advertising use cases.


        //static string[] countries = { ".de", ".fr", ".it", ".es", ".co.uk"};
        static string[] countries = { ".com" };

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
                var lines = new List<Book>();
                foreach (var country in countries)
                {
                    foreach (var product in products)
                    {
                        var url = string.Format("https://www.amazon{1}/dp/{0}", product, country);
                        var rets = CsvExport.UrunDetay(url).GetAwaiter().GetResult();
                        if (rets != null)
                        {
                            BookManager bm;
                            var book = new Book();
                            book.Url = url;
                            book.TimeStamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            book.Asin = product;
                            book.Country = country;

                            // name
                            var productStr = CsvExport.getBetween(rets, "<h1 id=\"title\" class=\"a-size-large a-spacing-none\">", "</h1>\n</div>");
                            if (string.IsNullOrWhiteSpace(productStr))
                            {
                                productStr = CsvExport.getBetween(rets, "<h1 id=\"title\" class=\"a-size-large a-spacing-none\">", "</h1>");
                            }

                            if (!string.IsNullOrWhiteSpace(productStr))
                            {
                                var title = CsvExport.getBetween(productStr, "<span id=\"productTitle\" class=\"a-size-large\">", "</span>");
                                title = CsvExport.clearHtmlText(title);
                                title = title.Replace("\n", "").Trim();
                                var values = string.Join(" ", CsvExport.getBeetweenList(productStr, "<span class=\"a-size-medium a-color-secondary a-text-normal\">", "</span>").ToArray());
                                values = CsvExport.clearHtmlText(values);
                                book.Name = title + " " + values;
                            }

                            if (product == "3811839217")// "3426652684") //"3815566355")
                            {
                                ;
                            }

                            if (country == ".de")
                            {
                                bm = new BookManager(new De());
                                book = bm.IslemYap(rets, book);
                            }
                            else
                            if (country == ".fr")
                            {
                                bm = new BookManager(new Fr());
                                book = bm.IslemYap(rets, book);
                            }
                            else
                            if (country == ".it")
                            {
                                bm = new BookManager(new It());
                                book = bm.IslemYap(rets, book);
                            }
                            else
                            if (country == ".co.uk")
                            {
                                bm = new BookManager(new Co());
                                book = bm.IslemYap(rets, book);
                            }
                            else
                            {
                                bm = new BookManager(new Com());
                                book = bm.IslemYap(rets, book);
                            }

                            var line = string.Format("{0} {1} {2}", country, product, book.Price);
                            lines.Add(book);
                            Console.WriteLine(line);
                        }
                    }
                }

                if (lines.Count > 0)
                {
                    var sonuc = lines.Select(e => e).OrderBy(e => e.Asin).ToList();
                    var ret = sonuc.ToHtmlTable();
                    File.WriteAllText("products.html",ret);
                }
            }
            catch (Exception ex)
            {
                string err = ex.Message;
            }
        }
    }
}
