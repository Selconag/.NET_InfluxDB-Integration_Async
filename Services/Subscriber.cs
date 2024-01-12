using MQTTnet.Client;
using MQTTnet;
using System.Text;
using MQTTnet.Server;
using System.Text.Json;
using app;
namespace app.Services;

public class Subscriber
{
    public static async Task Initialize()
    { 
        var mqttFactory = new MqttFactory();
        IMqttClient client = mqttFactory.CreateMqttClient();
        Settings.Settings settings = new Settings.Settings();
        //Build it first
        var options = mqttFactory.CreateClientOptionsBuilder()
            .WithClientId(settings.ClientID)
            .WithTcpServer(Settings.Settings.TcpServerAdress, 1883)
            .WithCleanSession()
            .Build();



        client.UseConnectedHandler(async e =>
        {
            Console.WriteLine("Connected to the Broker Succesfully");
            var topicFilter = new TopicFilterBuilder()
            .WithTopic("Houston")
            .Build();
            await client.SubscribeAsync(topicFilter);
        });

        client.UseDisconnectedHandler(e =>
        {
            Console.WriteLine("Disconnected to the Broker Succesfully");
        });

        client.UseApplicationMessageReceivedHandler(e =>
        {
            Console.WriteLine($"Received Message: {e.ApplicationMessage.Topic}- {Encoding.UTF8.GetString(e.ApplicationMessage.Payload)}");
            if(e.ApplicationMessage.Topic == "Shutdown")
            {
                client.DisconnectAsync();
            }

        });

        await client.ConnectAsync(options);

        //Console.ReadLine();

        //Send data to influx DB here
        //client.DisconnectAsync();


    }
}
