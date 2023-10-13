using System.Reflection;

namespace RequestManager.Server;

public partial class App
{
    private IEnumerable<Assembly> AdditionalAssemblies { get; set; } = Enumerable.Empty<Assembly>();
}