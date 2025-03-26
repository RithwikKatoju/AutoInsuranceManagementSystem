namespace AutoInsuranceManagementSystem.Models.PolicyViewModel
{
    public class PolicyMainModel
    {
        public CreatePolicyViewModel CreatePolicyViewModel { get; set; }
        public IEnumerable<PolicyEntityModel> ManagePolicyViewModel { get; set; }
    }
}
