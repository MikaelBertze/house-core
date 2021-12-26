using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();

var mongoFactory = () => { return new MongoClient("mongodb://192.168.50.5:27017"); };

app.UseSwagger();
app.MapGet("/powerinfo", () => { return new HouseService(mongoFactory()).GetInfo(); });
app.UseSwaggerUI();
app.Run();
