using Microsoft.AspNetCore.Mvc;
using NZWalks.UI.Models;
using NZWalks.UI.Models.DTO;
using System.Text;
using System.Text.Json;

namespace NZWalks.UI.Controllers
{
    public class RegionsController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public RegionsController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // 🔍 Helper: get JWT token from cookie
        private string? GetToken()
        {
            return Request.Cookies["JWTToken"];
        }



        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<RegionDTO> response = new();

            var token = GetToken();
            if (string.IsNullOrEmpty(token))
            {
                // User not logged in → redirect to login
                return RedirectToAction("Login", "Auth");
            }

            try
            {
                var client = _httpClientFactory.CreateClient();

                var httpRequestMsg = new HttpRequestMessage()
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri("https://localhost:7050/api/regions")
                };

                // ✅ Attach token for secured API
                httpRequestMsg.Headers.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var httpResponseMsg = await client.SendAsync(httpRequestMsg);
                httpResponseMsg.EnsureSuccessStatusCode();

                response.AddRange(await httpResponseMsg.Content
                    .ReadFromJsonAsync<IEnumerable<RegionDTO>>() ?? []);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error fetching regions: " + ex.Message);
            }

            return View(response);
        }




        [HttpGet]
        public IActionResult Add()
        {
            var token = GetToken();
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Auth");
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddRegionViewModel model)
        {
            var token = GetToken();
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Auth");
            }

            try
            {
                var client = _httpClientFactory.CreateClient();

                var httpRequestMsg = new HttpRequestMessage()
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri("https://localhost:7050/api/regions"),
                    Content = new StringContent(JsonSerializer.Serialize(model),
                        Encoding.UTF8,
                        "application/json")
                };

                // ✅ Attach token for protected API
                httpRequestMsg.Headers.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

                var httpResponseMsg = await client.SendAsync(httpRequestMsg);
                httpResponseMsg.EnsureSuccessStatusCode();

                var response = await httpResponseMsg.Content.ReadFromJsonAsync<RegionDTO>();

                if (response != null)
                {
                    return RedirectToAction("Index", "Regions");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error adding region: " + ex.Message);
            }

            return View(model);
        }
    }
}
