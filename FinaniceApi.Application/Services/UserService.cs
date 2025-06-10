using FinanceApi.Infrastructure.Abstraction;
using FinanceApi.Infrastructure.Entity.CoreModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinaniceApi.Application.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly SpaceService _spaceService;
        private readonly CategoryService _categoryService;
        public UserService(IUserRepository userRepository, SpaceService spaceService, CategoryService categoryService)
        {
            _userRepository = userRepository;
            _spaceService = spaceService;
            _categoryService = categoryService;
        }

        public Task<User?> GetByIdAsync(Guid id) => _userRepository.GetByIdAsync(id);

        /// <summary>
        /// Получение пользователей с пагинацией
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns></returns>
        public async Task<List<User>> GetUsersAsync(int skip, int take)
        {
            var all = await _userRepository.GetAllAsync();
            return all.Skip(skip).Take(take).ToList();
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            return user;
        }

        public async Task<User?> GetByPhoneAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            return user;
        }

        public async Task<User> CreateNew(string signType, string identify, string password, string nickName)
        {
            var user = new User()
            {
                AuthProvider = signType,
                CreatedAt = DateTime.UtcNow,
                LastLoginAt = DateTime.UtcNow,
                Id = Guid.NewGuid(),
                Status = "Free",
                Name = nickName,
                PasswordHash = CreateMD5(password)
            };
            switch (signType)
            {
                case "Email": user.Email = identify; break;
                case "Phone": user.Phone = identify; break;
                case "Apple": break;
                case "Google": break;
                default: throw new NotImplementedException($"Тип авторизации {signType} не поддерживается.");
            }
            ;

            await _userRepository.CreateAsync(user);
            return user;
        }

        private static string CreateMD5(string input)
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                return Convert.ToHexString(hashBytes);
            }
        }

        //обновить пользователя или время последнего захода\действия

        //авторизация пользователя по логин+пасс?
        public async Task<UserProfileInfo> GetProfile(Guid userId)
        {
            var user = await GetByIdAsync(userId);

            var userProfile = new UserProfileInfo()
            {
                UserId = userId,
                Email = user.Email,
                Nickname = user.Name,
                Status = user.Status,
                ProfileImage = user.AvatarUrl,
            };
            userProfile.Spaces = await _spaceService.GetUserSpaces(userId);
            userProfile.Categories = await _categoryService.GetAllForUserCategories(userId);
            return userProfile;
        }
    }

    public class UserProfileInfo
    {
        public Guid UserId { get; set; }
        public string Nickname { get; set; }
        public string Email { get; set; }
        public string Status { get; set; }
        public string? ProfileImage { get; set; }
        public List<Space> Spaces { get; set; } = new();
        public List<Category> Categories { get; set; } = new();
    }
}
