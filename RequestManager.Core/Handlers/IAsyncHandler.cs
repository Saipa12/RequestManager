namespace RequestManager.Core.Handlers;

public interface IAsyncHandler<in TRequest, TResponse> : IHandler<TRequest, Task<TResponse>>
{
}