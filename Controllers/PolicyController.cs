using Microsoft.AspNetCore.Mvc;
using policy_management.Services;
using policy_management.Entities;
using policy_management.DTOs;
using policy_management.Filters;

namespace policy_management
{
        [ApiController]
        [ServiceFilter(typeof(GlobalResponseFilter))]
        [ServiceFilter(typeof(ResponseTimeFilter))]
        [Route("api/policies")]
        public class PolicyController : ControllerBase
        {
            private readonly IPolicyService policyService;
            public PolicyController(IPolicyService _policyService)
            {
                this.policyService = _policyService;
            }

            [HttpGet (Name = "GetPolicies")]
            public async Task<IActionResult> GetPolicies()
            {
                // Return only active policies for users
                var policies = await policyService.GetPoliciesByStatusAsync(true);
                return Ok(policies);
            }
            [HttpGet("{id}", Name = "GetPolicyById")]
            public async Task<IActionResult> GetPolicyById(int id)
            {
                var policy = await policyService.GetPolicyByIdAsync(id);
                return Ok(policy);
            }
            [HttpGet("search", Name = "SearchPolicies")]
            public async Task<IActionResult> SearchPolicies([FromQuery] int minAmount, [FromQuery] int maxAmount)
            {
                var policies = await policyService.SearchPoliciesByAmountAsync(minAmount, maxAmount);
                return Ok(policies);
            }
            [HttpGet("status", Name = "GetPoliciesByStatus")]
            public async Task<IActionResult> GetPoliciesByStatus([FromQuery] string isActive)
            {
                bool isActiveBool = bool.Parse(isActive);
                var policies = await policyService.GetPoliciesByStatusAsync(isActiveBool);
                return Ok(policies);
            }
            // [HttpPost(Name = "CreatePolicy")]
            // public async Task<IActionResult> CreatePolicy([FromBody] Policy policy)
            // {
            //     // Check if model validation passed
            //     if (!ModelState.IsValid)
            //     {
            //         // Create detailed validation error response
            //         var validationErrors = ModelState
            //             .Where(x => x.Value.Errors.Count > 0)
            //             .ToDictionary(
            //                 kvp => kvp.Key,
            //                 kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
            //             );

            //         var errorResponse = new
            //         {
            //             Message = "Validation failed",
            //             Errors = validationErrors
            //         };

            //         return BadRequest(errorResponse);
            //     }

            //     try
            //     {
            //         var createdPolicy = await policyService.CreatePolicyAsync(policy);
            //         return CreatedAtRoute("GetPolicyById", new { id = createdPolicy.Id }, createdPolicy);
            //     }
            //     catch (Exception ex)
            //     {
            //         return StatusCode(500, new { Message = "An error occurred while creating the policy", Error = ex.Message });
            //     }
            // }
            // [HttpPut("{id}", Name = "UpdatePolicy")]
            // public async Task<IActionResult> UpdatePolicy([FromRoute] int id, [FromBody] PolicyDTO policyDTO)
            // {
            //     var existingPolicy = await policyService.GetPolicyByIdAsync(id);
            //     if (existingPolicy == null)
            //     {
            //         return NotFound();
            //     }       
            //     existingPolicy.Name = policyDTO.Name;
            //     existingPolicy.Description = policyDTO.Description;    
            //     existingPolicy.PremiumAmount = (int)policyDTO.PremiumAmount;
            //     existingPolicy.IsActive = policyDTO.IsActive;
            //     await policyService.UpdatePolicyAsync(existingPolicy);
            //     return NoContent();
            // }

        }
}