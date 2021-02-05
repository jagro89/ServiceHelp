using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ServiceHelp.Controllers
{
    public class BaseController : Controller
    {
        public ClaimsPrincipal GetCurrentUser() => HttpContext.User;

        public string GetCurrentUserID()
        {
            return GetCurrentUser().FindFirst(ClaimTypes.NameIdentifier).Value;
        }
    }
}