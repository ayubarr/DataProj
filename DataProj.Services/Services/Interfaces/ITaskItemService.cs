using DataProj.ApiModels.DTOs.EntitiesDTO.Assignment;
using DataProj.ApiModels.Response.Interfaces;
using DataProj.Domain.Models.Entities;
using DataProj.Domain.Models.Enums;

namespace DataProj.Services.Services.Interfaces
{
    public interface ITaskItemService
    {
        Task<IBaseResponse<IEnumerable<TaskItem>>> GetAsync(FilterTaskItemDTO? projectFilterDto, Sort? sortOrder);
        Task<IBaseResponse<TaskItem>> GetByIdAsync(Guid id);
        Task<IBaseResponse<bool>> UpdateAsync(Guid id, UpdateTaskItemDTO project);
        Task<IBaseResponse<bool>> DeleteAsync(Guid id);
        Task<IBaseResponse<Guid>> CreateAsync(CreateTaskItemDTO createProjectDto);
    }
}
