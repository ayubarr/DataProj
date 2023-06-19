using DataProj.ApiModels.DTOs.EntitiesDTO.TaskItem;
using DataProj.ApiModels.Response.Interfaces;
using DataProj.Domain.Models.Entities;
using DataProj.Domain.Models.Enums;
using FinalProj.ApiModels.DTOs.EntitiesDTO.TaskItem;

namespace DataProj.Services.Services.Interfaces
{
    public interface ITaskItemService
    {
        Task<IBaseResponse<IEnumerable<TaskItem>>> GetAsync(FilterTaskItemDTO? projectFilterDto, Sort? sortOrder);
        Task<IBaseResponse<TaskItem>> GetByIdAsync(Guid id);
        Task<IBaseResponse<bool>> UpdateExecutorAsync(Guid id, UpdateTaskItemWithExecutorDTO taskItemWithExecutorDto);
        Task<IBaseResponse<bool>> UpdateAuthorAsync(Guid id, UpdateTaskItemWithAuthorDTO taskItemWithAuthorDto);
        Task<IBaseResponse<bool>> DeleteAsync(Guid id);
        Task<IBaseResponse<Guid>> CreateAsync(CreateTaskItemDTO createProjectDto);
    }
}
