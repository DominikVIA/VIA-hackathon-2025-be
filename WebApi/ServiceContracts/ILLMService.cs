using Entities;

namespace WebApi.ServiceContracts;

public interface ILlmService
{
    Task<string> GetThoughFromPrompt(string title, string body);
}