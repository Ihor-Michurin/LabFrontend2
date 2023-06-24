using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LabFrontend.Pages.AnalysisResults
{
    public class DeleteModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly string _apiUrl;

        public DeleteModel(IConfiguration configuration)
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

                HttpResponseMessage response = await client.DeleteAsync($"{_apiUrl}/api/AnalysisResult/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToPage("Index");
                }
                else
                {
                    // Handle error response
                    return NotFound();
                }
            }
        }
    }
}
