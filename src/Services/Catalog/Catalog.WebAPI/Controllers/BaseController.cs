using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.WebAPI.Controllers;

public class BaseController : ControllerBase
{
    private IMediator _sender;
    protected IMediator Sender => _sender ??= HttpContext.RequestServices.GetService<IMediator>();
}
