using System.Collections.Generic;

namespace WinFormEImza.Nesneler
{
    public class BelgeImzalanacak
    {
        public string source { get; set; }
        public string sourceName { get; set; }
        public string targetUrl { get; set; }
        public string sourceType { get; set; }
        public string format { get; set; }
    }

    public class BelgeIstekCevap
    {
        public string id { get; set; }
        public List<BelgeImzalanacak> resources { get; set; }
        public string responseUrl { get; set; }
    }

    public class DosyaUploadCevap
    {
        public bool error { get; set; }
        public string msg { get; set; }
    }
}
