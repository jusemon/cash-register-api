var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
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

app.UseHttpsRedirection();

var todoList = new List<string>();

app.MapGet("/task", () =>
{
   return todoList;
})
.WithName("GetTasks");

app.MapGet("/task/{id}", (int id) =>
{
   return todoList[id];
})
.WithName("GetTask");

app.MapPost("/task/{value}", (string value) =>
{
   todoList.Add(value);
})
.WithName("PostTask");

app.MapPut("/task/{id}/{value}", (int id, string value) =>
{
   todoList[id] = value;
})
.WithName("PutTask");

app.MapDelete("/task/{id}", (int id) =>
{
   todoList.RemoveAt(id);
})
.WithName("DeleteTask");

app.Run();
