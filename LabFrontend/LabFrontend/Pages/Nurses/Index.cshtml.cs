using LabModels;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace LabFrontend.Pages.Nurses
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly string _apiUrl;

        public List<Nurse> Nurses { get; set; }

        public IndexModel(IConfiguration configuration)
        {
            _configuration = configuration;
            _apiUrl = configuration["AppSettings:MainApiUrl"];
        }

        public async Task<IActionResult> OnGetAsync()
        {
            string token = Request.Cookies["Token"];

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage response = await client.GetAsync(_apiUrl + "api/Nurses");

                if (response.IsSuccessStatusCode)
                {
                    string nursesJson = await response.Content.ReadAsStringAsync();
                    Nurses = JsonConvert.DeserializeObject<List<Nurse>>(nursesJson);
                }
                else
                {
                    // Handle error response
                    // You can display an error message or redirect to an error page
                }
            }

            return Page();
        }
    }
}
