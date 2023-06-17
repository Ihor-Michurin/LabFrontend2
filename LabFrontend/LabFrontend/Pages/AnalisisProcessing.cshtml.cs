using LabModels;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json.Linq;

namespace LabFrontend.Pages
{
    public class AnalisisProcessingModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly JsonSerializerOptions _jsonSerializerOptions;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpClient _httpClient;

        // Inject IConfiguration in the constructor
        public AnalisisProcessingModel(IConfiguration configuration, IHttpContextAccessor httpContextAccessor, HttpClient httpClient)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
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

                _httpContextAccessor.HttpContext.Session.SetString("AnalysisResults", responseData);

            }
            else
            {
                // Handle error response
                // For example, set an error message to display on the page
            }

            return Page();
        }

        public async Task<IActionResult> OnPostProcessResult(Guid analysisId)
        {
            var assistantId = SelectedLaboratoryAssistantId;
            var nurseId = SelectedNurseId;
            var registrarId = SelectedRegistrarId;
            var apiUrl = _configuration.GetSection("AppSettings:MainApiUrl").Value;

            var dto = new AnalysisResultProcessingDto
            {
                Id = analysisId,
                LaboratoryAssistantId = assistantId,
                NurseId = nurseId,
                RegistrarId = registrarId,
            };

            var jsonPayload = JsonSerializer.Serialize(dto);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
            var token = Request.Cookies["Token"];

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PostAsync(apiUrl + "/api/AnalysisResult/processing", content);

            if (response.IsSuccessStatusCode)
            {
                // Redirect to the desired page after processing
                return RedirectToPage("/Index");
            }
            else
            {
                // Handle the error scenario
                // For example, display an error message or redirect to an error page
                return Page();
            }
        }
    }
}
