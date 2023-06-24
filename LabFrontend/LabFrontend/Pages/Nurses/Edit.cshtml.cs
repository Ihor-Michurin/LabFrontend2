using LabModels;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace LabFrontend.Pages.Nurses
{
    public class EditModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly string _apiUrl;

        [BindProperty]
        public Nurse Nurse { get; set; }

        public EditModel(IConfiguration configuration)
        {
            _configuration = configuration;
            _apiUrl = configuration["AppSettings:MainApiUrl"];
        }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            string token = Request.Cookies["Token"];

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage response = await client.GetAsync(_apiUrl + "api/Nurses/" + id);

                if (response.IsSuccessStatusCode)
                {
                    string nurseJson = await response.Content.ReadAsStringAsync();
                    Nurse = JsonConvert.DeserializeObject<Nurse>(nurseJson);
                }
                else
                {
                    // Handle error response
                    // You can display an error message or redirect to an error page
                }
            }

            return Page();
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

                HttpResponseMessage response = await client.PutAsync(_apiUrl + "api/Nurses/" + Nurse.Id, content);

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
