using app.Services;
using Coravel;
using app.Invocables;
using app.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
//Asynchronously await an answer
await Subscriber.Initialize();
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<InfluxDBService>();
builder.Services.AddTransient<WriteRandomPlaneAltitudeInvocable>();
builder.Services.AddScheduler();

var app = builder.Build();
//// Schedule the WriteRandomPlaneAltitudeInvocable every minute
//app.Services.UseScheduler(scheduler =>
//{
//    scheduler
//        .Schedule<WriteRandomPlaneAltitudeInvocable>()
//        .EveryMinute();

//});
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); 

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


// Schedule the WriteRandomPlaneAltitudeInvocable every minute
app.Services.UseScheduler(scheduler =>
{
    scheduler
        .Schedule<WriteRandomPlaneAltitudeInvocable>()
        .EveryMinute();

});

app.ConfigureAwait(true);

// Async method to handle data reading logic
async Task HandleAsyncData(InfluxDBService influxDBService, Stream body)
{
    using (var reader = new StreamReader(body, Encoding.UTF8))
    {
        string jsonData = await reader.ReadToEndAsync();
        AltitudeModel altitudeModel = AltitudeModel.FromJson(jsonData);

        // Run the synchronous Write method asynchronously
        await Task.Run(() =>
        {
            influxDBService.Write(write =>
            {
                // Modify this part to match your actual writing logic
                write.WriteMeasurement(altitudeModel.Altitude);
            });
        });
    }
}
app.Run();


