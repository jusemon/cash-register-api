using CashRegister.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
{
    policy.WithOrigins(Environment.GetEnvironmentVariable("ALLOWED_ORIGINS")!.Split(","))
          .AllowAnyMethod()
          .AllowAnyHeader();
}));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddDbContext<CashRegisterContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.

bool tryParse = Boolean.TryParse(Environment.GetEnvironmentVariable("ENABLE_SWAGGER"), out bool enableSwagger);

if (app.Environment.IsDevelopment() || (tryParse && enableSwagger))
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();
