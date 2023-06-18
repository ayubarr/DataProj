using DataProj.ApiModels.DTOs.EntitiesDTO.Employee;
using DataProj.ApiModels.Response.Helpers;
using DataProj.ApiModels.Response.Interfaces;
using DataProj.DAL.Repository.Interfaces;
using DataProj.Domain.Models.Entities;
using DataProj.Domain.Models.Enums;
using DataProj.Services.Mapping.Helpers;
using DataProj.Services.Services.Interfaces;
using DataProj.ValidationHelper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DataProj.Services.Services.Implementations
{
    public class EmployeeService : IEmployeeService
    {
        private readonly UserManager<Employee> _userManager;
        private readonly IBaseAsyncRepository<Project> _repository;
        private readonly IBaseAsyncRepository<TaskItem> _taskItemRepository;

        public EmployeeService(UserManager<Employee> userManager,
            IBaseAsyncRepository<Project> repository,
            IBaseAsyncRepository<TaskItem> taskItemRepository)
        {
            _userManager = userManager;
            _repository = repository;
            _taskItemRepository = taskItemRepository;
        }

        public async Task<IBaseResponse<IEnumerable<Employee>>> GetAllAsync()
        {
            try
            {
                var users = await _userManager.Users.ToListAsync();
                ObjectValidator<IEnumerable<Employee>>.CheckIsNotNullObject(users);

                return ResponseFactory<IEnumerable<Employee>>.CreateSuccessResponse(users);
            }
            catch (ArgumentNullException ex)
            {
                return ResponseFactory<IEnumerable<Employee>>.CreateNotFoundResponse(ex);
            }
            catch (Exception ex)
            {
                return ResponseFactory<IEnumerable<Employee>>.CreateErrorResponse(ex);
            }
        }

        public async Task<IBaseResponse<Employee>> GetByIdAsync(string employeeId)
        {
            try
            {
                StringValidator.CheckIsNotNull(employeeId);

                var user = await _userManager.FindByIdAsync(employeeId);
                ObjectValidator<Employee>.CheckIsNotNullObject(user);

                return ResponseFactory<Employee>.CreateSuccessResponse(user);
            }
            catch (ArgumentNullException ex)
            {
                return ResponseFactory<Employee>.CreateNotFoundResponse(ex);
            }
            catch (Exception ex)
            {
                return ResponseFactory<Employee>.CreateErrorResponse(ex);
            }
        }

        public async Task<IBaseResponse<bool>> UpdateAsync(UpdateEmployeeDTO employeeDto)
        {
            try
            {
                ObjectValidator<UpdateEmployeeDTO>.CheckIsNotNullObject(employeeDto);
                var employee = MapperHelperForUser<UpdateEmployeeDTO, Employee>.Map(employeeDto);

                var result = await _userManager.UpdateAsync(employee);

                if (result.Succeeded)
                {
                    return ResponseFactory<bool>.CreateSuccessResponse(true);
                }
                else
                {
                    return ResponseFactory<bool>.CreateErrorResponse(new Exception("Failed to update user."));
                }
            }
            catch (ArgumentNullException ex)
            {
                return ResponseFactory<bool>.CreateNotFoundResponse(ex);
            }
            catch (Exception ex)
            {
                return ResponseFactory<bool>.CreateErrorResponse(ex);
            }
        }

        public async Task<IBaseResponse<bool>> DeleteByIdAsync(string employeeId)
        {
            try
            {
                StringValidator.CheckIsNotNull(employeeId);

                var employee = await _userManager.FindByIdAsync(employeeId);
                ObjectValidator<Employee>.CheckIsNotNullObject(employee);

                var result = await _userManager.DeleteAsync(employee);
                if (result.Succeeded)
                {
                    return ResponseFactory<bool>.CreateSuccessResponse(true);
                }
                else
                {
                    return ResponseFactory<bool>.CreateErrorResponse(new Exception("Failed to delete user."));
                }
            }
            catch (ArgumentNullException ex)
            {
                return ResponseFactory<bool>.CreateNotFoundResponse(ex);
            }
            catch (Exception ex)
            {
                return ResponseFactory<bool>.CreateErrorResponse(ex);
            }
        }

        public async Task<IBaseResponse<bool>> AssignEmployeeToProjectAsync(string employeeId, Guid projectId)
        {
            try
            {
                StringValidator.CheckIsNotNull(employeeId);
                ObjectValidator<Guid>.CheckIsNotNullObject(projectId);

                var employee = await _userManager.FindByIdAsync(employeeId);
                ObjectValidator<Employee>.CheckIsNotNullObject(employee);

                var project = await _repository.ReadByIdAsync(projectId);

                var projectEmployee = new ProjectEmployee
                {
                    Employee = employee,
                    Project = project,
                    EmployeeId = employeeId,
                    ProjectId = projectId
                };

                project.Employees.Add(projectEmployee);
                employee.Projects.Add(projectEmployee);

                await _repository.UpdateAsync(project);
                await _userManager.UpdateAsync(employee);

                return ResponseFactory<bool>.CreateSuccessResponse(true);
            }
            catch (ArgumentNullException ex)
            {
                return ResponseFactory<bool>.CreateNotFoundResponse(ex);
            }
            catch (Exception ex)
            {
                return ResponseFactory<bool>.CreateErrorResponse(ex);
            }
        }

        public async Task<IBaseResponse<bool>> AssignExecutorToTaskAsync(Guid taskId, string employeeId)
        {
            try
            {
                ObjectValidator<Guid>.CheckIsNotNullObject(taskId);
                StringValidator.CheckIsNotNull(employeeId);

                var employee = await _userManager.FindByIdAsync(employeeId);
                ObjectValidator<Employee>.CheckIsNotNullObject(employee);

                var taskItem = await _taskItemRepository.ReadByIdAsync(taskId);

                taskItem.Executor = employee;

                await _taskItemRepository.UpdateAsync(taskItem);

                return ResponseFactory<bool>.CreateSuccessResponse(true);
            }
            catch (ArgumentNullException ex)
            {
                return ResponseFactory<bool>.CreateNotFoundResponse(ex);
            }
            catch (Exception ex)
            {
                return ResponseFactory<bool>.CreateErrorResponse(ex);
            }
        }

        public async Task<IBaseResponse<bool>> ChangeTaskStatusAsync(Guid taskId, Status newStatus)
        {
            try
            {
                ObjectValidator<Guid>.CheckIsNotNullObject(taskId);


                var taskItem = await _taskItemRepository.ReadByIdAsync(taskId);

                taskItem.Status = newStatus;
                await _taskItemRepository.UpdateAsync(taskItem);

                return ResponseFactory<bool>.CreateSuccessResponse(true);
            }
            catch (ArgumentNullException ex)
            {
                return ResponseFactory<bool>.CreateNotFoundResponse(ex);
            }
            catch (Exception ex)
            {
                return ResponseFactory<bool>.CreateErrorResponse(ex);
            }
        }


        public async Task<IBaseResponse<IEnumerable<Project>>> GetEmployeeProjectsAsync(string employeeId)
        {
            try
            {
                StringValidator.CheckIsNotNull(employeeId);

                var employee = await _userManager.FindByIdAsync(employeeId);
                ObjectValidator<Employee>.CheckIsNotNullObject(employee);

                var projects = await _repository.ReadAllAsync().Result
                    .Where(p => p.Employees
                    .Any(pe => pe.EmployeeId == employeeId))
                    .ToListAsync();

                ObjectValidator<IEnumerable<Project>>.CheckIsNotNullObject(projects);

                return ResponseFactory<IEnumerable<Project>>.CreateSuccessResponse(projects);
            }
            catch (ArgumentNullException ex)
            {
                return ResponseFactory<IEnumerable<Project>>.CreateNotFoundResponse(ex);
            }
            catch (Exception ex)
            {
                return ResponseFactory<IEnumerable<Project>>.CreateErrorResponse(ex);
            }
        }

        public async Task<IBaseResponse<IEnumerable<TaskItem>>> GetProjectTasksAsync(string employeeId)
        {
            try
            {
                StringValidator.CheckIsNotNull(employeeId);

                var manager = await _userManager.FindByIdAsync(employeeId);

                var tasks = manager.ExecutorTasks;

                ObjectValidator<IEnumerable<TaskItem>>.CheckIsNotNullObject(tasks);

                return ResponseFactory<IEnumerable<TaskItem>>.CreateSuccessResponse(tasks);
            }
            catch (ArgumentNullException ex)
            {
                return ResponseFactory<IEnumerable<TaskItem>>.CreateNotFoundResponse(ex);
            }
            catch (Exception ex)
            {
                return ResponseFactory<IEnumerable<TaskItem>>.CreateErrorResponse(ex);
            }
        }

        public async Task<IBaseResponse<bool>> RemoveEmployeeFromProjectAsync(string employeeId, Guid projectId)
        {
            try
            {
                StringValidator.CheckIsNotNull(employeeId);
                ObjectValidator<Guid>.CheckIsNotNullObject(projectId);

                var employee = await _userManager.FindByIdAsync(employeeId);
                ObjectValidator<Employee>.CheckIsNotNullObject(employee);

                var project = await _repository.ReadByIdAsync(projectId);
                ObjectValidator<Project>.CheckIsNotNullObject(project);

                var projectEmployee = project.Employees.FirstOrDefault(pe => pe.EmployeeId == employeeId);
                if (projectEmployee != null)
                {
                    project.Employees.Remove(projectEmployee);
                    await _repository.UpdateAsync(project);
                }

                return ResponseFactory<bool>.CreateSuccessResponse(true);
            }
            catch (ArgumentNullException ex)
            {
                return ResponseFactory<bool>.CreateNotFoundResponse(ex);
            }
            catch (Exception ex)
            {
                return ResponseFactory<bool>.CreateErrorResponse(ex);
            }
        }

        public async Task<IBaseResponse<bool>> SetEmployeeNewRoleByIdAsync(string employeeId, Roles roleType)
        {
            try
            {
                StringValidator.CheckIsNotNull(employeeId);

                var employee = await _userManager.FindByIdAsync(employeeId);
                List<string> roles = new List<string>()
                {
                    Roles.Employee.ToString(),
                    Roles.Manager.ToString(),
                    Roles.Supervisor.ToString(),
                    Roles.Admin.ToString(),
                };

                await _userManager.RemoveFromRolesAsync(employee, roles);

                var result = await _userManager.AddToRoleAsync(employee, roleType.ToString());

                if (result.Succeeded)
                {
                    return ResponseFactory<bool>.CreateSuccessResponse(true);
                }
                else
                {
                    return ResponseFactory<bool>.CreateErrorResponse(new Exception("Failed to set user as role."));
                }

            }
            catch (ArgumentNullException argNullException)
            {
                return ResponseFactory<bool>.CreateNotFoundResponse(argNullException);
            }
            catch (Exception ex)
            {
                return ResponseFactory<bool>.CreateErrorResponse(ex);
            }
        }


    }
}
