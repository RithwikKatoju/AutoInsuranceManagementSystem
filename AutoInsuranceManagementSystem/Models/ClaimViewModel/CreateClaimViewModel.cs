using Microsoft.AspNetCore.Mvc.Rendering;

namespace AutoInsuranceManagementSystem.Models.ClaimViewModel
{
    public class CreateClaimViewModel
    {
        public string? PolicyNumber  { get; set; }
        public decimal? ClaimAmount { get; set; }
        public string? ClaimReason { get; set; }
        public IEnumerable<SelectListItem> PolicyNumbers { get; set; } = [];
    }
}
