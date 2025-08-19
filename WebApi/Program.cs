
//using System;
//using System.Data;
//using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.WebHost.UseUrls("http://0.0.0.0:80");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
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

// check API key for requests to /api/ endpoints
app.Use(async (context, next) =>
{
    logger.LogInformation("Checking API key for request: {Path}", context.Request.Path);
    if (context.Request.Path.StartsWithSegments("/api"))
    {
        if (!context.Request.Headers.TryGetValue(apiKeyHeaderName, out var extractedApiKey) || extractedApiKey != validApiKey)
        {
            logger.LogWarning("Unauthorized request: {Path}", context.Request.Path);
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Unauthorized");
            return;
        }
    }
    await next();
});

//Elevator POST endpoint
app.MapPost("/api/elevator/", async (ElevatorModel data, AppDbContext db) =>
{
    logger.LogInformation("Received elevator data: {@UtcId} {@ElevatorId} floor {@Floor} {Status}", data.EventIdUtc, data.ElevatorId, data.Floor, data.Status);
    db.Add(data);
    await db.SaveChangesAsync();
    return Results.Ok();
});

// Doors POST endpoint
app.MapPost("/api/door/", async (DoorsModel data, AppDbContext db) =>
{
    logger.LogInformation("Received door data: {@UtcId} {@doorId} {status}", data.EventIdUtc, data.DoorId, data.Status);
    db.Add(data);
    await db.SaveChangesAsync();
    return Results.Ok();
});

// Smoke detectors POST endpoint
app.MapPost("/api/smokedetector/", async (SmokeDetectorModel data, AppDbContext db) =>
{
    logger.LogInformation("Received smoke detector data: {@UtcId} {@Data}", data.EventIdUtc, data.DetectorId);
    db.Add(data);
    await db.SaveChangesAsync();
    return Results.Ok();
});

// Get latest elevator status
app.MapGet("/api/elevatorsstatus/", async (AppDbContext db) =>
{
    var elevatorsStatus = await (
        from d in db.DataElevator
            join latest in
                (from dd in db.DataElevator
                group dd by dd.ElevatorId into g
                select new
                {
                    ElevatorId = g.Key,
                    EventIdUtc = g.Max(dd => dd.EventIdUtc)
                })
            on new {d.ElevatorId, d.EventIdUtc} equals new {latest.ElevatorId, latest.EventIdUtc}
            select new
            {
                d.ElevatorId,
                d.EventIdUtc,
                d.Floor,
                d.Status
            }
        ).ToListAsync();
    logger.LogInformation("Sent message");
    logger.LogInformation("Latest door statuses count: {@ElevatorsCnt}", elevatorsStatus.Count);

    return Results.Ok(elevatorsStatus);
});


// Get latest door status
app.MapGet("/api/doorsstatus/", async (AppDbContext db) =>
{
    var doors = await (
        from d in db.DataDoor
        join latest in
            (from dd in db.DataDoor
             group dd by dd.DoorId into g
             select new
             {
                 DoorId = g.Key,
                 EventIdUtc = g.Max(dd => dd.EventIdUtc)
             })
        on new { d.DoorId, d.EventIdUtc } equals new { latest.DoorId, latest.EventIdUtc }
        select new
        {
            d.DoorId,
            d.EventIdUtc,
            d.Status
        }
        ).ToListAsync();
    logger.LogInformation("Sent message");
    logger.LogInformation("Latest door statuses count: {@DoorsCnt}", doors.Count);

    return Results.Ok(doors);
});

app.MapGet("/api/smokedetectorsstatus/", async (AppDbContext db) =>
{
    var detectors = await (
        from d in db.DataSmokeDetector
            join latest in
                (from dd in db.DataSmokeDetector
                group dd by dd.DetectorId into g
                select new
                {
                    DetectorId = g.Key,
                    EventIdUtc = g.Max(dd => dd.EventIdUtc)
                })
            on new {d.DetectorId, d.EventIdUtc} equals new {latest.DetectorId, latest.EventIdUtc}
            select new
            {
                d.DetectorId,
                d.EventIdUtc,
                d.Status
            }
        ).ToListAsync();
    logger.LogInformation("Sent message");
    logger.LogInformation("Latest door statuses count: {@DoorsCnt}", detectors.Count);

    return Results.Ok(detectors);
});

app.Run();

