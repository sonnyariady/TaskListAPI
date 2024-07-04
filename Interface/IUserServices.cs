using TasklistAPI.Model.Request;
using TasklistAPI.Model.Response;

namespace TasklistAPI.Interface
{
    public interface IUserServices
    {
        Task<GlobalResponse> Create(CreateUserRequest input);
        Task<GlobalResponse> Login(LoginRequest input);
    }
}
