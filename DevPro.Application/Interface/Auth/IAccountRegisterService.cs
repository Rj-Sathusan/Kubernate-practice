
using DevPro.Application.Common;
using DevPro.Application.Dtos.Auth;

namespace DevPro.Application.Interface.Auth
{
    public interface IAccountRegisterService
    {
        Task<ApiResponse> RegisterSuperAdminAsync(RegisterDto registerDto);
    }
}
