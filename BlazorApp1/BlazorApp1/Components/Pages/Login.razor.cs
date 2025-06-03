using BlazorApp1.DTOs;
using BlazorApp1.Services;
using Microsoft.AspNetCore.Components;



namespace BlazorApp1.Components.Pages
{
    public partial class Login : ComponentBase
    {
        protected LoginModelDTO loginModel = new LoginModelDTO(); // Đã khai báo ở đây
        protected string errorMessage = string.Empty;
        protected bool isLoading = false;

        [Inject]
        protected AuthService AuthService { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        protected async Task HandleLogin()
        {
            try
            {
                isLoading = true;
                errorMessage = string.Empty;

                var result = await AuthService.Login(loginModel);

                if (result.IsSuccess)
                {
                    NavigationManager.NavigateTo("/");
                }
                else
                {
                    errorMessage = result.ErrorMessage;
                }
            }
            catch (Exception)
            {
                errorMessage = "Có lỗi xảy ra khi đăng nhập. Vui lòng thử lại sau.";
            }
            finally
            {
                isLoading = false;
            }
        }
    }
}