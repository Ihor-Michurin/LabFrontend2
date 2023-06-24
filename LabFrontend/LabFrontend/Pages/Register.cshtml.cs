using LabModels;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace LabFrontend.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public RegisterModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        [BindProperty]
        public UserDto User { get; set; }

        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("https://auth20230614132728.azurewebsites.net/api/Auth/registerstaff", User);
                response.EnsureSuccessStatusCode();
                var userResult = await response.Content.ReadFromJsonAsync<UserResult>();

                Response.Cookies.Append("Token", userResult.Token ?? "");
                Response.Cookies.Append("Role", userResult.Role ?? "");
                Response.Cookies.Append("RoleId", userResult.RoleId.ToString() ?? "");

                return RedirectToPage("/Index"); // Redirect to a success page
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return Page();
            }
        }
    }
}
