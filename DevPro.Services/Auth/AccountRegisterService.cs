using DevPro.Application.Common;
using DevPro.Application.Dtos.Auth;
using DevPro.Application.Interface.Auth;
using DevPro.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace DevPro.Services.Auth
{
    public class AccountRegisterService : IAccountRegisterService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public AccountRegisterService(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<ApiResponse> RegisterSuperAdminAsync(RegisterDto registerDto)
        {
            try
            {
                const string superAdminRole = "SuperAdmin";

                // 1. Ensure SuperAdmin role exists
                if (!await _roleManager.RoleExistsAsync(superAdminRole))
                {
                    await _roleManager.CreateAsync(new ApplicationRole
                    {
                        Name = superAdminRole,
                    });
                }

                // 2. Validate email doesn't exist
                if (await _userManager.FindByEmailAsync(registerDto.Email) != null)
                {
                    return new ApiResponse
                    {
                        Code = 400,
                        Status = false,
                        Message = "Email already exists",
                        Data = null
                    };
                }

                // 3. Create user
                var user = new ApplicationUser
                {
                    UserName = registerDto.Email,
                    Email = registerDto.Email,
                    FirstName = registerDto.FirstName,
                    LastName = registerDto.LastName,
                    EmailConfirmed = true // Automatically confirm for SuperAdmin
                };

                var createResult = await _userManager.CreateAsync(user, registerDto.Password);
                if (!createResult.Succeeded)
                {
                    return new ApiResponse
                    {
                        Code = 400,
                        Status = false,
                        Message = string.Join(", ", createResult.Errors.Select(e => e.Description)),
                        Data = null
                    };
                }

                // 4. Assign role
                var roleResult = await _userManager.AddToRoleAsync(user, superAdminRole);
                if (!roleResult.Succeeded)
                {
                    // Rollback user creation if role assignment fails
                    await _userManager.DeleteAsync(user);
                    return new ApiResponse
                    {
                        Code = 500,
                        Status = false,
                        Message = "Failed to assign SuperAdmin role",
                        Data = null
                    };
                }

                // 5. Generate and confirm email token (even though already confirmed)
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                await _userManager.ConfirmEmailAsync(user, token);

                // 6. Return secure response (don't expose sensitive data)
                return new ApiResponse
                {
                    Code = 200,
                    Status = true,
                    Message = "SuperAdmin registered successfully",
                    Data = new
                    {
                        UserId = user.Id,
                        Email = user.Email,
                        Roles = new[] { superAdminRole },
                        EmailConfirmed = user.EmailConfirmed
                        // Excluded: password hash, security tokens, etc.
                    }
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse
                {
                    Code = 500,
                    Status = false,
                    Message = $"Internal server error: {ex.Message}",
                    Data = null
                };
            }
        }
    }
}