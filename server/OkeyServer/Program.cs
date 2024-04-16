using OkeyServer.Hubs;
using OkeyServer.Misc;
using OkeyServer.Security;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR();

builder.Services.AddScoped<JWTCheck>();

builder.Services.AddSingleton<IRoomManager, RoomManager>();

var app = builder.Build();

app.MapHub<OkeyHub>("OkeyHub");

app.Run();
