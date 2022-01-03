using ModelX.Logic.Common.Exceptions.Base;

namespace ModelX.Logic.Common.Exceptions.Api;

public class ForbiddenException : BaseException
{
    public ForbiddenException()
        : base("Access is denied.")
    {
        Failures = new Dictionary<string, string[]>();
    }

    public ForbiddenException(params string[] errors)
        : this()
    {
        if (errors != null)
        {
            Failures.Add(string.Empty, errors);
        }
    }

    public IDictionary<string, string[]> Failures { get; }
}