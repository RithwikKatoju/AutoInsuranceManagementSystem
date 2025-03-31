using AutoInsuranceManagementSystem.DbContext;
using AutoInsuranceManagementSystem.Models;
using AutoInsuranceManagementSystem.Models.PolicyViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using AutoInsuranceManagementSystem.Models.ClaimViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;
using AutoInsuranceManagementSystem.Models.TicketViewModel;
using AutoInsuranceManagementSystem.Areas.Admin.Models;



namespace AutoInsuranceManagementSystem.Controllers
{
    [Authorize("CUSTOMER")]
    public class CustomerController(AppDbContext dbContext, UserManager<UserEntityModel> userManager) : Controller
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

        //POLICIES - REQUEST AND MANAGE POLICIES
        public IActionResult _CreatePolicyPartial()
        {
            return View(new CreatePolicyViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> _CreatePolicyPartial(CreatePolicyViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (model != null)
            {
                var user = await userManager.GetUserAsync(User);

                PolicyEntityModel policy = new()
                {
                    PolicyId = Guid.NewGuid(),
                    UserId = _userManager.GetUserId(User),
                    PolicyNumber = GeneratePolicyNumber(),
                    VehicleDetails = model.VehicleDetails,
                    CoverageAmount = model.CoverageAmount,
                    CoverageType = model.CoverageType,
                    PremiumAmount = GetPremiumAmount(model.CoverageType, model.CoverageAmount),
                    PolicyStatus = PolicyStatus.INACTIVE
                };
                await _dbContext.Policies.AddAsync(policy);
                await _dbContext.SaveChangesAsync();
                await CreatePayments();

            }
            return View("SuccessPolicy");
        }

        private string GeneratePolicyNumber()
        {
            // Example: "POL" + current date + sequence number
            var datePart = DateTime.Now.ToString("yyyyMMdd");
            var sequenceNumber = _dbContext.Policies.Count() + 1;

            return $"POL{datePart}{sequenceNumber:D5}";
        }

        private decimal GetPremiumAmount(CoverageTypes coverageType, decimal coverageAmount)
        {
            switch (coverageType)
            {
                case CoverageTypes.Liability:
                    return (coverageAmount / 100) * 3;
                case CoverageTypes.Collision:
                    return (coverageAmount / 100) * 6;
                case CoverageTypes.PIP_PersonalInjuryProtection:
                    return (coverageAmount / 100) * 4;
                case CoverageTypes.Comprehensive:
                    return (coverageAmount / 100) * 4;
            }
            return 0;
        }



        private async Task<List<PolicyEntityModel>> GetPoliciesToManage()
        {
            var policies = await _dbContext.Policies
                            .Where(x => x.UserId == (_userManager.GetUserId(User)))
                            .ToListAsync();
            return policies;
        }

        //Policies Main View
        public async Task<IActionResult> PoliciesMainView()
        {
            var model = new PolicyMainModel
            {
                CreatePolicyViewModel = new CreatePolicyViewModel(),
                ManagePolicyViewModel = await GetPoliciesToManage()
            };

            return View(model);
        }









        //CLAIMS - CREATE AND MANAGE CLAIMS

        public IActionResult CreateClaimPartial()
        {
            return View(new CreateClaimViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> CreateClaimPartial(CreateClaimViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model != null)
            {
                var claim = new ClaimEntityModel()
                {
                    ClaimId = Guid.NewGuid(),
                    UniqueClaimNumber = GenerateClaimNumber(),
                    UserId = _userManager.GetUserId(User),
                    PolicyNumber = model.PolicyNumber,    
                    ClaimAmount = model.ClaimAmount,
                    ClaimReason = model.ClaimReason,
                    ClaimDate = DateOnly.FromDateTime(DateTime.Now),
                    ClaimStatus = ClaimStatus.OPEN,
                };

                await _dbContext.Claims.AddAsync(claim);
                await _dbContext.SaveChangesAsync();

                return View("SuccessClaim");
            }

            return View(model);
        }
        private string GenerateClaimNumber()
        {
            // Example: "CLA" + current date + sequence number
            var datePart = DateTime.Now.ToString("yyyyMMdd");
            var sequenceNumber = _dbContext.Claims.Count() + 1;

            return $"CLA{datePart}{sequenceNumber:D5}";
        }

        //public async Task<IActionResult> ManageClaims()
        //{

        //    return View(await GetClaimsToManage());
        //}

        private async Task<List<ClaimEntityModel>> GetClaimsToManage()
        {
            var claims = await _dbContext.Claims
                            .Include(x => x.AgentId)
                            .Where(x => x.UserId == (_userManager.GetUserId(User)))
                            .ToListAsync();
            return claims;
        }

        public async Task<IActionResult> ClaimsMainView()
        {
            var policyNumbers = await _dbContext.Policies
                        .Where(x => x.UserId == _userManager.GetUserId(User))
                        .Select(x => x.PolicyNumber)
                        .ToListAsync();

            var model = new ClaimMainModel()
            {
                CreateClaimViewModel = new CreateClaimViewModel
                {
                    PolicyNumbers = policyNumbers.Select(v => new SelectListItem
                    {
                        Text = v,
                        Value = v
                    }).ToList()
                },
                ManageClaimViewModel = await GetClaimsToManage(),
            };

            return View(model);
        }









        //PAYMENTS

        private async Task CreatePayments()
        {
            var PendingPolicies = await _dbContext.Policies
                .Where(x => x.UserId == (_userManager.GetUserId(User)))
                .Where(x => x.PolicyStatus == PolicyStatus.INACTIVE)
                .ToListAsync();
            foreach (var policy in PendingPolicies)
            {
                var payment = new PaymentEntityModel()
                {
                    PaymentId = Guid.NewGuid(),
                    UniquePaymentNumber = GeneratePaymentNumber(),
                    UserId = policy.UserId,
                    PolicyNumber = policy.PolicyNumber,
                    PaymentAmount = policy.PremiumAmount,
                    PaymentStatus = PaymentStatus.PENDING
                };

                await _dbContext.Payments.AddAsync(payment);
                await _dbContext.SaveChangesAsync();
            }
        }
        private string GeneratePaymentNumber()
        {
            // Example: "CLA" + current date + sequence number
            var datePart = DateTime.Now.ToString("yyyyMMdd");
            var sequenceNumber = _dbContext.Payments.Count() + 1;

            return $"PAY{datePart}{sequenceNumber:D5}";
        }

        public async Task<IActionResult> ManagePayments()
        {
            return View(await GetPaymentsToManage());
        }

        private async Task<List<PaymentEntityModel>> GetPaymentsToManage()
        {
            var payments = await _dbContext.Payments
                            .Where(x => x.UserId == (_userManager.GetUserId(User)))
                            .ToListAsync();
            return payments;
        }
        
        public async Task<IActionResult> PayNowPayment(string policyNumber)
        { 

            var payment = await _dbContext.Payments
                .Where(x => x.PolicyNumber == policyNumber)
                .FirstOrDefaultAsync();
            if (payment != null)
            {
                payment.PaymentDate = DateOnly.FromDateTime(DateTime.Now);
                _dbContext.SaveChanges();
            }
            return View(payment);
        }

        public async Task<IActionResult> ConfirmPayment(string policyNumber)
        {
            var payment = await _dbContext.Payments
                .Where(x => x.PolicyNumber == policyNumber)
                .FirstOrDefaultAsync();
            var policy = await _dbContext.Policies
                .Where(x => x.PolicyNumber == policyNumber)
                .FirstOrDefaultAsync();
            payment.PaymentStatus = PaymentStatus.PROCESSING;
            //policy.StartDate = DateOnly.FromDateTime(DateTime.Now);
            //policy.EndDate = DateOnly.FromDateTime(DateTime.Now.AddYears(1));
            //policy.PolicyStatus = PolicyStatus.ACTIVE;

            await _dbContext.SaveChangesAsync();

            return View();
        }

        public async Task<IActionResult> DeclinePayment(string policyNumber)
        {
            var payment = await _dbContext.Payments
                .Where(x => x.PolicyNumber == policyNumber)
                .FirstOrDefaultAsync();
            var policy = await _dbContext.Policies
                .Where(x => x.PolicyNumber == policyNumber)
                .FirstOrDefaultAsync();
            payment.PaymentStatus = PaymentStatus.FAILED;
            policy.PolicyStatus = PolicyStatus.INACTIVE;
            await _dbContext.SaveChangesAsync();
            return View();
        }









        //SUPPORT TICKETS

        public IActionResult CreateTicketPartial()
        {
            return View(new CreateTicketViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> CreateTicketPartial(CreateTicketViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model != null)
            {
                var ticket = new SupportTicketEntity()
                {
                    TicketId = Guid.NewGuid(),
                    UniqueTicketNumber = GenerateTicketNumber(),
                    UserId = _userManager.GetUserId(User),
                    PolicyNumber = model.PolicyNumber,    
                    IssueDescription = model.IssueDescription,
                    TicketStatus = TicketStatus.OPEN,
                    CreatedDate = DateOnly.FromDateTime(DateTime.Now),
                };

                await _dbContext.Tickets.AddAsync(ticket);
                await _dbContext.SaveChangesAsync();

                return View("SuccessTicket");
            }
            return View(model);
        }
        private string GenerateTicketNumber()
        {
            // Example: "TIC" + current date + sequence number
            var datePart = DateTime.Now.ToString("yyyyMMdd");
            var sequenceNumber = _dbContext.Tickets.Count() + 1;

            return $"TIC{datePart}{sequenceNumber:D5}";
        }

        private async Task<List<SupportTicketEntity>> GetTicketsToManage()
        {
            var tickets = await _dbContext.Tickets
                            .Include(x => x.AgentId)
                            .Where(x => x.UserId == (_userManager.GetUserId(User)))
                            .ToListAsync();
            return tickets;
        }

        public async Task<IActionResult> TicketsMainView()
        {
            var policyNumbers = await _dbContext.Policies
                            .Where(x => x.UserId == (_userManager.GetUserId(User)))
                            .Select(x => x.PolicyNumber)
                            .ToListAsync();
            var model = new TicketMainModel()
            {
                CreateTicketViewModel = new CreateTicketViewModel
                {
                    PolicyNumbers = policyNumbers.Select(v => new SelectListItem
                    {
                        Text = v,
                        Value = v
                    }).ToList()
                },
                ManageTicketsViewModel = await GetTicketsToManage(),
            };


            return View(model);
        }
    }
}
