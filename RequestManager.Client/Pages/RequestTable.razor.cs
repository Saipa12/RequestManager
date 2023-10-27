using AutoMapper;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using RequestManager.Api.Enums;
using RequestManager.API.Dto;
using RequestManager.API.Handlers.DeliverHandler;
using RequestManager.API.Handlers.RequestHandler;
using RequestManager.Core.Components;
using System.Reflection;

namespace RequestManager.Client.Pages;

public partial class RequestTable
{
    private List<RequestDto> Requests { get; set; }
    private int _page = 1;
    private int _pageSize = 10;
    private int _totalItems = 0;
    private bool _isEdit = false;
    private bool _isReadMode = true;
    private bool _isStatusReadMode = true;
    private long _editItemId = 0;
    private List<DeliverDto> Delivers { get; set; }

    //private List<RequestStatus> _statuses;
    [Inject] private IMapper Maper { get; set; }

    private MudTable<RequestDto> _mudTable;
    private string _searchString = "";
    private RequestDto _elementBeforeEdit;
    [Inject] private IDialogService DialogService { get; set; }
    [Inject] private GetRequestsHandler GetRequestsHandler { get; set; }
    [Inject] private GetDeliverHandler GetDeliverHandler { get; set; }
    [Inject] private GetRequestHandler GetRequestHandler { get; set; }
    [Inject] private AddRequestHandler AddRequestHandler { get; set; }
    [Inject] private EditRequestHandler EditRequestHandler { get; set; }
    [Inject] private DeleteRequestHandler DeleteRequestHandler { get; set; }
    [Inject] private RejectedRequestHandler RejectedRequestHandler { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            Requests = (await GetRequestsHandler.Handle(new GetRequests(true))).RequestDto.ToList();
            Delivers = (await GetDeliverHandler.Handle(new GetDeliverRequests(true))).DeliverDto.ToList();
            await InvokeAsync(StateHasChanged);
        }
        await base.OnAfterRenderAsync(firstRender);
    }

    private async void EditRequest(RequestDto request)
    {
        _isEdit = true;
        _editItemId = request.Id;
        if (request.Status == RequestStatus.InProgress)
        {
            _isStatusReadMode = false;
        }
        else
        {
            _isReadMode = false;
        }
        BackupItem(request);
        await InvokeAsync(StateHasChanged);
    }

    private async Task<TableData<RequestDto>> LoadPage(TableState state)
    {
        _page = state.Page;
        _pageSize = state.PageSize;

        var response = await GetRequestsHandler.Handle(new GetRequests(true, _page, _pageSize));
        Requests = response.RequestDto.ToList();
        _totalItems = (await GetRequestsHandler.Handle(new GetRequests(true, _page))).Count;
        return new TableData<RequestDto>() { TotalItems = _totalItems, Items = Requests };
    }

    private void BackupItem(RequestDto element)
    {
        _elementBeforeEdit = Maper.Map<RequestDto>(element);
    }

    public async Task RejectRequest(RequestDto request)
    {
        //var parameters = new DialogParameters<ReasonDialog> { };

        var dialog = await DialogService.ShowAsync<ReasonDialog>($"{request.Id} From {request.DispatchAddress} To {request.DeliveryAddress} "/*, parameters*/);
        var result = await dialog.Result;

        if (!result.Canceled)
        {
            await RejectedRequestHandler.Handle(new RejectedRequest(request, result.Data.ToString()));
        }
    }

    public async void Drop(RequestDto request)
    {
        if (request.Status == RequestStatus.New)
        {
            Requests.Remove(request);
            request.Deliver = null;
            await DeleteRequestHandler.Handle(new DeleteRequest(request));
        }
        else
        {
            await RejectRequest(request);
        }
        await InvokeAsync(StateHasChanged);
    }

    private async void ItemHasBeenCommitted(RequestDto element)
    {
        if (element.Id == 0)
        {
            element.Id = (await GetRequestsHandler.Handle(new GetRequests(false))).RequestDto.Last().Id;
            await EditRequestHandler.Handle(new EditRequest(element));
            Requests = (await GetRequestsHandler.Handle(new GetRequests(true))).RequestDto.ToList();
        }
        else
        {
            await EditRequestHandler.Handle(new EditRequest(element));
        }
        _isEdit = false;
        await InvokeAsync(StateHasChanged);
    }

    private void ResetItemToOriginalValues(RequestDto element)
    {
        element = Maper.Map<RequestDto>(_elementBeforeEdit);
        _isEdit = false;
    }

    private string GetRowCssClass(RequestStatus status)
    {
        return status switch
        {
            RequestStatus.New => "new-status",
            RequestStatus.Rejected => "rejected-status",
            RequestStatus.InProgress => "progres-status",// Если у вас есть стандартный стиль строки
            RequestStatus.Completed => "complited-status",// Если у вас есть стандартный стиль строки
        };
    }

    private bool FilterFunc(RequestDto element)
    {
        if (string.IsNullOrWhiteSpace(_searchString))
            return true;
        if (ConcatenateFields(element).Contains(_searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        return false;
    }

    public static string ConcatenateFields(object obj)
    {
        // Получаем все поля класса
        var fields = obj.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

        // Создаем пустую строку
        var result = string.Empty;

        // Объединяем значения полей в строку
        foreach (var field in fields)
        {
            // Если поле не является статическим, то добавляем его значение к результату
            if (!field.IsStatic)
            {
                result += field.GetValue(obj)?.ToString();
            }
        }

        return result;
    }

    private async void AddRecord()
    {
        _mudTable.SetEditingItem(null);
        var newRecord = new RequestDto
        {
            Status = RequestStatus.New,
            DeliveryAddress = "",
            DispatchAddress = "",
            DeliveryDate = DateTime.UtcNow,
            DeliveryTime = DateTime.UtcNow.AddDays(3)
        };

        Requests.Insert(0, newRecord);
        await AddRequestHandler.Handle(new AddRequest(newRecord));
        EditRequest(Requests.First());
        await InvokeAsync(StateHasChanged);
    }
}