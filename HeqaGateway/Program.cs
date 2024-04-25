
// <snippet_all>
using NSwag.AspNetCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<InterfaceConfigDB>(opt => opt.UseInMemoryDatabase("TodoList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "InterfaceConfigAPI";
    config.Title = "InterfaceConfigAPI v1";
    config.Version = "v1";
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi(config =>
    {
        config.DocumentTitle = "InterfaceConfigAPI";
        config.Path = "/swagger";
        config.DocumentPath = "/swagger/{documentName}/swagger.json";
        config.DocExpansion = "list";
    });
}

// <snippet_group>
RouteGroupBuilder todoItems = app.MapGroup("/interface-config");

todoItems.MapGet("/", GetAllTodos);
todoItems.MapGet("/complete", GetCompleteTodos);
todoItems.MapGet("/{id}", GetTodo);
todoItems.MapPost("/", CreateTodo);
todoItems.MapPut("/{id}", UpdateTodo);
todoItems.MapDelete("/{id}", DeleteTodo);
// </snippet_group>

app.Run();

// <snippet_handlers>
// <snippet_getalltodos>
static async Task<IResult> GetAllTodos(InterfaceConfigDB db)
{
    return TypedResults.Ok(await db.Todos.Select(x => new InterfaceConfigDTO(x)).ToArrayAsync());
}
// </snippet_getalltodos>

static async Task<IResult> GetCompleteTodos(InterfaceConfigDB db) {
    return TypedResults.Ok(await db.Todos.Where(t => t.IsComplete).Select(x => new InterfaceConfigDTO(x)).ToListAsync());
}

static async Task<IResult> GetTodo(int id, InterfaceConfigDB db)
{
    return await db.Todos.FindAsync(id)
        is InterfaceConfig todo
            ? TypedResults.Ok(new InterfaceConfigDTO(todo))
            : TypedResults.NotFound();
}

static async Task<IResult> CreateTodo(InterfaceConfigDTO todoItemDTO, InterfaceConfigDB db)
{
    var todoItem = new InterfaceConfig
    {
        IsComplete = todoItemDTO.IsComplete,
        Name = todoItemDTO.Name
    };

    db.Todos.Add(todoItem);
    await db.SaveChangesAsync();

    todoItemDTO = new InterfaceConfigDTO(todoItem);

    return TypedResults.Created($"/todoitems/{todoItem.Id}", todoItemDTO);
}

static async Task<IResult> UpdateTodo(int id, InterfaceConfigDTO todoItemDTO, InterfaceConfigDB db)
{
    var todo = await db.Todos.FindAsync(id);

    if (todo is null) return TypedResults.NotFound();

    todo.Name = todoItemDTO.Name;
    todo.IsComplete = todoItemDTO.IsComplete;

    await db.SaveChangesAsync();

    return TypedResults.NoContent();
}

static async Task<IResult> DeleteTodo(int id, InterfaceConfigDB db)
{
    if (await db.Todos.FindAsync(id) is InterfaceConfig todo)
    {
        db.Todos.Remove(todo);
        await db.SaveChangesAsync();
        return TypedResults.NoContent();
    }

    return TypedResults.NotFound();
}
// <snippet_handlers>
// </snippet_all>
