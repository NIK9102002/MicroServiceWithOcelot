using AuthenticationAPI.Application.DTOs;
using AuthenticationAPI.Application.Interfaces;
using AuthenticationAPI.Domain.Entities;
using AuthenticationAPI.Infrastructure.Data;
using eCommerce.SharedLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthenticationAPI.Infrastructure.Repositories
{
    public class UserRepository(AuthenticationDbContext context, IConfiguration config) : IUser
    {
        private async Task<AppUser> GetUserByEmail(string email)
        {
            var user = await context.AppUsers.FirstOrDefaultAsync(u => u.Email == email);
            return user is null ? null! : user!;
        }
        public async Task<GetUserDTO> GetUser(int userId)
        {
            var user = await context.AppUsers.FindAsync(userId);
            return user is not null ? new GetUserDTO(
                user.Id,
                user.Name!,
                user.TelephoneNumber!,
                user.Address!,
                user.Email!,
                user.Role!
            ) : null!;
        }

        public async Task<Response> Login(LoginDTO loginDTO)
        {
            var getuser = await GetUserByEmail(loginDTO.Email);
            if(getuser is null)
                return new Response(false, "User with this email does not exist.");

            bool verifyPassword = BCrypt.Net.BCrypt.Verify(loginDTO.Password, getuser.Password!);
            if(!verifyPassword)
                return new Response(false, "Invalid Credentials.");

            string token = GenerateToken(getuser);
            return new Response(true, token);
        }

        private string GenerateToken(AppUser user)
        {
            var key = Encoding.UTF8.GetBytes(config.GetSection("Authentication:SecretKey").Value!);
            var securityKey = new SymmetricSecurityKey(key);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new (ClaimTypes.Name, user.Name!),
                new (ClaimTypes.Email, user.Email!),
            };
            if (string.IsNullOrEmpty(user.Role) || !Equals("string", user.Role))
                claims.Add(new Claim(ClaimTypes.Role, user.Role!));

            var token = new JwtSecurityToken(
                issuer: config["Authentication:Issuer"],
                audience: config["Authentication:Audience"],
                claims: claims,
                expires: null,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<Response> Register(AppUserDTO appUserDTO)
        {
            var getuser = await GetUserByEmail(appUserDTO.Email);
            if (getuser is not null)            
                return new Response( false, "User with this email already exists.");
            
            var result = context.AppUsers.Add(new AppUser
            {
                Name = appUserDTO.Name,
                TelephoneNumber = appUserDTO.TelephoneNumber,
                Address = appUserDTO.Address,
                Email = appUserDTO.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(appUserDTO.Password),
                Role = appUserDTO.Role
            });
            
            await context.SaveChangesAsync();

            return result.Entity.Id > 0
                ? new Response(true, "User registered successfully.")
                : new Response(false, "Failed to register user.");
        }
    }
}
