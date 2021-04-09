using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Aris.ServerTest.Services
{
    /// <summary>
    /// A custom exception which contains additional properties for statuscode, error code and uuid
    /// </summary>
    public class KoreApiException : Exception
    { 
        public KoreApiException(string message, string errorCode, int statusCode, string uuid) : base(message)
        {
            this.ErrorCode = errorCode;
            this.StatusCode = statusCode;
            this.Uuid = uuid;
        }

        public int StatusCode { get; set; }

        public string ErrorCode { get; set; }

        public string Uuid { get; set; }
    }

    public abstract class KoreApiBase
    {
        /// <summary>
        /// Checks a response status code and throws a KoreApiException if it does not indicate a success
        /// </summary>
        public async Task CheckResponseForErrorAsync(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var koreError = JsonConvert.DeserializeObject<Models.KoreError>(data);
                throw new KoreApiException(koreError.Description, koreError.Error, (int)response.StatusCode, koreError.Uuid);
            }

        }
    }
}