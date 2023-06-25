using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using LabModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace LabFrontend.Pages.AirQualityMeasurements
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _httpClient;

        public IndexModel(IConfiguration config, HttpClient httpClient)
        {
            _config = config;
            _httpClient = httpClient;
        }

        public List<AirQualityMeasurement> AirQualityMeasurements { get; set; }

        public async Task<IActionResult> OnGetAsync(int? limit, int? page)
        {
            string apiUrl = _config.GetValue<string>("AppSettings:MainApiUrl");
            string token = Request.Cookies["Token"];

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync(apiUrl + "/api/AirQualityMeasurements");

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                var measurements = JsonConvert.DeserializeObject<List<AirQualityMeasurement>>(data);

                // Apply the limit to the list
                if (limit.HasValue && limit > 0 && measurements.Count > limit)
                {
                    measurements = measurements.GetRange(0, limit.Value);
                }

                AirQualityMeasurements = measurements;


            }
            else
            {
                return NotFound();
            }

            return Page();
        }
    }
}
