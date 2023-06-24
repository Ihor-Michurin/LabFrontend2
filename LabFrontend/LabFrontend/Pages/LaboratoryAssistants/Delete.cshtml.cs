using LabModels;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace LabFrontend.Pages.LaboratoryAssistants
{
    public class DeleteModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public DeleteModel(IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = new HttpClient();
        }

        public LaboratoryAssistant LaboratoryAssistant { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            string apiUrl = _configuration["AppSettings:MainApiUrl"];
            string token = Request.Cookies["Token"];

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response = await _httpClient.GetAsync(apiUrl + $"api/LaboratoryAssistants/{id}");

            if (response.IsSuccessStatusCode)
            {
                string responseData = await response.Content.ReadAsStringAsync();
                LaboratoryAssistant = JsonConvert.DeserializeObject<LaboratoryAssistant>(responseData);
                return Page();
            }
            else
            {
                // Handle error case
                return RedirectToPage("Index");
            }
        }

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            string apiUrl = _configuration["AppSettings:MainApiUrl"];
            string token = Request.Cookies["Token"];

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            HttpResponseMessage response = await _httpClient.DeleteAsync(apiUrl + $"api/LaboratoryAssistants/{id}");

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("Index");
            }
            else
            {
                // Handle error case
            }

            return Page();
        }
    }
}
