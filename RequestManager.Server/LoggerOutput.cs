namespace RequestManager
{
    public static class LoggerOutput
    {
        public const string DefaultOutputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level}] (Id:{RequestId}) {Message}{NewLine}{Exception}";

        public static string CreateMessage(string action, string handlerName, Type handlerType, Type requestType, Type responseType) =>
            CreateMessage(action, handlerName, handlerType, requestType, responseType, false, false);

        public static string CreateMessage(string action, string handlerName, Type handlerType, Type requestType, Type responseType, bool logResponse, bool logElapsed) =>
            $"{action} {handlerName}<{requestType}, {responseType}>" +
            $"{Environment.NewLine}  Handler: {handlerType}{Environment.NewLine}" +
            $"  Request: {{@{requestType.Name}}}" +
            (logResponse ? $"{Environment.NewLine}  Response: {{@{responseType.Name}}}" : "") +
            (logElapsed ? $"{Environment.NewLine}  Time elapsed: {{elapsed}}" : "");
    }
}