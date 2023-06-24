using LabModels;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace LabFrontend.Pages.AnalysisReceptionPoints
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly string _mainApiUrl;

        public List<AnalysisReceptionPoint> ReceptionPoints { get; set; }

        public IndexModel(IConfiguration configuration)
        {
            _configuration = configuration;
            _mainApiUrl = configuration.GetValue<string>("AppSettings:MainApiUrl");
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var token = Request.Cookies["Token"];
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToPage("Error");
            }

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await client.GetAsync(_mainApiUrl + "api/AnalysisReceptionPoints");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    ReceptionPoints = JsonConvert.DeserializeObject<List<AnalysisReceptionPoint>>(content);
                }
                else
                {
                    return RedirectToPage("Error");
                }
            }

            return Page();
        }
    }
}
