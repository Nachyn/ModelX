using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ModelX.Controllers;

[ApiController]
public class ApiController : ControllerBase
{
    private ISender? _mediator;

    protected ISender Mediator =>
        _mediator ??= HttpContext.RequestServices.GetService<ISender>();
}