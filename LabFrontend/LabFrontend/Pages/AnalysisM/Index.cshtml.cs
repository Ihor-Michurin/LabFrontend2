using LabModels;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace LabFrontend.Pages.AnalysisM
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public List<Analysis> Analyses { get; set; }

        public IndexModel(IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = new HttpClient();
        }

        public async Task<IActionResult> OnGet()
        {
            var apiUrl = _configuration["AppSettings:MainApiUrl"];
            var authToken = Request.Cookies["Token"];

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

            var response = await _httpClient.GetAsync($"{apiUrl}/api/Analysis");

            if (response.IsSuccessStatusCode)
            {
                var analysisJson = await response.Content.ReadAsStringAsync();
                Analyses = JsonConvert.DeserializeObject<List<Analysis>>(analysisJson);
            }
            else
            {
                Analyses = new List<Analysis>();
            }

            return Page();
        }
    }
}
