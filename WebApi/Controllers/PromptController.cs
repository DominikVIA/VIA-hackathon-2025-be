using Dtos;
using Microsoft.AspNetCore.Mvc;
using Repository;
using WebApi.ServiceContracts;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class PromptController : ControllerBase
{
    private readonly IRepositoryService _service;
    private readonly ILlmService _aiService;
    
    public PromptController(IRepositoryService service, ILlmService aiService)
    {
        _aiService = aiService;
        _service = service;
    }
    
    [HttpPatch]
    public async Task<IActionResult> AddPrompt([FromBody] ThoughtIdDto id)
    {
        Console.WriteLine(id);
        var thought = await _service.GetByIdAsync(id.Id);
        var output = await _aiService.GetThoughFromPrompt(thought.Title, thought.Content);
        Console.WriteLine(output);

        var updatedThought = new AddPromptToThoughtDto()
        {
            Id = thought.Id,
            Output = output
        };
        await _service.AddPromptToThoughtAsync(updatedThought);
        return Ok();
    }
}