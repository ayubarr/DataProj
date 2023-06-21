using DataProj.DAL.Repository.Interfaces;
using DataProj.DAL.SqlServer;
using DataProj.Domain.Models.Entities;
using DataProj.ValidationHelper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataProj.DAL.Repository.Implemintations
{
    public class ProjectRepository : BaseAsyncRepository<Project>, IProjectRepository
    {
        public ProjectRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Project>> GetFilteredProjectAsync(Expression<Func<Project, bool>> filter)
        {
            ObjectValidator<Expression<Func<Project, bool>>>.CheckIsNotNullObject(filter);

            return await ReadAll()
                .Where(filter)
                .ToListAsync();

        }

        public async Task<Project> GetFilteredProjectByIdAsync(Guid id)
        {
            var project = await ReadAll()
                  .Include(x => x.Employees)
                  .ThenInclude(x => x.Employee)
                  .Include(x => x.ProjectManager)
                  .FirstOrDefaultAsync(x => x.Id == id);

            return project == null
           ? throw new ArgumentNullException(nameof(id), $"Entity not found by id {id} in Repository")
           : project;
        }
    }
}
