using LabModels;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace LabFrontend.Pages.LaboratoryAssistants
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public IndexModel(IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = new HttpClient();
        }

        public List<LaboratoryAssistant> LaboratoryAssistants { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            string apiUrl = _configuration["AppSettings:MainApiUrl"];
            string token = Request.Cookies["Token"];

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response = await _httpClient.GetAsync(apiUrl + "api/LaboratoryAssistants");

            if (response.IsSuccessStatusCode)
            {
                string responseData = await response.Content.ReadAsStringAsync();
                LaboratoryAssistants = JsonConvert.DeserializeObject<List<LaboratoryAssistant>>(responseData);
            }
            else
            {
                // Handle error case
            }

            return Page();
        }
    }
}
