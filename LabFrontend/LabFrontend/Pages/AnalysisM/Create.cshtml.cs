using LabModels;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace LabFrontend.Pages.AnalysisM
{
    public class CreateModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        [BindProperty]
        public Analysis Analysis { get; set; }

        public CreateModel(IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = new HttpClient();
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            var apiUrl = _configuration["AppSettings:MainApiUrl"];
            var authToken = Request.Cookies["Token"];

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

            var analysisJson = JsonConvert.SerializeObject(Analysis);
            var content = new StringContent(analysisJson, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{apiUrl}/api/Analysis", content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "An error occurred while creating the analysis.");
                return Page();
            }
        }
    }
}
