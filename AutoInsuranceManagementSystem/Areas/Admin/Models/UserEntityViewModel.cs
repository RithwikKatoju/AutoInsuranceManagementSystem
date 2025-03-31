using AutoInsuranceManagementSystem.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AutoInsuranceManagementSystem.Areas.Admin.Models
{
    public class UserEntityViewModel
    {
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public Roles Role { get; set; }

        public IEnumerable<SelectListItem> DropRoles { get; set; } = [];
    }
}
