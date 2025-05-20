using DevPro.Application.Dtos.Auth;
using DevPro.Application.Interface.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DevPro.Api.Controllers.Auth
{
    [ApiController]
    [Route("api/register")]
    public class RegisterController : ControllerBase
    {
        private readonly IAccountRegisterService _accountRegisterService;

        public RegisterController(ILogger<RegisterController> logger,IAccountRegisterService accountRegisterService)
        {
            _accountRegisterService = accountRegisterService;
        }


        [HttpPost("superadmin")]
        public async Task<IActionResult> RegisterSuperAdmin([FromBody] RegisterDto registerDto)
        {
            try
            {

                var result = await _accountRegisterService.RegisterSuperAdminAsync(registerDto);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
