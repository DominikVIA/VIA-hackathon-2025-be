using Dtos;
using Microsoft.AspNetCore.Mvc;
using Repository;
using WebApi.ServiceContracts;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ThoughtController : ControllerBase
{
    private readonly IRepositoryService _service;
    private readonly ILlmService _aiService;
    
    public ThoughtController(IRepositoryService service, ILlmService aiService)
    {
        _aiService = aiService;
        _service = service;
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateThoughtDto story)
    {
        var response = await _service.AddAsync(story);
        return Ok(response);
    }
    
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var response = await _service.GetAllAsync();
        return Ok(response);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] string id)
    {
        var response = await _service.GetByIdAsync(id);
        return Ok(response);
    }
    
    [HttpPatch]
    public async Task<IActionResult> Update([FromBody] UpdateThoughtDto thoughtToUpdate)
    {
        await _service.UpdateThoughtAsync(thoughtToUpdate);
        return Ok();
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] string id)
    {
        await _service.DeleteThoughtAsync(id);
        return Ok();
    }
}