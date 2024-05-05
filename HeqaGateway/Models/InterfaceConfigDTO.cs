
public class InterfaceConfigDTO
{
    public int Id { get; set; }
    public string[] ? Ips { get; set; }
    public string? Description { get; set; }

    public InterfaceConfigDTO() { }
    public InterfaceConfigDTO(InterfaceConfig interfaceConfig) =>
    (Id, Ips, Description) = (interfaceConfig.Id, interfaceConfig.Ips, interfaceConfig.Description);
}
