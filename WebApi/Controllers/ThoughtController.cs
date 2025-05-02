using Dtos;
using Microsoft.AspNetCore.Mvc;
using Repository;

namespace WebApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ThoughtController : ControllerBase
{
    private readonly IRepositoryService _service;
    
    public ThoughtController(IRepositoryService service)
    {
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
    
}