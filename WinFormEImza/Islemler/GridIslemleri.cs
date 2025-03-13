using WinFormEImza.Nesneler;
using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormEImza.Islemler
{
    internal class GridIslemleri
    {
        public void GridiAyarla(DataGridView dgv)
        {
            try
            {
                dgv.AllowUserToAddRows = false;
                DataGridViewButtonColumn btnPdfGoruntule = new DataGridViewButtonColumn();
                btnPdfGoruntule.Name = "btnPdfGoruntule";
                btnPdfGoruntule.Text = "Pdf Görüntüle";
                btnPdfGoruntule.UseColumnTextForButtonValue = true;
                btnPdfGoruntule.FlatStyle = FlatStyle.Popup;
                btnPdfGoruntule.DefaultCellStyle.BackColor = Color.LightGoldenrodYellow;
                btnPdfGoruntule.DefaultCellStyle.ForeColor = Color.DarkSlateGray;
                if (dgv.Columns["btnPdfGoruntule"] == null)
                {
                    dgv.Columns.Insert(4, btnPdfGoruntule);
                }

                dgv.Columns[2].Visible = false;
                dgv.Columns[3].Visible = false;

                dgv.CellClick += dgvBelgeler_CellClick;
                dgv.Columns[0].HeaderText = "Seç";
                dgv.Columns[1].HeaderText = "Dosya Yolu";
                dgv.Columns[2].HeaderText = "";
                dgv.Columns[3].HeaderText = "";
                dgv.Columns[4].HeaderText = "Dosya Görüntüle";


                dgv.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dgv.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgv.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dgv.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dgv.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            }
            catch (Exception ex)
            {
                GenelIslemler.LogaYaz(" HATA [GridiAyarla] (" + ex.Message + ")");
            }
        }

        private void dgvBelgeler_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;
            if (e.ColumnIndex == dgv.Columns["btnPdfGoruntule"].Index)
            {
                ////MessageBox.Show(dgv.Rows[e.RowIndex].Cells[1].Value.ToString());
                Process.Start(dgv.Rows[e.RowIndex].Cells[1].Value.ToString());
            }
        }

        public void GridDoldur(DataGridView dgv)
        {
            try
            {
                dgv.DataSource = null;
                DataTable dataTable = new DataTable();
                dataTable.Columns.Add(new DataColumn("IseSecili", typeof(bool)));
                dataTable.Columns.Add(new DataColumn("DosyaYolu", typeof(string)));
                dataTable.Columns.Add(new DataColumn("HedefUploadUrl", typeof(string)));
                dataTable.Columns.Add(new DataColumn("HedefUploadQueryString", typeof(string)));
                dgv.DataSource = dataTable;
                ////this.GrideElemanEkle(dgv, new ImzaBelge() { IseSecili = false, DosyaYolu = @"c:\WinFormEImza\TEMP\Deneme.pdf" });
                ////this.GrideElemanEkle(dgv, new ImzaBelge() { IseSecili = false, DosyaYolu = @"c:\WinFormEImza\TEMP\Deneme.pdf" });
                ////this.GrideElemanEkle(dgv, new ImzaBelge() { IseSecili = false, DosyaYolu = @"c:\WinFormEImza\TEMP\Deneme3.pdf" });
                ////this.GrideElemanEkle(dgv, new ImzaBelge() { IseSecili = true, DosyaYolu = @"c:\WinFormEImza\TEMP\Deneme4.pdf" });
                ////this.GrideElemanEkle(dgv, new ImzaBelge() { IseSecili = false, DosyaYolu = @"c:\WinFormEImza\TEMP\Deneme5.pdf" });
                ////this.GrideElemanEkle(dgv, new ImzaBelge() { IseSecili = true, DosyaYolu = @"c:\WinFormEImza\TEMP\Deneme6.pdf" });
            }
            catch (Exception ex)
            {
                GenelIslemler.LogaYaz(" HATA [GridDoldur] (" + ex.Message + ")");
            }
        }


        public void GrideElemanEkle(DataGridView dgv, ImzaBelge eleman)
        {
            DataTable dataTable = (DataTable)dgv.DataSource;
            if (dataTable == null)
            {
                dataTable = new DataTable();
                dataTable.Columns.Add(new DataColumn("IseSecili", typeof(bool)));
                dataTable.Columns.Add(new DataColumn("DosyaYolu", typeof(string)));
                dataTable.Columns.Add(new DataColumn("HedefUploadUrl", typeof(string)));
                dataTable.Columns.Add(new DataColumn("HedefUploadQueryString", typeof(string)));
            }
            DataRow drToAdd = dataTable.NewRow();
            drToAdd["IseSecili"] = eleman.IseSecili;
            drToAdd["DosyaYolu"] = eleman.DosyaYolu;
            drToAdd["HedefUploadUrl"] = eleman.HedefUploadUrl;
            drToAdd["HedefUploadQueryString"] = eleman.HedefUploadQueryString;
            dataTable.Rows.Add(drToAdd);
            dataTable.AcceptChanges();
            dgv.DataSource = dataTable;
        }
    }
}
