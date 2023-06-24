using LabModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LabFrontend.Pages.Registrars
{
    public class DeleteModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        [BindProperty]
        public Registrar Registrar { get; set; }

        public DeleteModel(IConfiguration configuration, IHttpClientFactory httpClientFactory)
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

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            string apiUrl = _configuration["AppSettings:MainApiUrl"] + $"api/Registrars/{id}";
            string token = Request.Cookies["Token"];

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.DeleteAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("./Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Failed to delete registrar.");
                return Page();
            }
        }
    }
}
