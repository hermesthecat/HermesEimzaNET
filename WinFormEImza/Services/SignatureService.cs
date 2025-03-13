using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Net.Http;
using WinFormEImza.Models;
using WinFormEImza.Nesneler;
using System.Text.Json;
using System.Text;

namespace WinFormEImza.Services
{
    public class SignatureService : ISignatureService
    {
        private readonly ConcurrentDictionary<string, SignatureResponse> _signatureStatus;
        private readonly PdfSigner _pdfSigner;
        private readonly OfficeDocumentSigner _officeSigner;
        private readonly HttpClient _httpClient;

        public SignatureService()
        {
            _signatureStatus = new ConcurrentDictionary<string, SignatureResponse>();
            _pdfSigner = new PdfSigner();
            _officeSigner = new OfficeDocumentSigner();
            _httpClient = new HttpClient();
        }

        private bool IsOfficeDocument(string fileName)
        {
            string ext = Path.GetExtension(fileName).ToLower();
            return ext == ".docx" || ext == ".xlsx";
        }

        public Task<SignatureResponse> QueueSignatureRequestAsync(SignatureRequest request)
        {
            var response = new SignatureResponse
            {
                BatchId = request.BatchId,
                Status = "Queued",
                Results = new List<DocumentResult>()
            };

            _signatureStatus.TryAdd(request.BatchId, response);

            // Start processing in background
            Task.Run(() => ProcessSignatureRequestAsync(request));

            return Task.FromResult(response);
        }

        public Task<SignatureResponse> GetSignatureStatusAsync(string batchId)
        {
            _signatureStatus.TryGetValue(batchId, out var status);
            return Task.FromResult(status);
        }

        public async Task ProcessSignatureRequestAsync(SignatureRequest request)
        {
            var response = new SignatureResponse
            {
                BatchId = request.BatchId,
                Status = "Processing",
                Results = new List<DocumentResult>()
            };

            try
            {
                foreach (var doc in request.Documents)
                {
                    var result = new DocumentResult
                    {
                        DocumentId = Guid.NewGuid().ToString(),
                        Status = "Processing"
                    };

                    response.Results.Add(result);

                    try
                    {
                        // Decode base64 content
                        byte[] fileBytes = Convert.FromBase64String(doc.Content);
                        string tempInputPath = Path.GetTempFileName();
                        string tempOutputPath = Path.GetTempFileName();
                        string fileName = doc.FileName ?? "document" + Path.GetExtension(tempInputPath);

                        File.WriteAllBytes(tempInputPath, fileBytes);

                        if (IsOfficeDocument(fileName))
                        {
                            // Get certificate for Office document signing
                            SmartCardManager smartCardManager = SmartCardManager.getInstance();
                            var smartCardCertificate = smartCardManager.getSignatureCertificate(false, false);
                            var certificate = smartCardCertificate.asX509Certificate2();

                            // Sign Office document
                            _officeSigner.SignDocument(tempInputPath, tempOutputPath, certificate);
                        }
                        else
                        {
                            // Sign PDF document
                            var pdfRequest = new PdfRequestDTO
                            {
                                KaynakPdfYolu = tempInputPath,
                                HedefPdfYolu = tempOutputPath,
                                DonglePassword = "", // Handle smartcard password
                                IsBase64Content = true,
                                SignatureX = doc.SignaturePosition?.X,
                                SignatureY = doc.SignaturePosition?.Y
                            };

                            _pdfSigner.SignPDF(pdfRequest);
                        }

                        if (File.Exists(tempOutputPath))
                        {
                            // Read signed file and convert to base64
                            byte[] signedBytes = File.ReadAllBytes(tempOutputPath);
                            result.SignedContent = Convert.ToBase64String(signedBytes);
                            result.Status = "Completed";
                        }
                        else
                        {
                            result.Status = "Failed";
                            result.ErrorMessage = "Signed file not created";
                        }

                        // Cleanup temp files
                        File.Delete(tempInputPath);
                        File.Delete(tempOutputPath);
                    }
                    catch (Exception ex)
                    {
                        result.Status = "Failed";
                        result.ErrorMessage = ex.Message;
                    }
                }

                response.Status = response.Results.TrueForAll(r => r.Status == "Completed") 
                    ? "Completed" 
                    : "PartiallyCompleted";
            }
            catch (Exception ex)
            {
                response.Status = "Failed";
                foreach (var result in response.Results)
                {
                    if (result.Status == "Processing")
                    {
                        result.Status = "Failed";
                        result.ErrorMessage = ex.Message;
                    }
                }
            }

            // Update status
            _signatureStatus.TryUpdate(request.BatchId, response, _signatureStatus[request.BatchId]);

            // Send callback if URL provided
            if (!string.IsNullOrEmpty(request.CallbackUrl))
            {
                await SendCallbackNotificationAsync(request.BatchId, response);
            }
        }

        public async Task SendCallbackNotificationAsync(string batchId, SignatureResponse response)
        {
            try
            {
                var status = _signatureStatus[batchId];
                var content = new StringContent(
                    JsonSerializer.Serialize(status),
                    Encoding.UTF8,
                    "application/json");

                await _httpClient.PostAsync(status.StatusUrl, content);
            }
            catch (Exception ex)
            {
                // Log callback error
                Console.WriteLine($"Callback failed for batch {batchId}: {ex.Message}");
            }
        }
    }
}