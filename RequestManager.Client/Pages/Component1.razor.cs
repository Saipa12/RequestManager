using AutoMapper;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using RequestManager.Api.Enums;
using RequestManager.API.Dto;
using RequestManager.API.Handlers.DeliverHandler;
using RequestManager.API.Handlers.RequestHandler;

namespace RequestManager.Client.Pages;

public partial class Component1
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

    [Inject] private GetRequestsHandler GetRequestsHandler { get; set; }
    [Inject] private GetDeliverHandler GetDeliverHandler { get; set; }
    [Inject] private GetRequestHandler GetRequestHandler { get; set; }
    [Inject] private AddRequestHandler AddRequestHandler { get; set; }
    [Inject] private EditRequestHandler EditRequestHandler { get; set; }
    [Inject] private DeleteRequestHandler DeleteRequestHandler { get; set; }

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

    public async void Drop()
    {
        foreach (var request in _selectedItems)
        {
            Requests.Remove(request);
            request.Deliver = null;
            await DeleteRequestHandler.Handle(new DeleteRequest(request));
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
        if (element.Id.ToString().Contains(_searchString, StringComparison.OrdinalIgnoreCase))
            return true;
        return false;
    }

    private async void AddNewRecord()
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