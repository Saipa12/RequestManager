using AutoMapper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting.Server;
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

    private IEnumerable<DeliverDto> Delivers { get; set; }
    private IEnumerable<RequestStatus> _statuses;
    [Inject] private IMapper Maper { get; set; }
    private bool _canCancelEdit = true;
    private bool _blockSwitch = false;
    private string _searchString = "";
    private RequestDto _selectedItem = null;
    private RequestDto _elementBeforeEdit;
    private HashSet<RequestDto> _selectedItems;
    private TableApplyButtonPosition _applyButtonPosition = TableApplyButtonPosition.End;
    private TableEditButtonPosition _editButtonPosition = TableEditButtonPosition.End;
    private TableEditTrigger _editTrigger = TableEditTrigger.EditButton;
    [Inject] private IDialogService DialogService { get; set; }
    [Inject] private GetRequestsHandler GetRequestsHandler { get; set; }
    [Inject] private GetDeliverHandler GetDeliverHandler { get; set; }
    [Inject] private GetRequestHandler GetRequestHandler { get; set; }
    [Inject] private AddRequestHandler AddRequestHandler { get; set; }
    [Inject] private EditRequestHandler EditRequestHandler { get; set; }
    [Inject] private DeleteRequestHandler DeleteRequestHandler { get; set; }
    [Inject] private RejectedRequestHandler RejectedRequestHandler { get; set; }

    private MudTable<RequestDto> _mudTable;

    protected override async Task OnInitializedAsync()
    {
        //Requests = (await GetRequestsHandler.Handle(new GetRequests(true))).RequestDto;
        //Delivers = (await GetDeliverHandler.Handle(new GetDeliverRequests(true))).DeliverDto;
        //_statuses = Enum.GetValues(typeof(RequestStatus)).Cast<RequestStatus>();
        //_selectedItems = new HashSet<RequestDto>();
        //await InvokeAsync(StateHasChanged);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            Requests = (await GetRequestsHandler.Handle(new GetRequests(true))).RequestDto.ToList();
            _selectedItems = new();
            Delivers = (await GetDeliverHandler.Handle(new GetDeliverRequests(true))).DeliverDto;
            _statuses = Enum.GetValues(typeof(RequestStatus)).Cast<RequestStatus>();
            await InvokeAsync(StateHasChanged);
        }
        await base.OnAfterRenderAsync(firstRender);
    }

    private void BackupItem(RequestDto element)
    {
        _elementBeforeEdit = Maper.Map<RequestDto>(element);
    }

    public async void RejectRequest(RequestDto request)
    {
        var parameters = new DialogParameters<ReasonDialog> { };

        var dialog = await DialogService.ShowAsync<ReasonDialog>($"{request.Id} From {request.DispatchAddress} To {request.DeliveryAddress} ", parameters);
        var result = await dialog.Result;
        //var reason = await dialog.Result.;

        if (!result.Canceled)
        {
            await RejectedRequestHandler.Handle(new RejectedRequest(request, result.Data.ToString()));
        }
    }

    public async void Drop()
    {
        foreach (var request in _selectedItems)
        {
            if (request.Status == RequestStatus.New)
            {
                Requests.Remove(request);
                request.Deliver = null;
                await DeleteRequestHandler.Handle(new DeleteRequest(request));
            }
            else
            {
                RejectRequest(request);
            }
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

        await InvokeAsync(StateHasChanged);
    }

    private void ResetItemToOriginalValues(RequestDto element)
    {
        _elementBeforeEdit = Maper.Map<RequestDto>(element);
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
            DeliveryTime = DateTime.UtcNow.AddDays(1)
        };

        Requests.Insert(0, newRecord);
        await AddRequestHandler.Handle(new AddRequest(newRecord));
        _mudTable.SetSelectedItem(Requests.First());
        _mudTable.SetEditingItem(Requests.First());
        await InvokeAsync(StateHasChanged);
    }
}