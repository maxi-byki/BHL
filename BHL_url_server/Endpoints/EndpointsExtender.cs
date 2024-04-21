using System.Text.Json.Nodes;
using BHL_url_server.Controllers;
using BHL_url_server.DTOs;
using Microsoft.AspNetCore.Components.RenderTree;
using Newtonsoft.Json;
using Slack.NetStandard;
using Slack.NetStandard.EventsApi;
using Slack.NetStandard.EventsApi.CallbackEvents;
using Slack.NetStandard.Messages.Blocks;
using Slack.NetStandard.WebApi.Chat;
using BHL_url_server.HelpClasses;
using MaxiBot;
using Microsoft.AspNetCore.Http.HttpResults;

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

        app.MapPost("/slackapi", async (JsonObject payload) =>
        {
            switch (payload["type"]?.ToString())
            {
                case "url_verification":
                {
                    var challenge = payload["challenge"]?.ToString();

                    using (StreamWriter outputFile = new StreamWriter("./WriteLines.txt", false))
                    {
                        outputFile.WriteLine("Challenge verification.");
                    }

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
                    using (StreamWriter outputFile = new StreamWriter("./WriteLines.txt", true))
                    {
                        outputFile.WriteLine("Link verification");
                    }

                    switch (links.FindIndex(link => link.DomainAddress == payload["link"].ToString()))
                    {
                        case -1:
                            return Results.Content("Safe site");
                        default:
                            return Results.Content("Dangerous site");
                    }
                }
                    break;
                default:
                {
                    string token = payload["token"]?.ToString();
                    var eventObject = JsonConvert.DeserializeObject<Event>(payload.ToString());
                    Root root = JsonConvert.DeserializeObject<Root>(payload.ToString());

                    using (StreamWriter outputFile = new StreamWriter("./WriteLines.txt", true))
                    {
                        outputFile.WriteLine("Link verification, token:" + token);
                        outputFile.WriteLine(payload.ToString());
                    }

                    if (eventObject is EventCallback callback)
                    {
                        using (StreamWriter outputFile = new StreamWriter("./WriteLines.txt", true))
                        {
                            outputFile.WriteLine("Sucessfully entered if.");
                        }

                        switch (root.@event.type)
                        {
                            case "app_mention":
                            {
                                using (StreamWriter outputFile = new StreamWriter("./WriteLines.txt", true))
                                {
                                    outputFile.WriteLine("Sucessfully entered app mention.");
                                }

                                // string text = payload["text"]?.ToString();

                                // Uzyskaj dostęp do pól channel i text
                                string channel = root.@event.channel;
                                string text = root.@event.text;

                                using (StreamWriter outputFile = new StreamWriter("./WriteLines.txt", true))
                                {
                                    outputFile.WriteLine("Channel: " + channel + " Text: " + text);
                                }

                                var request = new PostMessageRequest { Channel = channel };

                                string chatRequest = await GptHelper.get_response(text);

                                request.Blocks.Add(new Section { Text = new PlainText(chatRequest) });

                                var client =
                                    /* HERE SHOULD BE KEY */
                                await client.Chat.Post(request);

                                return Results.NoContent();
                            }
                                break;
                            case "message":
                            {
                                string channel = root.@event.channel;
                                string text = root.@event.text;
                                
                                var mess = await Parser.Check(text, links);
                                if ( mess == "OK")
                                    return Results.NoContent();

                                var request = new PostMessageRequest { Channel = channel };
                                
                                request.Blocks.Add(new Section { Text = new PlainText(mess) });

                                var client =
                                    /* HERE SHOULD BE KEY */
                                await client.Chat.Post(request);

                                return Results.NoContent();
                            }
                                break;
                            default:
                                break;
                        }

                        break;
                    }
                }
                    // zwracamy cokolwie
                    return Results.BadRequest("Bad request");
            }
            return Results.BadRequest("Bad request");
        });
            
        return group;
    }
}