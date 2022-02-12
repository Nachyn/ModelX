using System.Security.Claims;

namespace ModelX.Logic.Common.UserAccessor;

public interface IUserAccessor
{
    public ClaimsPrincipal User { get; }

    public int UserId { get; }

    public string Host { get; }
}