using Application.DTOs.Request;
using Application.DTOs.Response;

namespace Application.interfaces.Services;

public interface IIdentityService
{
    Task<RegisterUserResponse> RegisterUser(RegisterUserRequest registerUser);
    Task<UserLoginResponse> Login(UserLoginRequest userLogin);
}