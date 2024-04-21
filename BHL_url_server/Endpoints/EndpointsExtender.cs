using System.Text.Json.Nodes;
using BHL_url_server.Controllers;
using BHL_url_server.DTOs;
using Microsoft.AspNetCore.Components.RenderTree;

namespace BHL_url_server.Endpoints;

public static class EndpointsExtender
{
    private static readonly string linkWithBadLinks = "https://hole.cert.pl/domains/v2/domains.json";
    private static List<LinkDTO> links;
    public async static Task<RouteGroupBuilder> MapSlackApiEndpointsAsync(this WebApplication app)
    {
        var group = app.MapGroup("slackapi");
        
        app.MapGet("/", () => "Hello World!");

        string linkWithBadSites = "https://hole.cert.pl/domains/v2/domains.json";
        links = await app.GetBadSitesAsync(linkWithBadSites);

        app.MapPost("/slackapi", (JsonObject payload) =>
        {
            switch (payload["type"]?.ToString())
            {
                case "url_verification":
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
                }
                    break;
                case "link_verification":
                {
                    switch(links.FindIndex(link => link.DomainAddress == payload["link"].ToString()))
                    {
                        case -1:
                            return Results.Content("Safe site");
                        default:
                            return Results.Content("Dangerous site");
                    }
                }
            }

            // zwracamy cokolwiek
            return Results.BadRequest("Bad request");
        });
            
        return group;
    }
}