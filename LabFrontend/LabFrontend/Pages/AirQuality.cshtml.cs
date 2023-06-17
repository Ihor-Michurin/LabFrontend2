using System.Net.Http;
using System.Net.Http.Headers;
using LabModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace LabFrontend.Pages
{
    public class AirQualityModel : PageModel
    {
        private readonly HttpClient httpClient;

        public AirQualityModel(IHttpClientFactory httpClientFactory)
        {
            httpClient = httpClientFactory.CreateClient();
        }

        public List<AnalysisReceptionPoint> ReceptionPoints { get; set; }
        public List<AirQualityMeasurementDto> Measurements { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? receptionPointId)
        {
            await LoadReceptionPoints();

            if (receptionPointId.HasValue)
            {
                await LoadMeasurements(receptionPointId.Value);
            }

            return Page();
        }

        private async Task LoadReceptionPoints()
        {
            var receptionPointsUrl = "https://labwebapi20230601225432.azurewebsites.net/api/AnalysisReceptionPoints";
            var token = Request.Cookies["Token"];
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var receptionPointsResponse = await httpClient.GetAsync(receptionPointsUrl);
            receptionPointsResponse.EnsureSuccessStatusCode();
            var receptionPointsJson = await receptionPointsResponse.Content.ReadAsStringAsync();
            var receptionPoints = JsonConvert.DeserializeObject<List<AnalysisReceptionPoint>>(receptionPointsJson);
            ReceptionPoints = receptionPoints;
        }

        private async Task LoadMeasurements(Guid receptionPointId)
        {
            var measurementsUrl = $"https://labwebapi20230601225432.azurewebsites.net/api/AirQualityMeasurements/data?receptionPointId={receptionPointId}";
            var token = Request.Cookies["Token"];
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var measurementsResponse = await httpClient.GetAsync(measurementsUrl);
            measurementsResponse.EnsureSuccessStatusCode();
            var measurementsJson = await measurementsResponse.Content.ReadAsStringAsync();
            var measurements = JsonConvert.DeserializeObject<List<AirQualityMeasurementDto>>(measurementsJson);
            Measurements = measurements;
        }
    }
}