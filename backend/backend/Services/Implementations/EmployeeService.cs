using backend.Data;
using backend.DTOs;
using backend.Models;
using backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace backend.Services.Implementations
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ApplicationDbContext _db;
        public EmployeeService(ApplicationDbContext db) => _db = db;

        public async Task<EmployeeResponseDto> CreateAsync(CreateEmployeeDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Code) || string.IsNullOrWhiteSpace(dto.Name))
                throw new ArgumentException("Code and Name are required.");

            var props = await _db.EmployeeProperties.AsNoTracking().ToListAsync();

            var emp = new Employee { Code = dto.Code.Trim(), Name = dto.Name.Trim() };
            _db.Employees.Add(emp);
            await _db.SaveChangesAsync();

            var provided = dto.PropertyValues ?? new List<EmployeePropertyValueDto>();

            foreach (var p in props)
            {
                var pv = provided.FirstOrDefault(x => x.PropertyId == p.Id);

                if (p.IsRequired && (pv == null || string.IsNullOrWhiteSpace(pv.Value)))
                    throw new ArgumentException($"Property '{p.Name}' is required.");

                if (pv != null && !string.IsNullOrWhiteSpace(pv.Value))
                {
                    var val = pv.Value;

                    switch (p.Type)
                    {
                        case PropertyType.Integer:
                            if (!int.TryParse(val, out _))
                                throw new ArgumentException($"Property '{p.Name}' must be an integer.");
                            break;

                        case PropertyType.Date:
                            if (!DateTime.TryParse(val, CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                                throw new ArgumentException($"Property '{p.Name}' must be a valid date (e.g., 2025-08-09).");
                            break;

                        case PropertyType.Dropdown:
                            var options = p.DropdownOptions ?? new List<string>();
                            if (options.Count > 0 && !options.Contains(val))
                                throw new ArgumentException($"Property '{p.Name}' must be one of: {string.Join(", ", options)}");
                            break;
                    }

                    _db.EmployeePropertyValues.Add(new EmployeePropertyValue
                    {
                        EmployeeId = emp.Id,
                        PropertyId = p.Id,
                        Value = val
                    });
                }
            }

            await _db.SaveChangesAsync();

            var created = await GetByIdAsync(emp.Id);
            return created!;
        }

        public async Task<EmployeeResponseDto?> GetByIdAsync(int id)
        {
            var props = await _db.EmployeeProperties.AsNoTracking().ToListAsync();

            var e = await _db.Employees
                .AsNoTracking()
                .Include(x => x.PropertyValues)
                .ThenInclude(v => v.Property)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (e is null) return null;

            return new EmployeeResponseDto
            {
                Id = e.Id,
                Code = e.Code,
                Name = e.Name,
                Properties = props.Select(p =>
                {
                    var val = e.PropertyValues.FirstOrDefault(x => x.PropertyId == p.Id);
                    return new EmployeePropWithValueDto
                    {
                        PropertyId = p.Id,
                        PropertyName = p.Name,
                        Type = p.Type.ToString(),
                        IsRequired = p.IsRequired,
                        DropdownOptions = p.DropdownOptions?.ToList(),
                        Value = val?.Value
                    };
                }).ToList()
            };
        }

        public async Task<IReadOnlyList<EmployeeResponseDto>> GetAllAsync()
        {
            var props = await _db.EmployeeProperties.AsNoTracking().ToListAsync();

            var employees = await _db.Employees
                .AsNoTracking()
                .Include(e => e.PropertyValues)
                .ThenInclude(v => v.Property)
                .ToListAsync();

            return employees.Select(e => new EmployeeResponseDto
            {
                Id = e.Id,
                Code = e.Code,
                Name = e.Name,
                Properties = props.Select(p =>
                {
                    var val = e.PropertyValues.FirstOrDefault(x => x.PropertyId == p.Id);
                    return new EmployeePropWithValueDto
                    {
                        PropertyId = p.Id,
                        PropertyName = p.Name,
                        Type = p.Type.ToString(),
                        IsRequired = p.IsRequired,
                        DropdownOptions = p.DropdownOptions?.ToList(),
                        Value = val?.Value
                    };
                }).ToList()
            }).ToList();
        }
    }
}
