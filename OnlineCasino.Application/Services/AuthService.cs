using Microsoft.Extensions.Logging;
using OnlineCasino.Application.DTOs;
using OnlineCasino.Application.Interfaces;

namespace OnlineCasino.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IJwtService _jwtService;
        private readonly ILogger<AuthService> _logger;

        public AuthService(IJwtService jwtService, ILogger<AuthService> logger)
        {
            _jwtService = jwtService;
            _logger = logger;
        }

        //for demo auth, on production will be getting username from db and compared with hashed passwords,
        //here it will work any combination of username and password just to generate token
        //to simulate authorization
        public Response<LoginResponse> LoginAsync(string username, string password)
        {
            var response = new Response<LoginResponse>();

            try
            {
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    response.Message = "Username and password are required";
                    return response;
                }

                var token = _jwtService.GenerateToken(username);
                return new Response<LoginResponse>(new LoginResponse
                {
                    Token = token,
                    Username = username,
                    Expires = DateTime.UtcNow.AddMinutes(60),
                    Message = "Login successful"
                });
            }
            catch (Exception ex)
            {
                var errorMessage = $"Error during login for username {username}";
                _logger.LogError(ex, errorMessage);
                response.Message = errorMessage;
                return response;
            }
        }
    }
}