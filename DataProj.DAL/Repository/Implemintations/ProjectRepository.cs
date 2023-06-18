using DataProj.DAL.Repository.Interfaces;
using DataProj.DAL.SqlServer;
using DataProj.Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataProj.DAL.Repository.Implemintations
{
    public class ProjectRepository : BaseAsyncRepository<Project>, IProjectRepository
    {
        public ProjectRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Project>> GetFilteredProjectAsync(Func<Project, bool> filter)
        {
            return await _dbSet
                .AsNoTracking()
                .Include(x => x.ProjectManager)
                .Where(filter)
                .AsQueryable()
                .ToListAsync();
        }

        public async Task<Project> GetFilteredProjectByIdAsync(Guid id)
        {
            var project = await _dbSet
                       .AsNoTracking()
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
