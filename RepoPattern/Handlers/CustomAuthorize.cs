using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using RepoPattern.Data;

public class CustomAuthorizeAttribute : Attribute, IAuthorizationFilter
{
    private readonly string[] _roles;

    public CustomAuthorizeAttribute(params string[] roles)
    {
        _roles = roles;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var userId = context.HttpContext.Session.GetString("LoggedInUserId");
        if (string.IsNullOrEmpty(userId))
        {
            context.Result = new ForbidResult();
        }
        var db = context.HttpContext.RequestServices.GetRequiredService<AppDbContext>();
        var user = db.Users.Where(m => m.Id.ToString() == userId).Include(m => m.Roles).FirstOrDefault();
        if (user == null)
        {
            context.Result = new ForbidResult();
        }

        if (_roles?.Length > 0 && _roles.Intersect(user.Roles.Select(m => m.Name)).Count() == 0)
        {
            context.Result = new ForbidResult();
        }

    }
}