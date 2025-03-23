using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

namespace AutoInsuranceManagementSystem.Models
{
    public class PolicyEntityModel
    {
        [Key]
        public Guid PolicyId { get; set; }
        public string? PolicyNumber { get; set; }
        public VehicleDetails? VehicleDetails { get; set; }
        public decimal? CoverageAmount { get; set; }
        public CoverageTypes? CoverageType { get; set; }
        public decimal? PremiumAmount { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public PolicyStatus? PolicyStatus { get; set; }
    }

    public class VehicleDetails
    {
        public string? Make { get; set; }
        public string? Model { get; set; }
        public DateOnly? Year { get; set; }
    }

    public enum CoverageTypes
    {
        Collision,
        Comprehensive,
        PIP_PersonalInjuryProtection,
        Liability
    }
    public enum PolicyStatus
    {
        ACTIVE,
        INACTIVE,
        RENEWED
    }
}
