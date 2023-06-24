using LabModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LabFrontend.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public LoginModel(IConfiguration configuration)
        {
            _configuration = configuration;
            _jsonSerializerOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            var authUrl = _configuration.GetValue<string>("AppSettings:AuthUrl");

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var userDto = new UserDto
                {
                    Email = Email,
                    Password = Password
                };

                var jsonContent = new StringContent(JsonSerializer.Serialize(userDto), Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync($"{authUrl}/api/Auth/login", jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var userResult = JsonSerializer.Deserialize<UserResult>(content, _jsonSerializerOptions);

                    // Store the user result in cookies
                    Response.Cookies.Append("Token", userResult.Token ?? "");
                    Response.Cookies.Append("Role", userResult.Role ?? "");
                    Response.Cookies.Append("RoleId", userResult.RoleId.ToString() ?? "");

                    return RedirectToPage("/Index");
                }

                // Login failed
                ModelState.AddModelError(string.Empty, "Invalid email or password");
                return Page();
            }
        }
    }
}
