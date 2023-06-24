using LabModels;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace LabFrontend.Pages.Nurses
{
    public class DeleteModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly string _apiUrl;

        public Nurse Nurse { get; set; }

        public DeleteModel(IConfiguration configuration)
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
                    string responseData = await response.Content.ReadAsStringAsync();
                    Nurse = JsonConvert.DeserializeObject<Nurse>(responseData);
                }
                else
                {
                    // Handle error response
                    // You can display an error message or redirect to an error page
                }
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            string token = Request.Cookies["Token"];

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage response = await client.DeleteAsync(_apiUrl + "api/Nurses/" + id);

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
