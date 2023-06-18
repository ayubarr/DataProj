using DataProj.Domain.Models.Entities;

namespace DataProj.DAL.Repository.Interfaces
{
    public interface ITaskItemRepository : IBaseAsyncRepository<TaskItem>
    {
        public Task<IEnumerable<TaskItem>> GetFilteredTaskItemsAsync(Func<TaskItem, bool> filter);
        public Task<TaskItem> GetFilteredTaskItemByIdAsync(Guid Id);
    }
}
