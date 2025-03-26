using AutoInsuranceManagementSystem.DbContext;
using AutoInsuranceManagementSystem.Models;
using AutoInsuranceManagementSystem.Models.PolicyViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using AutoInsuranceManagementSystem.Models.ClaimViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;



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
                //var user =await _userManager.GetUserAsync(User);

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



        //DashboardIndex
        public IActionResult DashboardIndex()
        {
            return View();
        }

        //ManagePolicies - gets all policies if the user including inactive 

        public async Task<IActionResult> ManagePolicies()
        {

            return View(await GetPoliciesToManage());
        }

        private async Task<List<PolicyEntityModel>> GetPoliciesToManage()
        {
            var users = await _dbContext.Policies
                            .Where(x => x.UserId == (_userManager.GetUserId(User)))
                            .ToListAsync();
            var listOfUserAccounts = new List<PolicyEntityModel>();
            foreach (var user in users)
            {
                listOfUserAccounts.Add(new PolicyEntityModel
                {
                    PolicyId = user.PolicyId,
                    PolicyNumber = user.PolicyNumber,
                    VehicleDetails = user.VehicleDetails,
                    CoverageAmount = user.CoverageAmount,
                    CoverageType = user.CoverageType,
                    StartDate = user.StartDate,
                    EndDate = user.EndDate,
                    PolicyStatus = user.PolicyStatus,
                    PremiumAmount = user.PremiumAmount

                });
            }
            return listOfUserAccounts;
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

        public async Task<IActionResult> CreateClaimPartial()
        {

            var policyNumbers = await _dbContext.Policies
                            .Where(x => x.UserId == (_userManager.GetUserId(User)))
                            .Select(x => x.PolicyNumber)
                            .ToListAsync();
            var model = new CreateClaimViewModel
            {
                PolicyNumbers = policyNumbers.Select(v => new SelectListItem
                {
                    Text = v,
                    Value = v
                }).ToList()
            };

            return PartialView(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateClaimPartial(CreateClaimViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if(model != null)
            {
                var ModelpolicyId = await _dbContext.Policies
                            .Where(x => x.PolicyNumber == model.PolicyNumber)
                            .FirstOrDefaultAsync();

                var claim = new ClaimEntityModel()
                {
                    ClaimId = Guid.NewGuid(),
                    UserId = _userManager.GetUserId(User),
                    PolicyNumber = ModelpolicyId.PolicyNumber,
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

        public async Task<IActionResult> ManageClaims()
        {

            return View(await GetClaimsToManage());
        }

        private async Task<List<ClaimEntityModel>> GetClaimsToManage()
        {
            var claims = await _dbContext.Claims
                            .Where(x => x.UserId == (_userManager.GetUserId(User)))
                            .ToListAsync();
            var listOfClaims = new List<ClaimEntityModel>();
            foreach (var claim in claims)
            {
                listOfClaims.Add(new ClaimEntityModel
                {
                    ClaimId = claim.ClaimId,
                    PolicyNumber = claim.PolicyNumber,
                    ClaimAmount = claim.ClaimAmount,
                    ClaimReason = claim.ClaimReason,
                    ClaimDate = claim.ClaimDate,
                    ClaimStatus = claim.ClaimStatus
                });
            }
            return listOfClaims;
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

    }
}
