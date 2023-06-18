using DataProj.ApiModels.DTOs.BaseDTOs;
using DataProj.Domain.Models.Enums;

namespace DataProj.ApiModels.DTOs.EntitiesDTO.Project
{
    public class SortProjectDTO : BaseEntityDTO
    {
        public Sort NameSort { get; set; }
        public Sort PrioritySort { get; set; }
    }
}
