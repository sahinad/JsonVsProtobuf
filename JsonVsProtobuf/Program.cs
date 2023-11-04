using ProtoBuf;
using System.Text.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    _ = app.UseSwagger();
    _ = app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast/protobuf/size", () =>
{
    var forecast = Enumerable.Range(1, 1000).Select(index =>
        new WeatherForecast
        (
            DateTime.Now.Date.AddDays(index),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();

    byte[] serializedData;
    using (var stream = new MemoryStream())
    {
        Serializer.Serialize(stream, forecast);
        serializedData = stream.ToArray();
    }

    return Results.Ok(serializedData.Length);
})
.WithName("GetWeatherForecastProtoBufSize")
.WithOpenApi();

app.MapGet("/weatherforecast/json/size", () =>
{
    var forecast = Enumerable.Range(1, 1000).Select(index =>
        new WeatherForecast
        (
            DateTime.Now.Date.AddDays(index),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();

    var serializedData = JsonSerializer.SerializeToUtf8Bytes(forecast);

    return Results.Ok(serializedData.Length);
})
.WithName("GetWeatherForecastJsonSize")
.WithOpenApi();

app.Run();

[ProtoContract]
public record WeatherForecast
{
    private WeatherForecast() { }

    public WeatherForecast(DateTime date, int temperatureC, string? summary)
    {
        Date = date;
        TemperatureC = temperatureC;
        Summary = summary;
    }

    [ProtoMember(1)]
    public DateTime Date { get; init; }

    [ProtoMember(2)]
    public int TemperatureC { get; init; }

    [ProtoMember(3)]
    public string? Summary { get; init; }

    [JsonIgnore]
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
