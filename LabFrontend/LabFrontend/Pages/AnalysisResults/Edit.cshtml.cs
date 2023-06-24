using LabModels;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace LabFrontend.Pages.AnalysisResults
{
    public class EditModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly string _apiUrl;

        [BindProperty]
        public AnalysisResult AnalysisResult { get; set; }

        public EditModel(IConfiguration configuration)
        {
            _configuration = configuration;
            _apiUrl = _configuration["AppSettings:MainApiUrl"];
        }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            string token = Request.Cookies["Token"];

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage response = await client.GetAsync($"{_apiUrl}/api/AnalysisResult/{id}");

                if (response.IsSuccessStatusCode)
                {
                    string jsonResult = await response.Content.ReadAsStringAsync();
                    AnalysisResult = JsonConvert.DeserializeObject<AnalysisResult>(jsonResult);
                    return Page();
                }
                else
                {
                    // Handle error response
                    return NotFound();
                }
            }
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

                HttpResponseMessage response = await client.PutAsync($"{_apiUrl}/api/AnalysisResult/{AnalysisResult.Id}", content);

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
