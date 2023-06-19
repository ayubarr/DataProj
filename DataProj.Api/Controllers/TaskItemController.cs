using DataProj.ApiModels.DTOs.EntitiesDTO.Assignment;
using DataProj.ApiModels.DTOs.EntitiesDTO.Project;
using DataProj.ApiModels.Response.Interfaces;
using DataProj.Domain.Models.Enums;
using DataProj.Services.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

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
            return CreateResponseFromBaseResponse(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var response = await _taskItemService.GetByIdAsync(id);
            return CreateResponseFromBaseResponse(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateTaskItemDTO createTaskItemDto)
        {
            var response = await _taskItemService.CreateAsync(createTaskItemDto);
            return CreateResponseFromBaseResponse(response);
        }

        [HttpPut("{taskId}/{taskDto}")]
        public async Task<IActionResult> Update(Guid taskId, UpdateTaskItemDTO taskDto)
        {
            var response = await _taskItemService.UpdateAsync(taskId, taskDto);
            return CreateResponseFromBaseResponse(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await _taskItemService.DeleteAsync(id);
            return CreateResponseFromBaseResponse(response);
        }

        private IActionResult CreateResponseFromBaseResponse<T>(IBaseResponse<T> baseResponse)
        {
            if (baseResponse.IsSuccess)
            {
                return Ok(baseResponse.Data);
            }
            return NotFound(baseResponse.Message);
        }
    }
}
