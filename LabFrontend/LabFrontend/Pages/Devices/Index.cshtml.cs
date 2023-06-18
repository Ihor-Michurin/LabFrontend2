using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LabModels;
using System.Text.Json;

namespace LabFrontend.Pages.Devices
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public IndexModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _jsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        }

        public List<Device> Devices { get; set; }
        
        public async Task<IActionResult> OnGetAsync()
        {
            var token = Request.Cookies["Token"];

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.GetAsync("https://labwebapi20230601225432.azurewebsites.net/api/Device");

            if (response.IsSuccessStatusCode)
            {
                string responseData = await response.Content.ReadAsStringAsync();
                Devices = JsonSerializer.Deserialize<List<Device>>(responseData, _jsonSerializerOptions);
            }
            else
            {
                // Handle error response
                return StatusCode((int)response.StatusCode);
            }

            return Page();
        }
    }
}
