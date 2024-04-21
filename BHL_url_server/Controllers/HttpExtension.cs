using System.Text.Json;
using BHL_url_server.DTOs;

namespace BHL_url_server.Controllers;

public static class HttpExtension
{
    public static async Task<List<LinkDTO>> GetBadSitesAsync(this WebApplication app, string url)
    {
        List<LinkDTO> badLinks = new List<LinkDTO>();
        HttpClient client = new HttpClient();

        try
        {
            var json = await client.GetStringAsync(url);
            badLinks = JsonSerializer.Deserialize<List<LinkDTO>>(json);
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"An error occurred while sending the HTTP request: {e.Message}");
        }
        catch (JsonException e)
        {
            Console.WriteLine($"An error occurred while deserializing JSON: {e.Message}");
        }
        catch (Exception e)
        {
            Console.WriteLine($"An error occurred: {e.Message}");
        }

        return badLinks;
    }
}