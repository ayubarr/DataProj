using DataProj.ApiModels.DTOs.EntitiesDTO.Project;
using DataProj.Domain.Models.Enums;
using DataProj.Services.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DataProj.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] FilterProjectDTO projectFilterDto, [FromQuery] Sort? sortOrder)
        {
            var response = await _projectService.GetAsync(projectFilterDto, sortOrder);
            return Ok(response.Data);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var response = await _projectService.GetByIdAsync(id);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProjectDTO createProjectDto)
        {
            var response = await _projectService.CreateAsync(createProjectDto);
            return Ok(response);
        }

        [HttpPut("{projectid}")]
        public async Task<IActionResult> Update(Guid projectid, UpdateProjectDTO projectDto)
        {
            var response = await _projectService.UpdateAsync(projectid, projectDto);
            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await _projectService.DeleteAsync(id);
            return Ok(response);
        }
    }
}
