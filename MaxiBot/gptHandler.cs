using System.Net.Http.Json;
using System.Text.Json;
using OpenAI_API;

namespace MaxiBot;

public class gptHandler
{
    // private static string apiKey = "KEY";

    public static async Task<string> get_response(string question)
    {
        APIAuthentication aPIAuthentication = new APIAuthentication(apiKey);
        OpenAIAPI openAiApi = new OpenAIAPI(aPIAuthentication);
        
        string prompt = $"You are a cybersecurity expert, who responds to questions from corporation employees asking you about the security issues in their company. You should answer their question to ensure maximum safety of company data.";
        string model = "gpt-4-turbo";
        int maxTokens = 250;
        
        string url = $"https://api.openai.com/v1/chat/completions";


        using (var client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            var requestBody = new
            {
                messages = new[]
                {
                    new { role = "system", content = prompt + ' ' + question }
                },
                model = model
            };

            var response = await client.PostAsJsonAsync(url, requestBody);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                
                ChatCompletionResponse chatCompletionResponse = JsonSerializer.Deserialize<ChatCompletionResponse>(responseBody);
                
                return chatCompletionResponse.choices[0].message.content;
            }
            else
            {
                throw new Exception($"Failed to request response from OpenAI API. Status code: {response.StatusCode}");
            }
        }
    }
}

public class ChatCompletionResponse
{
    public string id { get; set; }
    public string @object { get; set; }
    public long created { get; set; }
    public string model { get; set; }
    public Choice[] choices { get; set; }
    public Usage usage { get; set; }
    public string system_fingerprint { get; set; }
}

public class Choice
{
    public int index { get; set; }
    public Message message { get; set; }
    public object logprobs { get; set; }
    public string finish_reason { get; set; }
}

public class Message
{
    public string role { get; set; }
    public string content { get; set; }
}

public class Usage
{
    public int prompt_tokens { get; set; }
    public int completion_tokens { get; set; }
    public int total_tokens { get; set; }
}