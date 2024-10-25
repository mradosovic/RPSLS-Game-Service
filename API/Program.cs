using RPSLS_Game.Application.Interfaces;
using RPSLS_Game.Application.Services;
using Microsoft.OpenApi.Models;
using FluentValidation.AspNetCore;
using Infrastructure.Repositories;
using RPSLS_Game.Application.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
        .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<PlayRequestDTOValidator>());

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

// Register services
builder.Services.AddSingleton<IChoiceRepository, ChoiceRepository>();
builder.Services.AddTransient<IGameService, GameService>();
builder.Services.AddHttpClient();
builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<PlayHandler>());
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "RPSLS Game API", Version = "v1" });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "RPSLS Game API V1");
});

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();
app.UseCors("AllowAllOrigins");
app.UseAuthorization(); // Ensure authorization middleware is added if you have it.

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
