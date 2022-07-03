using MongoDB.Driver;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using HouseCore.HouseService;
using DnsClient.Protocol;
using MongoDB.Driver.Core.Connections;
using System.Buffers;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR()
    .AddJsonProtocol(options => {
        options.PayloadSerializerOptions.PropertyNamingPolicy = null;
    });
// builder.Services.AddCors(options => {
//      options.
// });

var app = builder.Build();

var mongoFactory = () => { return new MongoClient("mongodb://192.168.50.5:27017"); };

app.MapHub<HouseHub>("/signalr/house-hub");

app.UseSwagger();

// endpoints

app.MapGet("/api/powerinfo", () => { return new PowerService(mongoFactory()).GetInfo(); });
app.MapGet("/api/powerpriceinfo/{date}", (string date) => { return new PowerPriceService(mongoFactory()).GetInfo(date); });
app.MapGet("/api/waterinfo", () => { return new WaterService(mongoFactory()).GetInfo(); });
app.MapGet("/api/temperatureInfo", () => { return new TemperatureService(mongoFactory()).GetInfo(); });

app.MapGet("/api/house-settings", () => { return new SettingsService(mongoFactory()).GetSettings(); });
app.MapPut("/api/house-settings/powerLimit/{limit}", (int limit) => { return new SettingsService(mongoFactory()).SetPowerLimit(limit); });

app.UseSwaggerUI();
app.Run();
