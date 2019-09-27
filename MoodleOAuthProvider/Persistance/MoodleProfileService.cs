using IdentityServer4.Models;
using IdentityServer4.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MoodleOAuthProvider.Persistance
{
    public class MoodleProfileService : IProfileService
    {
        private MoodleUserContext _userContext;
        public MoodleProfileService(MoodleUserContext userContext)
        {
            _userContext = userContext;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            Console.WriteLine(context.Subject.ToString());
            var user = await _userContext.FindBySubjectAsync(context.Subject);
            var claims = new List<Claim>()
            {
                new Claim("name", user.FullName),
                new Claim("email", user.Email),
                new Claim("surname", user.LastName)
            };
            context.IssuedClaims.AddRange(claims);
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var usr = await _userContext.FindBySubjectAsync(context.Subject);
            bool result = usr != null;
            context.IsActive = result;
        }
    }
}
