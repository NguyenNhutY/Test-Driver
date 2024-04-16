using Microsoft.AspNet.Identity.EntityFramework;

namespace DemoGPLX.Models
{
    public class UserGplx
    {
        private string username;
        private string password;
        public UserGplx()
        {
        }

        public string Username { get => username; set => username = value; }
        public string Password { get => password; set => password = value; }
    }
}
