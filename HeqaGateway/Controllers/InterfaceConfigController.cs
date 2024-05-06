using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


[ApiController]
[Route("interface-config")]
public class InterfaceConfigController : ControllerBase
{

    [HttpGet("/")]
    public async Task<IResult> GetAllInterfaceConfigs(InterfaceConfigDB db)
    {
        return TypedResults.Ok(await db.interfaceConfig.Select(x => new InterfaceConfigDTO(x)).ToArrayAsync());
    }

    [HttpGet("/with-one-ip")]
    public async Task<IResult> GetInterfaceConfigWithOneIP(InterfaceConfigDB db) {
        return TypedResults.Ok(await db.interfaceConfig.Where(t => (t.Ips != null & t.Ips.GetLength(0) == 1)).Select(x => new InterfaceConfigDTO(x)).ToListAsync());
    }

    [HttpGet("/{id}")]
    public async Task<IResult> GetInterfaceConfig(int id, InterfaceConfigDB db)
    {
        return await db.interfaceConfig.FindAsync(id)
            is InterfaceConfig interfaceConfig
                ? TypedResults.Ok(new InterfaceConfigDTO(interfaceConfig))
                : TypedResults.NotFound();
    }

    [HttpPost("/")]
    public async Task<IResult> CreateInterfaceConfig(InterfaceConfigDTO interfaceConfigDTO, InterfaceConfigDB db)
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

    [HttpPut("/{id}")]
    public async Task<IResult> UpdateInterfaceConfig(int id, InterfaceConfigDTO interfaceConfigDTO, InterfaceConfigDB db)
    {
        var interfaceConfig = await db.interfaceConfig.FindAsync(id);

        if (interfaceConfig is null) return TypedResults.NotFound();

        interfaceConfig.Ips = interfaceConfigDTO.Ips;
        interfaceConfig.Description = interfaceConfigDTO.Description;

        await db.SaveChangesAsync();

        return TypedResults.NoContent();
    }

    [HttpDelete("/{id}")]
    public async Task<IResult> DeleteInterfaceConfig(int id, InterfaceConfigDB db)
    {
        if (await db.interfaceConfig.FindAsync(id) is InterfaceConfig interfaceConfig)
        {
            db.interfaceConfig.Remove(interfaceConfig);
            await db.SaveChangesAsync();
            return TypedResults.NoContent();
        }

        return TypedResults.NotFound();
    }

}
