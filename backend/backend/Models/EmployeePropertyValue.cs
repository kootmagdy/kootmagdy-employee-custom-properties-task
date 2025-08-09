namespace backend.Models
{
    public class EmployeePropertyValue
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }
        public Employee Employee { get; set; } = null!;

        public int PropertyId { get; set; }
        public EmployeeProperty Property { get; set; } = null!;

        public string? Value { get; set; }
    }
}
