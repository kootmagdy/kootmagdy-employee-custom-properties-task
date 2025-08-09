using backend.Data;
using backend.DTOs;
using backend.Models;
using backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace backend.Services.Implementations
{
    public class PropertyService : IPropertyService
    {
        private readonly ApplicationDbContext _db;

        public PropertyService(ApplicationDbContext db) => _db = db;

        public async Task<PropertyResponseDto> CreateAsync(CreatePropertyDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("Name is required.", nameof(dto.Name));

            if (dto.Type == PropertyType.Dropdown &&
                (dto.DropdownOptions is null || dto.DropdownOptions.Count == 0))
                throw new ArgumentException("Dropdown type requires at least one option.", nameof(dto.DropdownOptions));

            var entity = new EmployeeProperty
            {
                Name = dto.Name.Trim(),
                Type = dto.Type,
                IsRequired = dto.IsRequired,
                // EF ValueConverter هيحفظها JSON في العمود
                DropdownOptions = dto.Type == PropertyType.Dropdown
                    ? dto.DropdownOptions
                    : null
            };

            _db.EmployeeProperties.Add(entity);
            await _db.SaveChangesAsync();

            return new PropertyResponseDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Type = entity.Type.ToString(),
                IsRequired = entity.IsRequired,
                DropdownOptions = entity.DropdownOptions?.ToList()
            };
        }

        public async Task<IReadOnlyList<PropertyResponseDto>> GetAllAsync()
        {
            var props = await _db.EmployeeProperties.AsNoTracking().ToListAsync();

            return props.Select(p => new PropertyResponseDto
            {
                Id = p.Id,
                Name = p.Name,
                Type = p.Type.ToString(),
                IsRequired = p.IsRequired,
                DropdownOptions = p.DropdownOptions?.ToList()
            }).ToList();
        }
    }
}
