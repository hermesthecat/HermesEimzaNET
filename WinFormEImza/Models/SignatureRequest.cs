using System;
using System.Collections.Generic;

namespace WinFormEImza.Models
{
    public class SignatureRequest
    {
        public List<DocumentInfo> Documents { get; set; }
        public string BatchId { get; set; }
        public string CallbackUrl { get; set; }
    }

    public class DocumentInfo
    {
        public string Content { get; set; }  // Base64 encoded document
        public string FileName { get; set; }  // Original file name with extension
        public string CertSerial { get; set; }
        public SignaturePosition SignaturePosition { get; set; }
    }

    public class SignaturePosition
    {
        public float? X { get; set; }  // Optional for PDF only
        public float? Y { get; set; }  // Optional for PDF only
    }

    public class SignatureResponse
    {
        public string BatchId { get; set; }
        public string StatusUrl { get; set; }
        public string Status { get; set; }  // Queued, Processing, Completed, PartiallyCompleted, Failed
        public List<DocumentResult> Results { get; set; }
    }

    public class DocumentResult
    {
        public string DocumentId { get; set; }
        public string Status { get; set; }  // Processing, Completed, Failed
        public string SignedContent { get; set; }  // Base64 encoded signed document
        public string ErrorMessage { get; set; }
    }
}