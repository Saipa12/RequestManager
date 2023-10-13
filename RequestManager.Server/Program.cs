using RequestManager;
using Serilog;

using B = RequestManager;
Environment.SetEnvironmentVariable("DOTNET_hostBuilder:reloadConfigOnChange", "false");
Environment.SetEnvironmentVariable("ASPNETCORE_hostBuilder:reloadConfigOnChange", "false");
Log.Logger = B.Server.LoggerFactory.Create(ConfigureSerilogBuilder(new ConfigurationBuilder()).Build());
try
{
    Log.Information("Starting web host...");
    await CreateHostBuilder(args).Build().RunAsync();
}
catch (Exception e)
{
    Log.Fatal(e, "Host terminated");
}
finally
{
    Log.CloseAndFlush();
}
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.Run();

static IConfigurationBuilder ConfigureSerilogBuilder(IConfigurationBuilder builder) =>
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddEnvironmentVariables()
                .AddJsonFile("appsettings.json", false, false)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", true, false);
static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((_, config) =>
    {
        config
            .SetBasePath(Directory.GetCurrentDirectory());
        //.AddCipherCatalogs("CipherCatalogs"); // TODO
    })
    //.UseServiceProviderFactory(new DryIocServiceProviderFactory()) // TODO
    .UseSerilog()
    .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());