using LabModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LabFrontend.Pages.Registrars
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public List<Registrar> Registrars { get; set; }

        public IndexModel(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task OnGetAsync()
        {
            string apiUrl = _configuration["AppSettings:MainApiUrl"] + "api/Registrars";
            string token = Request.Cookies["Token"];

            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            Registrars = await _httpClient.GetFromJsonAsync<List<Registrar>>(apiUrl);
        }
    }
}
