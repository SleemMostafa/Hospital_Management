using Hospital_Management.DTO;
using Hospital_Management.Models;
using System.Threading.Tasks;

namespace Hospital_Management.AccountService
{
    public interface IAuthService
    {
        Task<AuthModel> Register(RegisterModel model);
        Task<AuthModel> Login(TokenRequestModel model);
        Task<string> AddRole(AddRoleModel model);
        Task<AuthModel> ResetPassword(ResetPasswordModel model);
        Task<AuthModel> ForgetPassword(string email);

    }
}
