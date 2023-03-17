using Application.DTOs.Request;
using Application.interfaces.Services;
using Microsoft.AspNetCore.Authorization;

namespace API_Authentication.ApiEndpoints;

public static class LoginEndpoints
{
    public static void MapLoginEndpoints(this WebApplication app)
    {

        app.MapPost("/register", 
        async (RegisterUserRequest registerUserRequest, IIdentityService identityServices) =>
        {
            var modelErrors = registerUserRequest.Validate();
            if(modelErrors.Count() > 0) return Results.BadRequest(modelErrors);

            var result = await identityServices.RegisterUser(registerUserRequest);

            if(result.Success) 
                return Results.Ok(result);
            else if (result.Errors.Count() > 0) 
                return Results.BadRequest(result);
    
            return Results.StatusCode(StatusCodes.Status500InternalServerError);
        });

        app.MapPost("/login",
        async (UserLoginRequest userLoginRequest, IIdentityService identityServices) => 
        {
            var modelErrors = userLoginRequest.Validate();
            if(modelErrors.Count() > 0) return Results.BadRequest(modelErrors);

            var result = await identityServices.Login(userLoginRequest);
            if(result.Success) return Results.Ok(result);
            
            return Results.NotFound(result);
        });
    }
}