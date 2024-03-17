using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Configure Ocelot
/*builder.Host.ConfigureServices((context, services) =>
{
    services.AddOcelot(context.Configuration);
});*/
builder.Configuration.AddJsonFile("ocelot.json", optional: false, reloadOnChange: true);
builder.Services.AddOcelot(builder.Configuration);

var app = builder.Build();

// Use Ocelot middleware
app.UseOcelot().Wait();

app.MapGet("/", () => "Hello World!");

app.Run();
