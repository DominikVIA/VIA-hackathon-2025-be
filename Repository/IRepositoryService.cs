using Dtos;
using Entities;

namespace Repository;

public interface IRepositoryService
{
    Task<List<Thought>> GetAllAsync();
    Task<Thought> GetByIdAsync(string id);
    Task<Thought> AddAsync(CreateThoughtDto thoughtToAdd);
    Task UpdateThoughtAsync(string id, UpdateThoughtDto thoughtToUpdate);
    Task DeleteThoughtAsync(string id);
}