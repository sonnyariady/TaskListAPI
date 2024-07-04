using TasklistAPI.Model.Entity;
using TasklistAPI.Model.Request;
using TasklistAPI.Model.Response;

namespace TasklistAPI.Interface
{
    public interface ITaskServices
    {
        Task<GlobalResponse> GetAll();
        Task<GlobalResponse> Create(TaskRequest input);
        Task<GlobalResponse> Delete(int id);
        Task<GlobalResponse> Edit(int id, TaskRequest input);
        Task<GlobalResponse> SetCompleted(int id, string actionby);
        Task<GlobalResponse> ShareTask(TaskShareRequest input);
    }
}
