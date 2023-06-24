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
        public int Limit { get; set; } = 10;
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }

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

                // Calculate total pages
                TotalPages = (int)Math.Ceiling((double)measurements.Count / Limit);

                // Apply pagination
                page = page == null ? 1 : page;

                CurrentPage = page.Value;
                var startIndex = (CurrentPage - 1) * Limit;
                AirQualityMeasurements = measurements.Skip(startIndex).Take(Limit).ToList();

            }
            else
            {
                return NotFound();
            }

            return Page();
        }
    }
}
