namespace cManagement.Models.Dtos
{
    /// <summary>
    /// Represents a generic assignment request between two entities.
    /// For example: assigning an Employee to a Shipment, or a Driver to a Truck.
    /// </summary>
    public class AssignmentRequestDto
    {
        public int SourceId { get; set; }    
        public int TargetId { get; set; }    
        public string AssignmentType { get; set; } = string.Empty; 
    }
}
