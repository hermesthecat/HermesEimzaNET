namespace WinFormEImza
{
    partial class WinFormEImza
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dgvBelgeler = new System.Windows.Forms.DataGridView();
            this.btnSeciliBelgeleriImzala = new System.Windows.Forms.Button();
            this.btnDosyaSec = new System.Windows.Forms.Button();
            this.lstBoxLog = new System.Windows.Forms.ListBox();
            this.lblLog = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSertifikaDeposuYenie = new System.Windows.Forms.Button();
            this.chkGunBoyuTekrarSorma = new System.Windows.Forms.CheckBox();
            this.chkDebugMod = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBelgeler)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvBelgeler
            // 
            this.dgvBelgeler.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvBelgeler.BackgroundColor = System.Drawing.Color.MintCream;
            this.dgvBelgeler.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBelgeler.Location = new System.Drawing.Point(14, 50);
            this.dgvBelgeler.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dgvBelgeler.Name = "dgvBelgeler";
            this.dgvBelgeler.RowHeadersVisible = false;
            this.dgvBelgeler.RowHeadersWidth = 51;
            this.dgvBelgeler.RowTemplate.Height = 24;
            this.dgvBelgeler.Size = new System.Drawing.Size(1133, 221);
            this.dgvBelgeler.TabIndex = 0;
            // 
            // btnSeciliBelgeleriImzala
            // 
            this.btnSeciliBelgeleriImzala.BackColor = System.Drawing.Color.LightGoldenrodYellow;
            this.btnSeciliBelgeleriImzala.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnSeciliBelgeleriImzala.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSeciliBelgeleriImzala.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnSeciliBelgeleriImzala.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.btnSeciliBelgeleriImzala.Location = new System.Drawing.Point(948, 279);
            this.btnSeciliBelgeleriImzala.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSeciliBelgeleriImzala.Name = "btnSeciliBelgeleriImzala";
            this.btnSeciliBelgeleriImzala.Size = new System.Drawing.Size(199, 31);
            this.btnSeciliBelgeleriImzala.TabIndex = 1;
            this.btnSeciliBelgeleriImzala.Text = "Seçili Belgeleri İmzala";
            this.btnSeciliBelgeleriImzala.UseVisualStyleBackColor = false;
            this.btnSeciliBelgeleriImzala.Click += new System.EventHandler(this.btnSeciliBelgeleriImzala_Click);
            // 
            // btnDosyaSec
            // 
            this.btnDosyaSec.BackColor = System.Drawing.Color.LightGoldenrodYellow;
            this.btnDosyaSec.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnDosyaSec.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnDosyaSec.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnDosyaSec.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.btnDosyaSec.Location = new System.Drawing.Point(14, 13);
            this.btnDosyaSec.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnDosyaSec.Name = "btnDosyaSec";
            this.btnDosyaSec.Size = new System.Drawing.Size(199, 29);
            this.btnDosyaSec.TabIndex = 2;
            this.btnDosyaSec.Text = "Dosya Seç";
            this.btnDosyaSec.UseVisualStyleBackColor = false;
            this.btnDosyaSec.Click += new System.EventHandler(this.btnDosyaSec_Click);
            // 
            // lstBoxLog
            // 
            this.lstBoxLog.BackColor = System.Drawing.Color.FloralWhite;
            this.lstBoxLog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lstBoxLog.FormattingEnabled = true;
            this.lstBoxLog.ItemHeight = 15;
            this.lstBoxLog.Location = new System.Drawing.Point(14, 330);
            this.lstBoxLog.Name = "lstBoxLog";
            this.lstBoxLog.Size = new System.Drawing.Size(1134, 180);
            this.lstBoxLog.TabIndex = 3;
            // 
            // lblLog
            // 
            this.lblLog.AutoSize = true;
            this.lblLog.BackColor = System.Drawing.Color.LightYellow;
            this.lblLog.Location = new System.Drawing.Point(12, 307);
            this.lblLog.Name = "lblLog";
            this.lblLog.Size = new System.Drawing.Size(28, 15);
            this.lblLog.TabIndex = 4;
            this.lblLog.Text = "Log";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(1047, 17);
            this.textBox1.MaxLength = 4;
            this.textBox1.Name = "textBox1";
            this.textBox1.PasswordChar = '*';
            this.textBox1.Size = new System.Drawing.Size(100, 21);
            this.textBox1.TabIndex = 5;
            this.textBox1.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.LightYellow;
            this.label1.Location = new System.Drawing.Point(960, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 15);
            this.label1.TabIndex = 6;
            this.label1.Text = "E-İmza Pin";
            this.label1.Visible = false;
            // 
            // btnSertifikaDeposuYenie
            // 
            this.btnSertifikaDeposuYenie.BackColor = System.Drawing.Color.LightGoldenrodYellow;
            this.btnSertifikaDeposuYenie.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnSertifikaDeposuYenie.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSertifikaDeposuYenie.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnSertifikaDeposuYenie.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.btnSertifikaDeposuYenie.Location = new System.Drawing.Point(755, 11);
            this.btnSertifikaDeposuYenie.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSertifikaDeposuYenie.Name = "btnSertifikaDeposuYenie";
            this.btnSertifikaDeposuYenie.Size = new System.Drawing.Size(199, 29);
            this.btnSertifikaDeposuYenie.TabIndex = 7;
            this.btnSertifikaDeposuYenie.Text = "Sertifika Deposu Yenile";
            this.btnSertifikaDeposuYenie.UseVisualStyleBackColor = false;
            this.btnSertifikaDeposuYenie.Visible = false;
            this.btnSertifikaDeposuYenie.Click += new System.EventHandler(this.btnSertifikaDeposuYenie_Click);
            // 
            // chkGunBoyuTekrarSorma
            // 
            this.chkGunBoyuTekrarSorma.AutoSize = true;
            this.chkGunBoyuTekrarSorma.Location = new System.Drawing.Point(219, 20);
            this.chkGunBoyuTekrarSorma.Name = "chkGunBoyuTekrarSorma";
            this.chkGunBoyuTekrarSorma.Size = new System.Drawing.Size(139, 19);
            this.chkGunBoyuTekrarSorma.TabIndex = 8;
            this.chkGunBoyuTekrarSorma.Text = "Gün boyu PIN sorma";
            this.chkGunBoyuTekrarSorma.UseVisualStyleBackColor = true;
            this.chkGunBoyuTekrarSorma.CheckedChanged += new System.EventHandler(this.chkGunBoyuTekrarSorma_CheckedChanged);
            // 
            // chkDebugMod
            // 
            this.chkDebugMod.AutoSize = true;
            this.chkDebugMod.Location = new System.Drawing.Point(382, 21);
            this.chkDebugMod.Name = "chkDebugMod";
            this.chkDebugMod.Size = new System.Drawing.Size(91, 19);
            this.chkDebugMod.TabIndex = 9;
            this.chkDebugMod.Text = "Debug Mod";
            this.chkDebugMod.UseVisualStyleBackColor = true;
            this.chkDebugMod.CheckedChanged += new System.EventHandler(this.chkDebugMod_CheckedChanged);
            // 
            // WinFormEImza
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Honeydew;
            this.ClientSize = new System.Drawing.Size(1160, 562);
            this.Controls.Add(this.chkDebugMod);
            this.Controls.Add(this.chkGunBoyuTekrarSorma);
            this.Controls.Add(this.btnSertifikaDeposuYenie);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.lblLog);
            this.Controls.Add(this.lstBoxLog);
            this.Controls.Add(this.btnDosyaSec);
            this.Controls.Add(this.btnSeciliBelgeleriImzala);
            this.Controls.Add(this.dgvBelgeler);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "WinFormEImza";
            this.Text = "Hermes E-imza";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.WinFormEImza_FormClosing);
            this.Load += new System.EventHandler(this.WinFormEImza_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBelgeler)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvBelgeler;
        private System.Windows.Forms.Button btnSeciliBelgeleriImzala;
        private System.Windows.Forms.Button btnDosyaSec;
        private System.Windows.Forms.ListBox lstBoxLog;
        private System.Windows.Forms.Label lblLog;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSertifikaDeposuYenie;
        private System.Windows.Forms.CheckBox chkGunBoyuTekrarSorma;
        private System.Windows.Forms.CheckBox chkDebugMod;
    }
}

