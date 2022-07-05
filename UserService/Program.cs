using Microsoft.EntityFrameworkCore;
using UserService.Database;
using UserService.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//string connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION ");
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DatabaseContext>(x =>
{
    x.UseSqlServer(connectionString);
});
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
