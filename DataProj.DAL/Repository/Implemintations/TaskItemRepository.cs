using DataProj.DAL.Repository.Interfaces;
using DataProj.DAL.SqlServer;
using DataProj.Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataProj.DAL.Repository.Implemintations
{
    public class TaskItemRepository : BaseAsyncRepository<TaskItem>, ITaskItemRepository
    {
        public TaskItemRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<TaskItem>> GetFilteredTaskItemsAsync(Expression<Func<TaskItem, bool>> filter)
        {
            return await ReadAll().Where(filter).ToListAsync();

        }

        public async Task<TaskItem> GetFilteredTaskItemByIdAsync(Guid id)
        {
            var taskItem = await _dbSet
                                 .Include(t => t.Author)
                                 .ThenInclude(e => e.AuthoredTasks)
                                 .Include(t => t.Executor)
                                 .ThenInclude(e => e.ExecutorTasks)
                                 .FirstOrDefaultAsync(t => t.Id == id);

            return taskItem == null
           ? throw new ArgumentNullException(nameof(id), $"Entity not found by id {id} in Repository")
           : taskItem;
        }     
    }
}
