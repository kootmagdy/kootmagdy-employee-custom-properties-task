using System.ComponentModel.DataAnnotations;

namespace backend.Models

{
    public enum PropertyType
    {
        String,
        Integer,
        Date,
        Dropdown
    }

    public class EmployeeProperty
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public PropertyType Type { get; set; }

        public bool IsRequired { get; set; }

        public ICollection<string>? DropdownOptions { get; set; } 
    }

}
