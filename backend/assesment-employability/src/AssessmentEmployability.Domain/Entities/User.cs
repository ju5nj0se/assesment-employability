using Microsoft.AspNetCore.Identity;

namespace AssessmentEmployability.Domain.Entities;

public class User : IdentityUser<Guid>
{
    public string FullName { get; set; }
    public string Document { get; set; }
}