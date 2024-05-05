
// <snippet_all>
using NSwag.AspNetCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<InterfaceConfigDB>(opt => opt.UseInMemoryDatabase("Interface Config"));
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
RouteGroupBuilder interfaceConfig = app.MapGroup("/interface-config");

interfaceConfig.MapGet("/", GetAllInterfaceConfigs);
interfaceConfig.MapGet("/with-one-ip", GetInterfaceConfigWithOneIP);
interfaceConfig.MapGet("/{id}", GetInterfaceConfig);
interfaceConfig.MapPost("/", CreateInterfaceConfig);
interfaceConfig.MapPut("/{id}", UpdateInterfaceConfig);
interfaceConfig.MapDelete("/{id}", DeleteInterfaceConfig);
// </snippet_group>

app.Run();

// <snippet_handlers>
// <snippet_getallinterfaceconfigs>
static async Task<IResult> GetAllInterfaceConfigs(InterfaceConfigDB db)
{
    return TypedResults.Ok(await db.interfaceConfig.Select(x => new InterfaceConfigDTO(x)).ToArrayAsync());
}
// </snippet_getallinterfaceconfig>

static async Task<IResult> GetInterfaceConfigWithOneIP(InterfaceConfigDB db) {
    return TypedResults.Ok(await db.interfaceConfig.Where(t => (t.Ips != null & t.Ips.GetLength(0) == 1)).Select(x => new InterfaceConfigDTO(x)).ToListAsync());
}

static async Task<IResult> GetInterfaceConfig(int id, InterfaceConfigDB db)
{
    return await db.interfaceConfig.FindAsync(id)
        is InterfaceConfig interfaceConfig
            ? TypedResults.Ok(new InterfaceConfigDTO(interfaceConfig))
            : TypedResults.NotFound();
}

static async Task<IResult> CreateInterfaceConfig(InterfaceConfigDTO interfaceConfigDTO, InterfaceConfigDB db)
{
    var interfaceConfig = new InterfaceConfig
    {
        Ips = interfaceConfigDTO.Ips,
        Description = interfaceConfigDTO.Description
    };

    db.interfaceConfig.Add(interfaceConfig);
    await db.SaveChangesAsync();

    interfaceConfigDTO = new InterfaceConfigDTO(interfaceConfig);

    return TypedResults.Created($"/todoitems/{interfaceConfig.Id}", interfaceConfigDTO);
}

static async Task<IResult> UpdateInterfaceConfig(int id, InterfaceConfigDTO interfaceConfigDTO, InterfaceConfigDB db)
{
    var interfaceConfig = await db.interfaceConfig.FindAsync(id);

    if (interfaceConfig is null) return TypedResults.NotFound();

    interfaceConfig.Ips = interfaceConfigDTO.Ips;
    interfaceConfig.Description = interfaceConfigDTO.Description;

    await db.SaveChangesAsync();

    return TypedResults.NoContent();
}

static async Task<IResult> DeleteInterfaceConfig(int id, InterfaceConfigDB db)
{
    if (await db.interfaceConfig.FindAsync(id) is InterfaceConfig interfaceConfig)
    {
        db.interfaceConfig.Remove(interfaceConfig);
        await db.SaveChangesAsync();
        return TypedResults.NoContent();
    }

    return TypedResults.NotFound();
}
// <snippet_handlers>
// </snippet_all>
