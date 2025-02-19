using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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
        public UserController(IConfiguration configuration, UserManager<User> userManager, IMapper mapper, RoleManager<Role> roleManager, IUnitOfWork unitOfWork, IEmailSender emailSender)
        {
            _configuration = configuration;
            _userManager = userManager;
            _mapper = mapper;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
            _emailSender = emailSender;
        }

        [HttpPost]
        [Route("user/register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegistration userModel)
        {
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
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
                    transaction.Complete();

                    return Ok(new { message = "Register Successful" });
                }
                catch (Exception ex)
                {
                    return BadRequest(new { message = "Register fail" });
                }
            }
        }

        [HttpPost]
        [Route("user/login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByEmailAsync(loginModel.Email);
            if (user == null) {
                return Unauthorized(new {message = "Invalid email"});
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
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                };

                foreach (var role in userRole)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, role));
                }

                var token = GetToken(authClaims);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                    message = "Login successful!"
                });

            }

            return Unauthorized(new { message = "Invalid password" });
        }

        [HttpGet]
        [Route("user/confirmationEmail")]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            if (String.IsNullOrEmpty(token) || String.IsNullOrEmpty(email))
            {
                return Redirect("http://localhost:4200/EmailConfirmed/invalid");
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return Redirect("http://localhost:4200/EmailConfirmed/notFound");
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return Redirect("http://localhost:4200/EmailConfirmed/success");
            }
            return Redirect("http://localhost:4200/EmailConfirmed/fail");
        }

        [HttpGet]
        [Route("user/getInfor")]
        public async Task<IActionResult> GetInforUserByToken(string token)
        {
            var user = _unitOfWork.User.GetUserByToken(token);
            var userInfor = _mapper.Map<UserDto>(user);
            return Ok(userInfor);
        }


        [HttpPut]
        [Route("user/changeTheme")]
        public async Task<IActionResult> ChangeTheme(Guid id, string theme)
        {
            var user = await FindUserById(id);
            if (user == null) {
                return BadRequest(new {message = "User does not exist"});
            }

            user.Theme = theme;
            _unitOfWork.Complete();
            return Ok();
        }

        private async Task<UserDto?> FindUserById (Guid id)
        {
            var user = await _unitOfWork.User.GetByIdAsync(id);
            var userDto = _mapper.Map<UserDto>(user);
            if (userDto == null) {
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
