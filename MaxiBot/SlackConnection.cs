using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;

namespace MaxiBot
{
    public class SlackConnection
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            /*if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }*/

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // Endpoint do obsługi zdarzeń Slacka
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapPost("https://maxibykibhl3.azurewebsites.net/", async context =>
                {
                    using (StreamReader reader = new StreamReader(context.Request.Body))
                    {
                        string requestBody = await reader.ReadToEndAsync();
                        dynamic eventData = JObject.Parse(requestBody);

                        // Sprawdź typ zdarzenia
                        string eventType = eventData.type;

                        if (eventType == "event_callback")
                        {
                            dynamic eventDetails = eventData.@event;
                            string eventTypeCallback = eventDetails.type;

                            // Sprawdź czy zdarzenie dotyczy nowej wiadomości w kanale
                            if (eventTypeCallback == "message" && eventDetails.subtype != "bot_message")
                            {
                                string channelId = eventDetails.channel;
                                string userId = eventDetails.user;
                                string messageText = eventDetails.text;
                                string timestamp = eventDetails.ts;

                                
                            }
                        }

                        context.Response.StatusCode = 200;
                    }
                });
            });
        }
    }
}
