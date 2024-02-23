using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Dynamic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Nodes;


namespace webapi_client_211223.Models
{
    public class ApiClient : IAPIClient
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ApiClient(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("https://localhost:7156/"); // Replace with your API endpoint
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
        }


        public async Task<Result> deleteToAPI(string apiEndpoint, int id)
        {
            Result myresult = new Result();
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var accessToken = _httpContextAccessor.HttpContext.Request.Cookies["access_token"];
                    if (!string.IsNullOrEmpty(accessToken))
                    {
                        // Thêm token vào header Bearer của yêu cầu
                        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                    }
                    if (id != null)
                    {
                        apiEndpoint = apiEndpoint + "/" + id;
                    }
                    HttpResponseMessage response = await _httpClient.DeleteAsync(apiEndpoint); // Replace with your API endpoint
                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResult = await response.Content.ReadAsStringAsync();
                        //  result = JsonConvert.DeserializeObject<IEnumerable<Object>>(jsonResult);
                        myresult.type = "Success";
                        myresult.message = jsonResult;
                    }
                    else
                    {
                        myresult.type = "Failure";
                        myresult.message = $"Error: {response.StatusCode}" + await response.Content.ReadAsStringAsync();
                    }
                }
                catch (Exception ex)
                {
                    myresult.type = "Failure";
                    myresult.message = $"Exception: {ex.Message}";
                    // return ex.ToString();
                }
                return myresult;
            }
        }

        public async Task<Result> patchToAPI(string apiEndpoint, int id,string jsonpatch)
        {
            Result myresult = new Result();
            using (HttpClient client = new HttpClient())
            {

                // Tạo nội dung yêu cầu
                //   var jsonContent = JsonConvert.SerializeObject(obj);
                var jsonContent = jsonpatch;
                // Create a StringContent with your JSON message
                StringContent content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

                try
                {
                    var accessToken = _httpContextAccessor.HttpContext.Request.Cookies["access_token"];
                    if (!string.IsNullOrEmpty(accessToken))
                    {
                        // Thêm token vào header Bearer của yêu cầu
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                    }
                    if (id != null)
                    {
                        apiEndpoint = apiEndpoint + "/" + id;
                    }
                    // Send a PUT request to the API endpoint with the JSON message
                    HttpResponseMessage response = await client.PatchAsync(apiEndpoint, content);

                    // Check if the request was successful (status code 2xx)
                    if (response.IsSuccessStatusCode)
                    {
                        myresult.type = "Success";
                        myresult.message = "Success";
                    }
                    else
                    {
                        myresult.type = "Failure";
                        string a = response.StatusCode.ToString();
                        string b = await response.Content.ReadAsStringAsync();
                        myresult.message = $"Error: {a}" + b;
                    }
                }
                catch (Exception ex)
                {
                    myresult.type = "Failure";
                    myresult.message = $"Exception: {ex.Message}";
                }
            }
            return myresult;
        }

        public async Task<Result> getFromAPI(string apiEndpoint)
        {
            Result myresult = new Result();
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var accessToken = _httpContextAccessor.HttpContext.Request.Cookies["access_token"];
                    if (!string.IsNullOrEmpty(accessToken))
                    {
                        // Thêm token vào header Bearer của yêu cầu
                        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                    }
                    
                    HttpResponseMessage response = await _httpClient.GetAsync(apiEndpoint); // Replace with your API endpoint
                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResult = await response.Content.ReadAsStringAsync();
                        //  result = JsonConvert.DeserializeObject<IEnumerable<Object>>(jsonResult);
                        myresult.type = "Success";
                        myresult.message = jsonResult;
                    }
                    else
                    {
                        myresult.type = "Failure";
                        myresult.message = $"Error: {response.StatusCode}" + await response.Content.ReadAsStringAsync();
                    }
                }
                catch (Exception ex)
                {
                    myresult.type = "Failure";
                    myresult.message = $"Exception: {ex.Message}";
                }
               // return result;
            }
            return myresult;
        }

        public async Task<Result> getSingleObjectFromAPI(string apiEndpoint, int id)
        {
            Result myresult = new Result();
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    var accessToken = _httpContextAccessor.HttpContext.Request.Cookies["access_token"];
                    if (!string.IsNullOrEmpty(accessToken))
                    {
                        // Thêm token vào header Bearer của yêu cầu
                        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                    }
                    if (id != null)
                    {
                        apiEndpoint = apiEndpoint + "/" + id;
                    }
                    HttpResponseMessage response = await _httpClient.GetAsync(apiEndpoint); // Replace with your API endpoint
                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResult = await response.Content.ReadAsStringAsync();
                        //  result = JsonConvert.DeserializeObject<IEnumerable<Object>>(jsonResult);
                        myresult.type = "Success";
                        myresult.message = jsonResult;
                    }
                    else
                    {
                        myresult.type = "Failure";
                        myresult.message = $"Error: {response.StatusCode}" + await response.Content.ReadAsStringAsync();
                    }
                }
                catch (Exception ex)
                {
                    myresult.type = "Failure";
                    myresult.message = $"Exception: {ex.Message}";
                    // return ex.ToString();
                }
                return myresult;
            }
        }


        public async Task<Result> postToAPI(string apiEndpoint, string jsonObject)
        {
            Result myresult = new Result();
            using (HttpClient client = new HttpClient())
            {

                // Tạo nội dung yêu cầu
                //   var jsonContent = JsonConvert.SerializeObject(obj);
                var jsonContent = jsonObject;
                // Create a StringContent with your JSON message
                StringContent content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

                try
                {
                    var accessToken = _httpContextAccessor.HttpContext.Request.Cookies["access_token"];
                    if (!string.IsNullOrEmpty(accessToken))
                    {
                        // Thêm token vào header Bearer của yêu cầu
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                    }
                    // Send a POST request to the API endpoint with the JSON message
                    HttpResponseMessage response = await client.PostAsync(apiEndpoint, content);

                    // Check if the request was successful (status code 2xx)
                    if (response.IsSuccessStatusCode)
                    {
                        myresult.type = "Success";
                        myresult.message = await response.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        myresult.type = "Failure";
                        myresult.message =  $"Error: {response.StatusCode}" ;
                    }
                }
                catch (Exception ex)
                {
                    myresult.type = "Failure";
                    myresult.message = $"Exception: {ex.Message}";
                }
            }
            return myresult;
        }

        public async Task<Result> putToAPI(string apiEndpoint, string jsonObject)
        {
            Result myresult = new Result();
            using (HttpClient client = new HttpClient())
            {

                // Tạo nội dung yêu cầu
                //   var jsonContent = JsonConvert.SerializeObject(obj);
                var jsonContent = jsonObject;
                // Create a StringContent with your JSON message
                StringContent content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

                try
                {
                    var accessToken = _httpContextAccessor.HttpContext.Request.Cookies["access_token"];
                    if (!string.IsNullOrEmpty(accessToken))
                    {
                        // Thêm token vào header Bearer của yêu cầu
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                    }
                    // Send a PUT request to the API endpoint with the JSON message
                    HttpResponseMessage response = await client.PutAsync(apiEndpoint, content);

                    // Check if the request was successful (status code 2xx)
                    if (response.IsSuccessStatusCode)
                    {
                        myresult.type = "Success";
                        myresult.message = "Success";
                    }
                    else
                    {
                        myresult.type = "Failure";
                        string a = response.StatusCode.ToString();
                        string b = await response.Content.ReadAsStringAsync();
                        myresult.message = $"Error: {a}" +  b;
                    }
                }
                catch (Exception ex)
                {
                    myresult.type = "Failure";
                    myresult.message = $"Exception: {ex.Message}";
                }
            }
            return myresult;
        }


        public async Task<string> CreateKhachHang(KhachHangModel model)
        {
            string apiEndpoint = @"https://localhost:7156/Home";
            using (HttpClient client = new HttpClient())
            {

                // Tạo nội dung yêu cầu
                var jsonContent = JsonConvert.SerializeObject(model);

                // Create a StringContent with your JSON message
                StringContent content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

                try
                {
                    var accessToken = _httpContextAccessor.HttpContext.Request.Cookies["access_token"];
                    if (!string.IsNullOrEmpty(accessToken))
                    {
                        // Thêm token vào header Bearer của yêu cầu
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                    }
                    // Send a POST request to the API endpoint with the JSON message
                    HttpResponseMessage response = await client.PostAsync(apiEndpoint, content);

                    // Check if the request was successful (status code 2xx)
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        return result;
                    }
                    else
                    {
                        return $"Error: {response.StatusCode}" + await response.Content.ReadAsStringAsync();
                    }
                }
                catch (Exception ex)
                {
                    return $"Exception: {ex.Message}";
                }
            }
        }

        public async Task<string> Login(LoginRequest request)
        {
            using (var httpClient = new HttpClient())
            {
                // Tạo nội dung yêu cầu
                var jsonContent = JsonConvert.SerializeObject(request);
                var apiUrl = @"https://localhost:7156/api/NguoiDung/authenticate";
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                // Gửi yêu cầu POST
                var response = await httpClient.PostAsync(apiUrl, content);

                // Đọc nội dung phản hồi
                var responseContent = await response.Content.ReadAsStringAsync();

                // Kiểm tra mã trạng thái của phản hồi
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsStringAsync();
                    // Sử dụng thư viện NewtonSoft.Json để đọc JSON
                    JObject jsonObject = JObject.Parse(result);

                    // Lấy giá trị của thuộc tính "token"
                    string token = jsonObject["token"].ToString();

                    //đọc token
                    var handler = new JwtSecurityTokenHandler();
                    var jsonToken = handler.ReadToken(token) as JwtSecurityToken;
                    if (jsonToken != null)
                    {
                        //Xác thực
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, request.sUserName),
                            new Claim(ClaimTypes.NameIdentifier, jsonToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value),
                            new Claim(ClaimTypes.GivenName, jsonToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value),
                            new Claim(ClaimTypes.Email, jsonToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value),
                            new Claim(ClaimTypes.Role, jsonToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value),
                        // Các thông tin khác nếu cần   
                        };
                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var authProperties = new AuthenticationProperties
                        {
                            // Cấu hình các thuộc tính xác thực
                            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(180),
                            IsPersistent = true,
                            AllowRefresh = true
                        };

                        await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
                        // Thêm token vào cookies
                        DateTimeOffset expirationTime = DateTimeOffset.Now.AddHours(3);
                        _httpContextAccessor.HttpContext.Response.Cookies.Append("access_token", token, new CookieOptions
                        {
                            Expires = expirationTime,
                            SameSite = SameSiteMode.Strict, // Ngăn chặn việc gửi cookie trong các yêu cầu chuyển tiếp từ các trang web khác
                            HttpOnly = true,  // không cho truy cập từ js
                            Secure = true     // Set to true if using HTTPS
                        });
                    }
                    return result;
                }
                else
                {
                    var error = await response.Content.ReadAsStringAsync();
                    return error;
                }
            }


         /*   using (var httpClient = _httpClientFactory.CreateClient())
            {
                var apiUrl = @"https://localhost:7156/api/NguoiDung/authenticate";

                var response = await httpClient.PostAsJsonAsync(apiUrl, request);

                if (response.IsSuccessStatusCode)
                {
                    // Xử lý phản hồi thành công
                    var result = await response.Content.ReadAsStringAsync();
                    return result;
                }
                else
                {
                    // Xử lý phản hồi không thành công
                    var error = await response.Content.ReadAsStringAsync();
                    return error;
                }
            }*/
        }

        public async Task<List<KhachHangModel>> GetKhachHang()
        {
            List<KhachHangModel> result = null;



            try
            {
                var accessToken = _httpContextAccessor.HttpContext.Request.Cookies["access_token"];
                if (!string.IsNullOrEmpty(accessToken))
                {
                    // Thêm token vào header Bearer của yêu cầu
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                    // Gửi yêu cầu đến API

                    // Xử lý phản hồi từ API
                    // ...
                }

                HttpResponseMessage response = await _httpClient.GetAsync("/Home"); // Replace with your API endpoint

                if (response.IsSuccessStatusCode)
                {
                    string jsonResult = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<List<KhachHangModel>>(jsonResult);
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }
        public async Task<List<WeatherData>> GetSampleDataAsync()
        {
            List<WeatherData> result = null;
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync("/WeatherForecast"); // Replace with your API endpoint
                if (response.IsSuccessStatusCode)
                {
                    string jsonResult = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<List<WeatherData>>(jsonResult);
                }
            }
            catch(Exception ex)
            {

            }
            return result;
        }
        public async Task<string> SendMessage()
        {
            string apiEndpoint = @"https://localhost:7156/WeatherForecast";
        //    string jsonMessage = "{ 'test_key': 'testvalue' }";
            TestModel mymodel = new TestModel("testvalue");
            using (HttpClient client = new HttpClient())
            {

                // Tạo nội dung yêu cầu
                var jsonContent = JsonConvert.SerializeObject(mymodel);
              //  var apiUrl = @"https://localhost:7156/api/NguoiDung/authenticate";
           //     var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                // Create a StringContent with your JSON message
                StringContent content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

                try
                {
                    // Send a POST request to the API endpoint with the JSON message
                    HttpResponseMessage response = await client.PostAsync(apiEndpoint, content);

                    // Check if the request was successful (status code 2xx)
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsStringAsync();
                        return result;
                    }
                    else
                    {
                        return $"Error: {response.StatusCode}"  + await response.Content.ReadAsStringAsync();
                    }
                }
                catch (Exception ex)
                {
                    return $"Exception: {ex.Message}";
                }
            }
        }
    }

}
