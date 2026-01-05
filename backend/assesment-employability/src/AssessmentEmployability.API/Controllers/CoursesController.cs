using AssessmentEmployability.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AssessmentEmployability.API.Controllers;

[ApiController]
[Route("api/courses")]
[Authorize]
public class CoursesController : ControllerBase
{
    // [HttpGet]
    // [Route("[status]")]
    // public async Task<IActionResult> GetCoursesByStatus(string status)
    // {
    //        
    // }
}