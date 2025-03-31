using AutoInsuranceManagementSystem.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AutoInsuranceManagementSystem.Areas.Admin.Models
{
    public class CreateUserEntityViewModel
    {
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? ConfirmEmail { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
        public UserEntityModel Role { get; set; }

        public List<SelectListItem> DropRoles
        {
            get
            {
                List<SelectListItem> resultListItems = Enum.GetValues<Roles>().Select(x => new SelectListItem
                {
                    Text = Enum.GetName(x),
                    Value = x.ToString()
                }).ToList();
                return resultListItems;
            }
        }
    }
}
