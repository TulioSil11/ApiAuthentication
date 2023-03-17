namespace Application.DTOs.Response;

public class RegisterUserResponse
{
    public bool Success { get; set; }
    public List<string> Errors { get; private set; }

    public RegisterUserResponse() => Errors = new List<string>();
    public RegisterUserResponse(bool success) : this() => Success = success;

    public void AddErrors(IEnumerable<string> erros) => Errors.AddRange(erros);
}