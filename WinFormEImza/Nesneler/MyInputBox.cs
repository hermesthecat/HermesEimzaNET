using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinFormEImza.Nesneler
{
    internal class MyInputBox
    {
        private string SifreInputBoxGetir(string Prompt)
        {
            Form frmInput = new Form();
            frmInput.Size = new Size(300, 90);
            frmInput.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            System.Windows.Forms.Button btn = new System.Windows.Forms.Button();
            btn.Text = "OK";
            frmInput.Controls.Add(btn);
            frmInput.MaximizeBox = false;
            frmInput.MinimizeBox = false;
            btn.Location = new Point(210, 40);
            btn.Width = 80;
            btn.Click += inputclose;
            System.Windows.Forms.TextBox txtbox = new System.Windows.Forms.TextBox();
            txtbox.Width = 280;
            frmInput.Controls.Add(txtbox);
            txtbox.Location = new Point(10, 10);
            frmInput.Text = "Enter Password";
            txtbox.PasswordChar = '*';
            frmInput.ShowDialog();
            return txtbox.Text;
        }

        public void inputclose(object s, EventArgs e)
        {
            ((Form)(((Control)s).Parent)).Close();
        }
    }
}
