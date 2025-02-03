namespace VTR.Framework.Application.Contracts;

public interface IApplicationManager
{
    Task<TResponse> DispatchCommandAsync<TResponse>(RequestBase<TResponse> request, CancellationToken cancellationToken);

    Task<TResponse> DispatchQueryAsync<TResponse>(RequestBase<TResponse> request, CancellationToken cancellationToken);

    Task<TResponse> DispatchTransactionCommandAsync<TResponse>(RequestBase<TResponse> request, CancellationToken cancellationToken);
}