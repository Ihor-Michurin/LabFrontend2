using LabModels;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LabFrontend.Pages.AirQualityMeasurements
{
    public class CreateModel : PageModel
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _httpClient;

        public CreateModel(IConfiguration config, HttpClient httpClient)
        {
            _config = config;
            _httpClient = httpClient;
        }

        [BindProperty]
        public AirQualityMeasurement AirQualityMeasurement { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();
            AirQualityMeasurement.Date = DateTime.SpecifyKind(AirQualityMeasurement.Date, DateTimeKind.Utc);

            string apiUrl = _config.GetValue<string>("AppSettings:MainApiUrl");
            string token = Request.Cookies["Token"];

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.PostAsJsonAsync(apiUrl + "/api/AirQualityMeasurements", AirQualityMeasurement);

            if (!response.IsSuccessStatusCode)
                return Page();

            return RedirectToPage("Index");
        }
    }
}
