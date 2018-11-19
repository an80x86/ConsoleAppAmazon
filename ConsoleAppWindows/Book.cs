using Microsoft.SqlServer.Server;

namespace ConsoleAppWindows
{
    public class Book
    {
        public string Url { get; set; }
        public string Asin { get; set; }
        public string Name { get; set; }
        public string Price { get; set; }
        public string Country { get; set; }
        public string NewCount { get; set; }
        public string NewPrice { get; set; }
        public string UsedCount { get; set; }
        public string UsedPrice { get; set; }
        public string CollectibleCount { get; set; }
        public string CollectiblePrice { get; set; }
        public string TimeStamp { get; set; }
    }
}