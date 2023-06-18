using DataProj.ApiModels.DTOs.EntitiesDTO.Project;
using DataProj.ApiModels.Response.Interfaces;
using DataProj.Domain.Models.Entities;
using DataProj.Domain.Models.Enums;

namespace DataProj.Services.Services.Interfaces
{
    public interface IProjectService
    {
        Task<IBaseResponse<IEnumerable<Project>>> GetAsync(FilterProjectDTO? projectFilterDto, Sort? sortOrder);
        Task<IBaseResponse<Project>> GetByIdAsync(Guid id);
        Task<IBaseResponse<bool>> UpdateAsync(UpdateProjectDTO project);
        Task<IBaseResponse<bool>> DeleteAsync(Guid id);
        Task<IBaseResponse<Guid>> CreateAsync(CreateProjectDTO createProjectDto);
    }
}
