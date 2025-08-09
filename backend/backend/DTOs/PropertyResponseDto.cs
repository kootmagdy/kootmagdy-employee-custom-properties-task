namespace backend.DTOs;

public class PropertyResponseDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Type { get; set; } = null!;
    public bool IsRequired { get; set; }
    public List<string>? DropdownOptions { get; set; }
}
