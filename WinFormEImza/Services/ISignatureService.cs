using System.Threading.Tasks;
using WinFormEImza.Models;

namespace WinFormEImza.Services
{
    public interface ISignatureService
    {
        Task<SignatureResponse> QueueSignatureRequestAsync(SignatureRequest request);
        Task<SignatureResponse> GetSignatureStatusAsync(string batchId);
        Task ProcessSignatureRequestAsync(SignatureRequest request);
        Task SendCallbackNotificationAsync(string batchId, SignatureResponse response);
    }
}