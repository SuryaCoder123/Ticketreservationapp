using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Ticketreservationapp.Data;
using TicketReservationApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Configure CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// Configure DbContext with SQL Server connection string
builder.Services.AddDbContext<TicketReservationContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register services and dependencies
builder.Services.AddScoped<IEventService, EventService>();

// Add controllers and API documentation (Swagger)
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure Swagger for development environment
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Apply CORS policy
app.UseCors("AllowAllOrigins");

// Add Authorization Middleware
app.UseAuthorization();

// Map controller routes
app.MapControllers();

app.Run();
