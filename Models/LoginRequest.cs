using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace webapi_client_211223.Models
{
    public class LoginRequest
    {
        [Required]

        public string? sUserName { get; set; }
        [Required]
        public string? sPassword { get; set; }
        [DisplayName("Remember Me?")]
        public bool RememberMe { get; set; }
    }
}
