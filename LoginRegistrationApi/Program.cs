using Microsoft.EntityFrameworkCore;
using LoginRegistrationApi.Data;

var builder = WebApplication.CreateBuilder(args);

// Loading Configuration from appsettings.json
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Services.AddControllers();

// Add DbContext with SQLite connection
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
);


// Add CORS policy 
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

//Create the final application instance with all the configured services.
var app = builder.Build();

// Handling Errors in Development Mode
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseCors("AllowAll");  // Apply CORS policy globally
app.UseRouting();  // Enable routing
app.MapControllers();  // Map API controllers
app.Run();  // Start the application
