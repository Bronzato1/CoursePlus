using System.Threading.Tasks;
using CoursePlus.Shared.Models;

namespace CoursePlus.Client.Services
{
    public interface IAuthService
    {
        Task<LoginResult> Login(LoginModel loginModel);

        Task Logout();

        Task<RegisterResult> Register(RegisterModel registerModel);
    }
}