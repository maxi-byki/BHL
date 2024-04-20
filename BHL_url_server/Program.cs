using BHL_url_server.Controllers;
using BHL_url_server.DTOs;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

string linkWithBadSites = "https://hole.cert.pl/domains/v2/domains.json";
List<LinkDTO> links = await app.GetBadSitesAsync(linkWithBadSites);

app.Run();