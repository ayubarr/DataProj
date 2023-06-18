using DataProj.ApiModels.DTOs.EntitiesDTO.Assignment;
using DataProj.ApiModels.DTOs.EntitiesDTO.Project;
using DataProj.ApiModels.Response.Helpers;
using DataProj.ApiModels.Response.Interfaces;
using DataProj.DAL.Repository.Implemintations;
using DataProj.DAL.Repository.Interfaces;
using DataProj.Domain.Models.Entities;
using DataProj.Domain.Models.Enums;
using DataProj.Services.Helpers;
using DataProj.Services.Mapping.Helpers;
using DataProj.Services.Services.Interfaces;
using DataProj.ValidationHelper;

namespace DataProj.Services.Services.Implementations
{
    public class TaskItemService : ITaskItemService
    {
        private readonly ITaskItemRepository _taskItemRepository;

        public TaskItemService(ITaskItemRepository taskItemRepository)
        {
            _taskItemRepository = taskItemRepository;
        }

        public async Task<IBaseResponse<Guid>> CreateAsync(CreateTaskItemDTO createProjectDto)
        {
            try
            {
                ObjectValidator<CreateTaskItemDTO>.CheckIsNotNullObject(createProjectDto);

                var task = MapperHelperForEntity<CreateTaskItemDTO, TaskItem>.Map(createProjectDto);
                task.Id = Guid.NewGuid();

                await _taskItemRepository.Create(task);

                return ResponseFactory<Guid>.CreateSuccessResponse(task.Id);
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

                await _taskItemRepository.DeleteByIdAsync(id);

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

        public async Task<IBaseResponse<IEnumerable<TaskItem>>> GetAsync(FilterTaskItemDTO? taskItemFilterDto, Sort? sortOrder)
        {

            try
            {
                ObjectValidator<FilterTaskItemDTO>.CheckIsNotNullObject(taskItemFilterDto);

                var filter = FilterHelper.CreateTaskItemFilter(taskItemFilterDto);
                var query = _taskItemRepository.GetFilteredTaskItemsAsync(filter).Result;

                if (sortOrder.HasValue)
                {
                    query = sortOrder switch
                    {
                        Sort.NameDesc => query.OrderByDescending(t => t.Name),
                        Sort.NameAsc => query.OrderBy(t => t.Name),
                        Sort.PriorityDesc => query.OrderByDescending(t => t.Priority),
                        Sort.PriorityAsc => query.OrderBy(t => t.Priority),
                        _ => query,
                    };
                }

                return ResponseFactory<IEnumerable<TaskItem>>.CreateSuccessResponse(query);
            }
            catch (ArgumentNullException argNullException)
            {
                return ResponseFactory<IEnumerable<TaskItem>>.CreateNotFoundResponse(argNullException);
            }
            catch (Exception exception)
            {
                return ResponseFactory<IEnumerable<TaskItem>>.CreateErrorResponse(exception);
            }

           
        }

        public async Task<IBaseResponse<TaskItem>> GetByIdAsync(Guid id)
        {
            try
            {
                ObjectValidator<Guid>.CheckIsNotNullObject(id);

                var taskItem = await _taskItemRepository.GetFilteredTaskItemByIdAsync(id);

                return ResponseFactory<TaskItem>.CreateSuccessResponse(taskItem);
            }
            catch (ArgumentNullException argNullException)
            {
                return ResponseFactory<TaskItem>.CreateNotFoundResponse(argNullException);
            }
            catch (Exception exception)
            {
                return ResponseFactory<TaskItem>.CreateErrorResponse(exception);
            }
        }

        public async Task<IBaseResponse<bool>> UpdateAsync(UpdateTaskItemDTO taskItemDto)
        {
            try
            {
                ObjectValidator<UpdateTaskItemDTO>.CheckIsNotNullObject(taskItemDto);

                var entity = MapperHelperForEntity<UpdateTaskItemDTO, TaskItem>.Map(taskItemDto);
                await _taskItemRepository.UpdateAsync(entity);

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
