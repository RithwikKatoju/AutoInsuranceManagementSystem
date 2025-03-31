using AutoInsuranceManagementSystem.Models;

namespace AutoInsuranceManagementSystem.Areas.Admin.Models.ClaimsViewModel
{
    public class ClaimsApproveMainModel
    {
        public ClaimEntityModel Claim { get; set; }
        public PolicyEntityModel Policy { get; set; }
    }
}
