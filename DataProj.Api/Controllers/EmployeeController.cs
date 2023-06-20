using DataProj.ApiModels.Auth.Models;
using DataProj.ApiModels.DTOs.EntitiesDTO.Employee;
using DataProj.Domain.Models.Entities;
using DataProj.Domain.Models.Enums;
using DataProj.Services.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DataProj.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly IAuthManager<Employee> _authService;

        public EmployeeController(IEmployeeService employeeService, IAuthManager<Employee> authManager)
        {
            _employeeService = employeeService;
            _authService = authManager;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _employeeService.GetAllAsync();
            return Ok(response.Data);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var response = await _employeeService.GetByIdAsync(id);
            return Ok(response.Data);
        }

        [HttpGet]
        [Route("get-projects/{employeeId}")]
        public async Task<IActionResult> GetProjects(string employeeId)
        {
            var response = await _employeeService.GetEmployeeProjectsAsync(employeeId);
            return Ok(response.Data);
        }

        [HttpGet]
        [Route("get-projects-tasks/{employeeId}")]
        public async Task<IActionResult> GetTasks(string employeeId)
        {
            var response = await _employeeService.GetProjectTasksAsync(employeeId);
            return Ok(response.Data);
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string id, UpdateEmployeeDTO employeeDto)
        {
            var response = await _employeeService.UpdateAsync(id, employeeDto);
            return Ok(response.Data);
        }


        [HttpPut]
        [Route("put-role/{userid}/{roletype}")]
        public async Task<IActionResult> PutRoleById(string userId, Roles roleType)
        {
            var response = await _employeeService.SetEmployeeNewRoleByIdAsync(userId, roleType);
            return Ok(response);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var response = await _employeeService.DeleteByIdAsync(id);
            return Ok(response);
        }


        [HttpPost]
        [Route("assign-to-project/{projectId}")]
        public async Task<IActionResult> Assign(List<string> employeesId, Guid projectId)
        {
            var response = await _employeeService.AssignEmployeeToProjectAsync(employeesId, projectId);
            return Ok(response);
        }


        [HttpPost]
        [Route("remove-from-project/{projectId}")]
        public async Task<IActionResult> RemoveFromProject(List<string> employeesId, Guid projectId)
        {
            var response = await _employeeService.RemoveEmployeeFromProjectAsync(employeesId, projectId);
            return Ok(response);
        }


        [HttpPost]
        [Route("assign-executor-to-task/{taskId}/{employeeId}")]
        public async Task<IActionResult> AssignExecutorToTask(Guid taskId, string employeeId)
        {
            var response = await _employeeService.AssignExecutorToTaskAsync(taskId, employeeId);
            return Ok(response);
        }

        [HttpPut]
        [Route("change-task-status/{taskId}/{newStatus}")]
        public async Task<IActionResult> ChangeTaskStatus(Guid taskId, Status newStatus)
        {
            var response = await _employeeService.ChangeTaskStatusAsync(taskId, newStatus);
            return Ok(response);
        }


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var response = await _authService.Login(model);

            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return Unauthorized(response.Message);
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var result = await _authService.Register(model);
            await _employeeService.SetEmployeeNewRoleByIdAsync(result.Data, Roles.Employee);

            return Ok(result);
        }

        [HttpPost]
        [Route("refresh-token")]
        public async Task<IActionResult> RefreshToken(TokenModel tokenModel)
        {
            var result = await _authService.RefreshToken(tokenModel);
            return Ok(result);
        }

        [HttpPost]
        [Route("revoke/{username}")]
        public async Task<IActionResult> Revoke(string username)
        {
            try
            {
                await _authService.RevokeRefreshTokenByUsernameAsync(username);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("revoke-all")]
        public async Task<IActionResult> RevokeAll()
        {
            await _authService.RevokeAllRefreshTokensAsync();
            return NoContent();
        }

    }
}
