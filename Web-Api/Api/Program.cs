using Pickfc.BLL.Interfaces;
using Pickfc.BLL.Services;
using Pickfc.DAL.Factories;
using Pickfc.DAL.Interfaces;
using Pickfc.DAL.Infrastructure;
using Pickfc.DAL.Interfaces.IDB;
using Pickfc.DAL.Repositories;
using Pickfc.DAL.WorkUnits;
using Pickfc.Model.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Http.Features;

var builder = WebApplication.CreateBuilder(args);

ConfigurationManager jwsConfig = builder.Configuration;
//ConfigurationManager mailConfig = builder.Configuration;

builder.Services.AddScoped<IDBFactory<PickfcContext>, PickfcDbFactory>();
builder.Services.AddScoped<WorkUnit<PickfcContext>, PickfcWorkUnit>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICompRepository, CompRepository>();
builder.Services.AddScoped<ICompTeamRepository, CompTeamRepository>();
builder.Services.AddScoped<IGameRepository, GameRepository>();
builder.Services.AddScoped<ITeamRepository, TeamRepository>();
builder.Services.AddScoped<IPlayerRepository, PlayerRepository>();
builder.Services.AddScoped<IBackupRepository, BackupRepository>();
builder.Services.AddScoped<IRoundRepository, RoundRepository>();
builder.Services.AddScoped<IFixtureRepository, FixtureRepository>();
builder.Services.AddScoped<IPickRepository, PickRepository>();
builder.Services.AddScoped<IArtRepository, ArtRepository>();


builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICompService, CompService>();
builder.Services.AddScoped<ICompTeamService, CompTeamService>();
builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddScoped<ITeamService, TeamService>();
builder.Services.AddScoped<IPlayerService, PlayerService>();
builder.Services.AddScoped<IBackupService, BackupService>();
builder.Services.AddScoped<IRoundService, RoundService>();
builder.Services.AddScoped<IFixtureService, FixtureService>();
builder.Services.AddScoped<IPickService, PickService>();
builder.Services.AddScoped<IMailService, MailService>();
builder.Services.AddScoped<IArtService, ArtService>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("EnableCORS", builder =>
    {
        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(opt => {
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwsConfig["AppSettings:URL"],
        ValidAudience = jwsConfig["AppSettings:URL"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwsConfig["AppSettings:Secret"])),
    };
    });

builder.Services.AddSingleton<IFileProvider>(new PhysicalFileProvider(Directory.GetCurrentDirectory()));
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddSignalR();
builder.Services.AddMemoryCache();
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();

builder.Services.Configure<FormOptions>(o =>
{
    o.ValueLengthLimit = int.MaxValue;
    o.MultipartBodyLengthLimit = int.MaxValue;
    o.MemoryBufferThreshold = int.MaxValue;
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseCors("EnableCORS");
    //app.UseHsts();
}

app.UseStaticFiles();

app.UseCors("EnableCORS");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html"); ;

app.Run();