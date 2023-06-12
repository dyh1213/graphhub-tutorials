using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using GraphHub.Client;
using MudBlazor.Services;
using System.Net.Http.Headers;
using MudBlazor;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddSingleton<GraphHub.Client.Shared.GraphDatabaseService>();

string graphQLServerPath = builder.HostEnvironment.BaseAddress + "graphql";
builder.Services.AddGHGQLAPI()
           .ConfigureHttpClient(client =>
           {
               client.BaseAddress = new Uri(graphQLServerPath);
           }
);


builder.Services.AddMudServices();
builder.Services.AddMudMarkdownServices();

await builder.Build().RunAsync();

