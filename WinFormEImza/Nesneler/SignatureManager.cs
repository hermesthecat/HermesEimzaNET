using System;
using WinFormEImza.Islemler;

namespace WinFormEImza.Nesneler
{
    internal class SignatureManager
    {
        private PdfSigner _pdfSigner;

        public SignatureManager()
        {
            _pdfSigner = new PdfSigner();
        }

        public string SignPdf(PdfRequestDTO requestDTO)
        {
            string sonuc = "";
            try
            {
                GenelIslemler.GetPolicy();
                sonuc = _pdfSigner.SignPDF(requestDTO);
                GenelIslemler.LogaYaz(" Belge imzalandı..(" + requestDTO.KaynakPdfYolu + "-- > " + requestDTO.HedefPdfYolu + ")");
            }
            catch (Exception ex)
            {
                GenelIslemler.LogaYaz(" Dosyanın imzalanması sırasında bir hata oluştu. (" + ex.Message + ")");
            }
            return sonuc;
        }
    }
}
