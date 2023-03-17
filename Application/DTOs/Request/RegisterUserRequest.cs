using System.ComponentModel.DataAnnotations;
namespace Application.DTOs.Request;

public class RegisterUserRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword{ get; set; }

    public List<string> Validate() 
    {
        var errors = new List<string>();

        if(String.IsNullOrEmpty(Email) || String.IsNullOrWhiteSpace(Email)) 
            errors.Add("The field email is mandatory");
        if(!new EmailAddressAttribute().IsValid(Email)) 
            errors.Add("Field email is invalid");
        if(String.IsNullOrEmpty(Password) || String.IsNullOrWhiteSpace(Password))
            errors.Add("The field password is mandatory");
        if(Password.Length < 6 || Password.Length > 50)
            errors.Add("Field password must be between 6 and 50 characters");
        if(ConfirmPassword != Password)
            errors.Add("Passwords must be the same");

        return errors;
    }

    
}