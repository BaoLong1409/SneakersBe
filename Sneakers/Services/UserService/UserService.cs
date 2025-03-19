using AutoMapper;
using Domain.Interfaces;
using Domain.ViewModel.User;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Domain.Enum;

namespace Sneakers.Services.UserService
{
    public class UserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper, HttpClient httpClient , IConfiguration configuration)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<(EnumUser,UserDto)> UpdateUserInformation(UpdateUserRequest userInfo)
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
            if (userExist == null) {
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

    }
}
