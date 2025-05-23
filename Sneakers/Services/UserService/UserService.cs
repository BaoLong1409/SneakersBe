﻿using AutoMapper;
using Domain.Interfaces;
using Domain.ViewModel.User;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Domain.Enum;
using Google.Apis.Auth;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System.Transactions;
using System.ComponentModel.DataAnnotations;

namespace Sneakers.Services.UserService
{
    public class UserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Domain.Entities.Role> _roleManager;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper, HttpClient httpClient, IConfiguration configuration, UserManager<User> userManager, RoleManager<Domain.Entities.Role> roleManager)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _httpClient = httpClient;
            _configuration = configuration;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<(EnumUser, UserDto?)> UpdateUserInformation(UpdateUserRequest userInfo)
        {
            string? avatarUrl = null;
            if (userInfo.AvatarFile != null)
            {
                var cloudinaryApi = _configuration["CloudinaryApi:Url"];
                Cloudinary cloudinary = new Cloudinary(cloudinaryApi);
                cloudinary.Api.Secure = true;

                byte[] imageBytes = Convert.FromBase64String(userInfo.AvatarFile.Split(',')[1]);
                using var stream = new MemoryStream(imageBytes);

                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(userInfo.AvatarName, stream),
                    AssetFolder = "Avatar"
                };

                var result = await cloudinary.UploadAsync(uploadParams);
                avatarUrl = result.SecureUrl.AbsoluteUri;
            }

            var userExist = await _unitOfWork.User.GetFirstOrDefaultAsync(u => u.Id == userInfo.Id);
            if (userExist == null)
            {
                return (EnumUser.NotExist, null);
            }
            _mapper.Map(userInfo, userExist);
            if (avatarUrl != null)
            {
                userExist.AvatarUrl = avatarUrl;
            }
            _unitOfWork.Complete();

            var returnUserData = _mapper.Map<UserDto>(userExist);
            return (EnumUser.UpdateSuccessfully, returnUserData);
        }

        public async Task<(EnumUser, string?)> GoogleLogin(string googleToken)
        {
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                GoogleJsonWebSignature.Payload payLoad = await GoogleJsonWebSignature.ValidateAsync(googleToken);
                var user = await _userManager.FindByEmailAsync(payLoad.Email);

                if (user == null)
                {
                    var userInfo = new
                    {
                        Id = Guid.NewGuid(),
                        AvatarUrl = payLoad.Picture,
                        UserName = payLoad.Email,
                        FirstName = payLoad.GivenName,
                        LastName = "",
                        Email = payLoad.Email,
                        EmailConfirmed = payLoad.EmailVerified ? 1 : 0
                    };


                    user = JsonConvert.DeserializeObject<User>(JsonConvert.SerializeObject(userInfo));
                    var createUserResult = await _userManager.CreateAsync(user);
                    if (!createUserResult.Succeeded)
                    {
                        return (EnumUser.LoginFail, null);
                    }
                    await _userManager.AddToRoleAsync(user, "User");
                }

                if (!await _roleManager.RoleExistsAsync("User"))
                {
                    var role = new Domain.Entities.Role { Name = "User" };
                    var check = await _roleManager.CreateAsync(role);
                    if (!check.Succeeded)
                    {
                        return (EnumUser.CreateRoleFail, null);
                    }
                }

                if (!user.EmailConfirmed)
                {
                    return (EnumUser.NotConfirmed, null);
                }
                if (user != null)
                {
                    var userRoles = await _userManager.GetRolesAsync(user);
                    var authClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Email, payLoad.Email),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                    };

                    foreach (var role in userRoles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, role));
                    }

                    var token = GetToken(authClaims);
                    var returnToken = new JwtSecurityTokenHandler().WriteToken(token);
                    transaction.Complete();
                    return (EnumUser.LoginSuccess, returnToken);
                }
            }


            return (EnumUser.LoginFail, null);
        }

        public async Task<EnumUser> ChangePassword(ChangePasswordRequest changePassReq, string token)
        {
            if (changePassReq.OldPassword.Equals(changePassReq.NewPassword))
            {
                return EnumUser.DuplicatePassword;
            }
            var user = _unitOfWork.User.GetUserByToken(token);
            if (user == null) {
                return EnumUser.TokenInvalid;
            }

            var changePassRes = await _userManager.ChangePasswordAsync(user, changePassReq.OldPassword, changePassReq.NewPassword);
            if (!changePassRes.Succeeded)
            {
                return EnumUser.ChangePasswordFail;
            }

            return EnumUser.ChangePasswordSuccess;
        }

        public async Task<EnumUser> SetNewPassword(SetNewPasswordRequest setNewPassReq, string token)
        {
            var user = _unitOfWork.User.GetUserByToken(token);
            if (user == null)
            {
                return EnumUser.TokenInvalid;
            }
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var otpVerified = jwtToken.Claims.FirstOrDefault(c => c.Type == "OTPVerified")?.Value;
            if (otpVerified == "1")
            {
                var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                var changePassRes = await _userManager.ResetPasswordAsync(user, resetToken, setNewPassReq.NewPassword);
                if (changePassRes.Succeeded)
                {
                    return EnumUser.ResetPasswordSuccess;
                }

                return EnumUser.ResetPasswordFail;
            }
            return EnumUser.WrongOTP;
        }

        public JwtSecurityToken GetToken(List<Claim> authClaims)
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

        public JwtSecurityToken GetSessionToken(List<Claim> authClaims)
        {
            var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));

            return new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                expires: DateTime.UtcNow.AddMinutes(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authKey, SecurityAlgorithms.HmacSha256)
            );
        }

        public async Task<List<string>> GetUserRoles(User user)
        {
            if (user == null)
            {
                return new List<string>();
            }
            var roles = await _userManager.GetRolesAsync(user);
            return roles.ToList();
        }

    }
}
