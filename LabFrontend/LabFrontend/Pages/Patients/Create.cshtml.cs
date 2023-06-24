using LabModels;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace LabFrontend.Pages.Patients
{
    public class CreateModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly string _apiUrl;

        [BindProperty]
        public Patient Patient { get; set; }

        public CreateModel(IConfiguration configuration)
        {
            _configuration = configuration;
            _apiUrl = _configuration.GetValue<string>("AppSettings:MainApiUrl");
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
                var response = await httpClient.PostAsync($"{_apiUrl}/api/Patients", content);

                if (response.IsSuccessStatusCode)
                    return RedirectToPage("Index");
                else
                {
                    // Handle the error response
                    // e.g., ModelState.AddModelError("", "Failed to create the patient.");
                    return Page();
                }
            }
        }
    }
}
