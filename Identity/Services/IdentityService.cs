using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Application.DTOs.Request;
using Application.DTOs.Response;
using Application.interfaces.Services;
using Identity.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Identity.Services;

public class IdentityService: IIdentityService
{
    private readonly SignInManager<IdentityUser> _singInManager;
    private readonly UserManager<IdentityUser> _userManage;
    private readonly JwtOptions _jwtOptions;

   public IdentityService(SignInManager<IdentityUser> signInManager,
                           UserManager<IdentityUser> userManager,
                           IOptions<JwtOptions> jwtOptions)
   {
        _singInManager = signInManager;
        _userManage = userManager;
        _jwtOptions = jwtOptions.Value;
   }

    public async Task<RegisterUserResponse> RegisterUser(RegisterUserRequest registerUser)
    {
        var identityUser = new IdentityUser
        {
            UserName = registerUser.Email,
            Email = registerUser.Email,
            EmailConfirmed = true
        };

        var result = await _userManage.CreateAsync(identityUser, registerUser.Password);

        if(result.Succeeded)
            await _userManage.SetLockoutEnabledAsync(identityUser, false);

        var userLoginResponse = new RegisterUserResponse(result.Succeeded);
        if(!result.Succeeded && result.Errors.Count() > 0) 
            userLoginResponse.AddErrors(result.Errors.Select(result => result.Description));

        return userLoginResponse;
    }

    public async Task<UserLoginResponse> Login(UserLoginRequest userLogin)
    {
        var result = await _singInManager.PasswordSignInAsync(userLogin.Email, userLogin.Password, false, true);
        if(result.Succeeded)
            return await GenerateToken(userLogin.Email);

        var userLoginResponse = new UserLoginResponse(result.Succeeded);
        if(!result.Succeeded)
        {
            if(result.IsLockedOut)
                userLoginResponse.AddError("This account is blocked");
            else if (result.IsNotAllowed)
                userLoginResponse.AddError("This account does not have permission to login.");
            else if (result.RequiresTwoFactor)
                userLoginResponse.AddError("It is necessary to confirm your login in your email.");
            else
                userLoginResponse.AddError("User or password is incorrect.");
        }

        return userLoginResponse;
    }

    private async Task<UserLoginResponse> GenerateToken(string email)
    {
        var user = await _userManage.FindByEmailAsync(email);
        var tokenClaims = await GetClaims(user);

        var expirationDate = DateTime.Now.AddSeconds(_jwtOptions.Expiration);

        var jwt = new JwtSecurityToken
        (
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: tokenClaims,
            notBefore: DateTime.Now,
            expires: expirationDate,
            signingCredentials: _jwtOptions.SigningCredentials
        );

        var token = new JwtSecurityTokenHandler().WriteToken(jwt);

        return new UserLoginResponse
        (
            success: true,
            token: token,
            expirationDate: expirationDate
        );
         
    }

    private async Task<IList<Claim>> GetClaims(IdentityUser user)
    {
        var claims = await _userManage.GetClaimsAsync(user);
        var roles = await _userManage.GetRolesAsync(user);

        claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.Id));
        claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
        claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
        claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, DateTime.Now.ToString()));
        claims.Add(new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToString()));

        foreach (var role in roles) claims.Add(new Claim("role", role));

        return claims;     
    }
}