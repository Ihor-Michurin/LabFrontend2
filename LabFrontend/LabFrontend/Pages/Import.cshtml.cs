using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PlasmaFrontend.Pages
{
    public class ImportModel : PageModel
    {
        private string _baseUrl;
        public ImportModel(IConfiguration configuration)
        {
            _baseUrl = configuration["AppSettings:MainApiUrl"];
        }
        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public string DatabaseData { get; set; } = default!;


        public async Task<IActionResult> OnPostAsync()
        {
            bool result = false;
            try
            {
                using (var client = new HttpClient())
                {
                    string url = $"{_baseUrl}/api/DatabaseData";
                    var token = Request.Cookies["Token"];

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var apiResponse = await client.PostAsync(url, new StringContent(DatabaseData, Encoding.UTF8, "application/json"));
                    result = true;
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                result = false;
            }
            if (!result)
            {
                return Page();
            }


            return RedirectToPage("/Index");
        }
    }
}
