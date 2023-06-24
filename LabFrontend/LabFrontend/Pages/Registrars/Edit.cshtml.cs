using LabModels;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace LabFrontend.Pages.Registrars
{
    public class EditModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        [BindProperty]
        public Registrar Registrar { get; set; }

        public EditModel(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            string apiUrl = _configuration["AppSettings:MainApiUrl"] + $"api/Registrars/{id}";
            string token = Request.Cookies["Token"];

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            Registrar = await _httpClient.GetFromJsonAsync<Registrar>(apiUrl);

            if (Registrar == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            string apiUrl = _configuration["AppSettings:MainApiUrl"] + $"api/Registrars/{Registrar.Id}";
            string token = Request.Cookies["Token"];
            Registrar.DateOfBirth = DateTime.SpecifyKind(Registrar.DateOfBirth, DateTimeKind.Utc);

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var jsonContent = new StringContent(JsonConvert.SerializeObject(Registrar), Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(apiUrl, jsonContent);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("./Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Failed to update registrar.");
                return Page();
            }
        }
    }
}
