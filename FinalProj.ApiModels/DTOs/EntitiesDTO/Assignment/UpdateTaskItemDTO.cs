using DataProj.ApiModels.DTOs.BaseDTOs;

namespace DataProj.ApiModels.DTOs.EntitiesDTO.Assignment
{
    public class UpdateTaskItemDTO : BaseEntityDTO
    {
        /// <summary>
        /// Gets or sets the name of the task.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the comment associated with the task.
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Gets or sets the status of the task.
        /// </summary>
        public TaskStatus Status { get; set; }

        /// <summary>
        /// Gets or sets the priority of the task.
        /// </summary>
        public int Priority { get; set; }
    }
}
