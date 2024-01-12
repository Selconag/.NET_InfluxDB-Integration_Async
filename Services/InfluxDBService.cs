using System;
using System.Threading.Tasks;
using InfluxDB.Client;
using Microsoft.Extensions.Configuration;
using app.Settings;
namespace app.Services;

public class InfluxDBService
{
    private readonly string _token;
    private InfluxDBClient _client;

    public InfluxDBService(IConfiguration configuration)
    {
        _token = configuration.GetValue<string>(Settings.Settings.InfluxDBToken);
    }

    /// <summary>
    /// This method uses obsolete caller. Please refrain it from using except if language problem occurs.
    /// Instead please use <see cref="Write"/>
    /// </summary>
    /// <param name="action"></param>
    [Obsolete]
    public void Write_Obsolete(Action<WriteApi> action)
    {
        InfluxDBClient client = InfluxDBClientFactory.Create(Settings.Settings.ConnectionAdress, _token);
        WriteApi write = client.GetWriteApi();
        action(write);
    }

    public void Write(Action<WriteApi> action)
    {
        _client = new InfluxDBClient(Settings.Settings.ConnectionAdress, _token);
        WriteApi write = _client.GetWriteApi();
        action(write);
    }

    public async Task<T> QueryAsync<T>(Func<QueryApi, Task<T>> action)
    {
        using var client = InfluxDBClientFactory.Create(Settings.Settings.ConnectionAdress, _token);
        var query = client.GetQueryApi();
        return await action(query);
    }
}