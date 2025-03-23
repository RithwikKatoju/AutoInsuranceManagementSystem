﻿using System.ComponentModel.DataAnnotations;

namespace AutoInsuranceManagementSystem.Models
{
    public class ClaimEntityModel
    {
        [Key]
        public Guid? ClaimId { get; set; }
        public PolicyEntityModel? PolicyId { get; set; }
        public decimal? ClaimAmount { get; set; }
        public DateOnly? ClaimDate { get; set; }
        public ClaimStatus? ClaimStatus { get; set; }
        public UserEntityModel? AdjusterId { get; set; }
    }

    public enum ClaimStatus
    {
        OPEN,
        APPROVED,
        REJECTED
    }
}
