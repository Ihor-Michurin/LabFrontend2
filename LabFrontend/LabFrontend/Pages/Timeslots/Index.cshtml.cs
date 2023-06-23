using LabModels;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace LabFrontend.Pages.Timeslots
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public IndexModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<Timeslot> Timeslots { get; set; }
        public List<Guid> ReceptionPointIds { get; set; }
        public string SelectedReceptionPointId { get; set; }
        public string SortOrder { get; set; }

        public async Task<IActionResult> OnGetAsync(string receptionPointId, string sortOrder)
        {
            var apiUrl = _configuration["AppSettings:MainApiUrl"];
            var authToken = Request.Cookies["Token"];

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
                var response = await httpClient.GetAsync(apiUrl + "/api/TimeSlot");
                response.EnsureSuccessStatusCode();
                var timeslotsJson = await response.Content.ReadAsStringAsync();
                Timeslots = JsonConvert.DeserializeObject<List<Timeslot>>(timeslotsJson);
            }

            // Fetch distinct Reception Point IDs for filtering options
            ReceptionPointIds = Timeslots.Select(t => t.AnalysisReceptionPointId).Distinct().ToList();

            // Apply the filter
            // Apply the filter
            if (!string.IsNullOrEmpty(receptionPointId))
            {
                var selectedReceptionPointIdGuid = Guid.Parse(receptionPointId);
                Timeslots = Timeslots.Where(t => t.AnalysisReceptionPointId == selectedReceptionPointIdGuid).ToList();
                SelectedReceptionPointId = receptionPointId;
            }

            // Apply the filter
            if (!string.IsNullOrEmpty(receptionPointId))
            {
                var selectedReceptionPointIdGuid = Guid.Parse(receptionPointId);
                Timeslots = Timeslots.Where(t => t.AnalysisReceptionPointId == selectedReceptionPointIdGuid).ToList();
                SelectedReceptionPointId = receptionPointId;
            }

            // Apply the filter
            if (!string.IsNullOrEmpty(receptionPointId))
            {
                var selectedReceptionPointIdGuid = Guid.Parse(receptionPointId);
                Timeslots = Timeslots.Where(t => t.AnalysisReceptionPointId == selectedReceptionPointIdGuid).ToList();
                SelectedReceptionPointId = receptionPointId;
            }

            // Apply sorting
            switch (sortOrder)
            {
                case "time_desc":
                    Timeslots = Timeslots.OrderByDescending(t => t.Time).ToList();
                    break;
                case "available_asc":
                    Timeslots = Timeslots.OrderBy(t => t.Avaliable).ToList();
                    break;
                case "available_desc":
                    Timeslots = Timeslots.OrderByDescending(t => t.Avaliable).ToList();
                    break;
                case "reception_point_id_asc":
                    Timeslots = Timeslots.OrderBy(t => t.AnalysisReceptionPointId).ToList();
                    break;
                case "reception_point_id_desc":
                    Timeslots = Timeslots.OrderByDescending(t => t.AnalysisReceptionPointId).ToList();
                    break;
                case "analysis_result_id_asc":
                    Timeslots = Timeslots.OrderBy(t => t.AnalysisResultId).ToList();
                    break;
                case "analysis_result_id_desc":
                    Timeslots = Timeslots.OrderByDescending(t => t.AnalysisResultId).ToList();
                    break;
                case "id_asc":
                    Timeslots = Timeslots.OrderBy(t => t.Id).ToList();
                    break;
                case "id_desc":
                    Timeslots = Timeslots.OrderByDescending(t => t.Id).ToList();
                    break;
                default:
                    // Default sorting: time ascending
                    Timeslots = Timeslots.OrderBy(t => t.Time).ToList();
                    break;
            }

            SortOrder = sortOrder; // Store the current sort order for use in the view

            return Page();
        }
        

    }
}
