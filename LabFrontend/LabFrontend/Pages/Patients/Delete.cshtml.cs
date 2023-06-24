using LabModels;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace LabFrontend.Pages.Patients
{
    public class DeleteModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly string _apiUrl;

        public Patient Patient { get; set; }

        public DeleteModel(IConfiguration configuration)
        {
            _configuration = configuration;
            _apiUrl = _configuration.GetValue<string>("AppSettings:MainApiUrl");
        }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["Token"]);
                var response = await httpClient.GetAsync($"{_apiUrl}/api/Patients/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    Patient = JsonConvert.DeserializeObject<Patient>(json);
                }
                else
                {
                    // Handle the error response
                    // e.g., ModelState.AddModelError("", "Failed to retrieve the patient.");
                    return RedirectToPage("Index");
                }
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["Token"]);
                var response = await httpClient.DeleteAsync($"{_apiUrl}/api/Patients/{id}");

                if (response.IsSuccessStatusCode)
                    return RedirectToPage("Index");
                else
                {
                    // Handle the error response
                    // e.g., ModelState.AddModelError("", "Failed to delete the patient.");
                    return Page();
                }
            }
        }
    }
}
