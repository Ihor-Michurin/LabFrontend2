using LabModels;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace LabFrontend.Pages.Timeslots
{
    public class DeleteModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;

        public DeleteModel(IConfiguration configuration, IHttpClientFactory httpClientFactory)
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
            var request = new HttpRequestMessage(HttpMethod.Delete, _apiUrl + $"api/TimeSlot/{Timeslot.Id}");
            var authToken = Request.Cookies["Token"];

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

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
