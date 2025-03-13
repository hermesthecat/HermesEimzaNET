using WinFormEImza.Nesneler;
using Newtonsoft.Json;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using WinFormEImza.Islemler;

namespace WinFormEImza
{
    public partial class WinFormEImza : Form
    {
        private bool iseIlkYukleme;
        public WinFormEImza()
        {
            InitializeComponent();
        }

        private void WinFormEImza_Load(object sender, EventArgs e)
        {
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                                   | SecurityProtocolType.Tls11
                                   | SecurityProtocolType.Tls12
                                   | SecurityProtocolType.Ssl3;
                ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

                GenelIslemler.LogaYaz(" Uygulama başlatıldı...");
                GenelIslemler.AyarlarDosyasiniYukle();
                iseIlkYukleme = true;
                chkGunBoyuTekrarSorma.Checked = GenelIslemler.IseGunBoyuPinSorma;
                chkDebugMod.Checked = GenelIslemler.IseDebugMode;
                iseIlkYukleme = false;
                GenelIslemler.LogaYaz(" Ayarlar alındı...");
                GenelIslemler.SertifikaKamuSmKontrol(false);
                gridResetle();
            }
            catch (Exception ex)
            {
                GenelIslemler.LogaYaz(" HATA [HermesEImza_Load] (" + ex.Message + ")");
                MessageBox.Show("HermesEImza_Load", " Exception [HermesEImza_Load] (" + ex.Message + ")", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void gridResetle()
        {
            try
            {
                GridIslemleri objGridIslem = new GridIslemleri();
                objGridIslem.GridDoldur(dgvBelgeler);
                dgvBelgeler.Refresh();
                objGridIslem.GridiAyarla(dgvBelgeler);
            }
            catch (Exception ex)
            {
                GenelIslemler.LogaYaz(" HATA [gridResetle] (" + ex.Message + ")");
            }
        }

        private void WinFormEImza_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                string filesToDelete = @"*_LogDetay.txt";
                string[] fileList = Directory.GetFiles(GenelIslemler.ROOT_DIR, filesToDelete);
                foreach (string file in fileList)
                {
                    if (File.GetCreationTime(file) < DateTime.Now.AddDays(-7))
                    {
                        File.Delete(file);
                    }
                }
                string[] arrStr = lstBoxLog.Items.Cast<string>().ToArray();
                char[] charDizi = string.Join("", arrStr).ToCharArray();
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(charDizi);

                File.AppendAllLines(GenelIslemler.ROOT_DIR + "\\" + DateTime.Now.ToString("yyyyMMdd") + "_LogDetay.txt", arrStr);
            }
            catch (Exception ex)
            {
                GenelIslemler.LogaYaz(" HATA [HermesEImza_FormClosing] (" + ex.Message + ")");
            }
        }


        private static string inputAl()
        {
            if (GenelIslemler.IseGunBoyuPinSorma && !string.IsNullOrEmpty(GenelIslemler.GetPin()))
            {
                return GenelIslemler.GetPin();
            }
            else
            {
                return GenelIslemler.SifreInputBoxGetir("Lütfen e-imza şifrenizi girin:");
            }
        }

        private void btnDosyaSec_Click(object sender, EventArgs e)
        {
            string _dosyaYolu = "";
            OpenFileDialog dosyaAcmaPenceresi = new OpenFileDialog();
            dosyaAcmaPenceresi.InitialDirectory = "C:/";
            dosyaAcmaPenceresi.Filter = "PDF Dosyaları|*.pdf";
            dosyaAcmaPenceresi.Title = "Bir dosya seçin";
            dosyaAcmaPenceresi.RestoreDirectory = true;
            if (dosyaAcmaPenceresi.ShowDialog() == DialogResult.OK)
            {
                _dosyaYolu = dosyaAcmaPenceresi.FileName;
            }
            new GridIslemleri().GrideElemanEkle(dgvBelgeler, new ImzaBelge()
            {
                IseSecili = true,
                DosyaYolu = _dosyaYolu,
                HedefUploadQueryString = "",
                HedefUploadUrl = ""
            });
        }

        private void btnSeciliBelgeleriImzala_Click(object sender, EventArgs e)
        {
            string eImzaSifre = inputAl();
            if (!string.IsNullOrEmpty(eImzaSifre))
            {
                if (GenelIslemler.IseGunBoyuPinSorma && string.IsNullOrEmpty(GenelIslemler.GetPin()))
                {
                    GenelIslemler.XmlPinKaydet(eImzaSifre);
                }
                GenelIslemler.SetPin(eImzaSifre);
                foreach (DataGridViewRow row in dgvBelgeler.Rows)
                {
                    if ((bool)((DataGridViewCheckBoxCell)row.Cells[0]).Value)
                    {
                        string imzalanacakDosya = row.Cells[1].Value.ToString();
                        string imzalanmisDosya = Regex.Replace(imzalanacakDosya, ".pdf", "-HermesEImzali.pdf", RegexOptions.IgnoreCase);
                        string hedefUploadUrl = row.Cells[2].Value.ToString();
                        string hedefUploadQueryString = row.Cells[3].Value.ToString();
                        PdfRequestDTO requestDTO = new PdfRequestDTO()
                        {
                            DonglePassword = GenelIslemler.GetPin(),
                            KaynakPdfYolu = imzalanacakDosya,
                            HedefPdfYolu = imzalanmisDosya
                        };
                        try
                        {
                            string imzaSonuc = dosyayiImzala(requestDTO);
                            GenelIslemler.LogaYaz(" Dosya imzalama sonuc: (" + imzaSonuc + ")");
                        }
                        catch (Exception ex)
                        {
                            GenelIslemler.LogaYaz(" Dosya imzalama hata: (" + ex.Message + ")");
                        }                       
                    }
                }
                gridResetle();
            }
        }

        private string dosyayiImzala(PdfRequestDTO requestDTO)
        {
            GenelIslemler.LogaYaz(" " + requestDTO.KaynakPdfYolu + " dosyası imzalanıyor, lütfen bekleyiniz.");
            SignatureManager signManager = new SignatureManager();
            return signManager.SignPdf(requestDTO);
        }

        private void btnSertifikaDeposuYenie_Click(object sender, EventArgs e)
        {
            GenelIslemler.SertifikaKamuSmKontrol(true);
        }
        private void chkGunBoyuTekrarSorma_CheckedChanged(object sender, EventArgs e)
        {
            if (!iseIlkYukleme)
                GenelIslemler.GunBoyuPinSorma(chkGunBoyuTekrarSorma.Checked);
        }

        private void chkDebugMod_CheckedChanged(object sender, EventArgs e)
        {
            if (!iseIlkYukleme)
                GenelIslemler.DebugModAktifPasif(chkDebugMod.Checked);
        }

    }
}
