using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppWindows
{
    public class Fr : ICountry
    {
        public Book IslemYap(string rets, Book book)
        {
            string price = CsvExport.getBetween(rets, "<span class=\"a-color-base\">", "<");
            if (string.IsNullOrWhiteSpace(price))
            {
                price = CsvExport.getBetween(rets, "<span id=\"priceblock_ourprice\" class=\"a-size-medium a-color-price\">", "<");
            }
            price = CsvExport.getBetween(rets, "<span class=\"a-size-base a-color-price a-color-price\">", "</span>");
            if (string.IsNullOrWhiteSpace(price))
            {
                price = CsvExport.getBetween(rets, "<span class=\"a-color-base\">", "<");
            }

            if (!string.IsNullOrWhiteSpace(price)) price = price.Replace("\n", "").Trim().Replace("ab ", "");
            book.Price = price;

            // tablo fiyat
            string tablo = CsvExport.getBetween(rets, "<span class=\"olp-");
            if (!string.IsNullOrWhiteSpace(tablo))
            {
                tablo = "<span class=\"olp-" + tablo;
                var list = CsvExport.getBeetweenList(tablo, "<a class=\"a-size-mini a-link-normal\" href=\"", " </a>");
                foreach (var l in list)
                {
                    if (l.ToLower().Contains("=new"))
                    {
                        var tmp = CsvExport.getBetween(l, "=new");
                        if (!string.IsNullOrWhiteSpace(tmp))
                        {
                            tmp = tmp.Replace("\n", "");
                            book.NewCount = CsvExport.getBetween(tmp, "sr=\">", "<span").Trim();
                            book.NewPrice = CsvExport.getBetween(tmp, "</span>").Trim();
                        }
                        continue;
                    }
                    if (l.ToLower().Contains("=used"))
                    {
                        var tmp = CsvExport.getBetween(l, "=used");
                        if (!string.IsNullOrWhiteSpace(tmp))
                        {
                            tmp = tmp.Replace("\n", "");
                            book.UsedCount = CsvExport.getBetween(tmp, "sr=\">", "<span").Trim();
                            book.UsedPrice = CsvExport.getBetween(tmp, "</span>").Trim();
                        }
                        continue;
                    }
                    if (l.ToLower().Contains("=collectible"))
                    {
                        var tmp = CsvExport.getBetween(l, "=collectible");
                        if (!string.IsNullOrWhiteSpace(tmp))
                        {
                            book.CollectibleCount = CsvExport.getBetween(tmp, ">", "</a>");
                            book.CollectiblePrice = CsvExport.getBetween(tmp, "<span class='a-color-price'>");
                        }
                        continue;
                    }
                }
            }

            return book;
        }
    }
}
