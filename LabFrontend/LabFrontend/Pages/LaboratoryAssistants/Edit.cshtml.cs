using LabModels;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace LabFrontend.Pages.LaboratoryAssistants
{
    public class EditModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public EditModel(IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = new HttpClient();
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync()
        {
            string apiUrl = _configuration["AppSettings:MainApiUrl"] + $"api/LaboratoryAssistants/{LaboratoryAssistant.Id}";
            string token = Request.Cookies["Token"];
            LaboratoryAssistant.DateOfBirth = DateTime.SpecifyKind(LaboratoryAssistant.DateOfBirth, DateTimeKind.Utc);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var json = JsonConvert.SerializeObject(LaboratoryAssistant);
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(apiUrl, content);
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
