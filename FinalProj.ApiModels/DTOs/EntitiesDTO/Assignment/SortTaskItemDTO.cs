using DataProj.ApiModels.DTOs.BaseDTOs;
using DataProj.Domain.Models.Enums;

namespace DataProj.ApiModels.DTOs.EntitiesDTO.Assignment
{
    public class SortTaskItemDTO : BaseEntityDTO
    {
        public Sort NameSort { get; set; }
        public Sort PrioritySort { get; set; }
    }
}
