using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

public class MailchimpSubscriber
{
    public string Email { get; set; }
    public string FirstName { get; set; }
}

public class MailchimpAPI
{
    private string apiKey;
    private string listId;
    private HttpClient httpClient;

    public MailchimpAPI(string ApiKey, string listId)
    {
        this.apiKey = ApiKey;
        this.listId = listId;
        this.httpClient = new HttpClient();
        this.httpClient.BaseAddress = new Uri("https://us21.api.mailchimp.com/3.0/"); // Replace <dc> with the data center of your Mailchimp account
        this.httpClient.DefaultRequestHeaders.Add("Authorization", "apikey " + apiKey);
    }

    public async Task<bool> AddSubscriber(MailchimpSubscriber subscriber)
    {
        try
        {
            var payload = new
            {
                email_address = subscriber.Email,
                merge_fields = new
                {
                    FNAME = subscriber.FirstName,
                },
                status = "subscribed"
            };

            var jsonPayload = JsonSerializer.Serialize(payload);
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync($"lists/{listId}/members", content);

            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error adding subscriber to Mailchimp: " + ex.Message);
            return false;
        }
    }
}
