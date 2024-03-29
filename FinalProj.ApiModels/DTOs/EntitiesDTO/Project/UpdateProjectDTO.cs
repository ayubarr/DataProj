﻿using DataProj.ApiModels.DTOs.BaseDTOs;

namespace DataProj.ApiModels.DTOs.EntitiesDTO.Project
{
    /// <summary>
    /// Data Transfer Object (DTO) class for update a project.
    /// Inherits from BaseEntityDTO.
    /// </summary>
    public class UpdateProjectDTO : BaseEntityDTO
    {
        /// <summary>
        /// Gets or sets the name of the project.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the name of the client company.
        /// </summary>
        public string ClientCompanyName { get; set; }

        /// <summary>
        /// Gets or sets the name of the executing company.
        /// </summary>
        public string ExecutiveCompanyName { get; set; }

        /// <summary>
        /// Gets or sets the start date of the project.
        /// The default value is set to the current UTC date and time.
        /// </summary>
        public DateTime StartDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the end date of the project.
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Gets or sets the priority of the project.
        /// </summary>
        public int Priority { get; set; }
        /// <summary>
        /// Gets or sets Employees Id of the project
        /// </summary>

    }
}
