using System.ComponentModel.DataAnnotations;

namespace AutoInsuranceManagementSystem.Models
{
    public class PaymentEntityModel
    {
        [Key]
        public Guid? PaymentId { get; set; }
        public string? UserId { get; set; }
        public PolicyEntityModel? PolicyId { get; set; }
        public decimal? PaymentAmount { get; set; }
        public DateOnly? PaymentDate { get; set; }
        public PaymentStatus? PaymentStatus { get; set; }
    }

    public enum PaymentStatus
    {
        SUCCESS,
        PENDING,
        FAILED
    }
}
