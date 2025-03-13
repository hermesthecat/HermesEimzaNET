using System;

namespace WinFormEImza.Nesneler
{
    [Serializable]
    public class PdfRequestDTO
    {
        public string DonglePassword { get; set; }
        public string KaynakPdfYolu { get; set; }
        public string HedefPdfYolu { get; set; }

    }
}
