using Microsoft.AspNetCore.Http;

namespace VTR.Framework.WebApplication;

[Authorize("Bearer")]
public class ApiControllerAuthorizeBase<T>(
    ILogger<T> logger,
    IHttpContextAccessor accessor,
    IApplicationManager applicationManager) : ApiControllerBase<T>(logger, applicationManager)
{
    [NonAction]
    public bool CheckRoles(params string[] roles)
    {
        var context = accessor.HttpContext;
        if (context != null)
        {
            foreach (var role in roles)
            {
                if (context.User.IsInRole(role))
                    return true;
            }
        }
        return false;
    }
}

[ApiController]
[Route("[controller]")]
public class ApiControllerBase<T>(
    ILogger<T> logger,
    IApplicationManager applicationManager) : Controller
{
    public ILogger<T> Logger { get; } = logger;
    public IApplicationManager ApplicationManager { get; } = applicationManager;

    [NonAction]
    public async Task<IActionResult> TryQuery<TResponse>(
       RequestBase<TResponse> request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await ApplicationManager.DispatchQueryAsync(request, cancellationToken);

            if ((response is ResponseBase responseBase) && responseBase.OperationResult.MessageType != MessageType.Success)
                return BadRequest(response);

            return base.Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequestWithException(ex);
        }
    }

    [NonAction]
    public async Task<IActionResult> TryCommand<TResponse>(
       RequestBase<TResponse> request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await ApplicationManager.DispatchCommandAsync(request, cancellationToken);

            if ((response is ResponseBase responseBase) && responseBase.OperationResult.MessageType != MessageType.Success)
                return BadRequest(response);

            return base.Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequestWithException(ex);
        }
    }

    [NonAction]
    public async Task<IActionResult> TryTransactionCommand<TResponse>(
       RequestBase<TResponse> request, CancellationToken cancellationToken)
    {
        try
        {
            var response = await ApplicationManager.DispatchTransactionCommandAsync(request, cancellationToken);

            if ((response is ResponseBase responseBase) && responseBase.OperationResult.MessageType != MessageType.Success)
                return BadRequest(response);

            return base.Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequestWithException(ex);
        }
    }

    [NonAction]
    public IActionResult BadRequestWithException(Exception? ex)
    {
        StringBuilder stringBuilder = new();
        while (ex != null)
        {
            stringBuilder.AppendLine(ex.Message);
            ex = ex.InnerException;
        }

        return base.BadRequest(new
        {
            OperationResult = new OperationResult(stringBuilder.ToString(), MessageType.Error)
        });
    }
}