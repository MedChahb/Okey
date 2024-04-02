using OkeyServer.Hubs;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR();

var app = builder.Build();

app.MapHub<OkeyHub>("OkeyHub");

app.Run();
