using LabModels;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace LabFrontend.Pages.LaboratoryAssistants
{
    public class CreateModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public CreateModel(IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = new HttpClient();
        }

        [BindProperty]
        public LaboratoryAssistant LaboratoryAssistant { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            string apiUrl = _configuration["AppSettings:MainApiUrl"] +"api/LaboratoryAssistants";
            string token = Request.Cookies["Token"];
            LaboratoryAssistant.DateOfBirth = DateTime.SpecifyKind(LaboratoryAssistant.DateOfBirth, DateTimeKind.Utc);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var json = JsonConvert.SerializeObject(LaboratoryAssistant);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(apiUrl, content);
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
