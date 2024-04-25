public class InterfaceConfigDTO
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public bool IsComplete { get; set; }

    public InterfaceConfigDTO() { }
    public InterfaceConfigDTO(InterfaceConfig todoItem) =>
    (Id, Name, IsComplete) = (todoItem.Id, todoItem.Name, todoItem.IsComplete);
}
