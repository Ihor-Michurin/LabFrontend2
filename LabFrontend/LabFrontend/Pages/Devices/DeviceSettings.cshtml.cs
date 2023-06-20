using LabModels;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace LabFrontend.Pages.Devices
{
    public class DeviceSettingsModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public DeviceSettingsModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }

        [BindProperty]
        public Device Device { get; set; }

        [BindProperty]
        public DeviceSettings DeviceSettings { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.GetAsync($"https://labwebapi20230601225432.azurewebsites.net/api/Device/{id}");

            if (response.IsSuccessStatusCode)
            {
                string responseData = await response.Content.ReadAsStringAsync();


                Device = JsonConvert.DeserializeObject<Device>(responseData);

                response = await _httpClient.GetAsync($"{Device.Url}get");

                responseData = await response.Content.ReadAsStringAsync();


                DeviceSettings = JsonConvert.DeserializeObject<DeviceSettings>(responseData);

                return Page();
            }

            return NotFound();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var token = Request.Cookies["Token"];
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            HttpResponseMessage response = await _httpClient.GetAsync($"https://labwebapi20230601225432.azurewebsites.net/api/Device/{Id}");

            if (response.IsSuccessStatusCode)
            {
                string responseData = await response.Content.ReadAsStringAsync();
                Device = JsonConvert.DeserializeObject<Device>(responseData);


                var parameters = new Dictionary<string, string>
{
    { "countercount", DeviceSettings.CounterCount.ToString() },
    { "season", DeviceSettings.Season },
    { "receptionPointId", DeviceSettings.ReceptionPointId.ToString() },
    { "deviceId", DeviceSettings.DeviceId.ToString() },
    { "authEmail", DeviceSettings.AuthEmail },
    { "authPassword", DeviceSettings.AuthPassword }
};

                var content = new FormUrlEncodedContent(parameters);
                response = await _httpClient.PostAsync($"{Device.Url}update", content);


                if (response.IsSuccessStatusCode)
                {
                    // Device settings updated successfully
                    return RedirectToPage("Index");
                }
            }
            // Handle the failure case
            return Page();
        }
    }
}
