using System.Data;
using System.Net;
using System.Net.Sockets;
using System.Security.Principal;
using Serilog;
using Serilog.Context;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using ILogger = Serilog.ILogger;

public enum LogLevel
{
    Information,
    Warning,
    Error,
    Fatal
}

public static class Logger
{
    private static readonly ILogger _logger;

    static Logger()
    {
        _logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .Enrich.WithProperty("ComputerName", Environment.MachineName)
            .Enrich.WithProperty("UserDomainNamePC", Environment.UserName)
            .Enrich.WithProperty("UserNamePC", Environment.UserDomainName)
            .WriteTo.Console()
            .WriteTo.File("Logs/logs.log", rollingInterval: RollingInterval.Day,
                outputTemplate:
                "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}] [{Level:u3}] {ComputerName} {RemoteIpAddress} {UserNamePC} {UserDomainNamePC}  - {Message:lj}{NewLine}{Exception}")
            .WriteTo.MSSqlServer(
                "Server=localhost\\SQLEXPRESS;Database=CocukSbDilenciYakalamaProje; Integrated Security=True;TrustServerCertificate=True;Trusted_Connection=True;MultipleActiveResultSets=true",
                new MSSqlServerSinkOptions
                {
                    TableName = "Logs",
                    AutoCreateSqlTable = true
                },
                columnOptions: new ColumnOptions
                {
                    AdditionalDataColumns = new List<DataColumn>
                    {
                        new() { DataType = typeof(string), ColumnName = "UserName" },
                        new() { DataType = typeof(string), ColumnName = "UserNamePC" },
                        new() { DataType = typeof(string), ColumnName = "UserDomainNamePC" },
                        new() { DataType = typeof(string), ColumnName = "IpAddress" },
                        new() { DataType = typeof(string), ColumnName = "ComputerName" }
                    }
                })
            .CreateLogger();
    }

    public static string GetClientComputerName(HttpContext context)
    {
        var remoteIpAddress = context.Connection.RemoteIpAddress;
        if (remoteIpAddress == null) return "unknown";

        try
        {
            var hostEntry = Dns.GetHostEntry(remoteIpAddress);
            return hostEntry.HostName;
        }
        catch (SocketException)
        {
            return "unknown";
        }
    }

    public static void LogMessage(string message, string username, LogLevel level = LogLevel.Information,
        HttpContext context = null)
    {
        using (LogContext.PushProperty("UserName", username))
        {
            using (LogContext.PushProperty("ComputerName", GetClientComputerName(context)))
            {
                using (LogContext.PushProperty("UserDomainNamePC", WindowsIdentity.GetCurrent()?.Name?.Split('\\')[0]))
                {
                    using (LogContext.PushProperty("UserNamePC", WindowsIdentity.GetCurrent()?.Name?.Split('\\')[1]))
                    {
                        using (LogContext.PushProperty("IpAddress",
                                   context?.Connection?.RemoteIpAddress?.ToString() ?? "unknown"))
                        {
                            switch (level)
                            {
                                case LogLevel.Information:
                                    _logger.Information(message + " İşlemi yapan kullanıcı: " + username);
                                    break;

                                case LogLevel.Warning:
                                    _logger.Warning(message + " İşlemi yapan kullanıcı: " + username);
                                    break;

                                case LogLevel.Error:
                                    _logger.Error(message + " İşlemi yapan kullanıcı: " + username);
                                    break;

                                case LogLevel.Fatal:
                                    _logger.Fatal(message + " İşlemi yapan kullanıcı: " + username);
                                    break;
                            }
                        }
                    }
                }
            }
        }
    }
}