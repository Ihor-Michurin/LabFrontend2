using LabModels;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace LabFrontend.Pages.Nurses
{
    public class CreateModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly string _apiUrl;

        [BindProperty]
        public Nurse Nurse { get; set; }

        public CreateModel(IConfiguration configuration)
        {
            _configuration = configuration;
            _apiUrl = configuration["AppSettings:MainApiUrl"];
        }

        public void OnGet()
        {
            Nurse = new Nurse();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            string token = Request.Cookies["Token"];

            using (HttpClient client = new HttpClient())
            {
                Nurse.DateOfBirth = DateTime.SpecifyKind(Nurse.DateOfBirth, DateTimeKind.Utc);

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                string nurseJson = JsonConvert.SerializeObject(Nurse);
                StringContent content = new StringContent(nurseJson, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(_apiUrl + "api/Nurses", content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToPage("Index");
                }
                else
                {
                    // Handle error response
                    // You can display an error message or redirect to an error page
                }
            }

            return Page();
        }
    }
}
