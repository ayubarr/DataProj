using DataProj.Domain.Models.Abstractions.BaseEntities;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataProj.Domain.Models.Entities
{
    public class Project : BaseEntity
    {
        public string ProjectName { get; set; }
        public string ClientCompanyName { get; set; }
        public string ExecutiveCompanyName { get; set; }
        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        public DateTime EndDate { get; set; }
        public int Priority { get; set; }

        public ICollection<ProjectEmployee> Employees { get; set; }

        [ForeignKey("ProjectManagerId")]
        public Employee ProjectManager { get; set; }
        public string ProjectManagerId { get; set; }

    }
}
