using MongoDB.Driver;
using Microsoft.AspNetCore.Mvc;
using HouseCore.HouseService;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

var mongoFactory = () => { return new MongoClient("mongodb://192.168.50.5:27017"); };

app.UseSwagger();

// endpoints
app.MapGet("/powerinfo", () => { return new PowerService(mongoFactory()).GetInfo(); });
app.MapGet("/powerpriceinfo", () => { return new PowerPriceService(mongoFactory()).GetInfo(); });
app.MapGet("/waterinfo", () => { return new WaterService(mongoFactory()).GetInfo(); });
app.MapGet("/temperaureInfo", () => { return new TemperatureService(mongoFactory()).GetInfo(); });



app.UseSwaggerUI();
app.Run();
