using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using webapi_client_211223.Models;
using System.Net.WebSockets;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.JsonPatch;

namespace webapi_client_211223.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAPIClient _ApiClient;

        public HomeController(ILogger<HomeController> logger, IAPIClient Apiclent)
        {
            _logger = logger;
            _ApiClient = Apiclent;
        }

        public IActionResult Login()
        {
            if (!User.Identity.IsAuthenticated)
                return View();
            else
                return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest model)
        {
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                if (ModelState.IsValid)
                {
                    var result = await _ApiClient.Login(model);
                    TempData["Message"] = result;
                    return RedirectToAction("KhachHang", "Home");
                }
                else
                    return View(model);
            }
            else
                return RedirectToAction("KhachHang", "Home");
        }


        public async Task<IActionResult> ChiTietKH(int id)
        {
            var data = await _ApiClient.getSingleObjectFromAPI(@"https://localhost:7156/Home",id);
            if (data.type.Equals("Success"))
            {
                var model = JsonConvert.DeserializeObject<KhachHangModel>(data.message);
                return View(model);
            }
            TempData["Message"] = data.message;
            return RedirectToAction("KhachHang", "Home");

        }


        public async Task<IActionResult> EditKH(int id)
        {
            if(id == null)
            {
                return RedirectToAction("KhachHang","Home");
            }
            else
            {
                var data = await _ApiClient.getSingleObjectFromAPI(@"https://localhost:7156/Home", id);
                if (data.type.Equals("Success"))
                {
                    var model = JsonConvert.DeserializeObject<KhachHangModel>(data.message);
                    return View(model);
                }
                TempData["Message"] = data.message;
                return RedirectToAction("KhachHang", "Home");
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditKH(int id, KhachHangModel model)
        {
            if (id == null)
            {
                return RedirectToAction("KhachHang", "Home");
            }
            else
            {
                var jsonObject = JsonConvert.SerializeObject(model);
                var apiEndPoint = @"https://localhost:7156/Home" + "/" + id;
                var data = await _ApiClient.putToAPI(apiEndPoint, jsonObject);
                if (data.type.Equals("Success"))
                {
                    TempData["Message"] = data.message;
                    return RedirectToAction("KhachHang", "Home");
                }
                else
                {
                    TempData["Message"] = data.message;
                    return View(model);
                }
            }

        }




        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            // Xóa cookie chứa token
            HttpContext.Response.Cookies.Delete("access_token");
            return RedirectToAction("Index", "Home");
        }
        
        public async Task<IActionResult> KhachHang()
        {
            // ApiClient _ApiClient = new ApiClient();
            //  List<KhachHangModel> data = await _ApiClient.GetKhachHang();
         //   List<KhachHangModel> data = (List<KhachHangModel>)await _ApiClient.getFromAPI(@"https://localhost:7156/Home");
            var data = await _ApiClient.getFromAPI(@"https://localhost:7156/Home");
            
        //    List<KhachHangModel> model = (List<KhachHangModel>)data;
            if(data.type.Equals("Success"))
            {
                var model  = JsonConvert.DeserializeObject<IEnumerable<KhachHangModel>>(data.message);
                ViewBag.Status = "Ok";
                return View(model);
            }
           
            ViewBag.Status = data.message;
            return View();
            
        }
        public async Task<IActionResult> Index()
        {
          //  ApiClient _ApiClient = new ApiClient();
            List<WeatherData> data = await _ApiClient.GetSampleDataAsync();
            if (data != null)
            {
                // Process the retrieved data
                ViewBag.Status = "Ok";
                return View(data);
            }
            else
            {
                ViewBag.Status = "NotFound";
                return View();
            }
        }
        public IActionResult CreateKhachHang()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateKhachHang(KhachHangModel model)
        {
            if (ModelState.IsValid)
            {
                var jsonObject = JsonConvert.SerializeObject(model);
                var result = await _ApiClient.postToAPI(@"https://localhost:7156/Home", jsonObject);
            //    var result = await _ApiClient.CreateKhachHang(model);
                TempData["Message"] = result.message;
                return RedirectToAction("KhachHang", "Home");
            }
            else
                return View(model);
        }

        public IActionResult UpdateDiachi(int id)
        {
            ViewBag.myid = id;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateDiachi(int id,string? diachi)
        {
            if (diachi == null)
                return View();
            else
            {
                var patchDocument = new JsonPatchDocument<KhachHangModel>();
                patchDocument.Replace(x => x.sDiachi, diachi);
                var jsonstring = JsonConvert.SerializeObject(patchDocument);
                var result = await _ApiClient.patchToAPI(@"https://localhost:7156/Home", id, jsonstring);
                TempData["Message"] = result.message;
                return RedirectToAction("KhachHang", "Home");
            }
        }


        
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
              var result = await _ApiClient.deleteToAPI(@"https://localhost:7156/Home", id);
              TempData["Message"] = result.message;
             return RedirectToAction("KhachHang", "Home");
           
        }

        [HttpPost]
        public async Task<string> SendMessageToAPI()
        {
            string result = "";
         //   ApiClient _ApiClient = new ApiClient();
            result = await _ApiClient.SendMessage();
            return result;
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}