namespace backend.DTOs;

public class CreateEmployeeDto
{
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public List<EmployeePropertyValueDto>? PropertyValues { get; set; }
}

public class EmployeePropertyValueDto
{
    public int PropertyId { get; set; }
    public string? Value { get; set; }
}
