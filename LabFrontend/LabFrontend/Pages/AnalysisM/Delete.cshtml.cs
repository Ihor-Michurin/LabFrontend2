using LabModels;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace LabFrontend.Pages.AnalysisM
{
    public class DeleteModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        [BindProperty]
        public Analysis Analysis { get; set; }

        public DeleteModel(IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = new HttpClient();
        }

        public async Task<IActionResult> OnGet(Guid id)
        {
            var apiUrl = _configuration["AppSettings:MainApiUrl"];
            var authToken = Request.Cookies["Token"];

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

            var response = await _httpClient.GetAsync($"{apiUrl}/api/Analysis/{id}");

            if (response.IsSuccessStatusCode)
            {
                var analysisJson = await response.Content.ReadAsStringAsync();
                Analysis = JsonConvert.DeserializeObject<Analysis>(analysisJson);
                return Page();
            }
            else
            {
                return RedirectToPage("Index");
            }
        }

        public async Task<IActionResult> OnPost(Guid id)
        {
            var apiUrl = _configuration["AppSettings:MainApiUrl"];
            var authToken = Request.Cookies["Token"];

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

            var response = await _httpClient.DeleteAsync($"{apiUrl}/api/Analysis/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "An error occurred while deleting the analysis.");
                return Page();
            }
        }
    }
}
