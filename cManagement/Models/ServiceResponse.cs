using System.Collections.Generic;

namespace cManagement.Models
{
    /// <summary>
    /// Represents a response from a service-layer operation (business logic).
    /// Provides operation status, optional created ID, and relevant messages.
    /// </summary>
    public class ServiceResponse
    {
        /// <summary>
        /// Enumeration of possible outcomes for a service operation.
        /// </summary>
        public enum ServiceStatus
        {
            NotFound,
            Created,
            Updated,
            Deleted,
            Error
        }

        /// <summary>
        /// The result status of the service operation.
        /// </summary>
        public ServiceStatus Status { get; set; }

        /// <summary>
        /// ID of the entity created by the service.
        /// </summary>
        public int? CreatedId { get; set; }

        /// <summary>
        /// List of messages (info, validation, or error) returned by the service.
        /// </summary>
        public List<string> Messages { get; set; } = new List<string>();
    }
}
