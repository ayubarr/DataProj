using DataProj.DAL.SqlServer;
using DataProj.Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataProj.DAL.Repository.Interfaces
{
    public interface IProjectRepository : IBaseAsyncRepository<Project>
    {
        public Task<IEnumerable<Project>> GetFilteredProjectAsync(Expression<Func<Project, bool>> filter);

        public Task<Project> GetFilteredProjectByIdAsync( Guid Id);

    }
}
