using LabModels;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace LabFrontend.Pages.AnalysisResults
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly string _apiUrl;

        public List<AnalysisResult> AnalysisResults { get; set; }

        public IndexModel(IConfiguration configuration)
        {
            _configuration = configuration;
            _apiUrl = _configuration["AppSettings:MainApiUrl"];
        }

        public async Task<IActionResult> OnGetAsync()
        {
            string token = Request.Cookies["Token"];

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await client.GetAsync($"{_apiUrl}/api/AnalysisResult");

                if (response.IsSuccessStatusCode)
                {
                    string jsonResult = await response.Content.ReadAsStringAsync();
                    AnalysisResults = JsonConvert.DeserializeObject<List<AnalysisResult>>(jsonResult);
                }
                else
                {
                    // Handle error response
                }
            }

            return Page();
        }
    }
}
