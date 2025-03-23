using Microsoft.AspNetCore.Identity;

namespace AutoInsuranceManagementSystem.Models
{
    public class UserEntityModel : IdentityUser
    {
        public string? FullName { get; set; } = string.Empty;
        public Roles? Role { get; set; }
    }

    public enum Roles
    {
        CUSTOMER,
        ADMIN,
        AGENT
    }
}
