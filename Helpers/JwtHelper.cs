using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CSP_Redemption_WebApi.Helpers
{
    public class JwtHelper
    {
        public static string GetToken(string Key
            , int ExpireInMinutes
            , string Issuer
            , string Audience
            , int UserId
            , string firstName
            , string lastName
            , string Email
            , int BrandId
            , string AvatarPath)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, "Subject"),
                new Claim("userId", UserId.ToString()),
                new Claim("firstName", firstName),
                new Claim("lastName", lastName),
                new Claim("email", Email),
                new Claim("brandId", BrandId.ToString()),
                new Claim("avatarPath", AvatarPath),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddMinutes(Convert.ToDouble(ExpireInMinutes));

            var token = new JwtSecurityToken(
                issuer: Issuer,
                audience: Audience,
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string Decrypt(string stream, string key)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = handler.ReadToken(stream) as JwtSecurityToken;

            return tokenS.Claims.First(claim => claim.Type == key).Value;
        }
    }
}
