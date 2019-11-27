using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using YfCloud.Framework.WebApi;
using YfCloud.Models;

namespace YfCloud.Framework
{
    /// <summary>
    /// ToKen 管理
    /// </summary>
   public class TokenManager
    {
        /// <summary>
        /// 私钥
        /// </summary>
        public static string SecurityKey { get; set; }
        /// <summary>
        /// 创建JWT TOKEN
        /// </summary>
        /// <param name="payLoad">payLoad</param>
        /// <param name="expiresMinute">过期时间以分为单位</param>
        /// <returns></returns>
        public static string CreateTokenByHandler(Dictionary<string, object> payLoad, int expiresMinute)
        {
            var now = DateTime.UtcNow;

            var claims = new List<Claim>();
            foreach (var key in payLoad.Keys)
            {
                var tempClaim = new Claim(key, payLoad[key]?.ToString());
                claims.Add(tempClaim);
            }


            // Create the JWT and write it to a string
            var jwt = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                notBefore: now,
                expires: now.Add(TimeSpan.FromMinutes(expiresMinute)),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SecurityKey)), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }

        /// <summary>
        /// 验证token是否合法
        /// </summary>
        /// <param name="encodeJwt">Token</param>
        /// <returns></returns>
        public static bool Validate(string encodeJwt)
        {
            var success = true;
            var jwtArr = encodeJwt.Split('.');
            var header = JsonConvert.DeserializeObject<Dictionary<string, object>>(Base64UrlEncoder.Decode(jwtArr[0]));
            var payLoad = JsonConvert.DeserializeObject<Dictionary<string, object>>(Base64UrlEncoder.Decode(jwtArr[1]));

            var hs256 = new HMACSHA256(Encoding.ASCII.GetBytes(SecurityKey));
            //首先验证签名是否正确
            success = success && string.Equals(jwtArr[2], Base64UrlEncoder.Encode(hs256.ComputeHash(Encoding.UTF8.GetBytes(string.Concat(jwtArr[0], ".", jwtArr[1])))));
            if (!success)
            {
                return success;//签名不正确直接返回
            }
            //其次验证是否在有效期内（也应该必须）
            var now = ToUnixEpochDate(DateTime.UtcNow);
            success = success && (now >= long.Parse(payLoad["nbf"].ToString()) && now < long.Parse(payLoad["exp"].ToString()));

            return success;
        }

        /// <summary>
        /// 根据TOKEN 获取用户ID
        /// </summary>
        /// <param name="heads">传递HttpHeader</param>
        /// <returns></returns>
        public static int GetUserIDbyToken(IHeaderDictionary heads)
        {
            string token = heads[TokenConfig.Instace.TokenKey].ToString().Split('.')[1];
            var payLoad = JsonConvert.DeserializeObject<Dictionary<string, object>>(Base64UrlEncoder.Decode(token));

            return Convert.ToInt32(payLoad["UserID"]);
        }

        /// <summary>
        /// 根据TOKEN 获取用户ID
        /// </summary>
        /// <param name="heads">传递HttpHeader</param>
        /// <returns></returns>
        public static CurrentUser GetCurentUserbyToken(IHeaderDictionary heads)
        {
            CurrentUser curentUser = new CurrentUser();
            string token = heads[TokenConfig.Instace.TokenKey].ToString().Split('.')[1];
            var payLoad = JsonConvert.DeserializeObject<Dictionary<string, object>>(Base64UrlEncoder.Decode(token));
            curentUser.ID = Convert.ToString(payLoad["ID"]);
            curentUser.UserID= Convert.ToInt32(payLoad["UserID"]);
            curentUser.CompanyID= Convert.ToInt32(payLoad["CompanyID"]);
            curentUser.UserName = payLoad["UserName"].ToString();
            return curentUser;
        }

        public static CurrentUser GetIDbyToken(IHeaderDictionary heads)
        {
            CurrentUser currentUser = new CurrentUser();
            string token = heads[TokenConfig.Instace.TokenKey].ToString().Split('.')[1];
            var payLoad = JsonConvert.DeserializeObject<Dictionary<string, object>>(Base64UrlEncoder.Decode(token));

            currentUser.ID = Convert.ToString(payLoad["ID"]);
            currentUser.UserID = Convert.ToInt32(payLoad["UserID"]);
            currentUser.CompanyID = Convert.ToInt32(payLoad["CompanyID"]);
            currentUser.UserName = payLoad["UserName"].ToString();
            return currentUser;
        }

        public static string GetToken(IHeaderDictionary heads)
        {
            string token = heads[TokenConfig.Instace.TokenKey].ToString();

            return token;
        }


        private static long ToUnixEpochDate(DateTime date) =>(long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);


    }
}
