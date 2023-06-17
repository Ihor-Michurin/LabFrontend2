using LabModels;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LabFrontend.Pages
{
    public class AnalisisNurseModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly JsonSerializerOptions _jsonSerializerOptions;
        private readonly HttpClient _httpClient;

        // Inject IConfiguration in the constructor
        public AnalisisNurseModel(IConfiguration configuration, HttpClient httpClient)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _jsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }
        public List<AnalysisResultProcessing> AnalysisResults { get; set; }
        public List<LaboratoryAssistant> LaboratoryAssistants { get; set; }
        public List<Nurse> Nurses { get; set; }
        public List<Registrar> Registrars { get; set; }

        [BindProperty(SupportsGet = true)]
        public Guid ResultId { get; set; }

        [BindProperty]
        public Guid SelectedLaboratoryAssistantId { get; set; }

        [BindProperty]
        public Guid SelectedNurseId { get; set; }

        [BindProperty]
        public Guid SelectedRegistrarId { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var apiUrl = _configuration.GetSection("AppSettings:MainApiUrl").Value;
            var httpClient = new HttpClient();

            // Get token from the response cookies
            var token = Request.Cookies["Token"];

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await httpClient.GetAsync(apiUrl + "api/AnalysisResult/processing/" + Request.Cookies["RoleId"]);
            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                var analysisResultProcessing = JsonSerializer.Deserialize<List<AnalysisResultProcessing>>(responseData, _jsonSerializerOptions);
                AnalysisResults = analysisResultProcessing;
                LaboratoryAssistants = analysisResultProcessing?.First().LaboratoryAssistants;
                Nurses = analysisResultProcessing?.First().Nurses;
                Registrars = analysisResultProcessing?.First().Registrars;
            }
            else
            {
                // Handle error response
                // For example, set an error message to display on the page
            }

            return Page();
        }
    }
}
