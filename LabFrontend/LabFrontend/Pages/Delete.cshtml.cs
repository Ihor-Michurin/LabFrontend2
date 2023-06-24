using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace PlasmaFrontend.Pages
{
    public class DeleteModel : PageModel
    {
        private string _baseUrl = "https://labapi123.azurewebsites.net";
        public DeleteModel(IConfiguration configuration)
        {
            _baseUrl = configuration["AppSettings:MainApiUrl"];
        }
        public PageResult OnGet()
        {
            return Page();
        }

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
                    var apiResponse = await client.DeleteAsync(url);

                    if (apiResponse.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        result = true;
                    }
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
