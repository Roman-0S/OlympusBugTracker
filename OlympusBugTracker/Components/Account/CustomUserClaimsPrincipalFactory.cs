﻿using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using OlympusBugTracker.Client;
using OlympusBugTracker.Data;
using OlympusBugTracker.Helpers;
using System.Security.Claims;

namespace OlympusBugTracker.Components.Account
{
    public class CustomUserClaimsPrincipalFactory(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<IdentityOptions> options) : UserClaimsPrincipalFactory<ApplicationUser, IdentityRole>(userManager, roleManager, options)
    {
        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
        {
            ClaimsIdentity identity = await base.GenerateClaimsAsync(user);

            string profilePictureUrl = user.ImageId.HasValue ? $"/api/uploads/{user.ImageId}" : UploadHelper.DefaultProfilePicture;

            List<Claim> customClaims = [
                new Claim(nameof(UserInfo.FirstName), user.FirstName!),
                new Claim(nameof(UserInfo.LastName), user.LastName!),
                new Claim(nameof(UserInfo.ProfilePictureUrl), profilePictureUrl!),
                new Claim("CompanyId", user.CompanyId.ToString())
            ];

            identity.AddClaims(customClaims);

            return identity;
        }
    }
}
