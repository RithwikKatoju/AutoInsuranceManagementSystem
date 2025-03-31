using AutoInsuranceManagementSystem.Models;

namespace AutoInsuranceManagementSystem.Areas.Admin.Models.TicketsViewModel
{
    public class TicketsResolveMainModel
    {
        public SupportTicketEntity Ticket { get; set; }
        public PolicyEntityModel Policy { get; set; }
    }
}
