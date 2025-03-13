extern alias ma3Bouncy;
extern alias merged;
using merged::iTextSharp.text.pdf;
using merged::iTextSharp.text.pdf.security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using WinFormEImza.Islemler;

namespace WinFormEImza.Nesneler
{
    public class PdfSigner
    {
        static public ICrlClient crl;
        static public List<ICrlClient> crlList;
        static public OcspClientBouncyCastle ocsp;
        private static Object lockSign = new Object();
        private static Object lockToken = new Object();

        public string SignPDF(PdfRequestDTO req)
        {
            string sonuc = "";
            string tempInputPath = null;
            string tempOutputPath = null;

            try
            {
                // Handle base64 content if provided
                if (req.IsBase64Content && !string.IsNullOrEmpty(req.PdfContent))
                {
                    tempInputPath = Path.GetTempFileName();
                    byte[] pdfBytes = Convert.FromBase64String(req.PdfContent);
                    File.WriteAllBytes(tempInputPath, pdfBytes);
                    req.KaynakPdfYolu = tempInputPath;

                    tempOutputPath = Path.GetTempFileName();
                    req.HedefPdfYolu = tempOutputPath;
                }

                X509Certificate2 signingCertificate;
                IExternalSignature externalSignature;
                this.SelectSignature(req, out signingCertificate, out externalSignature);
                
                X509Certificate2[] chain = generateCertificateChain(signingCertificate);
                ICollection<Org.BouncyCastle.X509.X509Certificate> Bouncychain = chainToBouncyCastle(chain);
                
                ocsp = new OcspClientBouncyCastle();
                crl = new merged::iTextSharp.text.pdf.security.CrlClientOnline(Bouncychain);
                
                using (PdfReader pdfReader = new PdfReader(req.KaynakPdfYolu))
                using (FileStream signedPdf = new FileStream(req.HedefPdfYolu, FileMode.Create))
                {
                    PdfStamper pdfStamper = PdfStamper.CreateSignature(pdfReader, signedPdf, '\0', null, true);
                    PdfSignatureAppearance signatureAppearance = pdfStamper.SignatureAppearance;

                    // Set signature position if provided
                    if (req.SignatureX.HasValue && req.SignatureY.HasValue)
                    {
                        signatureAppearance.SetVisibleSignature(
                            new iTextSharp.text.Rectangle(
                                req.SignatureX.Value,
                                req.SignatureY.Value,
                                req.SignatureX.Value + 100, // Width
                                req.SignatureY.Value + 50   // Height
                            ),
                            1,  // First page
                            null
                        );
                    }

                    crlList = new List<ICrlClient>();
                    crlList.Add(crl);
                    
                    lock (lockSign)
                    {
                        MakeSignature.SignDetached(signatureAppearance, externalSignature, Bouncychain, crlList, ocsp, null, 0, CryptoStandard.CMS);
                    }

                    // If base64 content was provided, read the signed file and convert back to base64
                    if (req.IsBase64Content)
                    {
                        byte[] signedBytes = File.ReadAllBytes(req.HedefPdfYolu);
                        sonuc = Convert.ToBase64String(signedBytes);
                    }
                    else
                    {
                        sonuc = req.HedefPdfYolu;
                    }
                }
            }
            catch (Exception ex)
            {
                GenelIslemler.LogaYaz($"HATA: [SignPDF] ({ex.Message})");
                throw;
            }
            finally
            {
                // Cleanup temporary files if they were created
                if (!string.IsNullOrEmpty(tempInputPath) && File.Exists(tempInputPath))
                {
                    File.Delete(tempInputPath);
                }
                if (!string.IsNullOrEmpty(tempOutputPath) && File.Exists(tempOutputPath))
                {
                    File.Delete(tempOutputPath);
                }
            }
            return sonuc;
        }

        private X509Certificate2[] generateCertificateChain(X509Certificate2 signingCertificate)
        {
            X509Chain Xchain = new X509Chain();
            Xchain.ChainPolicy.ExtraStore.Add(signingCertificate);
            Xchain.Build(signingCertificate);
            X509Certificate2[] chain = new X509Certificate2[Xchain.ChainElements.Count];
            int index = 0;
            foreach (X509ChainElement element in Xchain.ChainElements)
            {
                chain[index++] = element.Certificate;
            }
            return chain;
        }

        private static ICollection<Org.BouncyCastle.X509.X509Certificate> chainToBouncyCastle(X509Certificate2[] chain)
        {
            Org.BouncyCastle.X509.X509CertificateParser cp = new Org.BouncyCastle.X509.X509CertificateParser();
            ICollection<Org.BouncyCastle.X509.X509Certificate> Bouncychain = new List<Org.BouncyCastle.X509.X509Certificate>();
            foreach (var item in chain)
            {
                Bouncychain.Add(cp.ReadCertificate(item.RawData));
            }
            return Bouncychain;
        }

        private void SelectSignature(PdfRequestDTO req, out X509Certificate2 CERTIFICATE, out IExternalSignature externalSignature)
        {
            try
            {
                SmartCardManager smartCardManager = SmartCardManager.getInstance();
                var smartCardCertificate = smartCardManager.getSignatureCertificate(false, false);
                var signer = smartCardManager.getSigner(req.DonglePassword, smartCardCertificate);
                CERTIFICATE = smartCardCertificate.asX509Certificate2();
                externalSignature = new SmartCardSignature(signer, CERTIFICATE, "SHA-256");
            }
            catch (Exception ex)
            {
                CERTIFICATE = null;
                externalSignature = null;
                GenelIslemler.LogaYaz($"HATA: [SelectSignature] ({ex.Message})");
                throw;
            }
        }
    }
}
