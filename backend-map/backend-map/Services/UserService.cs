using backend_map.DataContext;
using backend_map.Models;
using backend_map.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace backend_map.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        private readonly JwtService _jwtService;
        private readonly PasswordService _passwordService;

        public UserService(AppDbContext context, JwtService jwtService, PasswordService passwordService)
        {
            _context = context;
            _jwtService = jwtService;
            _passwordService = passwordService;
        }
        public async Task<AuthResult> Login(UserLoginRequest request)
        {
            try
            {
                var existingUser = await _context.Users.SingleOrDefaultAsync(u => u.Email == request.Email);

                if (existingUser == null)
                {
                    return new AuthResult()
                    {
                        IsSucceeded = false,
                        Message = new List<string>
                        {
                            "User not found, please register an account."
                        }
                    };
                }

                else if (!_passwordService.VerifyPasswordHash(request.Password,
                                                      existingUser.PasswordHash,
                                                      existingUser.PasswordSalt))
                {
                    return new AuthResult()
                    {
                        IsSucceeded = false,
                        Message = new List<string>
                            {
                                "Password is incorrect."
                            }
                    };
                }

                return new AuthResult()
                {
                    IsSucceeded = true,
                    Token = existingUser.Token
                };
            }
            catch (Exception ex)
            {
                return new AuthResult()
                {
                    IsSucceeded = false,
                    Message = new List<string>
                    {
                        $"An error message has occurred: {ex.Message}"
                    }
                };
            }
        }

        public async Task<AuthResult> Register(UserRegisterRequest request)
        {
            var validatingContext = new ValidationContext(request, serviceProvider: null, items: null);
            var validationResult = new List<ValidationResult>();
            if (!Validator.TryValidateObject(request,
                validatingContext,
                validationResult,
                validateAllProperties: true))
                return new AuthResult()
                {
                    IsSucceeded = false,
                    Message = new List<string>
                    {
                        "Invalid data."
                    }
                };

            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
                return new AuthResult()
                {
                    IsSucceeded = false,
                    Message = new List<string>
                    {
                        "Email already in use, please log in."
                    }
                };

            _passwordService.CreatePasswordHash(request.Password,
                                    out byte[] passwordHash,
                                    out byte[] passwordSalt);

            var newUser = new User
            {
                Name = request.Name,
                Email = request.Email,
                Password = request.Password,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                CreatedAt = DateTime.UtcNow
            };

            var tokenString = _jwtService.GenerateToken(newUser);
            newUser.Token = tokenString;

            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();

            return new AuthResult
            {
                IsSucceeded = true,
                Token = tokenString
            };
        }
    }
}
