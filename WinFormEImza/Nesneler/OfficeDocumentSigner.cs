using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;

namespace WinFormEImza.Nesneler
{
    public class OfficeDocumentSigner
    {
        public string SignDocument(string filePath, string outputPath, X509Certificate2 certificate)
        {
            string extension = Path.GetExtension(filePath).ToLower();
            switch (extension)
            {
                case ".docx":
                    return SignWordDocument(filePath, outputPath, certificate);
                case ".xlsx":
                    return SignExcelDocument(filePath, outputPath, certificate);
                default:
                    throw new NotSupportedException($"File format not supported: {extension}");
            }
        }

        private string SignWordDocument(string filePath, string outputPath, X509Certificate2 certificate)
        {
            // Önce dosyayı yeni konuma kopyala
            File.Copy(filePath, outputPath, true);

            using (WordprocessingDocument document = WordprocessingDocument.Open(outputPath, true))
            {
                // Dijital imza bilgisini ekle
                DigitalSignatureOriginPart originPart = document.AddNewPart<DigitalSignatureOriginPart>();
                
                // İmza özelliklerini ayarla
                var signatureProperties = document.PackageProperties;
                signatureProperties.Creator = certificate.Subject;
                signatureProperties.Created = DateTime.Now;
                signatureProperties.Modified = DateTime.Now;

                // İmza bilgisini kaydet
                document.Save();
            }

            return outputPath;
        }

        private string SignExcelDocument(string filePath, string outputPath, X509Certificate2 certificate)
        {
            // Önce dosyayı yeni konuma kopyala
            File.Copy(filePath, outputPath, true);

            using (SpreadsheetDocument document = SpreadsheetDocument.Open(outputPath, true))
            {
                // Dijital imza bilgisini ekle
                DigitalSignatureOriginPart originPart = document.AddNewPart<DigitalSignatureOriginPart>();
                
                // İmza özelliklerini ayarla
                var signatureProperties = document.PackageProperties;
                signatureProperties.Creator = certificate.Subject;
                signatureProperties.Created = DateTime.Now;
                signatureProperties.Modified = DateTime.Now;

                // İmza bilgisini kaydet
                document.Save();
            }

            return outputPath;
        }
    }
}