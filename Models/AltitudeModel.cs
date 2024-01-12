namespace app.Models;

using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
public class AltitudeModel
{
    public string Time { get; init; }
    public int Altitude { get; init; }
    public string DisplayText => $"Plane was at altitude {Altitude} ft. at {Time}.";

    // Method to serialize the object to JSON
    public string ToJson()
    {
        return JsonSerializer.Serialize(this);
    }

    // Static method to deserialize JSON to an AltitudeModel object
    public static AltitudeModel FromJson(string json)
    {
        return JsonSerializer.Deserialize<AltitudeModel>(json);
    }

    public static DateTime TryConvertStringToDateTime(string dateString, string format)
    {
        // Specify the format of the input string
        // For example, "yyyy-MM-dd HH:mm:ss"
        if(format == null)
        {
            format = "dd-MM-yyyy HH:mm:ss";
        }
        try
        {
            DateTime.TryParseExact(dateString, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result);
            return result;
        }
        catch (Exception)
        {

            throw;
        }
    }

}
