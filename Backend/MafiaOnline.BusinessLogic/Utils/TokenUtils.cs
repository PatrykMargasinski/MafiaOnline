using MafiaOnline.BusinessLogic.Entities;
using MafiaOnline.DataAccess.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MafiaOnline.BusinessLogic.Utils
{
    public interface ITokenUtils
    {
        public string CreateToken(Player player);
        public string GenerateRefreshToken();
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        public TokenDatas DecodeToken(string jwt);
    }

    public class TokenUtils : ITokenUtils
    {
        private readonly IConfiguration _config;
        public TokenUtils(IConfiguration config)
        {
            _config = config;
        }


        /// <summary>
        /// Creates a JWT token
        /// </summary>
        public string CreateToken(Player player)
        {
            var key = _config.GetValue<string>("Security:AuthKey");
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, player.Nick),
                new Claim(ClaimTypes.Role, "Player"),
                new Claim(type: "bossId",value: player.BossId.ToString()),
                new Claim(type: "playerId",value: player.Id.ToString())
            };

            var tokenOptions = new JwtSecurityToken(
                expires: DateTime.Now.AddHours(1),
                claims: claims,
                signingCredentials: signingCredentials
                );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return tokenString;
        }

        /// <summary>
        /// Creates a refresh token
        /// </summary>
        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        /// <summary>
        /// Returns principal from expired token
        /// </summary>
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetValue<string>("Security:AuthKey"))),
                ValidateLifetime = false
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");
            return principal;
        }

        public TokenDatas DecodeToken(string jwt)
        {
            var handler = new JwtSecurityTokenHandler();
            var decodedValue = handler.ReadJwtToken(jwt.Substring(7));
            var claims = decodedValue.Claims;
            var datas = new TokenDatas()
            {
                BossId = long.Parse(claims.FirstOrDefault(x => x.Type == "bossId").Value),
                PlayerId = long.Parse(claims.FirstOrDefault(x => x.Type == "playerId").Value),
                PlayerNick = claims.FirstOrDefault(x => x.Type == ClaimTypes.Name).Value,
                PlayerRole = claims.FirstOrDefault(x => x.Type == ClaimTypes.Role).Value
            };
            return datas;
        }
    }
}
