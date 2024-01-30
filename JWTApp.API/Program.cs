using JWTApp.Data;
using JWTApp.Data.MongoDbSettings;
using JWTApp.Service;
using Microsoft.Extensions.Options;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//var appsettingsSection = builder.Configuration.GetSection("AppSettings");
//builder.Services.Configure<MongoDbSettings>(appsettingsSection);
builder.Services.AddAutoMapper(typeof(DtoMapper));
builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDbConnectionSettings"));
builder.Services.AddSingleton<IMongoDBSettings>(sp =>
{
    return sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
});

var app = builder.Build();

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
