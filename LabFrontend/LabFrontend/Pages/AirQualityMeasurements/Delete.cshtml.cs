using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
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
   
    public class DeleteModel : PageModel
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _httpClient;

        public DeleteModel(IConfiguration config, HttpClient httpClient)
        {
            _config = config;
            _httpClient = httpClient;
        }

        [BindProperty]
        public AirQualityMeasurement AirQualityMeasurement { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            string apiUrl = _config.GetValue<string>("AppSettings:MainApiUrl");
            string token = Request.Cookies["Token"];

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync(apiUrl + "/api/AirQualityMeasurements/" + id);

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsStringAsync();
                AirQualityMeasurement = JsonConvert.DeserializeObject<AirQualityMeasurement>(data);
            }
            else
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            string apiUrl = _config.GetValue<string>("AppSettings:MainApiUrl");
            string token = Request.Cookies["Token"];

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.DeleteAsync(apiUrl + "/api/AirQualityMeasurements/" + id);

            if (!response.IsSuccessStatusCode)
                return Page();

            return RedirectToPage("Index");
        }
    }

}
