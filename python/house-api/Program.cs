var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();
app.UseSwagger();
app.MapGet("/", () => "Hello World!!!!");
app.MapGet("/powerinfo", () => { return new HouseService().GetInfo(); });
app.UseSwaggerUI();
app.Run();
