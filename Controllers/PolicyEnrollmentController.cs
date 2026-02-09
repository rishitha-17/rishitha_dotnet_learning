using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using policy_management.Services;
using policy_management.DTOs;
using policy_management.Filters;

namespace policy_management.Controllers
{
    [ApiController]
    [ServiceFilter(typeof(GlobalResponseFilter))]
    [ServiceFilter(typeof(ResponseTimeFilter))]
    [Route("api")]
    public class PolicyEnrollmentController : ControllerBase
    {
        private readonly IPolicyEnrollmentService _enrollmentService;

        public PolicyEnrollmentController(IPolicyEnrollmentService enrollmentService)
        {
            _enrollmentService = enrollmentService;
        }

        // User endpoint: Request enrollment in a policy
        [HttpPost("policies/{policyId}/enroll", Name = "RequestEnrollment")]
        public async Task<IActionResult> RequestEnrollment(int policyId, [FromQuery] int userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var enrollmentRequest = new EnrollmentRequestDTO { PolicyId = policyId };
            var enrollment = await _enrollmentService.RequestEnrollmentAsync(userId, enrollmentRequest);
            return CreatedAtRoute("GetEnrollmentById", new { id = enrollment.Id }, enrollment);
        }

        // User endpoint: Get user's enrollments
        [HttpGet("user/enrollments", Name = "GetMyEnrollments")]
        public async Task<IActionResult> GetMyEnrollments([FromQuery] int userId)
        {
            var enrollments = await _enrollmentService.GetUserEnrollmentsAsync(userId);
            return Ok(enrollments);
        }

        // Admin endpoint: Get all enrollments with optional status filter
        [HttpGet("admin/enrollments", Name = "GetAllEnrollments")]
        [Authorize(Roles = "Admin,admin")]
        public async Task<IActionResult> GetAllEnrollments([FromQuery] string? status)
        {
            try
            {
                if (!string.IsNullOrEmpty(status))
                {
                    if (status.Equals("Pending", StringComparison.OrdinalIgnoreCase))
                    {
                        var pendingEnrollments = await _enrollmentService.GetPendingEnrollmentsAsync();
                        return Ok(pendingEnrollments);
                    }
                    else
                    {
                        return BadRequest(new { Message = "Invalid status. Use 'Pending' or omit for all enrollments." });
                    }
                }

                var allEnrollments = await _enrollmentService.GetAllEnrollmentsAsync();
                return Ok(allEnrollments);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving enrollments", Error = ex.Message });
            }
        }

        // Admin endpoint: Approve enrollment
        [HttpPost("admin/enrollments/{id}/approve", Name = "ApproveEnrollment")]
        [Authorize(Roles = "Admin,admin")]
        public async Task<IActionResult> ApproveEnrollment(int id, [FromBody] AdminActionDTO? actionDto)
        {
            var enrollment = await _enrollmentService.ApproveEnrollmentAsync(id, actionDto?.Comments);
            return Ok(enrollment);
        }

        // Admin endpoint: Reject enrollment
        [HttpPost("admin/enrollments/{id}/reject", Name = "RejectEnrollment")]
        [Authorize(Roles = "Admin,admin")]
        public async Task<IActionResult> RejectEnrollment(int id, [FromBody] AdminActionDTO? actionDto)
        {
            var enrollment = await _enrollmentService.RejectEnrollmentAsync(id, actionDto?.Comments);
            return Ok(enrollment);
        }

        // Helper endpoint: Get enrollment by ID
        [HttpGet("enrollments/{id}", Name = "GetEnrollmentById")]
        public async Task<IActionResult> GetEnrollmentById(int id)
        {
            try
            {
                var enrollments = await _enrollmentService.GetAllEnrollmentsAsync();
                var enrollment = enrollments.FirstOrDefault(e => e.Id == id);
                
                if (enrollment == null)
                {
                    return NotFound(new { Message = "Enrollment not found" });
                }

                return Ok(enrollment);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while retrieving enrollment", Error = ex.Message });
            }
        }
    }
}

