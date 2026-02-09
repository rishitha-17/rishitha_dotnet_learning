using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using policy_management.Services;
using policy_management.Entities;
using policy_management.DTOs;
using policy_management.Filters;

namespace policy_management.Controllers
{
    [ApiController]
    [ServiceFilter(typeof(GlobalResponseFilter))]
    [ServiceFilter(typeof(ResponseTimeFilter))]
    [Route("api/admin/policies")]
    [Authorize(Roles = "Admin,admin")]
    public class AdminPolicyController : ControllerBase
    {
        private readonly IPolicyService _policyService;

        public AdminPolicyController(IPolicyService policyService)
        {
            _policyService = policyService;
        }

        // Admin endpoint: Add new policy
        [HttpPost(Name = "AdminCreatePolicy")]
        public async Task<IActionResult> AddPolicy([FromBody] Policy policy)
        {
            // Check if model validation passed
            if (!ModelState.IsValid)
            {
                // Create detailed validation error response
                var validationErrors = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                    );

                var errorResponse = new
                {
                    Message = "Validation failed",
                    Errors = validationErrors
                };

                return BadRequest(errorResponse);
            }

            try
            {
                // DateTime will be set in the repository to ensure UTC
                var createdPolicy = await _policyService.CreatePolicyAsync(policy);
                return CreatedAtRoute("AdminGetPolicyById", new { id = createdPolicy.Id }, createdPolicy);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while creating the policy", Error = ex.Message });
            }
        }

        // Admin endpoint: Update policy
        [HttpPut("{id}", Name = "AdminUpdatePolicy")]
        public async Task<IActionResult> UpdatePolicy(int id, [FromBody] PolicyDTO policyDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var existingPolicy = await _policyService.GetPolicyByIdAsync(id);
                if (existingPolicy == null)
                {
                    return NotFound(new { Message = "Policy not found" });
                }

                // Update policy properties
                existingPolicy.Name = policyDTO.Name;
                existingPolicy.Description = policyDTO.Description;
                existingPolicy.PremiumAmount = policyDTO.PremiumAmount;
                existingPolicy.IsActive = policyDTO.IsActive;
                // UpdatedAt will be set in the repository to ensure UTC

                var updatedPolicy = await _policyService.UpdatePolicyAsync(existingPolicy);
                return Ok(updatedPolicy);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while updating the policy", Error = ex.Message });
            }
        }


        [HttpPatch("{id}/status", Name = "AdminUpdatePolicyStatus")]
        public async Task<IActionResult> UpdatePolicyStatus(int id, [FromBody] PolicyStatusDTO statusDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var existingPolicy = await _policyService.GetPolicyByIdAsync(id);
                if (existingPolicy == null)
                {
                    return NotFound(new { Message = "Policy not found" });
                }

                // Update only the status
                existingPolicy.IsActive = statusDTO.IsActive;
                // UpdatedAt will be set in the repository to ensure UTC

                var updatedPolicy = await _policyService.UpdatePolicyAsync(existingPolicy);
                
                var response = new
                {
                    Id = updatedPolicy.Id,
                    Name = updatedPolicy.Name,
                    IsActive = updatedPolicy.IsActive,
                    UpdatedAt = updatedPolicy.UpdatedAt,
                    Message = statusDTO.IsActive ? "Policy activated successfully" : "Policy deactivated successfully"
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while updating policy status", Error = ex.Message });
            }
        }

        // Admin endpoint: Get all policies (including inactive)
        [HttpGet(Name = "AdminGetAllPolicies")]
        public async Task<IActionResult> GetAllPolicies()
        {
            try
            {
                var policies = await _policyService.GetAllPoliciesAsync();
                return Ok(policies);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving policies", Error = ex.Message });
            }
        }

        // Admin endpoint: Get policy by ID
        [HttpGet("{id}", Name = "AdminGetPolicyById")]
        public async Task<IActionResult> GetPolicyById(int id)
        {
            try
            {
                var policy = await _policyService.GetPolicyByIdAsync(id);
                if (policy == null)
                {
                    return NotFound(new { Message = "Policy not found" });
                }

                return Ok(policy);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving the policy", Error = ex.Message });
            }
        }
    }
}

