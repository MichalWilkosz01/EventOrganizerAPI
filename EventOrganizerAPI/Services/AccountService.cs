using EventOrganizerAPI.Entities;
using EventOrganizerAPI.Exceptions;
using EventOrganizerAPI.Models.Dto;
using EventOrganizerAPI.Persistance;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace EventOrganizerAPI.Services
{
    public class AccountService : IAccountService
    {
        private readonly EventOrganizerDbContext _dbContext;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly AuthenticationSettings _authenticationSettings;

        public AccountService(EventOrganizerDbContext dbContext, IPasswordHasher<User> passwordHasher, 
            AuthenticationSettings authenticationSettings)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
            _authenticationSettings = authenticationSettings;
        }

        public async Task RegisterUser(RegisterUserDto dto)
        {
            var newUser = new User()
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                DateOfBirth = dto.DateOfBirth,
                City = dto.City,
                RoleId = 1
            };

            var hashedPassword = _passwordHasher.HashPassword(newUser, dto.Password);

            newUser.PasswordHashed = hashedPassword;
            await _dbContext.AddAsync(newUser);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<string> GenerateJWT(LoginUserDto dto)
        {
            var user = await _dbContext.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == dto.Email) ?? throw new AuthenticationException("Invalid email or password");


            var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHashed, dto.Password);

            if(passwordVerificationResult == PasswordVerificationResult.Failed)
            {
                throw new AuthenticationException("Invalid email or password");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.Role, user.Role.Name),
                new Claim(ClaimTypes.DateOfBirth, user.DateOfBirth.ToString(), ClaimValueTypes.Date),
                new Claim("City", user.City)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);

            var token = new JwtSecurityToken(_authenticationSettings.JwtIssuer, _authenticationSettings.JwtIssuer, claims, 
                expires: expires, 
                signingCredentials: cred);

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }
    }
}
