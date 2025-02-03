namespace VTR.Framework.API;

[Authorize("Bearer")]
public class ApiControllerAuthorizeBase<T>(
    ILogger<T> logger,
    IApplicationManager applicationManager) : ApiControllerBase<T>(logger, applicationManager)
{

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

            return base.Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequestWithException(ex);
        }
    }

    [NonAction]
    public IActionResult BadRequestWithException(Exception ex)
    {
        return base.BadRequest(new { OperationResult = new OperationResult("An unexpected error occurred", ex) });
    }
}