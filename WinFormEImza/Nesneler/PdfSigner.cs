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
        public PdfSigner()
        {
            // LicenseUtil.setLicenseXml(new FileStream(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "lisans.xml"), FileMode.Open, FileAccess.Read));
        }
        static public ICrlClient crl;
        static public List<ICrlClient> crlList;
        static public OcspClientBouncyCastle ocsp;
        private static Object lockSign = new Object();
        private static Object lockToken = new Object();
        private X509Certificate2[] generateCertificateChain(X509Certificate2 signingCertificate)
        {
            X509Chain Xchain = new X509Chain();
            Xchain.ChainPolicy.ExtraStore.Add(signingCertificate);
            Xchain.Build(signingCertificate); // Whole chain!
            X509Certificate2[] chain = new X509Certificate2[Xchain.ChainElements.Count];
            int index = 0;
            foreach (X509ChainElement element in Xchain.ChainElements)
            {
                chain[index++] = element.Certificate;
            }
            return chain;
        }


        public string SignPDF(PdfRequestDTO req)
        {
            string sonuc = "";
            MemoryStream stream = new MemoryStream();
            try
            {
                //ITSAClient tsaClient = new TSAClientBouncyCastle("http://timestamp.sectigo.com");

                X509Certificate2 signingCertificate;
                IExternalSignature externalSignature;
                this.SelectSignature(req, out signingCertificate, out externalSignature);
                X509Certificate2[] chain = generateCertificateChain(signingCertificate);
                ICollection<Org.BouncyCastle.X509.X509Certificate> Bouncychain = chainToBouncyCastle(chain);
                ocsp = new OcspClientBouncyCastle();
                crl = new merged.iTextSharp.text.pdf.security.CrlClientOnline(Bouncychain);
                PdfReader pdfReader = new PdfReader(req.KaynakPdfYolu);
                FileStream signedPdf = new FileStream(req.HedefPdfYolu, FileMode.Create);  //the output pdf file
                PdfStamper pdfStamper = PdfStamper.CreateSignature(pdfReader, signedPdf, '\0', null, true);
                PdfSignatureAppearance signatureAppearance = pdfStamper.SignatureAppearance;
                crlList = new List<ICrlClient>();
                crlList.Add(crl);
                lock (lockSign)
                {
                    //MakeSignature.SignDetached(signatureAppearance, externalSignature, Bouncychain, crlList, ocsp, tsaClient, 0, CryptoStandard.CMS);
                    MakeSignature.SignDetached(signatureAppearance, externalSignature, Bouncychain, crlList, ocsp, null, 0, CryptoStandard.CMS);
                }
            }
            catch (Exception ex)
            {
                GenelIslemler.LogaYaz(" HATA: [SignPDF] (" + ex.Message + ")");
            }
            return sonuc;
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
                GenelIslemler.LogaYaz(" HATA: [SelectSignature] (" + ex.Message + ")");
            }
        }
    }
}
