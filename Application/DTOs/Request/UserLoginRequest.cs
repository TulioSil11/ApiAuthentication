using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Request;

public class UserLoginRequest
{
    public string Email { get; set; }
    public string Password { get; set; }

    public List<string> Validate() 
    {
        var errors = new List<string>();

        if(String.IsNullOrEmpty(Email) || String.IsNullOrWhiteSpace(Email)) 
            errors.Add("The field email is mandatory");
        if(!new EmailAddressAttribute().IsValid(Email)) 
            errors.Add("Field email is invalid");
        if(String.IsNullOrEmpty(Password) || String.IsNullOrWhiteSpace(Password))
            errors.Add("The field password is mandatory");

        return errors;
    }
}