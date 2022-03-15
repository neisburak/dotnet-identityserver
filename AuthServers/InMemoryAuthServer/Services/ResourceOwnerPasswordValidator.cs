using IdentityModel;
using IdentityServer4.Validation;
using InMemoryAuthServer;

namespace InMemoryAuthServer.Services
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        public Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var user = Config.TestUsers.FirstOrDefault(f => f.Username == context.UserName && f.Password == context.Password);
            if (user != null)
            {
                context.Result = new GrantValidationResult(user.SubjectId, OidcConstants.AuthenticationMethods.Password);
            }
            return Task.CompletedTask;
        }
    }
}