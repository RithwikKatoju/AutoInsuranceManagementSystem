using AutoInsuranceManagementSystem.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AutoInsuranceManagementSystem.Areas.Admin.Models
{
    public class AgentDropDownModel
    {
        public string? Agent { get; set; }
        public IEnumerable<SelectListItem> AgentUsers { get; set; } = [];
    }
}
