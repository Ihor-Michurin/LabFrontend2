using LabModels;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace LabFrontend.Pages.Patients
{
    public class EditModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly string _apiUrl;

        [BindProperty]
        public Patient Patient { get; set; }

        public EditModel(IConfiguration configuration)
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

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            using (var httpClient = new HttpClient())
            {
                Patient.DateOfBirth = DateTime.SpecifyKind(Patient.DateOfBirth, DateTimeKind.Utc);

                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Request.Cookies["Token"]);
                var json = JsonConvert.SerializeObject(Patient);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await httpClient.PutAsync($"{_apiUrl}/api/Patients/{Patient.Id}", content);

                if (response.IsSuccessStatusCode)
                    return RedirectToPage("Index");
                else
                {
                    // Handle the error response
                    // e.g., ModelState.AddModelError("", "Failed to update the patient.");
                    return Page();
                }
            }
        }
    }
}
