namespace backend_blazor.Authentication;

public class LoginModelDTO
{
    public string UserName { get; set; }
    public string Password { get; set; }  // Lưu mật khẩu đã hash
    
}

