using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Newtonsoft.Json;
using System.Security.Cryptography;

namespace TMS.Common.Jwt
{
    /// <summary>
    /// Token帮助类
    /// </summary>
    public class TokenHelper : ITokenHelper
    {
        //依赖注入配置项
        private readonly IOptions<JwtTokenConfig> _options;

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="options"></param>
        public TokenHelper(IOptions<JwtTokenConfig> options)
        {
            _options = options;
        }


        /// <summary>
        /// 根据一个对象通过反射提供负载，生成token  
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="user"></param>
        /// <returns></returns>
        public TnToken CreateToken<T>(T entity) where T : class
        {
            //定义声明的集合
            List<Claim> claims = new List<Claim>();

            //用反射把数据提供给它
            foreach (var item in entity.GetType().GetProperties())
            {
                object obj = item.GetValue(entity);
                string value = "";
                if(obj != null)
                {
                    value = obj.ToString();
                }

                claims.Add(new Claim(item.Name, value));
            }

            //根据声明 生成token字符串
            return CreateTokenString(claims);
        }

        /// <summary>
        /// 根据键值对提供负载，生成token
        /// </summary>
        /// <param name="keyValuePairs"></param>
        /// <returns></returns>
        public TnToken CreateToken(Dictionary<string, string> keyValuePairs)
        {
            //定义声明的集合
            List<Claim> claims = new List<Claim>();

            foreach (var item in keyValuePairs)
            {
                claims.Add(new Claim(item.Key, item.Value));
            }

            //根据声明 生成token字符串
            return CreateTokenString(claims);
        }

        /// <summary>
        /// 私有方法，用于生成Token字符串
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        private TnToken CreateTokenString(List<Claim> claims)
        {
            //过期时间
            DateTime expires = DateTime.Now.AddMinutes(_options.Value.AccessTokenExpiresMinutes);

            var token = new JwtSecurityToken(
                issuer: _options.Value.Issuer,
                audience: _options.Value.Audience,
                claims: claims,           //携带的荷载
                notBefore: DateTime.Now,  //token生成时间
                expires: expires,         //token过期时间
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Value.IssuerSigningKey)), SecurityAlgorithms.HmacSha256
                    )
                );

            return new TnToken
            {
                Expires = expires,
                TokenStr = new JwtSecurityTokenHandler().WriteToken(token)
            };
        }



        /// <summary>
        /// 解密token
        /// </summary>
        /// <param name="Token"></param>
        /// <returns></returns>
        public bool GetClaimFtomToken(string Token, out Dictionary<string, string> keyValuePairs)
        {
            //以.号分割Token
            string[] Jwtparte = Token.Split('.');

            if (Jwtparte.Length < 3)
            {
                keyValuePairs = null;
                return false;
            }

            //解析头部,并且反序列化
            string headerJson = Base64UrlEncoder.Decode(Jwtparte[0]);
            var header = JsonConvert.DeserializeObject<Dictionary<string, string>>(headerJson);


            //解析荷载,并且反序列化
            string PayJson = Base64UrlEncoder.Decode(Jwtparte[0]);
            var Pay = JsonConvert.DeserializeObject<Dictionary<string, string>>(PayJson);

            //签名无法解析，生成秘钥  进行比较
            var hs256 = new HMACSHA256(Encoding.UTF8.GetBytes(_options.Value.IssuerSigningKey));

            string signHash = Base64UrlEncoder.Encode(hs256.ComputeHash(Encoding.UTF8.GetBytes(Jwtparte[0] + "." + Jwtparte[1])));

            if (!signHash.Equals(Jwtparte[2]))
            {
                keyValuePairs = null;
                return false;
            }
            //解析时间
            keyValuePairs = Pay;
            return true;
        }


    }
}
