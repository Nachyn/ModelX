using System.Net;
using FluentValidation;
using Newtonsoft.Json;
using ModelX.Logic.Common.Exceptions.Api;
using ModelX.Logic.Common.Extensions;

namespace ModelX.Common.Middlewares;

public class CustomExceptionHandlerMiddleware
{
    private readonly ILogger<CustomExceptionHandlerMiddleware> _logger;

    private readonly RequestDelegate _next;

    public CustomExceptionHandlerMiddleware(RequestDelegate next
        , ILogger<CustomExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        if (exception is ValidationException fluentException)
        {
            exception = new BadRequestException(fluentException.Errors);
        }

        var code = HttpStatusCode.InternalServerError;

        IDictionary<string, string[]>? result = null;

        switch (exception)
        {
            case BadRequestException badRequestException:
                code = HttpStatusCode.BadRequest;
                result = badRequestException.Failures;
                break;

            case NotFoundException notFoundException:
                code = HttpStatusCode.NotFound;
                result = notFoundException.Failures;
                break;

            case ForbiddenException forbiddenException:
                code = HttpStatusCode.Forbidden;
                result = forbiddenException.Failures;
                break;

            default:
                _logger.LogError(exception, exception.Message);
                break;
        }

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int) code;

        if (result.IsNullOrEmpty())
        {
            return Task.CompletedTask;
        }

        var errors = new List<string>();
        foreach (var error in result)
        {
            errors.AddRange(error.Value);
        }

        return context.Response.WriteAsync(
            JsonConvert.SerializeObject(errors));
    }
}

public static class CustomExceptionHandlerMiddlewareExtensions
{
    public static IApplicationBuilder UseCustomExceptionHandler(
        this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<CustomExceptionHandlerMiddleware>();
    }
}