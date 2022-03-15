using System.Security.Claims;
using CustomMembershipAuthServer.Repositories.Interfaces;
using IdentityServer4;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.IdentityModel.JsonWebTokens;

namespace CustomMembershipAuthServer.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IUserRepository _userRepository;

        public ProfileService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var userId = context.Subject.GetSubjectId();
            var user = await _userRepository.GetByIdAsync(int.Parse(userId));

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Email, user.Email),
                new("name", user.UserName),
                new("city", user.City)
            };

            if (user.UserName == "bob")
            {
                claims.Add(new("role", "admin"));
            }
            else
            {
                claims.Add(new("role", "customer"));
            }

            context.AddRequestedClaims(claims);

            // Adds claims to inside access token
            //context.IssuedClaims = claims;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var userId = context.Subject.GetSubjectId();
            var user = await _userRepository.GetByIdAsync(int.Parse(userId));
            context.IsActive = user != null;
        }
    }
}