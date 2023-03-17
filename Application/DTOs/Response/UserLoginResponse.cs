using System.Text.Json.Serialization;

namespace Application.DTOs.Response;

public class UserLoginResponse
{
    public bool Success { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Token { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DateTime? ExpirationDate { get; set; }
    public List<string> Erros { get; private set; }

    public UserLoginResponse() => Erros = new List<string>();

    public UserLoginResponse(bool success = true) : this() => Success = success;
    
    public UserLoginResponse(bool success, string token, DateTime expirationDate): this(success) 
    {
        Token = token;
        ExpirationDate = expirationDate;
    }

    public void AddError(string error) => Erros.Add(error);
    public void AddErrors(IEnumerable<string> errors) => Erros.AddRange(errors);

}