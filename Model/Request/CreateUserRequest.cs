namespace TasklistAPI.Model.Request
{
    public class BasicUserRequest
    {
        public string Email { get; set; } = string.Empty;
    }
    public class CreateUserRequest : BasicUserRequest
    {
        public string Password { get; set; } = string.Empty;
        public string RepeatPassword { get; set; } = string.Empty;
    }

    public class LoginRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
