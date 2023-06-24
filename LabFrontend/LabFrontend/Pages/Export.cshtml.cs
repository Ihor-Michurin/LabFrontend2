using System.Net.Http.Headers;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PlasmaFrontend.Pages
{
    public class ExportModel : PageModel
    {
        public string _database = "";
        private string _baseUrl;
        public ExportModel(IConfiguration configuration)
        {
            _baseUrl = configuration["AppSettings:MainApiUrl"];
        }
        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    string url = $"{_baseUrl}/api/DatabaseData";
                    var token = Request.Cookies["Token"];

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var apiResponse = await client.GetAsync(url);

                    if (apiResponse.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        var response = await apiResponse.Content.ReadAsStringAsync();

                        _database = response;
                    }
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
            }
            return Page();
        }
    }
}
