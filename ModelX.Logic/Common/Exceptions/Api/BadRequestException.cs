using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using ModelX.Logic.Common.Exceptions.Base;

namespace ModelX.Logic.Common.Exceptions.Api;

public class BadRequestException : BaseException
{
    public BadRequestException()
        : base("One or more validation failures have occurred.")
    {
        Failures = new Dictionary<string, string[]>();
    }

    public BadRequestException(IEnumerable<ValidationFailure> failures)
        : this()
    {
        var failureGroups = failures
            .GroupBy(e => e.PropertyName, e => e.ErrorMessage);

        foreach (var failureGroup in failureGroups)
        {
            var propertyName = failureGroup.Key;
            var propertyFailures = failureGroup.ToArray();

            Failures.Add(propertyName, propertyFailures);
        }
    }

    public BadRequestException(IdentityResult identityResult)
        : this()
    {
        Failures = identityResult.Errors
            .GroupBy(e => e.Code)
            .ToDictionary(g => g.Key
                , g => g.Select(g => g.Description)
                    .ToArray());
    }

    public BadRequestException(params string[] errors)
        : this()
    {
        Failures.Add(string.Empty, errors);
    }

    public IDictionary<string, string[]> Failures { get; }
}