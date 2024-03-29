﻿using DataProj.ApiModels.DTOs.BaseDTOs;
using DataProj.Domain.Models.Enums;

namespace DataProj.ApiModels.DTOs.EntitiesDTO.TaskItem
{
    public class UpdateTaskItemWithExecutorDTO : BaseEntityDTO
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
        public Status Status { get; set; }

        /// <summary>
        /// Gets or sets the priority of the task.
        /// </summary>
        public int Priority { get; set; }
        /// <summary>
        /// Gets or sets ExecutorId
        /// </summary>
        public string? ExecutorId { get; set; }
    }
}
