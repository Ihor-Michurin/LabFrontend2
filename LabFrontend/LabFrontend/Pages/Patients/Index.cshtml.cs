using LabModels;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace LabFrontend.Pages.Patients
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly string _apiUrl;

        public List<Patient> Patients { get; set; }

        public IndexModel(IConfiguration configuration)
        {
            _configuration = configuration;
            _apiUrl = _configuration.GetValue<string>("AppSettings:MainApiUrl");
        }

        public async Task<IActionResult> OnGetAsync()
        {
            using (var httpClient = new HttpClient())
            {
                
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["Token"]);
                var response = await httpClient.GetAsync($"{_apiUrl}/api/Patients");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    Patients = JsonConvert.DeserializeObject<List<Patient>>(json);
                }
                else
                {
                    // Handle the error response
                    // e.g., ModelState.AddModelError("", "Failed to retrieve patients.");
                }
            }

            return Page();
        }
    }
}
