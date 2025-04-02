using System.Security.Claims;
using AutoInsuranceManagementSystem.Areas.Admin.Models;
using AutoInsuranceManagementSystem.Areas.Admin.Models.ClaimsViewModel;
using AutoInsuranceManagementSystem.Areas.Admin.Models.TicketsViewModel;
using AutoInsuranceManagementSystem.DbContext;
using AutoInsuranceManagementSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AutoInsuranceManagementSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "ADMIN")]
    [Route("Admin/[controller]/[action]")]
    public class HomeController(AppDbContext dbContext, UserManager<UserEntityModel> userManager) : BaseController
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



        public async Task<IActionResult> ManageUsers()
        {
            return View(await GetUsersToManage());
        }

        private async Task<List<UserEntityViewModel>> GetUsersToManage()
        {
            var users = await _userManager.Users
                            .Where(x => x.Role != Roles.ADMIN)
                            .ToListAsync();
            var listOfUserAccounts = new List<UserEntityViewModel>();
            foreach (var user in users)
            {
                listOfUserAccounts.Add(new UserEntityViewModel
                {
                    Email = user.Email,
                    FullName = user.FullName,
                    Role = user.Role
                });
            }
            return listOfUserAccounts;
        }


        public async Task<IActionResult> EditUser(string email)
        {
            var accountUser = await _userManager.FindByEmailAsync(email);
            if (accountUser != null)
            {
                UserEntityViewModel userEntityViewModel = new()
                {
                    Email = accountUser.Email,
                    FullName = accountUser.FullName,
                    Role = accountUser.Role,
                    DropRoles = Enum.GetValues<Roles>()
                    .Select(x => new SelectListItem
                    {
                        Text = Enum.GetName(x),
                        Value = x.ToString()
                    })

                };
                return View(userEntityViewModel);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(UserEntityViewModel userEntityViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(userEntityViewModel);
            }
            else
            {
                try
                {
                    UserEntityModel? userEntityModel = await _userManager.FindByEmailAsync(userEntityViewModel?.Email ?? "");
                    if (userEntityModel != null)
                    {
                        userEntityModel.Role = userEntityViewModel.Role;
                        userEntityModel.FullName = userEntityViewModel.FullName;
                        var claims = await _userManager.GetClaimsAsync(userEntityModel);
                        var removeResult = await _userManager.RemoveClaimsAsync(userEntityModel, claims);
                        if (!removeResult.Succeeded)
                        {
                            //DisplayError
                            ModelState.AddModelError(string.Empty, "Unable to Update Claims - removing existing claim failed");
                            return View(userEntityModel);
                        }
                        var claimsRequired = new List<Claim>
                    { new (ClaimTypes.Name, userEntityViewModel.FullName ?? ""),
                        new (ClaimTypes.Role, Enum.GetName(userEntityViewModel.Role)?? ""),
                        new (ClaimTypes.NameIdentifier, userEntityModel.Id),
                        new (ClaimTypes.Email, userEntityViewModel.Email ?? "")
                    };
                        var addClaimResult = await _userManager.AddClaimsAsync(userEntityModel, claimsRequired);
                        if (!addClaimResult.Succeeded)
                        {
                            //DisplayError
                            ModelState.AddModelError(string.Empty, "Unable to Update Claims - Adding new claim failed");
                            return View(userEntityViewModel);
                        }
                        var userUpdateResult = await _userManager.UpdateAsync(userEntityModel);
                        if (!userUpdateResult.Succeeded)
                        {
                            //DisplayError
                            ModelState.AddModelError(string.Empty, "Unable to Update Claims - Update new  claim failed");
                            return View(userEntityViewModel);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError(String.Empty, ex.Message);
                    return View(userEntityViewModel);
                }
            }
            return View("ManageUsers", await GetUsersToManage());
        }


        public IActionResult CreateUser()
        {
            return View(new CreateUserEntityViewModel());
        }

        [HttpPost]

        public async Task<IActionResult> CreateUser(CreateUserEntityViewModel createUserEntityViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(createUserEntityViewModel);
            }
            if (createUserEntityViewModel != null
                && createUserEntityViewModel.Email != null
                && !createUserEntityViewModel.Email.Equals(createUserEntityViewModel.ConfirmEmail))
            {
                ModelState.AddModelError(string.Empty, "Email and Confirm Email do not match!");
                return View(createUserEntityViewModel);
            }
            if (createUserEntityViewModel != null
                && createUserEntityViewModel.Password != null
                && !createUserEntityViewModel.Password.Equals(createUserEntityViewModel.ConfirmPassword))
            {
                ModelState.AddModelError(string.Empty, "Password and Confirm Password do not match!");
                return View(createUserEntityViewModel);
            }
            if (createUserEntityViewModel != null)
            {
                UserEntityModel userEntityModel = new()
                {
                    FullName = createUserEntityViewModel.FullName,
                    Email = createUserEntityViewModel.Email,
                    UserName = createUserEntityViewModel.Email,
                    Role = createUserEntityViewModel.Role.Role
                };
                var CreatedUser = await _userManager.CreateAsync(userEntityModel, createUserEntityViewModel?.Password ?? "Open@1234");
                if (!CreatedUser.Succeeded)
                {
                    ModelState.AddModelError(string.Empty, "User Not Created");
                    return View(createUserEntityViewModel);
                }
                if (createUserEntityViewModel != null)
                {
                    var claimsRequired = new List<Claim>
                    { new (ClaimTypes.Name, createUserEntityViewModel.FullName ?? ""),
                        new (ClaimTypes.Role, Enum.GetName(createUserEntityViewModel.Role.Role)?? ""),
                        new (ClaimTypes.NameIdentifier, userEntityModel.Id),
                        new (ClaimTypes.Email, createUserEntityViewModel.Email ?? "")
                    };
                    await _userManager.AddClaimsAsync(userEntityModel, claimsRequired);
                    await _userManager.UpdateAsync(userEntityModel);

                }
                return View("ManageUsers", await GetUsersToManage());
            }
            return View(new CreateUserEntityViewModel());
        }


        //POLICIES

        public async Task<IActionResult> PendingPolicies()
        {
            var ProcessingPayments = await _dbContext.Payments
                                        .Where(x => x.PaymentStatus == PaymentStatus.PROCESSING)
                                        .Select(x => x.PolicyNumber)
                                        .ToListAsync();
            var pendingPolicies = await _dbContext.Policies
                                        .Include(policy => policy.Agent)
                                        .Where(policy => ProcessingPayments.Contains(policy.PolicyNumber))
                                        .ToListAsync();
            //var policies = await _dbContext.Policies.ToListAsync();
            return View(pendingPolicies);
        }

        private async Task<AgentDropDownModel> AgentDropDown()
        {
            var agentUsers = await _userManager.Users
                .Where(x => x.Role == Roles.AGENT)
                .ToListAsync();
            // Map the users to SelectListItem for a dropdown
            var agentUserDropdown = new AgentDropDownModel()
            {
                AgentUsers = agentUsers.Select(user => new SelectListItem
                {
                    Text = user.FullName,
                    Value = user.Id
                }).ToList()
            };
            return agentUserDropdown;
        }
        public async Task<IActionResult> AssignAgentPolicy(Guid policyId)
        {

            ViewData["PolicyId"] = policyId;
            return View(await AgentDropDown());
        }

        [HttpPost]
        public async Task<IActionResult> AssignAgentPolicy(AgentDropDownModel agentDropDownModel, Guid policyId)
        {
            if (!ModelState.IsValid)
            {
                return View(agentDropDownModel);
            }
            var policy = await _dbContext.Policies
                                        .Where(x => x.PolicyId == policyId)
                                        .FirstOrDefaultAsync();
            if (policy != null)
            {
                var agent = await _dbContext.Users.FindAsync(agentDropDownModel.Agent);
                policy.Agent = agent;

                _dbContext.SaveChanges();
                return RedirectToAction("PendingPolicies");
            }
            return NotFound("Policy not found.");
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


        public async Task<IActionResult> OpenClaims()
        {
            var openClaims = await _dbContext.Claims
                                        .Include(x => x.AgentId)
                                        .Where(x => x.ClaimStatus == ClaimStatus.OPEN)
                                        .ToListAsync();
            return View(openClaims);
        }


        public async Task<IActionResult> ClaimMainView(Guid claimId)
        {
            // Fetch claim details
            var claim = await _dbContext.Claims
                .Include(x => x.AgentId)
                .Where(x => x.ClaimId == claimId)
                .FirstOrDefaultAsync();

            if (claim == null)
            {
                return NotFound("Claim not found.");
            }

            // Fetch policy details linked to the claim's PolicyNumber
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

            // Fetch policy details linked to the claim's PolicyNumber
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

        public async Task<IActionResult> AssignAgentClaim(Guid claimId)
        {

            ViewData["ClaimId"] = claimId;
            return View(await AgentDropDown());
        }

        [HttpPost]
        public async Task<IActionResult> AssignAgentClaim(AgentDropDownModel agentDropDownModel, Guid claimId)
        {
            if (!ModelState.IsValid)
            {
                return View(agentDropDownModel);
            }
            var claim = await _dbContext.Claims
                                        .Where(x => x.ClaimId == claimId)
                                        .FirstOrDefaultAsync();
            if (claim != null)
            {
                var agent = await _dbContext.Users.FindAsync(agentDropDownModel.Agent);
                claim.AgentId = agent;

                _dbContext.SaveChanges();
                return RedirectToAction("OpenClaims");
            }
            return NotFound("Claims not found.");
        }

        //PAYMENTS

        public async Task<IActionResult> AllPaymentsHistory()
        {
            var allPaymants = await _dbContext.Payments.ToListAsync();

            return View(allPaymants);
        }


        //TICKETS

        public async Task<IActionResult> OpenTickets()
        {
            var openTickets = await _dbContext.Tickets
                                        .Include(x => x.AgentId)
                                        .ToListAsync();
            return View(openTickets);
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

        public async Task<IActionResult> AssignAgentTicket(Guid ticketId)
        {

            ViewData["TicketId"] = ticketId;
            return View(await AgentDropDown());
        }

        [HttpPost]
        public async Task<IActionResult> AssignAgentTicket(AgentDropDownModel agentDropDownModel, Guid ticketId)
        {
            if (!ModelState.IsValid)
            {
                return View(agentDropDownModel);
            }
            var ticket = await _dbContext.Tickets
                                        .Where(x => x.TicketId == ticketId)
                                        .FirstOrDefaultAsync();
            if (ticket != null)
            {
                var agent = await _dbContext.Users.FindAsync(agentDropDownModel.Agent);
                ticket.AgentId = agent;

                _dbContext.SaveChanges();
                return RedirectToAction("OpenTickets");
            }
            return NotFound("Ticket not found.");
        }
    }
}
