namespace backend_blazor.Authentication;

public class AuthResultDTO
{
    public string Token { get; set; }
    public string Role { get; set; }
    public bool IsSuccess { get; set; }  // Thêm dòng này
    public string ErrorMessage { get; set; }  // Và dòng này
}
