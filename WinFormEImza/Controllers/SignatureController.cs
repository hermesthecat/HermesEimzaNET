using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using WinFormEImza.Models;
using WinFormEImza.Services;

namespace WinFormEImza.Controllers
{
    public class SignatureController : ApiController
    {
        private readonly ISignatureService _signatureService;

        public SignatureController()
        {
            _signatureService = new SignatureService();
        }

        [HttpPost]
        [Route("api/signature/sign")]
        public async Task<IHttpActionResult> SignDocuments([FromBody] SignatureRequest request)
        {
            try
            {
                if (request?.Documents == null || request.Documents.Count == 0)
                {
                    return BadRequest("No documents provided");
                }

                // Generate BatchId if not provided
                if (string.IsNullOrEmpty(request.BatchId))
                {
                    request.BatchId = Guid.NewGuid().ToString();
                }

                var response = await _signatureService.QueueSignatureRequestAsync(request);
                
                return Ok(new SignatureResponse
                {
                    BatchId = response.BatchId,
                    StatusUrl = $"/api/signature/status/{response.BatchId}",
                    Status = "Pending",
                    Results = new List<DocumentResult>()
                });
            }
            catch (Exception ex)
            {
                // Log error
                return InternalServerError(ex);
            }
        }

        [HttpGet]
        [Route("api/signature/status/{batchId}")]
        public async Task<IHttpActionResult> GetSignatureStatus(string batchId)
        {
            try
            {
                var status = await _signatureService.GetSignatureStatusAsync(batchId);
                if (status == null)
                {
                    return NotFound();
                }

                return Ok(status);
            }
            catch (Exception ex)
            {
                // Log error
                return InternalServerError(ex);
            }
        }
    }
}