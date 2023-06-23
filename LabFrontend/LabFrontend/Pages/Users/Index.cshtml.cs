using LabModels;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace LabFrontend.Pages.Users
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiUrl;

        public List<User> Users { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SortOrder { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Filter { get; set; }

        public IndexModel(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClient = httpClientFactory.CreateClient();
            _apiUrl = configuration.GetValue<string>("AppSettings:MainApiUrl");
        }

        public async Task OnGetAsync()
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync($"{_apiUrl}/api/Users");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var allUsers = JsonConvert.DeserializeObject<List<User>>(json);

                // Search
                if (!string.IsNullOrEmpty(SearchString))
                {
                    allUsers = allUsers.Where(u =>
                        u.Id.ToString().Contains(SearchString, StringComparison.OrdinalIgnoreCase) ||
                        u.Email.Contains(SearchString, StringComparison.OrdinalIgnoreCase) ||
                        u.Password.Contains(SearchString, StringComparison.OrdinalIgnoreCase) ||
                        u.Role.Contains(SearchString, StringComparison.OrdinalIgnoreCase) ||
                        u.RoleId.ToString().Contains(SearchString, StringComparison.OrdinalIgnoreCase)
                    ).ToList();
                }

                // Filter
                if (!string.IsNullOrEmpty(Filter))
                {
                    allUsers = allUsers.Where(u => u.Role == Filter).ToList();
                }

                // Sorting
                allUsers = SortOrder switch
                {
                    "id_asc" => allUsers.OrderBy(u => u.Id).ToList(),
                    "id_desc" => allUsers.OrderByDescending(u => u.Id).ToList(),
                    "email_asc" => allUsers.OrderBy(u => u.Email).ToList(),
                    "email_desc" => allUsers.OrderByDescending(u => u.Email).ToList(),
                    "password_asc" => allUsers.OrderBy(u => u.Password).ToList(),
                    "password_desc" => allUsers.OrderByDescending(u => u.Password).ToList(),
                    "role_asc" => allUsers.OrderBy(u => u.Role).ToList(),
                    "role_desc" => allUsers.OrderByDescending(u => u.Role).ToList(),
                    "roleId_asc" => allUsers.OrderBy(u => u.RoleId).ToList(),
                    "roleId_desc" => allUsers.OrderByDescending(u => u.RoleId).ToList(),
                    _ => allUsers.OrderBy(u => u.Id).ToList(),
                };

                Users = allUsers;
            }
            else
            {
                // Handle error
            }
        }
    }
}
