using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using LabModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace LabFrontend.Pages.Timeslots
{
    public class BatchCreateModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public BatchCreateModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [BindProperty]
        public TimeslotDto Timeslot { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var apiUrl = _configuration["AppSettings:MainApiUrl"];
            var token = Request.Cookies["Token"];

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                Timeslot.startDate = DateTime.SpecifyKind(Timeslot.startDate, DateTimeKind.Utc);
                Timeslot.endDate = DateTime.SpecifyKind(Timeslot.endDate, DateTimeKind.Utc);

                var jsonContent = JsonConvert.SerializeObject(Timeslot);
                var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await client.PostAsync($"{apiUrl}/api/TimeSlot/NewTimeslots", httpContent);

                if (response.IsSuccessStatusCode)
                {
                    // Handle success
                    return RedirectToPage("Index");
                }
                else
                {
                    // Handle error
                    ModelState.AddModelError(string.Empty, "Failed to create timeslots.");
                }
            }

            return Page();
        }
    }
}
