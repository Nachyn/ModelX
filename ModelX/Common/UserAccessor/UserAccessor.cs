using System.Security.Claims;
using ModelX.Logic.Common.UserAccessor;

namespace ModelX.Common.UserAccessor;

public class UserAccessor : IUserAccessor
{
    private readonly IHttpContextAccessor _accessor;

    public UserAccessor(IHttpContextAccessor accessor)
    {
        _accessor = accessor ?? throw new ArgumentNullException();
    }

    public ClaimsPrincipal User => _accessor.HttpContext.User;

    public int UserId => int.Parse(User.Identity.Name);

    public string Host =>
        @$"{_accessor.HttpContext.Request.Scheme}://{_accessor.HttpContext.Request.Host.Value}";
}