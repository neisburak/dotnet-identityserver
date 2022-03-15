using IdentityMembersipAuthServer.Models;
using IdentityModel;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;

namespace IdentityMembersipAuthServer.Services
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly UserManager<AppUser> _userManager;

        public ResourceOwnerPasswordValidator(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var user = await _userManager.FindByEmailAsync(context.UserName);
            if (user != null)
            {
                var passwordIsValid = await _userManager.CheckPasswordAsync(user, context.Password);

                if (passwordIsValid)
                {
                    context.Result = new GrantValidationResult(user.Id, OidcConstants.AuthenticationMethods.Password);
                }
            }
        }
    }
}