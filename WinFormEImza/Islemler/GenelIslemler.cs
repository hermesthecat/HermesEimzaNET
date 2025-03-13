extern alias merged;
using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using WinFormEImza.Nesneler;

namespace WinFormEImza.Islemler
{
    public class GenelIslemler : CadesSampleBase
    {
        public const string SifrePublicKey = "ZzZ";
        public static byte[] SifreSalt = Encoding.ASCII.GetBytes("ZzzzzzzZ");
        public static string ROOT_DIR = @"C:\EImza";
        public static string AyarlarDosyasi = ROOT_DIR + @"\config\Ayarlar.xml";
        // gets only qualified certificates in smart card
        public static readonly bool IS_QUALIFIED = true;
        // the pin of the smart card
        public static string PIN_SMARTCARD = "";
        public static bool IseGunBoyuPinSorma = false;
        public static bool IseDebugMode = true;


        protected static string GetRootDir()
        {
            return ROOT_DIR;
        }

        public static string GetPin()
        {
            return PIN_SMARTCARD;
        }

        public static void SetPin(string s)
        {
            PIN_SMARTCARD = s;
        }

        public static bool IsQualified()
        {
            return IS_QUALIFIED;
        }

        public static void LogaYaz(string p)
        {
            if (Application.OpenForms.Count > 0)
            {
                WinFormEImza mainForm = (WinFormEImza)Application.OpenForms[0];
                ListBox lstLog = (ListBox)mainForm.Controls.Find("lstBoxLog", false)[0];
                lstLog.Invoke((MethodInvoker)delegate
                {
                    lstLog.Items.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " | " + p);
                });

                //// lstLog.Items.Add(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " | " + p);
            }
        }

        public static void SertifikaKamuSmKontrol(bool iseYenidenIndir)
        {
            try
            {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\.sertifikadeposu";
                Directory.CreateDirectory(path);
                if (iseYenidenIndir)
                {
                    if (File.Exists(path + @"\SertifikaDeposu.svt"))
                        File.Move(path + @"\SertifikaDeposu.svt", path + @"\SertifikaDeposu-" + string.Format("{0:yyyyMMddHHmmssfffffff}", DateTime.Now) + ".svt");

                    if (File.Exists(path + @"\SertifikaDeposu.xml"))
                        File.Move(path + @"\SertifikaDeposu.xml", path + @"\SertifikaDeposu-" + string.Format("{0:yyyyMMddHHmmssfffffff}", DateTime.Now) + ".xml");
                }
                if (!File.Exists(path + @"\SertifikaDeposu.svt"))
                {
                    using (WebClient client = new WebClient())
                    {
                        client.DownloadFile("http://depo.kamusm.gov.tr/depo/SertifikaDeposu.svt", path + @"\SertifikaDeposu.svt");
                    }
                }
                if (!File.Exists(path + @"\SertifikaDeposu.xml"))
                {
                    using (WebClient client = new WebClient())
                    {
                        client.DownloadFile("http://depo.kamusm.gov.tr/depo/SertifikaDeposu.xml", path + @"\SertifikaDeposu.xml");
                    }
                }
                if (iseYenidenIndir)
                {
                    GenelIslemler.LogaYaz(" Sertifika depoları indirildi...");
                }
                else
                {
                    GenelIslemler.LogaYaz(" Sertifika depoları kontrol edildi...");
                }
            }
            catch (Exception ex)
            {
                GenelIslemler.LogaYaz(" HATA: [SertifikaKamuSmKontrol] (" + ex.Message + ")");
            }
        }

        public static string Sifrele(string s)
        {
            Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(SifrePublicKey, SifreSalt);
            MemoryStream ms = new MemoryStream();
            StreamWriter sw = new StreamWriter(new CryptoStream(ms, new RijndaelManaged().CreateEncryptor(key.GetBytes(32), key.GetBytes(16)), CryptoStreamMode.Write));
            sw.Write(s);
            sw.Close();
            ms.Close();
            return Convert.ToBase64String(ms.ToArray());
        }

        public static string SifreCoz(string s)
        {
            Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(SifrePublicKey, SifreSalt);
            ICryptoTransform d = new RijndaelManaged().CreateDecryptor(key.GetBytes(32), key.GetBytes(16));
            byte[] bytes = Convert.FromBase64String(s);
            return new StreamReader(new CryptoStream(new MemoryStream(bytes), d, CryptoStreamMode.Read)).ReadToEnd();
        }

        internal static void DebugModAktifPasif(bool v)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(AyarlarDosyasi);
            xmlDoc.SelectSingleNode("/KOK/DebugModAktifPasif").InnerText = v ? "1" : "0";
            xmlDoc.Save(AyarlarDosyasi);
            IseGunBoyuPinSorma = v;
        }
      
        public static string SifreInputBoxGetir(string Prompt)
        {
            Form frmInput = new Form()
            {
                Size = new Size(230, 120),
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                Text = Prompt
            };
            Button btn = new Button()
            {
                Text = "OK",
                Location = new System.Drawing.Point(100, 40),
                Width = 80                
            };
            btn.Click += inputclose;
            TextBox txtbox = new TextBox()
            {
                Width = 170,
                Location = new System.Drawing.Point(10, 10),
                PasswordChar = '*'
            };
            frmInput.Controls.Add(btn);
            frmInput.Controls.Add(txtbox);
            frmInput.ShowDialog();
            return txtbox.Text;
        }

        public static void inputclose(object s, EventArgs e)
        {
            ((Form)(((Control)s).Parent)).Close();
        }

        internal static void AyarlarDosyasiniYukle()
        {
            XmlDocument xmlDoc = new XmlDocument();

            // check root dir
            if (!Directory.Exists(ROOT_DIR))
            {
                Directory.CreateDirectory(ROOT_DIR);
            }

            // check config dir
            if (!Directory.Exists(ROOT_DIR + @"\config"))
            {
                Directory.CreateDirectory(ROOT_DIR + @"\config");
            }

            if (!File.Exists(AyarlarDosyasi))
            {
                xmlDoc.LoadXml(@"<?xml version=""1.0"" encoding=""utf-8"" ?><KOK><GunBoyuPinSorma></GunBoyuPinSorma><DebugModAktifPasif></DebugModAktifPasif><Pin></Pin><Tarih></Tarih></KOK>");
                xmlDoc.Save(AyarlarDosyasi);
            }
            xmlDoc.Load(AyarlarDosyasi);
            string strTarih = xmlDoc.SelectSingleNode("/KOK/Tarih").InnerText;
            if (!string.IsNullOrEmpty(xmlDoc.SelectSingleNode("/KOK/DebugModAktifPasif").InnerText))
            {
                IseDebugMode = Convert.ToBoolean(int.Parse(xmlDoc.SelectSingleNode("/KOK/DebugModAktifPasif").InnerText));
            }
            if (DateTime.Now.ToString("yyyyMMdd") == strTarih)
            {
                IseGunBoyuPinSorma = Convert.ToBoolean(int.Parse(xmlDoc.SelectSingleNode("/KOK/GunBoyuPinSorma").InnerText));
                if (IseGunBoyuPinSorma && !string.IsNullOrEmpty(xmlDoc.SelectSingleNode("/KOK/Pin").InnerText))
                {
                    SetPin(SifreCoz(xmlDoc.SelectSingleNode("/KOK/Pin").InnerText));
                }
            }
        }
        internal static void GunBoyuPinSorma(bool v)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(AyarlarDosyasi);
            xmlDoc.SelectSingleNode("/KOK/GunBoyuPinSorma").InnerText = v ? "1" : "0";
            xmlDoc.SelectSingleNode("/KOK/Tarih").InnerText = DateTime.Now.ToString("yyyyMMdd");
            xmlDoc.SelectSingleNode("/KOK/Pin").InnerText = "";
            xmlDoc.Save(AyarlarDosyasi);
            IseGunBoyuPinSorma = v;
        }
        internal static void XmlPinKaydet(string pin)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(AyarlarDosyasi);
            xmlDoc.SelectSingleNode("/KOK/Tarih").InnerText = DateTime.Now.ToString("yyyyMMdd");
            xmlDoc.SelectSingleNode("/KOK/Pin").InnerText = Sifrele(pin);
            xmlDoc.Save(AyarlarDosyasi);
        }

    }
}
