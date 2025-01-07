using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RepoPattern.Data;
using RepoPattern.Handlers;
using RepoPattern.Models;

public class CustomAuthorizeAttribute : Attribute, IAuthorizationFilter
{
    private readonly string[] _roles;

    public CustomAuthorizeAttribute(params string[] roles)
    {
        _roles = roles;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        //var userId = context.HttpContext.Session.GetString("LoggedInUserId");
        try
        {
            var user = JsonConvert.DeserializeObject<AuthViewModel>(EncryptionHelper.DecryptString(context.HttpContext.Request.Cookies["AuthToken"]));
            if (user == null)
            {
                //context.Result = new ForbidResult();
                context.Result = new RedirectResult("/Accounts/Login");
                return;
            }

            if (_roles?.Length > 0 && _roles.Intersect(user.Roles).Count() == 0)
            {
                context.Result = new ForbidResult();
                return;
            }
        }
        catch (Exception)
        {
            context.Result = new ForbidResult();
            return;
        }
    }
}