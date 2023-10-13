namespace RequestManager.Core.Handlers;

public interface IHandler<in TRequest, out TResponse>
{
    TResponse Handle(TRequest request = default);
}