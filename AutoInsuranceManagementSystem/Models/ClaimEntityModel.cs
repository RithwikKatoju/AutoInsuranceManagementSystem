using System.ComponentModel.DataAnnotations;

namespace AutoInsuranceManagementSystem.Models
{
    public class ClaimEntityModel
    {
        [Key]   
        public Guid? ClaimId { get; set; }
        public string? UniqueClaimNumber { get; set; }
        public string? UserId { get; set; }
        public string? PolicyNumber { get; set; }
        public decimal? ClaimAmount { get; set; }
        public string? ClaimReason { get; set; }
        public DateOnly? ClaimDate { get; set; }
        public ClaimStatus? ClaimStatus { get; set; }
        public UserEntityModel? AgentId { get; set; }
    }

    public enum ClaimStatus
    {
        OPEN,
        APPROVED,
        REJECTED
    }
}
