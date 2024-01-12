using System;
using System.Threading.Tasks;
using app.Services;
using Coravel.Invocable;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;
using app.Settings;
using app.Models;
namespace app.Invocables;

public class WriteRandomPlaneAltitudeInvocable : IInvocable
{
    private readonly InfluxDBService _service;
    private static readonly Random _random = new Random();
    public WriteRandomPlaneAltitudeInvocable(InfluxDBService service)
    {
        _service = service;
    }

    public Task Invoke()
    {
        _service.Write(write =>
        {
            var point = PointData.Measurement("altitude")
                .Tag("plane", "test-plane")
                .Field("value", _random.Next(1000, 5000))
                .Timestamp(DateTime.UtcNow, WritePrecision.Ns);

            write.WritePoint(point, Settings.Settings.FluxConfiguration_Bucket, Settings.Settings.FluxConfiguration_OrganizationName);
        });

        return Task.CompletedTask;
    }

    public Task SendDataManually(AltitudeModel dataObject)
    {
        _service.Write(write =>
        {
            var point = PointData.Measurement("altitude")
                    .Tag("plane", "test-plane")
                    .Field("value", dataObject.Altitude)
                    .Timestamp(AltitudeModel.TryConvertStringToDateTime(dataObject.Time, null), WritePrecision.Ns);

            write.WritePoint(point, Settings.Settings.FluxConfiguration_Bucket, Settings.Settings.FluxConfiguration_OrganizationName);
        });


        return Task.CompletedTask;
    }
}