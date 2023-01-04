using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using EducationSystem.Domain.Entities;
using EducationSystem.Domain.Enumerations;
using EducationSystem.Application.Common.Models;
using EducationSystem.Application.Common.Extensions;
using EducationSystem.Application.Common.Interfaces;

namespace EducationSystem.Infrastructure.Services
{
    public class TokenManagerService : ITokenManagerService
    {
        private readonly IAppDbContext _appDbContext;
        private readonly IDateTimeService _dateTimeService;
        private readonly JwtTokenSetting _jwtTokenSettings;

        public TokenManagerService(IAppDbContext appDbContext, IDateTimeService dateTimeService,
            JwtTokenSetting jwtTokenSettings)
        {
            _appDbContext = appDbContext;
            _dateTimeService = dateTimeService;
            _jwtTokenSettings = jwtTokenSettings;
        }

        public async Task<JwtToken> GenerateJwtTokenAsynce(int userId, List<Claim> claims)
        {
            var issuerSigningKeyBytes = _jwtTokenSettings.IssuerSigningKey.GetUTF8Bytes();
            var tokenDecryptionKeyBytes = _jwtTokenSettings.TokenDecryptionKey.GetUTF8Bytes();

            var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(issuerSigningKeyBytes), SecurityAlgorithms.HmacSha256Signature);

            var encryptingCredentials = new EncryptingCredentials(
            new SymmetricSecurityKey(tokenDecryptionKeyBytes), SecurityAlgorithms.Aes128KW, SecurityAlgorithms.Aes128CbcHmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                IssuedAt = _dateTimeService.Now,
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = signingCredentials,
                EncryptingCredentials = encryptingCredentials,
                CompressionAlgorithm = CompressionAlgorithms.Deflate,
                NotBefore = _dateTimeService.Now.AddMinutes(_jwtTokenSettings.NotBeforeMinutes),
                Expires = _dateTimeService.Now.AddMinutes(_jwtTokenSettings.AccessTokenExpiresMinutes)
            };

            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var securityToken = new JwtSecurityTokenHandler()
                .CreateToken(tokenDescriptor);

            var refreshToken =
                await CreateTokenAsync(userId, TokenType.RefreshToken,
                TokenDataType.AlphaNumerical, _jwtTokenSettings.RefreshTokenExpiresMinutes);

            var jwtToken = new JwtToken
            {
                RefreshToken = refreshToken,
                AccessToken = $"Bearer {jwtTokenHandler.WriteToken(securityToken)}",
                AccessTokenExpireTime = securityToken.ValidTo.ToLocalTime().ToUnixTimeStamp(),
                RefreshTokenExpireTime = _dateTimeService.Now.AddMinutes(_jwtTokenSettings.RefreshTokenExpiresMinutes).ToLocalTime().ToUnixTimeStamp()
            };

            return jwtToken;
        }



        public async Task<string> CreateTokenAsync(int userId, TokenType type, TokenDataType dataType, double expiresMinutes)
        {
            string token;

            do
            {
                if (dataType == TokenDataType.Numerical)
                {
                    token = Random.Shared.Next(10000, 99999).ToString();
                }
                else
                {
                    token = Guid.NewGuid().ToString();
                }
            }

            while (await IsTokenExistAsync(userId, token));

            _appDbContext.UserTokens.Add(new UserToken
            {
                Type = type,
                Value = token,
                UserId = userId,
                ExpireAt = _dateTimeService.Now.AddMinutes(expiresMinutes)
            });

            await _appDbContext.SaveChangesAsync();

            return token;
        }

        #region utility

        private async Task<bool> IsTokenExistAsync(int userId, string token)
        {
            var isExist = await _appDbContext.UserTokens
                .AnyAsync(x => x.UserId == userId && x.Value == token);

            return isExist;
        }

        #endregion
    }
}

