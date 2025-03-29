using DataAccess.DbContext;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DataAccess.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(SneakersDbContext context) : base(context)
        {
        }

        public User GetUserByToken(string token)
        {
            var userMail = GetUserMail(token);
            return _context.Set<User>().FirstOrDefault(u => u.Email == userMail);
        }

        private string GetUserMail(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);

            var emailClaim = jwt.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Email);
            return emailClaim.Value;
        }


    }
}
