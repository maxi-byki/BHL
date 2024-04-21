using System.Text.Json.Nodes;
using Newtonsoft.Json;
using Slack.NetStandard;
using Slack.NetStandard.EventsApi;
using Slack.NetStandard.Messages.Blocks;
using Slack.NetStandard.WebApi.Chat;

namespace BHL_url_server.HelpClasses;

public class AsyncSender
{
    public async static void SendAsync(JsonObject payload)
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
                                
                                request.Blocks.Add(new Section{Text = new PlainText(chatRequest)});
                                
                                /* HERE SHOULD BE KEY */
                                await client.Chat.Post(request);
                                
                            }
                                break;
                            default:
                                break;
                        }
                    }
    }
}