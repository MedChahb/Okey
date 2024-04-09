using OkeyServer.Hubs;
using OkeyServer.Security;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR();

builder.Services.AddScoped<JWTCheck>();

var app = builder.Build();

app.MapHub<OkeyHub>("OkeyHub");

app.Run();
