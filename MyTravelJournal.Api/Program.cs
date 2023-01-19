using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MyTravelJournal.Api.Data;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

// Adding password to connection string
// Value is saved in the .NET secrets vault
var connectionStringBuilder =
    new NpgsqlConnectionStringBuilder(builder.Configuration.GetConnectionString("MyTravelJournal"))
    {
        Password = builder.Configuration["DbPassword"],
    };
var connectionString = connectionStringBuilder.ConnectionString;

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "My Travel Journal",
            Description = "REST API for managing information about trips, users and locations",
        });
    }
);

builder.Services.AddDbContext<MyTravelJournalDbContext>(options =>
    options.UseNpgsql(connectionString)
);


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