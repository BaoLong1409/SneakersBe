using AutoMapper;
using Domain.Entities;
using Domain.Enum;
using Domain.Interfaces;
using Domain.ViewModel.User;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Sneakers.Services.OTPService;
using Sneakers.Services.UserService;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Transactions;

namespace Sneakers.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailSender _emailSender;
        private readonly UserService _userService;
        private readonly OTPService _oTPService;
        private readonly ILogger<UserController> _logger;
        public UserController(IConfiguration configuration, UserManager<User> userManager, IMapper mapper, RoleManager<Role> roleManager, IUnitOfWork unitOfWork, IEmailSender emailSender, UserService userService, OTPService otpService, ILogger<UserController> logger)
        {
            _configuration = configuration;
            _userManager = userManager;
            _mapper = mapper;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
            _emailSender = emailSender;
            _userService = userService;
            _oTPService = otpService;
            _logger = logger;
        }

        [HttpPost]
        [Route("user/register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegistration userModel)
        {
                try
                {
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }
                    var user = _mapper.Map<User>(userModel);
                    var result = await _userManager.CreateAsync(user, userModel.Password);

                    if (!result.Succeeded)
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.TryAddModelError(error.Code, error.Description);
                        }
                        return BadRequest(ModelState);
                    }

                    if (!await _roleManager.RoleExistsAsync("User"))
                    {
                        var role = new Role { Name = "User" };
                        var check = await _roleManager.CreateAsync(role);
                        if (!check.Succeeded)
                        {
                            return BadRequest(new { message = "Create role failed" });
                        }
                    }

                    string token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    string encodedToken = WebUtility.UrlEncode(token);
                    var confirmationLink = $"{Request.Scheme}://{Request.Host}/api/v1/user/confirmationEmail?token={encodedToken}&email={user.Email}";

                    await _emailSender.SendEmailAsync(user.Email, "Confirmation email link",
                         $@"
                            <div style='font-family: Arial, sans-serif; padding: 20px; border: 1px solid #ddd;'>
                            <h2 style='color: #333;'>Hello</h2>
                            <p style='margin-top: 10px;'>Thank you for signing up. Please confirm your email by clicking the button below:</p>
                            <div style='margin-top: 20px;'>
                                <a href='{confirmationLink}' style='background-color: #007BFF; color: white; padding: 10px 20px; text-decoration: none; border-radius: 5px;'>Confirm Email</a>
                            </div>
                            <p style='margin-top: 20px; font-size: 12px; color: #999;'>This email was sent by Your App Name. If you didn't sign up, you can safely ignore this email.</p>
                        </div>
                        "
                         );

                    await _userManager.AddToRoleAsync(user, "User");

                    return Ok(new { message = "Register Successful" });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Register reason fail with email {Email}", userModel.Email);
                    return BadRequest(new { message = "Register fail" });
                }
        }

        [HttpPost]
        [Route("user/login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByEmailAsync(loginModel.Email);
            if (user == null)
            {
                return Unauthorized(new { message = "Invalid email" });
            }

            if (!user.EmailConfirmed)
            {
                return Unauthorized(new { message = "Your email has not been confirmed." });
            }

            if (user != null && await _userManager.CheckPasswordAsync(user, loginModel.Password))
            {
                var userRole = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, loginModel.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                };

                foreach (var role in userRole)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, role));
                }

                var token = GetToken(authClaims);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    message = "Login successful!"
                });

            }

            return Unauthorized(new { message = "Invalid password" });
        }

        [HttpPost]
        [Route("user/googleLogin")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginRequest request)
        {
            var (result, token) = await _userService.GoogleLogin(request.GoogleToken);
            return result switch
            {
                EnumUser.LoginSuccess => Ok(new { token = token, message = result.GetMessage() }),
                EnumUser.LoginFail => BadRequest(new { message = result.GetMessage() }),
                EnumUser.InvalidEmail => Unauthorized(new { message = result.GetMessage() }),
                EnumUser.CreateRoleFail => BadRequest(new { message = result.GetMessage() }),
                EnumUser.NotConfirmed => Unauthorized(new { message = result.GetMessage() }),
                EnumUser.CreateUserFail => Unauthorized(new { message = result.GetMessage() }),

                _ => StatusCode(500, new { message = "Unknown Error" })
            };
        }

        [HttpPost]
        [Route("user/changePassword")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest changePassReq)
        {
            var authHeader = Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                return Unauthorized(new { message = EnumUser.TokenInvalid.GetMessage() });
            }

            var token = authHeader.Substring("Bearer ".Length).Trim();
            var result = await _userService.ChangePassword(changePassReq, token);
            return result switch
            {
                EnumUser.ChangePasswordSuccess => Ok(new { message = result.GetMessage() }),
                EnumUser.ChangePasswordFail => BadRequest(new { message = result.GetMessage() }),
                EnumUser.DuplicatePassword => BadRequest(new { message = result.GetMessage() }),
                _ => StatusCode(500, new { message = "Unknown Error" })
            };
        }

        [HttpPost]
        [Route("user/resetPassword")]
        [Authorize]
        public async Task<IActionResult> SetNewPassword([FromBody] SetNewPasswordRequest newPassReq)
        {
            var authHeader = Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer "))
            {
                return Unauthorized(new { message = EnumUser.TokenInvalid.GetMessage() });
            }

            var token = authHeader.Substring("Bearer ".Length).Trim();
            var result = await _userService.SetNewPassword(newPassReq, token);
            return result switch
            {
                EnumUser.ResetPasswordSuccess => Ok(new { message = result.GetMessage() }),
                EnumUser.ResetPasswordFail => BadRequest(new { message = result.GetMessage() }),
                EnumUser.WrongOTP => BadRequest(new { message = result.GetMessage() }),
                _ => StatusCode(500, new { message = "Unknown Error" })
            };
        }

        [HttpGet]
        [Route("user/forgetPassword/getOTP")]
        public async Task<IActionResult> ForgetPassword(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound(new { message = EnumUser.NotExist.GetMessage() });
            }

            // Tạo OTP và gửi email
            var (status, otp) = _oTPService.GenerateAndStoreOTP(email);

            if (status == EnumUser.SpamOTP)
            {
                return BadRequest(new {message = status.GetMessage()});
            }

            var emailBody = $@"
        <div style='font-family: Arial, sans-serif; padding: 20px; border: 1px solid #ddd; border-radius: 8px; max-width: 500px; margin: auto;'>
            <h2 style='color: #d9534f; text-align: center;'>Reset Your Password</h2>
            <p style='color: #333; text-align: center;'>Hello,</p>
            <p style='color: #333; text-align: center;'>
                We received a request to reset your password. Use the OTP below to proceed:
            </p>
            <div style='text-align: center; margin-top: 20px;'>
                <span style='font-size: 24px; font-weight: bold; color: #d9534f; letter-spacing: 5px;'>{otp}</span>
            </div>
            <p style='color: #555; text-align: center; margin-top: 20px;'>
                If you didn’t request a password reset, please ignore this email.
            </p>
            <p style='color: #999; text-align: center; font-size: 12px; margin-top: 20px;'>
                This email was sent by Sneakers. Please do not reply.
            </p>
        </div>";

            await _emailSender.SendEmailAsync(user.Email, "Reset Your Password - Sneakers", emailBody);

            return Ok(new {message = EnumUser.SentOTP.GetMessage()});
        }

        [HttpPost]
        [Route("user/forgetPassword/validateOTP")]
        public ActionResult ValidateOTP(ValidateOTPRequest req)
        {
            var checkOTP = _oTPService.ValidateOTP(req.Email, req.OTP);

            if (!checkOTP) {
                return BadRequest(new { message = EnumUser.WrongOTP.GetMessage() });
            }
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, req.Email),
                new Claim("OTPVerified", "1")
            };
            JwtSecurityToken jwtToken = _userService.GetSessionToken(authClaims);
            string tokenString = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            return Ok(new {message = EnumUser.AccurateOTP.GetMessage(), sessionToken = tokenString });
        }


        [HttpGet]
        [Route("user/confirmationEmail")]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            if (String.IsNullOrEmpty(token) || String.IsNullOrEmpty(email))
            {
                return Redirect("https://sneakers-7373f.web.app/EmailConfirmed/invalid");
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return Redirect("https://sneakers-7373f.web.app/EmailConfirmed/notFound");
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return Redirect("https://sneakers-7373f.web.app/EmailConfirmed/success");
            }
            return Redirect("https://sneakers-7373f.web.app/EmailConfirmed/fail");
        }

        [HttpGet]
        [Route("user/getInfor")]
        public async Task<IActionResult> GetInforUserByToken(string token)
        {
            var user = _unitOfWork.User.GetUserByToken(token);
            List<string> userRoles = await _userService.GetUserRoles(user);
            var userInfor = _mapper.Map<UserDto>(user);
            userInfor.RolesName = userRoles;
            return Ok(userInfor);
        }

        [HttpPut]
        [Route("user/updateInfo")]
        public async Task<IActionResult> UpdateUserInfo([FromBody] UpdateUserRequest userInfo)
        {
            var (status, result) = await _userService.UpdateUserInformation(userInfo);
            return status switch
            {
                Domain.Enum.EnumUser.UpdateSuccessfully => Ok(new { message = status.GetMessage(), data = result }),
                Domain.Enum.EnumUser.NotExist => BadRequest(new { message = status.GetMessage() }),
                _ => StatusCode(500, new { message = status.GetMessage() })
            };
        }


        [HttpPut]
        [Route("user/changeTheme")]
        public async Task<IActionResult> ChangeTheme(Guid id, string theme)
        {
            var user = await FindUserById(id);
            if (user == null)
            {
                return BadRequest(new { message = "User does not exist" });
            }

            user.Theme = theme;
            _unitOfWork.Complete();
            return Ok();
        }

        private async Task<UserDto?> FindUserById(Guid id)
        {
            var user = await _unitOfWork.User.GetByIdAsync(id);
            var userDto = _mapper.Map<UserDto>(user);
            if (userDto == null)
            {
                return null;
            }
            return userDto;
        }


        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));

            return new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                expires: DateTime.UtcNow.AddDays(5),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authKey, SecurityAlgorithms.HmacSha256)
            );
        }
    }
}
