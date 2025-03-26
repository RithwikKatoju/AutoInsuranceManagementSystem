using AutoInsuranceManagementSystem.Models.PolicyViewModel;

namespace AutoInsuranceManagementSystem.Models.ClaimViewModel
{
    public class ClaimMainModel
    {
        public CreateClaimViewModel CreateClaimViewModel { get; set; }
        public IEnumerable<ClaimEntityModel> ManageClaimViewModel { get; set; }
    }
}
