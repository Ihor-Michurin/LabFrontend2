using LabModels;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace LabFrontend.Pages.AnalysisReceptionPoints
{
    public class CreateModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly string _mainApiUrl;

        [BindProperty]
        public AnalysisReceptionPoint ReceptionPoint { get; set; }

        public CreateModel(IConfiguration configuration)
        {
            _configuration = configuration;
            _mainApiUrl = configuration.GetValue<string>("AppSettings:MainApiUrl");
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var token = Request.Cookies["Token"];
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToPage("Error");
            }

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var json = JsonConvert.SerializeObject(ReceptionPoint);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(_mainApiUrl + "api/AnalysisReceptionPoints", content);
                if (!response.IsSuccessStatusCode)
                {
                    return RedirectToPage("Error");
                }
            }

            return RedirectToPage("Index");
        }
    }
}
