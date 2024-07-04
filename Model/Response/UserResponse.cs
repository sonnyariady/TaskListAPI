using TasklistAPI.Model.Entity;

namespace TasklistAPI.Model.Response
{
    public class UserResponse
    {
        public string Email { get; set; } = string.Empty;

        public UserResponse()
        {

        }

        public UserResponse(UserAccount entity)
        {
            this.Email = entity.UserName;

        }
    }

    public class LoginResult
    {
        public string Email { get; set; } = string.Empty;
        public string token { get; set; } = string.Empty;
    }
}
