using System.ComponentModel.DataAnnotations;

namespace AutoInsuranceManagementSystem.Models
{
    public class PaymentEntityModel
    {
        [Key]
        public Guid? PaymentId { get; set; }
        public string? UniquePaymentNumber { get; set; }
        public string? UserId { get; set; }
        public string? PolicyNumber { get; set; }
        public decimal? PaymentAmount { get; set; }
        public DateOnly? PaymentDate { get; set; }
        public PaymentStatus? PaymentStatus { get; set; }
        public UserEntityModel? AgentId { get; set; }
    }

    public enum PaymentStatus
    {
        SUCCESS,
        PENDING,
        PROCESSING,
        FAILED
    }
}
