using Azure.Identity;
using Azure.Messaging.ServiceBus;
using PetCareAPI.EmailService.BackgroundServices;
using PetCareAPI.EmailService.Services;

var builder = WebApplication.CreateBuilder(args);

var keyVaultUrl = builder.Configuration["KeyVault:Url"]
                  ?? throw new InvalidOperationException(
                      "KeyVault:Url is not configured.");

builder.Configuration.AddAzureKeyVault(
    new Uri(keyVaultUrl),
    new AzureCliCredential());

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddSingleton(
    new ServiceBusClient(builder.Configuration.GetConnectionString("ServiceBus"), new AzureCliCredential()));

builder.Services.AddSingleton<IEmailService, EmailService>();
builder.Services.AddHostedService<OutboxConsumer>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();