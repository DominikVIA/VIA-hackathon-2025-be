using Dtos;
using Entities;

namespace Repository;

public interface IRepositoryService
{
    Task<List<Thought>> GetAllAsync();
    Task<Thought> GetByIdAsync(string id);
    Task<Thought> AddAsync(CreateThoughtDto thoughtToAdd);
    Task UpdateThoughtAsync(UpdateThoughtDto thoughtToUpdate);
    Task AddPromptToThoughtAsync(AddPromptToThoughtDto thoughtWithPrompt);
    Task DeleteThoughtAsync(string id);
}