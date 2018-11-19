using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppWindows
{
    public class De : ICountry
    {
        public Book IslemYap(string rets, Book book)
        {
            string price = CsvExport.getBetween(rets, "<span class=\"a-color-base\">", "<");
            if (string.IsNullOrWhiteSpace(price))
            {
                price = price = CsvExport.getBetween(rets, "<span class=\"a-size-base a-color-price offer-price a-text-normal\">", "</span>");
            }
            if (string.IsNullOrWhiteSpace(price))
            {
                price = CsvExport.getBetween(rets, "<span id=\"priceblock_ourprice\" class=\"a-size-medium a-color-price\">", "</span>");
            }

            if (!string.IsNullOrWhiteSpace(price)) price = price.Replace("\n", "").Trim().Replace("ab ", "");
            book.Price = price;

            // tablo fiyat
            string tablo = CsvExport.getBetween(rets, "<div class=\"a-section a-spacing-small a-spacing-top-small\">", "</div>");
            if (string.IsNullOrWhiteSpace(tablo))
            {
                tablo = CsvExport.getBetween(rets, "<div id=\"olp-sl-new-used\" class=\"a-section a-spacing-small a-spacing-top-small\">", "</div>");
            }

            if (!string.IsNullOrWhiteSpace(tablo))
            {
                var list = CsvExport.getBeetweenList(tablo, "<span class=\"olp-padding-right\"><a href=\"", "</span></span>");
                foreach (var l in list)
                {
                    if (l.ToLower().Contains("=new\""))
                    {
                        var tmp = CsvExport.getBetween(l, "=new\"");
                        if (!string.IsNullOrWhiteSpace(tmp))
                        {
                            book.NewCount = CsvExport.getBetween(tmp, ">", "</a>");
                            book.NewPrice = CsvExport.getBetween(tmp, "<span class='a-color-price'>");
                        }
                        continue;
                    }
                    if (l.ToLower().Contains("=used\""))
                    {
                        var tmp = CsvExport.getBetween(l, "=used\"");
                        if (!string.IsNullOrWhiteSpace(tmp))
                        {
                            book.UsedCount = CsvExport.getBetween(tmp, ">", "</a>");
                            book.UsedPrice = CsvExport.getBetween(tmp, "<span class='a-color-price'>");
                        }
                        continue;
                    }
                    if (l.ToLower().Contains("=collectible\""))
                    {
                        var tmp = CsvExport.getBetween(l, "=collectible\"");
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
