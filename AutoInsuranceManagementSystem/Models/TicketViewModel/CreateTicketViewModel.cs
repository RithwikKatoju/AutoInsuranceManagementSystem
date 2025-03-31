using Microsoft.AspNetCore.Mvc.Rendering;

namespace AutoInsuranceManagementSystem.Models.TicketViewModel
{
    public class CreateTicketViewModel
    {
        public string? PolicyNumber { get; set; }
        public string? IssueDescription { get; set; }
        public IEnumerable<SelectListItem> PolicyNumbers { get; set; } = [];
    }
}
