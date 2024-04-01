using OkeyServer.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer(); // A effacer
builder.Services.AddSwaggerGen(); // A effacer
builder.Services.AddSignalR();

var app = builder.Build();

if (app.Environment.IsDevelopment()) // Tout effacer
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection(); // A effacer

app.MapHub<OkeyHub>("OkeyHub");

app.Run();
