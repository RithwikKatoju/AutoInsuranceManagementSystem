using AutoInsuranceManagementSystem.Areas.Admin.Controllers;
using AutoInsuranceManagementSystem.Areas.Admin.Models.ClaimsViewModel;
using AutoInsuranceManagementSystem.Areas.Admin.Models.TicketsViewModel;
using AutoInsuranceManagementSystem.Areas.Admin.Models;
using AutoInsuranceManagementSystem.DbContext;
using AutoInsuranceManagementSystem.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AutoInsuranceManagementSystem.Areas.Agent.Controllers
{
    [Area("Agent")]
    [Authorize("AGENT")]
    [Route("Agent/[controller]/[action]")]
    public class HomeController(AppDbContext dbContext, UserManager<UserEntityModel> userManager) : Controller
    {
        private readonly AppDbContext _dbContext = dbContext;
        private readonly UserManager<UserEntityModel> _userManager = userManager;
        public IActionResult Index()
        {
            return View();
        }


        public async Task<IActionResult> Profile()
        {
            var accountUser = await _dbContext.Users
                                    .Where(x => x.Id == (_userManager.GetUserId(User)))
                                    .FirstOrDefaultAsync();
            if (accountUser != null)
            {
                UserEntityViewModel userEntityViewModel = new()
                {
                    Email = accountUser.Email,
                    FullName = accountUser.FullName,
                    Role = accountUser.Role
                };
                return View(userEntityViewModel);
            }
            return NotFound();
        }





        //POLICIES

        public async Task<IActionResult> AssignedPolicies()
        {
            var loggedInUserId = _userManager.GetUserId(User); 

            var processingPayments = await _dbContext.Payments
                .Where(payment => payment.PaymentStatus == PaymentStatus.PROCESSING)
                .Select(payment => payment.PolicyNumber)
                .ToListAsync();

            var assignedPolicies = await _dbContext.Policies
                .Where(policy => policy.Agent.Id == loggedInUserId && processingPayments.Contains(policy.PolicyNumber))
                .ToListAsync();

            return View(assignedPolicies);
        }

        public async Task<IActionResult> ApprovePolicy(Guid policyId)
        {
            if (!ModelState.IsValid)
            {
                return View(policyId);
            }
            var policy = await _dbContext.Policies
                                        .Where(x => x.PolicyId == policyId)
                                        .FirstOrDefaultAsync();
            if (policy != null)
            {
                policy.StartDate = DateOnly.FromDateTime(DateTime.Now);
                policy.EndDate = DateOnly.FromDateTime(DateTime.Now.AddYears(1));
                policy.PolicyStatus = PolicyStatus.ACTIVE;

                var PolicyPayment = await _dbContext.Payments
                                            .Where(x => x.PolicyNumber == policy.PolicyNumber)
                                            .FirstOrDefaultAsync();
                PolicyPayment.AgentId = await _dbContext.Users
                    .Where(x => x.Id == (_userManager.GetUserId(User)))
                    .FirstOrDefaultAsync();

                PolicyPayment.PaymentStatus = PaymentStatus.SUCCESS;

                await _dbContext.SaveChangesAsync();
                return View();
            }
            return NotFound("Policy not found.");
        }







        //CLAIMS


        public async Task<IActionResult> AssignedClaims()
        {
            var loggedInUserId = _userManager.GetUserId(User);

            var assignedClaims = await _dbContext.Claims
                .Include(claim => claim.AgentId) 
                .Where(claim => claim.ClaimStatus == ClaimStatus.OPEN && claim.AgentId.Id == loggedInUserId)
                .ToListAsync();

            return View(assignedClaims);
        }


        public async Task<IActionResult> ClaimMainView(Guid claimId)
        {

            var claim = await _dbContext.Claims
                .Include(x => x.AgentId)
                .Where(x => x.ClaimId == claimId)
                .FirstOrDefaultAsync();

            if (claim == null)
            {
                return NotFound("Claim not found.");
            }


            var policy = await _dbContext.Policies
                .Where(x => x.PolicyNumber == claim.PolicyNumber)
                .FirstOrDefaultAsync();

            if (policy == null)
            {
                return NotFound("Policy not found for this claim.");
            }

            // Combine both models into a ViewModel
            var claimsApproveMainModel = new ClaimsApproveMainModel
            {
                Claim = claim,
                Policy = policy
            };

            return View(claimsApproveMainModel);
        }

        public async Task<IActionResult> ApproveClaim(Guid claimId)
        {
            var claim = await _dbContext.Claims
                .Where(x => x.ClaimId == claimId)
                .FirstOrDefaultAsync();
            if (claim == null)
            {
                return NotFound("Claim not found.");
            }


            var policy = await _dbContext.Policies
                .Where(x => x.PolicyNumber == claim.PolicyNumber)
                .FirstOrDefaultAsync();

            policy.CoverageAmount = policy.CoverageAmount - claim.ClaimAmount;
            //policy.Agent = await _dbContext.Users
            //                    .Where(x=> x.Id == (_userManager.GetUserId(User)))
            //                    .FirstOrDefaultAsync();

            claim.ClaimStatus = ClaimStatus.APPROVED;
            claim.AgentId = await _dbContext.Users
                                .Where(x => x.Id == (_userManager.GetUserId(User)))
                                .FirstOrDefaultAsync();

            _dbContext.SaveChanges();
            return View();

        }

        public async Task<IActionResult> RejectClaim(Guid claimId)
        {
            var claim = await _dbContext.Claims
                .Where(x => x.ClaimId == claimId)
                .FirstOrDefaultAsync();
            if (claim == null)
            {
                return NotFound("Claim not found.");
            }

            claim.ClaimStatus = ClaimStatus.REJECTED;
            claim.AgentId = await _dbContext.Users
                                .Where(x => x.Id == (_userManager.GetUserId(User)))
                                .FirstOrDefaultAsync();
            _dbContext.SaveChanges();
            return View();
        }




        //PAYMENTS

        public async Task<IActionResult> AllPaymentsHistory()
        {
            var allPaymants = await _dbContext.Payments.ToListAsync();

            return View(allPaymants);
        }


        //TICKETS

        public async Task<IActionResult> AssignedTickets()
        {
            var loggedInUserId = _userManager.GetUserId(User); 

            var assignedTickets = await _dbContext.Tickets
                .Where(ticket => ticket.AgentId.Id == loggedInUserId) 
                .ToListAsync();

            return View(assignedTickets);
        }

        public async Task<IActionResult> TicketsMainView(Guid ticketId)
        {
            // Fetch ticket details
            var ticket = await _dbContext.Tickets
                .Include(x => x.AgentId)
                .Where(x => x.TicketId == ticketId)
                .FirstOrDefaultAsync();

            if (ticket == null)
            {
                return NotFound("Ticket not found.");
            }

            // Fetch Ticket details linked to the Tixket's PolicyNumber
            var policy = await _dbContext.Policies
                .Where(x => x.PolicyNumber == ticket.PolicyNumber)
                .FirstOrDefaultAsync();

            if (policy == null)
            {
                return NotFound("Policy not found for this claim.");
            }

            // Combine both models into a ViewModel
            var ticketsResolveMainModel = new TicketsResolveMainModel
            {
                Ticket = ticket,
                Policy = policy
            };

            return View(ticketsResolveMainModel);
        }

        [HttpPost]
        public async Task<IActionResult> ResolveTicket(Guid ticketId, string feedback)
        {
            var ticket = await _dbContext.Tickets
                .Where(x => x.TicketId == ticketId)
                .FirstOrDefaultAsync();

            if (ticket == null)
            {
                return NotFound("Ticket not found.");
            }

            ticket.ResolvedDate = DateOnly.FromDateTime(DateTime.Now);
            ticket.Feedback = feedback;
            ticket.AgentId = await _dbContext.Users
                                .Where(x => x.Id == (_userManager.GetUserId(User)))
                                .FirstOrDefaultAsync();
            ticket.TicketStatus = TicketStatus.RESOLVED;

            await _dbContext.SaveChangesAsync();

            return RedirectToAction("OpenTickets");
        }
    }
}
