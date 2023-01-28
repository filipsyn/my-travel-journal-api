using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MyTravelJournal.Api.Data;
using MyTravelJournal.Api.Services.Auth;
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

builder.Services.AddControllers()
    .AddNewtonsoftJson();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Version = "v1",
            Title = "My Travel Journal",
            Description = "REST API for managing information about trips, users and locations",
        });

        var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
    }
);

builder.Services.AddDbContext<DataContext>(options =>
    options.UseNpgsql(connectionString)
);
builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddSingleton<IAuthService, AuthService>();


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