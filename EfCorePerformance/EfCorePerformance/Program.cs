using System.Text.Json.Serialization;
using EfCorePerformance;
using EfCorePerformance.Data;
using Microsoft.AspNetCore.Http.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
});

builder.Services.AddSwaggerGen();

builder.Services.AddScoped<CompanyService>();
builder.Services.ConfigureDb(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet(
    "/companies/{num}",
    async (int num, CompanyService companyService) =>
    {
        var companies = await companyService.GetCompaniesAsync(num);
        return Results.Ok(companies);
    });

app.Run();