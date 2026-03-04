using Scalar.AspNetCore;
using Microsoft.EntityFrameworkCore;
using TaskManager.API;
using TaskManager.API.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.VisualBasic;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddCors();
builder.Services.AddDbContext<TaskDb>(opt=>opt.UseInMemoryDatabase("TaskList"));
var app = builder.Build();
app.UseCors(policy=> policy.WithOrigins("http://localhost:5173")
.AllowAnyMethod()
.AllowAnyHeader());
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.MapGet("/Tasks", async (TaskDb Db) =>
{
   return await Db.Tasks.ToListAsync();
});

app.MapPost("/Tasks",async (TaskDb Db,TaskItem t) =>
{
   Db.Tasks.Add(t);
   await Db.SaveChangesAsync();
   return Results.Created($"Tasks/{t.Id}",t);
});
app.MapPut("/Tasks", async (int Id,TaskDb Db,TaskItem t) =>
{
    var task = await Db.Tasks.FindAsync(Id);
    if(task ==null)return Results.NotFound();
  task.Title=t.Title;
  task.IsComplete=t.IsComplete;
   await Db.SaveChangesAsync();
   return Results.NoContent();
});

app.MapDelete("/Tasks",async (TaskDb Db,int Id) =>
{
if(await Db.Tasks.FindAsync(Id) is TaskItem task){
   Db.Tasks.Remove(task);
   await Db.SaveChangesAsync();
   return Results.NoContent();
}
return Results.NotFound();
});

app.Run();


