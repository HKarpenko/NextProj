using Hangfire;
using Microsoft.EntityFrameworkCore;
using Notification.Application.Interfaces;
using Notification.Application.Services;
using Notification.Domain.ConfigurationModels;
using Notification.Infrastructure.Data;
using Notification.Infrastructure.Repositories;
using Notification.Infrastructure.Repositories.Interfaces;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection"));
});

builder.Services.AddHangfire(configuration => 
    configuration.UseSqlServerStorage(builder.Configuration.GetConnectionString("DbConnection"))
);

builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddTransient<IEmailService, EmailService>();

builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

// Use Hangfire dashboard
app.UseHangfireDashboard();

// Use Hangfire server
app.UseHangfireServer();

app.Run();