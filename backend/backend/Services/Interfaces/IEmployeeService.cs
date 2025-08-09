using backend.DTOs;

namespace backend.Services.Interfaces
{
    public interface IEmployeeService
    {
        Task<EmployeeResponseDto> CreateAsync(CreateEmployeeDto dto);
        Task<EmployeeResponseDto?> GetByIdAsync(int id);
        Task<IReadOnlyList<EmployeeResponseDto>> GetAllAsync();
    }
}
