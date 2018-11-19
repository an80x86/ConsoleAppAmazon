using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppWindows
{
    public class BookManager
    {
        private ICountry country;

        public BookManager(ICountry country)
        {
            this.country = country;
        }

        public Book IslemYap(string rets, Book book)
        {
            return country.IslemYap(rets, book);
        }
    }
}
