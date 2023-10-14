using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using RequestManager.API.Dto;
using RequestManager.API.Handlers.RequestHandler;

namespace RequestManager.Client.Pages;

public partial class Component1
{
    private IEnumerable<RequestDto> _requests;

    [Inject] private IServiceProvider ServiceProvider { get; set; }

    protected override async Task OnInitializedAsync()
    {
        // Получите экземпляр GetRequestsHandler из DI контейнера
        var getRequestsHandler = ServiceProvider.GetRequiredService<GetRequestsHandler>();

        // Вызовите Handle для получения данных
        _requests = (await getRequestsHandler.Handle(new GetRequests(true))).RequestDto;
        // SaveChanges();
        // Обновите состояние компонента
    }
}