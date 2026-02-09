using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Warehouse.API.Services;
using Warehouse.API.DTOs;

namespace Warehouse.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RollsController : ControllerBase
    {
        private readonly IRollService _rollService;
        private readonly ILogger<RollsController> _logger;
        
        public RollsController(IRollService rollService, ILogger<RollsController> logger)
        {
            _rollService = rollService;
            _logger = logger;
        }
        
        [HttpPost]
        public async Task<IActionResult> AddRoll([FromBody] CreateRollDTO createRoll)  
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                
                var roll = await _rollService.AddRollAsync(createRoll);  
                
                return Ok(roll);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Ошибка валидации при добавлении рулона");
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при добавлении рулона");
                return StatusCode(500, new { error = "Внутренняя ошибка сервера" });
            }
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveRoll(int id)
        {
            try
            {
                var roll = await _rollService.RemoveRollAsync(id); 
                
                if (roll == null)
                    return NotFound(new { error = $"Рулон с ID {id} не найден" });
                
                return Ok(roll);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Попытка удалить уже удаленный рулон");
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при удалении рулона с ID {id}");
                return StatusCode(500, new { error = "Внутренняя ошибка сервера" });
            }
        }
        
        [HttpGet]
        public async Task<IActionResult> GetRolls([FromQuery] RollFilterDTO filter)  
        {
            try
            {
                var rolls = await _rollService.GetRollsAsync(filter);  
                return Ok(rolls);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении списка рулонов");
                return StatusCode(500, new { error = "Внутренняя ошибка сервера" });
            }
        }
        
        [HttpGet("statistics")]
public async Task<IActionResult> GetStatistics(
    [FromQuery] string? startDate = null,
    [FromQuery] string? endDate = null)
{
    try
    {
        DateTime start = string.IsNullOrEmpty(startDate) 
            ? DateTime.Now.AddDays(-30) 
            : DateTime.Parse(startDate);
        
        DateTime end = string.IsNullOrEmpty(endDate) 
            ? DateTime.Now 
            : DateTime.Parse(endDate);
        
        if (start > end)
            return BadRequest(new { error = "Начальная дата не может быть позже конечной" });
        
        var request = new StatisticsRequestDTO
        {
            StartDate = start,
            EndDate = end
        };
        
        var statistics = await _rollService.GetStatisticsAsync(request);
        
        return Ok(statistics);
    }
    catch (FormatException)
    {
        return BadRequest(new { error = "Неверный формат даты. Используйте '2026-10-02' или '2026-10-02T14:30:00'" });
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Ошибка при получении статистики");
        return StatusCode(500, new { error = "Внутренняя ошибка сервера" });
    }
}
    }
}