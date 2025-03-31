using System.ComponentModel.DataAnnotations;

namespace AutoInsuranceManagementSystem.Models
{
    public class SupportTicketEntity
    {
        [Key]
        public Guid? TicketId { get; set; }
        public string? UniqueTicketNumber { get; set; }
        public string? UserId { get; set; }
        public string? PolicyNumber { get; set; }
        public string? IssueDescription { get; set; }
        public TicketStatus? TicketStatus { get; set; }
        public DateOnly? CreatedDate { get; set; }
        public DateOnly? ResolvedDate { get; set; }
        public string? Feedback { get; set; }
        public UserEntityModel? AgentId { get; set; }


    }

    public enum TicketStatus
    {
        OPEN,
        RESOLVED
    }
}
