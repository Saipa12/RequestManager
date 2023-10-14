using BlackSun.Core.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlackSun.Core.Components.Base;

public abstract class JsComponent : ComponentBase, IAsyncDisposable
{
    [Parameter] public string Class { get; set; }
    [Parameter] public string Style { get; set; }

    [Inject] public IJSRuntime JsRuntime { get; set; }

    protected string Identifier { get; private set; }
    protected IJSObjectReference JsReference { get; private set; }

    protected virtual string JsPath => $"./output/{JsClassName.FirstCharacterToLower()}.js";
    protected virtual string JsClassName => GetType().Name;

    protected override async Task OnInitializedAsync()
    {
        Identifier = Guid.NewGuid().ToString();

        await base.OnInitializedAsync();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            //var js = await JsRuntime.InvokeAsync<IJSInProcessObjectReference>("import", JsPath);
            //JsReference = await js.InvokeAsync<IJSObjectReference>($"create");
            JsReference = await JsRuntime.InvokeAsync<IJSObjectReference>($"blacksun.{JsClassName}.create");

            await JsReference.InvokeVoidAsync("initialize", Identifier);
            await base.OnAfterRenderAsync(firstRender);

            await InvokeAsync(StateHasChanged);
        }
    }

    public async ValueTask DisposeAsync()
    {
        try
        {
            await JsReference.InvokeAsync<IJSObjectReference>("destroy");
            JsReference = null;
        }
        catch { }
    }
}