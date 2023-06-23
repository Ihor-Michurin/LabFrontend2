using LabModels;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace LabFrontend.Pages.Timeslots
{
    public class EditModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;

        public EditModel(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _apiUrl = configuration["AppSettings:MainApiUrl"];
            _httpClient = httpClientFactory.CreateClient();
        }

        [BindProperty]
        public Timeslot Timeslot { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, _apiUrl + $"api/TimeSlot/{id}");
            var authToken = Request.Cookies["Token"];

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

            var response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                Timeslot = JsonConvert.DeserializeObject<Timeslot>(json);

                if (Timeslot == null)
                {
                    return NotFound();
                }

                return Page();
            }
            else
            {
                // Handle error
                return NotFound();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Set the DateTime kind to UTC
            Timeslot.Time = DateTime.SpecifyKind(Timeslot.Time, DateTimeKind.Utc);

            var json = JsonConvert.SerializeObject(Timeslot);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var authToken = Request.Cookies["Token"];

            var request = new HttpRequestMessage(HttpMethod.Put, _apiUrl + $"api/TimeSlot/{Timeslot.Id}");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
            request.Content = content;

            var response = await _httpClient.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToPage("./Index");
            }
            else
            {
                // Handle error
                return Page();
            }
        }
    }

}
