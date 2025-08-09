using backend.DTOs;

namespace backend.Services.Interfaces
{
    public interface IPropertyService
    {
        Task<PropertyResponseDto> CreateAsync(CreatePropertyDto dto);
        Task<IReadOnlyList<PropertyResponseDto>> GetAllAsync();
    }
}
