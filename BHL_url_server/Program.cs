using System.Net.Mime;
using System.Text.Json.Nodes;
using BHL_url_server.Controllers;
using BHL_url_server.DTOs;

var builder = WebApplication.CreateBuilder(args);

/* TRYING TO CONFIGURE CROS */
var  MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy  =>
        {
            policy.WithOrigins("http://10.105.24.152");
        });
});

// services.AddResponseCaching();

builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();
/* END OF CORS SPECIFICATION */

app.MapGet("/", () => "Hello World!");

string linkWithBadSites = "https://hole.cert.pl/domains/v2/domains.json";
List<LinkDTO> links = await app.GetBadSitesAsync(linkWithBadSites);

app.MapPost("/", (JsonObject payload) =>
{
    var challenge = payload["challenge"]?.ToString();

    if (challenge != null)
    {
        return Results.Content(challenge, "text/plain");
    }
    else
    {
        return Results.BadRequest("Bad request");
    }
});

app.Run();