using RequestManager.API.Repositories;
using RequestManager.Database.Models;
using Microsoft.AspNetCore.Components;

namespace RequestManager.Client.Pages;

public partial class Component1
{
    [Inject] private UserRepository UserRepository { get; set; }
    [Inject] private RequestRepository RequestRepository { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var a = await UserRepository.CreateAsync(new User() { Email = "asfasfasffsafa@gmail.com" });
        var b = await UserRepository.GetFirstOrDefaultAsync();
        b.CreatedBy = a;
        await UserRepository.UpdateAsync(b);
        await base.OnInitializedAsync();
        var c = await UserRepository.GetFirstOrDefaultAsync(x => x.Id == b.Id);
    }
}