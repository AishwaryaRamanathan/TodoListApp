using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using TodoListApp.Data;
using TodoListApp.Models;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Injecting Dbcontext service to communicate with database
builder.Services.AddDbContext<TodoListDbContext>(Options => Options.UseInMemoryDatabase("TodoListDb"));


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Fetching the tasks
app.MapGet("/TodoList/FetchTasks", async (TodoListDbContext todoDb) => await todoDb.todoLists.ToListAsync());

//Fetching Data for particular Id
app.MapGet("/TodoList/FetchTaskswithId{id}", async (int id, TodoListDbContext todoDb) =>
    await todoDb.todoLists.FindAsync(id)
        is TodoList todoList
            ? Results.Ok(todoList)
            : Results.NotFound());


//Creating the Tasks
app.MapPost("/TodoList/CreateTasks", async (TodoList todoList, TodoListDbContext todoDb) =>
{

    todoDb.Add(todoList);
    await todoDb.SaveChangesAsync();

    return Results.Created($"/TodoLists/{todoList.TaskName}", todoList);

});


//Updating the Tasks
app.MapPut("/TodoList/EditTasks{id:int}", async (int id, TodoList InputTodoList, TodoListDbContext todoDb
    ) =>
{
    var todo = await todoDb.todoLists.FindAsync(id);


    if (todo is null) return Results.NotFound();

      todo.TaskName = InputTodoList.TaskName;
    todo.IsCompleted = InputTodoList.IsCompleted;
    todo.Duedate = InputTodoList.Duedate;

   await todoDb.SaveChangesAsync();
    return Results.NoContent();

  
});


//Creating Pending task List
app.MapGet("/TodoList/PendingTaskList", async (TodoListDbContext todoDb) =>

await todoDb.todoLists.Where(t => t.IsCompleted == false &&
(t.Duedate == null || t.Duedate >= DateTime.Now)).ToListAsync());



//Creating Overdue task List
app.MapGet("/TodoList/ OverdueTaskList", async (TodoListDbContext todoDb) =>

await todoDb.todoLists.Where(t => t.IsCompleted == false &&
(t.Duedate == null || t.Duedate <= DateTime.Now)).ToListAsync());



//Deleting task 
app.MapDelete("/TodoList/DeletingTask{id:int}", async (int id, TodoListDbContext todoDb) =>
{
    if (await todoDb.todoLists.FindAsync(id) is TodoList todoList)
    {
        todoDb.Remove(todoList);
        await todoDb.SaveChangesAsync();
        return Results.Ok(todoList);
    }

    return Results.NotFound();
}
);






app.Run();

//Make Program.cs into a public class so that other solution in this project can use it for testing purpose

public partial class Program { }












