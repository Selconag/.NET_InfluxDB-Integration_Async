using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace app.Settings;

/// <summary>
/// This is a temporary implementation for testing. If you want to implement it correcty;
/// Create a interface named ISettings.
/// Then create an instance of that interface to access values be calling getter functions.
/// At last, this record must be implemented to accesed its values by getters.
/// </summary>
public record Settings
{
    public readonly string ClientID = Guid.NewGuid().ToString();
    public static readonly string TcpServerAdress = "test.mosquitto.org";
    public static string FluxConfiguration_Bucket = "test";
    public static readonly string FluxConfiguration_OrganizationName = "organization";
    public static string ConnectionAdress = "http://localhost:8086";
    public static string InfluxDBToken = "InfluxDB:Token";

    public Settings GetSettings
    {
        get { return this; }
    }
}
