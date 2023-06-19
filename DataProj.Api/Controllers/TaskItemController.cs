using DataProj.ApiModels.DTOs.EntitiesDTO.TaskItem;
using DataProj.ApiModels.DTOs.EntitiesDTO.Project;
using DataProj.ApiModels.Response.Interfaces;
using DataProj.Domain.Models.Enums;
using DataProj.Services.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using FinalProj.ApiModels.DTOs.EntitiesDTO.TaskItem;

namespace DataProj.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskItemController : ControllerBase
    {
        private readonly ITaskItemService _taskItemService;

        public TaskItemController(ITaskItemService taskItemService)
        {
            _taskItemService = taskItemService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] FilterTaskItemDTO taskItemFilterDto, [FromQuery] Sort? sortOrder)
        {
            var response = await _taskItemService.GetAsync(taskItemFilterDto, sortOrder);
            return Ok(response.Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var response = await _taskItemService.GetByIdAsync(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTaskItemDTO createTaskItemDto)
        {
            var response = await _taskItemService.CreateAsync(createTaskItemDto);
            return Ok(response);
        }

        [HttpPut("update-executor/{taskId}")]
        public async Task<IActionResult> UpdateExecutor(Guid taskId, UpdateTaskItemWithExecutorDTO taskDto)
        {
            var response = await _taskItemService.UpdateExecutorAsync(taskId, taskDto);
            return Ok(response.Data);
        }

        [HttpPut("update-author/{taskId}")]
        public async Task<IActionResult> UpdateAuthor(Guid taskId, UpdateTaskItemWithAuthorDTO taskDto)
        {
            var response = await _taskItemService.UpdateAuthorAsync(taskId, taskDto);
            return Ok(response.Data);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await _taskItemService.DeleteAsync(id);
            return Ok(response);
        }
    }
}
