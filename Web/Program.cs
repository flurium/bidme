using Bll.Infrastructure;
using Dal.Context;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;
using Web.Controllers;
using Web.Helpers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var aspEnv = Env("ASPNETCORE_ENVIRONMENT");
if (aspEnv == EnvName.Production)
{
  var connectionString = Env("DB_CONNECTION_STR");
  ConfigureDbAndSendGrid(
    builder,
    dbOptions => dbOptions.UseNpgsql(connectionString),
    sgOptions =>
    {
      sgOptions.UserMail = Env("SG_USER_MAIL");
      sgOptions.SendGridKey = Env("SG_API_KEY");
    }
  );
}
else if (aspEnv == EnvName.LocalDevelopment)
{
  var connectionString = builder.Configuration.GetConnectionString("Local");
  ConfigureDbAndSendGrid(
    builder,
    dbOptions => dbOptions.UseSqlServer(connectionString),
    sgOptions => builder.Configuration.GetSection("SendGridOptions").Bind(sgOptions)
  );
}
else if (aspEnv == EnvName.Development)
{
  var connectionString = builder.Configuration.GetConnectionString("Global");
  ConfigureDbAndSendGrid(
    builder,
    dbOptions => dbOptions.UseNpgsql(connectionString),
    sgOptions => builder.Configuration.GetSection("SendGridOptions").Bind(sgOptions)
  );
}

// AUTH
builder.Services.AddIdentity<User, IdentityRole>(options => options.SignIn.RequireConfirmedEmail = true)
   .AddEntityFrameworkStores<BidMeDbContext>().AddDefaultTokenProviders()
   .AddTokenProvider<EmailConfirmationTokenProvider<User>>("emailConfirmationProvider");

// Logs
var logger = new LoggerConfiguration()
    .WriteTo.File("./logs.json", rollingInterval: RollingInterval.Day)
    .MinimumLevel.Fatal()
    .CreateLogger();

builder.Logging.AddSerilog(logger);

builder.Services.Configure<EmailConfirmationProviderOptions>(options => options.TokenLifespan = TimeSpan.FromDays(1));

// Buisness servises
BllConfiguration.ConfigureServices(builder.Services);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
  app.UseExceptionHandler("/Home/Error");
  app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: $"{{controller={HomeController.Name}}}/{{action={nameof(HomeController.Index)}}}/{{id?}}");

app.MapControllerRoute(
    name: "emailConfirmation",
    pattern: "confirmation/",
    defaults: new { controller = EmailConfirmController.Name, action = nameof(EmailConfirmController.Confirm) });

app.Run();

static string Env(string key) => Environment.GetEnvironmentVariable("DB_CONNECTION_STR") ?? "";

static void ConfigureDbAndSendGrid(WebApplicationBuilder builder, Action<DbContextOptionsBuilder> configDbOptions, Action<SendGridOptions> configSendGridOptions)
{
  builder.Services.AddDbContext<BidMeDbContext>(configDbOptions);
  builder.Services.Configure(configSendGridOptions);
}