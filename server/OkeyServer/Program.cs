using Microsoft.EntityFrameworkCore;
using OkeyApi.Data;
using OkeyServer.Hubs;
using OkeyServer.Misc;
using OkeyServer.Security;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSignalR();

builder.Services.AddScoped<JWTCheck>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySQL(
        builder.Configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException()
    )
);

builder.Services.AddSingleton<IRoomManager, RoomManager>();

var app = builder.Build();

app.MapHub<OkeyHub>("OkeyHub");

app.Run();
