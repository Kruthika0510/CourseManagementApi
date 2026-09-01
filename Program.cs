using CourseManagementApi.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Register DbContext with SQL Server provider
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
//test
//test1
//test2
// 2. Register AutoMapper
builder.Services.AddAutoMapper(cfg => { }, typeof(Program));

// 3. Register CORS Policy for Angular Frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200") // Angular default dev server URL
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// 4. Register Controllers and Swagger/OpenAPI
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 5. Configure HTTP Request Middleware Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Enable CORS (Must be placed after UseHttpsRedirection and before UseAuthorization)
app.UseCors("AllowAngular");

app.UseAuthorization();
app.MapControllers();

app.Run();