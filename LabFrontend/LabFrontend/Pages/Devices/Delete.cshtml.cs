using LabModels;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace LabFrontend.Pages.Devices
{
    public class DeleteModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public DeleteModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _jsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        }

        [BindProperty]
        public Device Device { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var token = Request.Cookies["Token"];

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.GetAsync($"https://labwebapi20230601225432.azurewebsites.net/api/Device/{id}");

            if (response.IsSuccessStatusCode)
            {
                string responseData = await response.Content.ReadAsStringAsync();
                Device = JsonSerializer.Deserialize<Device>(responseData, _jsonSerializerOptions);
                if (Device == null)
                {
                    return NotFound();
                }
            }
            else
            {
                // Handle error response
                return StatusCode((int)response.StatusCode);
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            var token = Request.Cookies["Token"];

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.DeleteAsync($"https://labwebapi20230601225432.azurewebsites.net/api/Device/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("Index");
            }
            else
            {
                // Handle error response
                return StatusCode((int)response.StatusCode);
            }
        }
    }
}
