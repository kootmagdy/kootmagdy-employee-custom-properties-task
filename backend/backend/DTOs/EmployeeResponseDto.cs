namespace backend.DTOs;

public class EmployeeResponseDto
{
    public int Id { get; set; }
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public List<EmployeePropWithValueDto> Properties { get; set; } = new();
}

public class EmployeePropWithValueDto
{
    public int PropertyId { get; set; }
    public string PropertyName { get; set; } = null!;
    public string Type { get; set; } = null!;
    public bool IsRequired { get; set; }
    public List<string>? DropdownOptions { get; set; }
    public string? Value { get; set; }
}
