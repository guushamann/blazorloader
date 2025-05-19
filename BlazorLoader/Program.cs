using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

using blazorloader;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddMemoryCache();
builder.Services.AddTransient<IMemoryCacheService, MemoryCacheService>();
builder.Services.AddHttpClient();

builder.Services.AddTransient(typeof(HttpService));
builder.Services.AddTransient(typeof(BlazorQuery<>));
builder.Services.AddSingleton<DataChangeNotifier>();

await builder.Build().RunAsync();
