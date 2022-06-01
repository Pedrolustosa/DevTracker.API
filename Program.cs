using Microsoft.OpenApi.Models;
using DevTracker.API.Persistence;
using Microsoft.EntityFrameworkCore;
using DevTracker.API.Persistence.Repository;
using SendGrid.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DevTrackerCs");

//builder.Services.AddDbContext<DevTrackerContext>(o => o.UseSqlServer(connectionString));
builder.Services.AddDbContext<DevTrackerContext>(o => o.UseInMemoryDatabase("DevTrackerDB"));

builder.Services.AddScoped<IPackageRepository, PackageRepository>();

var sendGridApiKey = builder.Configuration.GetSection("SendGridApiKey").Value;

builder.Services.AddSendGrid(o => o.ApiKey = sendGridApiKey);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(o =>
{
    o.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "DevJobs.API",
        Version = "v1",
        Contact = new OpenApiContact
        {
            Name = "Pedro Lustosa",
            Email = "pedroeternalss@gmail.com",
            Url = new Uri("https://www.linkedin.com/in/pedro-henrique-lustosa-e-silva-29b827144/")
        }
    });

    var xmlFile = "DevTracker.API.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

    o.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (true)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
