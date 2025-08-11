using Microsoft.Extensions.Configuration;
using PruebaTecnica.Application.Contracts.Infrastructure;
using PruebaTecnica.Application.Models.Requests;
using System.Net.Http.Json;

namespace PruebaTecnica.Infrastructure.Services
{
    public class MockService : IMockService
    {
        private readonly HttpClient _httpClient;
        private readonly string _simulateUrl;
        public MockService(HttpClient httpClient, IConfiguration configuration)
        {          
            _httpClient = httpClient;
            _simulateUrl = configuration["MockService:SimulateUrl"]!;
        }

        public async Task<string> SimulateAcquirerResponse(TransactionRequest request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(_simulateUrl, request);
                var result = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
                return result["isoCode"];
            }
            catch (Exception ex)
            {
                throw new Exception($"Conexión con Mock fallida:");
            }
        }

    }
}
