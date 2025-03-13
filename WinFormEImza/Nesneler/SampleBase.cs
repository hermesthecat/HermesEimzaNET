using System;
using System.IO;
using tr.gov.tubitak.uekae.esya.api.common.util;
using WinFormEImza.Islemler;

namespace WinFormEImza.Nesneler
{
    public class SampleBase
    {
        // bundle root directory of project
        private static readonly string ROOT_DIR = GenelIslemler.ROOT_DIR;

        // gets only qualified certificates in smart card
        private static readonly bool IS_QUALIFIED = GenelIslemler.IS_QUALIFIED;

        // the pin of the smart card
        private static string PIN_SMARTCARD = GenelIslemler.PIN_SMARTCARD;

        static SampleBase()
        {
            try
            {
                LicenseUtil.setLicenseXml(new FileStream(ROOT_DIR + "/lisans/lisans.xml", FileMode.Open, FileAccess.Read));
                DateTime expirationDate = LicenseUtil.getExpirationDate();
                GenelIslemler.LogaYaz("License expiration date : " + expirationDate.ToShortDateString());

                /* // To set class path
                URL root = CadesSampleBase.class.getResource("/");
                String classPath = root.getPath();
                File binDir = new File(classPath);
                ROOT_DIR = binDir.getParentFile().getParent();
                */

                /* // To sign with pfx file
                String PFX_FILE = ROOT_DIR + "/sertifika deposu/testuc@test.com.tr_313729.pfx";
                String PFX_PASS = "313729";
                PfxSigner pfxSigner = new PfxSigner(SignatureAlg.RSA_SHA256, PFX_FILE, PFX_PASS.toCharArray());
                certificate = pfxSigner.getSignersCertificate();
                */

                /*
                string dir = Directory.GetCurrentDirectory();
                ROOT_DIR = Directory.GetParent(dir).Parent.Parent.Parent.FullName;
                if (dir.Contains("x86") || dir.Contains("x64"))
                {
                    ROOT_DIR = Directory.GetParent(ROOT_DIR).FullName;
                }
                */
            }
            catch (Exception e)
            {
                GenelIslemler.LogaYaz(" Hata : (" + e.Message + ")");
            }
        }


        protected static string getRootDir()
        {
            return GenelIslemler.ROOT_DIR;
        }

        protected static string getPin()
        {
            return GenelIslemler.PIN_SMARTCARD;
        }

        protected static void setPin(string s)
        {
            GenelIslemler.PIN_SMARTCARD = s;
        }

        protected static bool isQualified()
        {
            return GenelIslemler.IS_QUALIFIED;
        }
    }
}
