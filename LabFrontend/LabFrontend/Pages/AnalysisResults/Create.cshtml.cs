using LabModels;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace LabFrontend.Pages.AnalysisResults
{
    public class CreateModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly string _apiUrl;

        [BindProperty]
        public AnalysisResult AnalysisResult { get; set; }

        public CreateModel(IConfiguration configuration)
        {
            _configuration = configuration;
            _apiUrl = _configuration["AppSettings:MainApiUrl"];
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            string token = Request.Cookies["Token"];

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                string json = JsonConvert.SerializeObject(AnalysisResult);
                HttpContent content = new StringContent(json);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                HttpResponseMessage response = await client.PostAsync($"{_apiUrl}/api/AnalysisResult", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToPage("Index");
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
