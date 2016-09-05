using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WebFileExplorer.RestClient.Services.Interfaces;
using System.Text;

namespace WebFileExplorer.RestClient.Services
{
    internal class RestClientService : IRestClientService
    {
        public async Task<TResponse> PostRequestAsync<TRequest, TResponse>(string fullUrlRequest, TRequest request)
        {
            var serializedRequest = await Task.Run(() => JsonConvert.SerializeObject(request));
            var httpContent = new StringContent(serializedRequest, Encoding.UTF8, "application/json");
            using (var httpClient = new HttpClient())
            {
                var httpResponse = await httpClient.PostAsync(fullUrlRequest, httpContent);
                if (httpResponse.Content == null)
                    return default(TResponse);

                var responseContent = await httpResponse.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<TResponse>(responseContent);
            }
        }
    }
}
