using System;
using System.IO;
using System.Runtime.CompilerServices;
using tr.gov.tubitak.uekae.esya.api.certificate.validation.policy;
using tr.gov.tubitak.uekae.esya.api.crypto.alg;
using tr.gov.tubitak.uekae.esya.api.infra.tsclient;
using WinFormEImza.Islemler;

namespace WinFormEImza.Nesneler
{
    /**
     * Provides required variables and functions for ASiC examples
     */

    public class CadesSampleBase : SampleBase
    {

        private static readonly string tempDataFolder;
        private static readonly string policyFile;
        private static readonly TSSettings tsSettings;
        private static ValidationPolicy policy;

        static CadesSampleBase()
        {
            try
            {
                tempDataFolder = getRootDir() + @"\cTemp";
                policyFile = getRootDir() + @"\config\certval-policy.xml";
                tsSettings = new TSSettings("http://zdsd.test3.kamusm.gov.tr/", 11166, "0eLivEY0", DigestAlg.SHA256);
            }
            catch (Exception e)
            {
                GenelIslemler.LogaYaz(" Hata : (" + e.Message + ")");
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public static ValidationPolicy GetPolicy()
        {
            if (policy == null)
                try
                {
                    policy = PolicyReader.readValidationPolicy(new FileStream(policyFile, FileMode.Open));
                }
                catch (FileNotFoundException e)
                {
                    GenelIslemler.LogaYaz(" Hata : ( Policy file could not be found! " + e.Message + ")");
                    ////throw new SystemException("Policy file could not be found", e);
                }
            return policy;
        }

        public static string getTempDataFolder()
        {
            return tempDataFolder;
        }

        public static TSSettings getTSSettings()
        {
            //for getting test TimeStamp or qualified TimeStamp account, mail to bilgi@kamusm.gov.tr.
            //This configuration, user ID (2) and password (PASSWORD), is invalid. 
            return tsSettings;
        }
    }
}
