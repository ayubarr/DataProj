using DataProj.ApiModels.DTOs.EntitiesDTO.Project;
using DataProj.ApiModels.Response.Helpers;
using DataProj.ApiModels.Response.Interfaces;
using DataProj.DAL.Repository.Interfaces;
using DataProj.Domain.Models.Entities;
using DataProj.Domain.Models.Enums;
using DataProj.Services.Helpers;
using DataProj.Services.Mapping.Helpers;
using DataProj.Services.Services.Interfaces;
using DataProj.ValidationHelper;

namespace DataProj.Services.Services.Implementations
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectService(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<IBaseResponse<Guid>> CreateAsync(CreateProjectDTO createProjectDto)
        {
            try
            {
                ObjectValidator<CreateProjectDTO>.CheckIsNotNullObject(createProjectDto);

                var project = MapperHelperForEntity<CreateProjectDTO, Project>.Map(createProjectDto);
                project.Id = Guid.NewGuid();

                var projectEmployee = new ProjectEmployee
                {
                    Employee = project.ProjectManager,
                    Project = project,
                    EmployeeId = createProjectDto.ProjectManagerId,
                    ProjectId = project.Id
                };

                project.Employees = new List<ProjectEmployee> { projectEmployee };

                await _projectRepository.Create(project);

                return ResponseFactory<Guid>.CreateSuccessResponse(project.Id);
            }
            catch (ArgumentNullException argNullException)
            {
                return ResponseFactory<Guid>.CreateNotFoundResponse(argNullException);
            }
            catch (Exception exception)
            {
                return ResponseFactory<Guid>.CreateErrorResponse(exception);
            }
        }

        public async Task<IBaseResponse<bool>> DeleteAsync(Guid id)
        {
            try
            {
                ObjectValidator<Guid>.CheckIsNotNullObject(id);

                await _projectRepository.DeleteByIdAsync(id);

                return ResponseFactory<bool>.CreateSuccessResponse(true);
            }
            catch (ArgumentNullException argNullException)
            {
                return ResponseFactory<bool>.CreateNotFoundResponse(argNullException);
            }
            catch (Exception exception)
            {
                return ResponseFactory<bool>.CreateErrorResponse(exception);
            }
        }

        public async Task<IBaseResponse<IEnumerable<Project>>> GetAsync(FilterProjectDTO? projectFilterDto, Sort? sortOrder)
        {
            try
            {
                ObjectValidator<FilterProjectDTO>.CheckIsNotNullObject(projectFilterDto);

                var filter = FilterHelper.CreateProjectFilter(projectFilterDto);
                var query = _projectRepository.GetFilteredProjectAsync(filter).Result;

                if (sortOrder.HasValue)
                {
                    query = sortOrder switch
                    {
                        Sort.NameDesc => query.OrderByDescending(p => p.ProjectName),
                        Sort.NameAsc => query.OrderBy(p => p.ProjectName),
                        Sort.PriorityDesc => query.OrderByDescending(p => p.Priority),
                        Sort.PriorityAsc => query.OrderBy(p => p.Priority),
                        _ => query,
                    };
                }

                return ResponseFactory<IEnumerable<Project>>.CreateSuccessResponse(query);
            }
            catch (ArgumentNullException argNullException)
            {
                return ResponseFactory<IEnumerable<Project>>.CreateNotFoundResponse(argNullException);
            }
            catch (Exception exception)
            {
                return ResponseFactory<IEnumerable<Project>>.CreateErrorResponse(exception);
            }
        }

        public async Task<IBaseResponse<Project>> GetByIdAsync(Guid id)
        {
            try
            {
                ObjectValidator<Guid>.CheckIsNotNullObject(id);

                var project = await _projectRepository.GetFilteredProjectByIdAsync(id);

                return ResponseFactory<Project>.CreateSuccessResponse(project);
            }
            catch (ArgumentNullException argNullException)
            {
                return ResponseFactory<Project>.CreateNotFoundResponse(argNullException);
            }
            catch (Exception exception)
            {
                return ResponseFactory<Project>.CreateErrorResponse(exception);
            }
        }

        public async Task<IBaseResponse<bool>> UpdateAsync(UpdateProjectDTO projectDto)
        {
            try
            {
                ObjectValidator<UpdateProjectDTO>.CheckIsNotNullObject(projectDto);

                var entity = MapperHelperForEntity<UpdateProjectDTO, Project>.Map(projectDto);
                await _projectRepository.UpdateAsync(entity);

                return ResponseFactory<bool>.CreateSuccessResponse(true);
            }
            catch (ArgumentNullException argNullException)
            {
                return ResponseFactory<bool>.CreateNotFoundResponse(argNullException);
            }
            catch (Exception exception)
            {
                return ResponseFactory<bool>.CreateErrorResponse(exception);
            }
        }
    }
}
