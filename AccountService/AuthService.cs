using Hospital_Management.DTO;
using Hospital_Management.Helper;
using Hospital_Management.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Hospital_Management.AccountService
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration configuration;
        private readonly JWT jwt;
        public AuthService(
            UserManager<ApplicationUser> _userManager,
            RoleManager<IdentityRole> _roleManager,
            IOptions<JWT> _jwt,
            IConfiguration _configuration
            )
        {
            userManager = _userManager;
            roleManager = _roleManager;
            this.configuration = _configuration;
            jwt = _jwt.Value;

        }
        public async Task<AuthModel> Register(RegisterModel model)
        {
            if(await userManager.FindByEmailAsync(model.Email)is not null)
            {
                return new AuthModel { Message = "Email is already exist" };
            }
            if (await userManager.FindByNameAsync(model.UserName) is not null)
            {
                return new AuthModel { Message = "User Name is already exist" };
            }
            ApplicationUser user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                PasswordHash = model.Password,
            };
            IdentityResult result = await userManager.CreateAsync(user, model.Password);
            if(!result.Succeeded)
            {
                string errors = string.Empty;
                foreach (var error in result.Errors)
                {
                    errors += $"{error.Description},";
                }
                return new AuthModel { Message = errors };
            }
            //await userManager.AddToRoleAsync(user, "User");

            var jwtSecurityToken = await CreateJwtToken(user);

            return new AuthModel
            {
                Email = user.Email,
                ExpiresOn = jwtSecurityToken.ValidTo,
                IsAuthenticated = true,
                Roles = new List<string> { "User" },
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                UserName = user.UserName
            };
        }
        public async Task<AuthModel> Login(TokenRequestModel model)
        {
            AuthModel authModel = new AuthModel();

            ApplicationUser user = await userManager.FindByEmailAsync(model.Email);

            if (user is null || !await userManager.CheckPasswordAsync(user, model.Password))
            {
                authModel.Message = "Email or Password is incorrect!";
                return authModel;
            }

            var jwtSecurityToken = await CreateJwtToken(user);
            var rolesList = await userManager.GetRolesAsync(user);

            authModel.IsAuthenticated = true;
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authModel.Email = user.Email;
            authModel.UserName = user.UserName;
            authModel.ExpiresOn = jwtSecurityToken.ValidTo;
            authModel.Roles = rolesList.ToList();

            return authModel;
        }
        public async Task<string> AddRole(AddRoleModel model)
        {
            var user = await userManager.FindByIdAsync(model.UserId);

            if (user is null || !await roleManager.RoleExistsAsync(model.Role))
                return "Invalid user ID or Role";

            if (await userManager.IsInRoleAsync(user, model.Role))
                return "User already assigned to this role";

            IdentityResult result = await userManager.AddToRoleAsync(user, model.Role);

            return result.Succeeded ? string.Empty : "Sonething went wrong";
        }
        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            var userClaims = await userManager.GetClaimsAsync(user);
            var roles = await userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id) 
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.SecretKey));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                issuer: jwt.ValidIssuer,
                audience: jwt.ValidAudience,
                claims: claims,
                expires: DateTime.Now.AddDays(jwt.DurationInDays),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }
        public async Task<AuthModel> ResetPassword(ResetPasswordModel model)
        {
            ApplicationUser user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return new AuthModel
                {
                    IsSuccess = false,
                    Message = "Email does not exists!",
                };

            if (string.Compare(model.NewPassword, model.ConfirmNewPassword)!=0)
                return new AuthModel
                {
                    IsSuccess = false,
                    Message = "The new password and confirm new password does not match!",
                };

            var result = await userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);

            if (result.Succeeded)
                return new AuthModel
                {
                    Message = "Password has been reset successfully!",
                    IsSuccess = true,
                };

            return new AuthModel
            {
                Message = "Something went wrong",
                IsSuccess = false,
            };
        }
        public async Task<AuthModel> ForgetPassword(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
                return new AuthModel
                {
                    IsSuccess = false,
                    Message = "Email does not exists!",
                };
            string token = await userManager.GeneratePasswordResetTokenAsync(user);
            return new AuthModel
            {
                IsSuccess = true,
                Token = token
            };
        }
    }
}
