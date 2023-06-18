using DataProj.Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataProj.DAL.SqlServer.Configuration
{
    public class ProjectConfiguration : IEntityTypeConfiguration<Project>
    {
        public void Configure(EntityTypeBuilder<Project> builder)
        {

            builder.HasKey(x => x.Id);

            builder.Property(x => x.ProjectName).HasMaxLength(50);

            builder.Property(x => x.ExecutiveCompanyName).HasMaxLength(50);
            builder.Property(x => x.ClientCompanyName).HasMaxLength(50);
        }
    }
}
