using Bll.Infrastructure;
using Dal.Context;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Web.Controllers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Variables
var aspEnv = Env("ASPNETCORE_ENVIRONMENT");

string connectionString;
if (aspEnv == "Development")
{
  connectionString = builder.Configuration.GetConnectionString("DbConnectionStr");
  builder.Services.Configure<SendGridOptions>(options => builder.Configuration.GetSection("SendGridOptions").Bind(options));
}
else
{
  connectionString = Env("DB_CONNECTION_STR");
  builder.Services.Configure<SendGridOptions>(options =>
  {
    options.UserMail = Env("SG_USER_MAIL");
    options.SendGridKey = Env("SG_API_KEY");
  });
}

// DB
builder.Services.AddDbContext<BidMeDbContext>(options => options.UseNpgsql(connectionString));

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