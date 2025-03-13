using System;
using System.Windows.Forms;
using Microsoft.Owin.Hosting;

namespace WinFormEImza
{
    static class Program
    {
        private static IDisposable _webApp;

        [STAThread]
        static void Main()
        {
            // Start OWIN host for Web API
            string baseAddress = "http://localhost:5000/";
            
            try
            {
                _webApp = WebApp.Start<Startup>(url: baseAddress);
                Console.WriteLine("Web API is running at " + baseAddress);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Web API başlatılamadı: {ex.Message}", "Hata", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Start Windows Forms application
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            try
            {
                Application.Run(new WinFormEImza());
            }
            finally
            {
                // Dispose Web API host when application closes
                if (_webApp != null)
                {
                    _webApp.Dispose();
                }
            }
        }
    }
}
