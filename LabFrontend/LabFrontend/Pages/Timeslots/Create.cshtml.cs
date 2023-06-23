using LabModels;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace LabFrontend.Pages.Timeslots
{
    public class CreateModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public CreateModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [BindProperty]
        public Timeslot Timeslot { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            Timeslot.Time = DateTime.SpecifyKind(Timeslot.Time, DateTimeKind.Utc);

            var apiUrl = _configuration["AppSettings:MainApiUrl"];
            var authToken = Request.Cookies["Token"];
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
                var timeslotJson = JsonConvert.SerializeObject(Timeslot);
                var content = new StringContent(timeslotJson, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(apiUrl + "/api/TimeSlot", content);
                response.EnsureSuccessStatusCode();
            }

            return RedirectToPage("Index");
        }
    }
}
