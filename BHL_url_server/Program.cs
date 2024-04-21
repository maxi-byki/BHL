using BHL_url_server.Endpoints;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

await app.MapSlackApiEndpointsAsync();

app.Run();