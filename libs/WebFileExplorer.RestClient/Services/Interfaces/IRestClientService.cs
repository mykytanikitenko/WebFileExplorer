using System.Threading.Tasks;

namespace WebFileExplorer.RestClient.Services.Interfaces
{
    public interface IRestClientService
    {
        Task<TResponse> PostRequestAsync<TRequest, TResponse>(string fullUrlRequest, TRequest request);
    }
}
