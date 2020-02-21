using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ParkingLot.Web.Extensions
{
    public static class HttpClientExtensions
    {
        public static async Task<T> GetJsonAsync<T>(this HttpClient http, string requestUri)
        {
            await using var stream = await http.GetStreamAsync(requestUri);
            var response = await JsonSerializer.DeserializeAsync<T>(stream, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            return response;
        }

        public static async Task<HttpResponseMessage> PostJsonAsync(this HttpClient http, string requestUri, object content)
        {
            var json = JsonSerializer.Serialize(content);
            var response = await http.PostAsync(requestUri, new StringContent(json, Encoding.UTF8, "application/json"));
            return response;
        }

        public static async Task<T> PostJsonAsync<T>(this HttpClient http, string requestUri, object content)
        {
            var response = await http.PostJsonAsync(requestUri, content);
            await using var stream = await response.Content.ReadAsStreamAsync();
            var result = await JsonSerializer.DeserializeAsync<T>(stream, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            return result;
        }
    }
}
