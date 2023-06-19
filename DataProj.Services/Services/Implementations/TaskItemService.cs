using DataProj.ApiModels.DTOs.EntitiesDTO.Assignment;
using DataProj.ApiModels.Response.Helpers;
using DataProj.ApiModels.Response.Interfaces;
using DataProj.DAL.Repository.Interfaces;
using DataProj.Domain.Models.Entities;
using DataProj.Domain.Models.Enums;
using DataProj.Services.Helpers;
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

        public async Task<IBaseResponse<Guid>> CreateAsync(CreateTaskItemDTO createTaskItemDto)
        {
            try
            {
                var taskItem = new TaskItem
                {
                    Name = createTaskItemDto.Name,
                    Comment = createTaskItemDto.Comment,
                    Status = createTaskItemDto.Status,
                    Priority = createTaskItemDto.Priority,
                    AuthorId = createTaskItemDto.AuthorId,
                    ExecutorId = createTaskItemDto.ExecutorId
                };

                await _taskItemRepository.Create(taskItem);

                return ResponseFactory<Guid>.CreateSuccessResponse(taskItem.Id);
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

                var taskItems = await _taskItemRepository.GetFilteredTaskItemsAsync(filter);

                if (sortOrder.HasValue)
                {
                    taskItems = sortOrder switch
                    {
                        Sort.NameDesc => taskItems.OrderByDescending(t => t.Name),
                        Sort.NameAsc => taskItems.OrderBy(t => t.Name),
                        Sort.PriorityDesc => taskItems.OrderByDescending(t => t.Priority),
                        Sort.PriorityAsc => taskItems.OrderBy(t => t.Priority),
                        _ => taskItems,
                    };
                }

                return ResponseFactory<IEnumerable<TaskItem>>.CreateSuccessResponse(taskItems);
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

        public async Task<IBaseResponse<bool>> UpdateAsync(Guid id, UpdateTaskItemDTO taskItemDto)
        {
            try
            {
                ObjectValidator<UpdateTaskItemDTO>.CheckIsNotNullObject(taskItemDto);

                var taskItem = await _taskItemRepository.GetFilteredTaskItemByIdAsync(id);

                taskItem.Name = taskItemDto.Name;
                taskItem.Comment = taskItemDto.Comment;
                taskItem.Status = taskItemDto.Status;
                taskItem.Priority = taskItemDto.Priority;
                taskItem.AuthorId = taskItemDto.AuthorId;
                taskItem.ExecutorId = taskItemDto.ExecutorId;

                await _taskItemRepository.UpdateAsync(taskItem);

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
