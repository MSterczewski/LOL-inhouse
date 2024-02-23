using InhouseServer.Hubs;
using Interfaces;
using NoSqlRepositories;
using NoSqlRepositoryInterfaces;
using Repositories;
using RepositoryInterfaces;
using ServiceInterfaces;
using Services;
using SignalRInterfaces;

string corsPolicy = "MyCorsPolicy";
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        name: corsPolicy,
        policy =>
        {
            policy.WithOrigins("http://localhost:3000").AllowAnyHeader().AllowCredentials();
        }
    );
});

// Add services to the container.
builder.Services.AddScoped<IDbUnitOfWork, DbUnitOfWork>();

builder.Services.AddScoped<ILobbyDataService, LobbyDataService>();
builder.Services.AddScoped<ILobbyAdminDataService, LobbyAdminDataService>();
builder.Services.AddScoped<IMatchmakingService, MatchmakingService>();
builder.Services.AddScoped<IAccountService, AccountService>();

//nosql repositories
builder.Services.AddScoped<IWaitingPlayersRepository, WaitingPlayersRepository>();
builder.Services.AddScoped<IMatchRepository, MatchRepository>();

builder.Services.AddSignalR();
builder.Services.AddScoped<ILobbyHub, LobbyHub>();

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseCors(corsPolicy);

app.MapHub<LobbyHub>("/lobbyHub");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
