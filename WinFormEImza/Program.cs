using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WinFormEImza.Services;
using System.Windows.Forms;
using System;

namespace WinFormEImza
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            // Create and start the web application
            var builder = WebApplication.CreateBuilder();
            
            // Add services
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddSingleton<ISignatureService, SignatureService>();

            var app = builder.Build();

            // Configure middleware
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            // Start web application in background
            var webTask = app.RunAsync("http://localhost:5000");

            // Start Windows Forms application
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new WinFormEImza());

            // Wait for web application to complete
            webTask.Wait();
        }
    }
}
