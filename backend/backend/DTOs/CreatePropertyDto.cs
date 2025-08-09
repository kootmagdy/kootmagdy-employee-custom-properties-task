namespace backend.DTOs
{
    public class CreatePropertyDto
    {
        public string Name { get; set; } = null!;
        public backend.Models.PropertyType Type { get; set; }
        public bool IsRequired { get; set; }
        public List<string>? DropdownOptions { get; set; } 
    }
}
