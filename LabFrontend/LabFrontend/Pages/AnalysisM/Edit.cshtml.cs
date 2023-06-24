using LabModels;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace LabFrontend.Pages.AnalysisM
{
    public class EditModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        [BindProperty]
        public Analysis Analysis { get; set; }

        public EditModel(IConfiguration configuration)
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

        public async Task<IActionResult> OnPost()
        {
            var apiUrl = _configuration["AppSettings:MainApiUrl"];
            var authToken = Request.Cookies["Token"];

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

            var analysisJson = JsonConvert.SerializeObject(Analysis);
            var content = new StringContent(analysisJson, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"{apiUrl}/api/Analysis/{Analysis.Id}", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "An error occurred while updating the analysis.");
                return Page();
            }
        }
    }
}
