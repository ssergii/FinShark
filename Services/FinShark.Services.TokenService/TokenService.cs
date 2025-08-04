using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FinShark.Services.Interfaces;
using FinShark.Services.Interfaces.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace FinShark.Services.TokenService;

public class TokenService : ITokenService
{
    #region fields
    private readonly IConfiguration _config;
    private readonly SymmetricSecurityKey _key;
    #endregion

    #region constructor
    public TokenService(IConfiguration config)
    {
        _config = config;
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:SigningKey"]));
    }
    #endregion

    public string GenerateToken(User user)
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user));

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.GivenName, user.Name)
        };

        var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddDays(7),
            signingCredentials: credentials
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return tokenString;

        // var tokenDescriptor = new SecurityTokenDescriptor
        // {
        //     Subject = new ClaimsIdentity(claims),
        //     Expires = DateTime.Now.AddDays(7),
        //     SigningCredentials = credentials,
        //     Issuer = _config["Jwt:Issuer"],
        //     Audience = _config["Jwt:Audience"]
        // };

        // var tokenHandler = new JwtSecurityTokenHandler();
        // var token = tokenHandler.CreateToken(tokenDescriptor);

        // return tokenHandler.WriteToken(token);
    }
}
