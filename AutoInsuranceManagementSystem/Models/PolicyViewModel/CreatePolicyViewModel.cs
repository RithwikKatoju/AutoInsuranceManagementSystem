using Microsoft.AspNetCore.Mvc.Rendering;

namespace AutoInsuranceManagementSystem.Models.PolicyViewModel
{
    public class CreatePolicyViewModel
    {
        public VehicleDetails? VehicleDetails { get; set; }
        public decimal CoverageAmount { get; set; }
        public CoverageTypes CoverageType { get; set; }

        public List<SelectListItem>? AllCoverageTypes
        {
            get
            {
                List<SelectListItem> selectListItems = Enum.GetValues<CoverageTypes>().Select(x => new SelectListItem
                {
                    Text = Enum.GetName(x),
                    Value = x.ToString()
                }).ToList();
                return selectListItems;
            }
        }
    }
}
