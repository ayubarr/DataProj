using DataProj.Domain.Models.Abstractions.BaseEntities;

namespace DataProj.Domain.Models.Logs
{
    public class Log : BaseEntity
    {
        public string LogLevel { get; set; }
        public int? ThreadId { get; set; }
        public int? EventId { get; set; }
        public string? EventName { get; set; }
        public string? Message { get; set; }
        public string? ExceptionMessage { get; set; }
        public string? ExceptionStackTrace { get; set; }
        public string? ExceptionSource { get; set; }
    }
}
