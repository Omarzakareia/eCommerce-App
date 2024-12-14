using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services;

namespace Talabat.Service;

public class TokenService : ITokenService
{
    private readonly IConfiguration configuration;
    public TokenService(IConfiguration configuration)
    {
        this.configuration = configuration;
    }
    public async Task<string> CreateTokenAsync(AppUser User, UserManager<AppUser> userManager)
    {
        // Payload
        // 1. Private Claims [User - Defined]

        var AuthClaims = new List<Claim>()
        {
            new Claim(ClaimTypes.GivenName , User.DisplayName),
            new Claim(ClaimTypes.Email , User.Email)
        };
        var UserRoles = await userManager.GetRolesAsync(User);
        foreach (var Role in UserRoles)
        {
            AuthClaims.Add(new Claim(ClaimTypes.Role, Role));
        }

        var AuthKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]));

        var Token = new JwtSecurityToken(
            issuer: configuration["JWT:ValidIssuer"],
            audience: configuration["JWT:ValidAudience"],
            expires: DateTime.Now.AddDays(double.Parse(configuration["JWT:DurationInDays"])) ,
            claims : AuthClaims ,
            signingCredentials : new SigningCredentials(AuthKey , SecurityAlgorithms.HmacSha256)
            );
        return new JwtSecurityTokenHandler().WriteToken(Token);


    }
}
