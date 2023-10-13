using Serilog;

using S = Serilog;

namespace RequestManager.Server;

public static class LoggerFactory
{
    public static S.ILogger Create(IConfiguration configuration)
    {
        //LinqToDB.Data.DataConnection.TurnTraceSwitchOn();
        //LinqToDB.Data.DataConnection.WriteTraceLine = (message, displayName, level) =>
        //{
        //    Log.Logger.Information(message);
        //};

        return new LoggerConfiguration().ReadFrom.Configuration(configuration)
            .Enrich.FromLogContext()
            //.IgnoreProperties()
            .Enrich.WithProperty("Host", configuration["HostName"])
            .Enrich.WithProperty("HostIpAddress", configuration["HostIpAddress"])
            .WriteTo.Console(outputTemplate: LoggerOutput.DefaultOutputTemplate)
            .CreateLogger();
    }

    // TODO: Добавить
    //static LoggerConfiguration IgnoreProperties(this LoggerConfiguration configuration) =>
    //    configuration.Destructure.ByIgnoringProperties<CreateUserParameter>(x => x.Email, x => x.Password)
    //        .Destructure.ByIgnoringProperties<LoginParameter>(x => x.Email, x => x.Password)
    //        .Destructure.ByIgnoringProperties<LoginResult>(x => x.Token)
    //        .Destructure.ByIgnoringProperties<ResetPasswordParameter>(x => x.Email, x => x.Token, x => x.NewPassword)
    //        .Destructure.ByTransforming<EmailService.EmailMessage>(x => x.Body);
}