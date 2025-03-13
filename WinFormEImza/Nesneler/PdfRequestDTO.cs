using System;

namespace WinFormEImza.Nesneler
{
    [Serializable]
    public class PdfRequestDTO
    {
        public string DonglePassword { get; set; }
        public string KaynakPdfYolu { get; set; }
        public string HedefPdfYolu { get; set; }
        
        // Base64 formatında PDF içeriği için yeni özellikler
        public string PdfContent { get; set; }
        public bool IsBase64Content { get; set; }
        
        // İmza pozisyonu için yeni özellikler
        public float? SignatureX { get; set; }
        public float? SignatureY { get; set; }
    }
}
