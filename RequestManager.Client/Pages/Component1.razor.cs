using AutoMapper;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor;
using RequestManager.Api.Enums;
using RequestManager.API.Dto;
using RequestManager.API.Handlers.DeliverHandler;
using RequestManager.API.Handlers.RequestHandler;

namespace RequestManager.Client.Pages;

public partial class Component1
{
    private IEnumerable<RequestDto> _requests;
    private IEnumerable<DeliverDto> _delivers;
    private IEnumerable<RequestStatus> _statuses;
    [Inject] private IServiceProvider ServiceProvider { get; set; }
    [Inject] private IMapper Maper { get; set; }
    private bool _canCancelEdit = true;
    private bool _blockSwitch = false;
    private string _searchString = "";
    private RequestDto _selectedItem = null;
    private RequestDto _elementBeforeEdit;
    private HashSet<RequestDto> _selectedItems = new();
    private TableApplyButtonPosition _applyButtonPosition = TableApplyButtonPosition.End;
    private TableEditButtonPosition _editButtonPosition = TableEditButtonPosition.End;
    private TableEditTrigger _editTrigger = TableEditTrigger.EditButton;
    [Inject] private GetRequestsHandler GetRequestsHandler { get; set; }
    [Inject] private GetDeliverHandler GetDeliverHandler { get; set; }
    [Inject] private GetRequestHandler GetRequestHandler { get; set; }
    [Inject] private AddRequestHandler AddRequestHandler { get; set; }
    [Inject] private EditRequestHandler EditRequestHandler { get; set; }
    [Inject] private DeleteRequestHandler DeleteRequestHandler { get; set; }

    protected override async Task OnInitializedAsync()
    {
        _requests = (await GetRequestsHandler.Handle(new GetRequests(true))).RequestDto;
        _delivers = (await GetDeliverHandler.Handle(new GetDeliverRequests(true))).DeliverDto;
        _statuses = Enum.GetValues(typeof(RequestStatus)).Cast<RequestStatus>();
    }

    private void BackupItem(RequestDto element)
    {
        _elementBeforeEdit = Maper.Map<RequestDto>(element);
    }

    public async void Drop()
    {
        foreach (var request in _selectedItems)
        {
            await DeleteRequestHandler.Handle(new DeleteRequest(request));
        }
    }

    private async void ItemHasBeenCommitted(RequestDto element)
    {
        await EditRequestHandler.Handle(new EditRequest(element));
    }

    private void ResetItemToOriginalValues(RequestDto element)
    {
        // DatabaseContext model = new(connString);
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

    private async void AddRequest(RequestDto element)
    {
        await AddRequestHandler.Handle(new AddRequest(element));
    }
}