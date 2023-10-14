using BlackSun.Core.Components.Base;
using Microsoft.AspNetCore.Components;

namespace BlackSun.Core.Components.PhotoCards;

public partial class PhotoCard : JsComponent
{
    [Parameter] public string PhotoUrl { get; set; }
}