using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WinFormEImza.Models;
using WinFormEImza.Services;
using System.Collections.Generic;

namespace WinFormEImza.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SignatureController : ControllerBase
    {
        private readonly ISignatureService _signatureService;

        public SignatureController(ISignatureService signatureService)
        {
            _signatureService = signatureService;
        }

        [HttpPost("sign")]
        public async Task<ActionResult<SignatureResponse>> SignDocuments([FromBody] SignatureRequest request)
        {
            try
            {
                if (request.Documents == null || request.Documents.Count == 0)
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
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("status/{batchId}")]
        public async Task<ActionResult<SignatureResponse>> GetSignatureStatus(string batchId)
        {
            try
            {
                var status = await _signatureService.GetSignatureStatusAsync(batchId);
                if (status == null)
                {
                    return NotFound($"No signature batch found with id: {batchId}");
                }

                return Ok(status);
            }
            catch (Exception ex)
            {
                // Log error
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}