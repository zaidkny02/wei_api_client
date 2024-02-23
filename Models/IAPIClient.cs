using System.Dynamic;

namespace webapi_client_211223.Models
{
    public interface IAPIClient
    {
        Task<List<KhachHangModel>> GetKhachHang();
        Task<List<WeatherData>> GetSampleDataAsync();
        Task<string> SendMessage();
        Task<string> Login(LoginRequest request);
        Task<string> CreateKhachHang(KhachHangModel model);
        Task<Result> getFromAPI(string apiEndPoint);
        Task<Result> getSingleObjectFromAPI(string apiEndPoint, int id);
        Task<Result> postToAPI(string apiEndpoint, string jsonObject);
        Task<Result> putToAPI(string apiEndpoint, string jsonObject);
        Task<Result> deleteToAPI(string apiEndpoint, int id);
        Task<Result> patchToAPI(string apiEndpoint,int id, string jsonpatch);
    }
}
