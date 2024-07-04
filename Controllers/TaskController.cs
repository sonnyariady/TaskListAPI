using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TasklistAPI.Interface;
using TasklistAPI.Model.Request;

namespace TasklistAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskServices _taskServices;
        public TaskController(ITaskServices taskServices)
        {
            _taskServices = taskServices;
        }

        [HttpGet]
        [Route("/api/task/all")]
        public async Task<IActionResult> GetAll()
        {
            var tasks = await _taskServices.GetAll();
            return Ok(tasks);
        }

        [HttpPost]
        [Route("/api/task/create")]
        public async Task<IActionResult> Create([FromBody] TaskRequest input)
        {
            var userid = User.Claims.Where(x => x.Type == "Email").FirstOrDefault()?.Value;
            input.ActionBy = userid ?? string.Empty;
            var tasks = await _taskServices.Create(input);
            
            return Ok(tasks);
        }

        [HttpPut]
        [Route("/api/task/edit/{id}")]
        public async Task<IActionResult> Edit(int id, TaskRequest input)
        {
            var userid = User.Claims.Where(x => x.Type == "Email").FirstOrDefault()?.Value;
            input.ActionBy = userid ?? string.Empty;
            var result = await _taskServices.Edit(id, input);
            return Ok(result);
        }

        [HttpPost]
        [Route("/api/task/share")]
        public async Task<IActionResult> ShareTask(TaskShareRequest input)
        {
            var userid = User.Claims.Where(x => x.Type == "Email").FirstOrDefault()?.Value;
            var result = await _taskServices.ShareTask(input);
            return Ok(result);
        }

        [HttpPut]
        [Route("/api/task/markcompleted/{id}")]
        public async Task<IActionResult> MarkCompleted(int id)
        {
            var userid = User.Claims.Where(x => x.Type == "Email").FirstOrDefault()?.Value;
            var result = await _taskServices.SetCompleted(id, userid ?? string.Empty);
            return Ok(result);
        }

        [HttpDelete]
        [Route("/api/task/delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var tasks = await _taskServices.Delete(id);
            return Ok(tasks);
        }

    }
}
