using AutoInsuranceManagementSystem.Models.PolicyViewModel;

namespace AutoInsuranceManagementSystem.Models.TicketViewModel
{
    public class TicketMainModel
    {
        public CreateTicketViewModel CreateTicketViewModel { get; set; }
        public IEnumerable<SupportTicketEntity> ManageTicketsViewModel { get; set; }
    }
}
