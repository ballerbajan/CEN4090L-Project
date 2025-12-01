using Microsoft.EntityFrameworkCore;
using CEN4090L_Project.Data; 

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DbConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();      // Exposes /openapi/v1.json
    app.UseSwaggerUI();    // Exposes Swagger UI at /swagger
    app.UseSwagger();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
