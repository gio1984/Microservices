using System;
using System.Data;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.WebHost.UseUrls("http://0.0.0.0:80");
var app = builder.Build();
var logger = app.Services.GetRequiredService<ILogger<Program>>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

const string apiKeyHeaderName = "X-Api-Key";
const string validApiKey = "secret";

app.Use(async (context, next) =>
{
    if (context.Request.Path.StartsWithSegments("/api/"))
    {
        if (!context.Request.Headers.TryGetValue(apiKeyHeaderName, out var extractedApiKey) || extractedApiKey != validApiKey)
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Unauthorized");
            return;
        }
    }
    await next();
});

//Elevator POST endpoint
app.MapPost("/api/elevator/", async (ElevatorData data) => //, AppDbContext db) =>
{
    logger.LogInformation("Received elevator data: {@Data}", data);
    //db.Add(data);
    //await db.SaveChangesAsync();
    return Results.Ok();
});

app.Run();


public class ElevatorData
{
    public DateOnly Date { get; set; }
    public int Id { get; set; }
    public int Floor { get; set; }
    public string status { get; set; }
}