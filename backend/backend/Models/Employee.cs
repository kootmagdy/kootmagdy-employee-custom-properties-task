using System.ComponentModel.DataAnnotations;


namespace backend.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required]
        public string Code { get; set; } = null!;

        [Required]
        public string Name { get; set; } = null!;

        public ICollection<EmployeePropertyValue> PropertyValues { get; set; } = new List<EmployeePropertyValue>();
    }
}
