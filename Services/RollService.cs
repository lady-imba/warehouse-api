using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Warehouse.API.Data;
using Warehouse.API.Models;
using Warehouse.API.DTOs;

namespace Warehouse.API.Services
{
    public class RollService : IRollService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<RollService> _logger;
        
        public RollService(ApplicationDbContext context, ILogger<RollService> logger)
        {
            _context = context;
            _logger = logger;
        }
        
        public async Task<Roll> AddRollAsync(CreateRollDTO createRoll) 
        {
            if (createRoll.Length <= 0)
                throw new ArgumentException("Длина должна быть положительным числом");
                
            if (createRoll.Weight <= 0)
                throw new ArgumentException("Вес должен быть положительным числом");
            
            var roll = new Roll
            {
                Length = createRoll.Length,
                Weight = createRoll.Weight,
                AddedDate = DateTime.UtcNow
            };
            
            _context.Rolls.Add(roll);
            await _context.SaveChangesAsync();
            
            _logger.LogInformation($"Добавлен новый рулон с ID: {roll.Id}");
            
            return roll;
        }
        
        public async Task<Roll?> RemoveRollAsync(int id)
        {
            var roll = await _context.Rolls.FindAsync(id);
            
            if (roll == null)
                return null;
                
            if (roll.RemovedDate.HasValue)
                throw new InvalidOperationException("Рулон уже удален");
            
            roll.RemovedDate = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            
            _logger.LogInformation($"Удален рулон с ID: {roll.Id}");
            return roll;
        }
        
        public async Task<IEnumerable<Roll>> GetRollsAsync(RollFilterDTO filter)  
        {
            var query = _context.Rolls.AsQueryable();
            
            if (filter.IdRange?.Min.HasValue == true)
                query = query.Where(r => r.Id >= filter.IdRange.Min.Value);
                
            if (filter.IdRange?.Max.HasValue == true)
                query = query.Where(r => r.Id <= filter.IdRange.Max.Value);
            
            if (filter.LengthRange?.Min.HasValue == true)
                query = query.Where(r => r.Length >= filter.LengthRange.Min.Value);
                
            if (filter.LengthRange?.Max.HasValue == true)
                query = query.Where(r => r.Length <= filter.LengthRange.Max.Value);
            
            if (filter.WeightRange?.Min.HasValue == true)
                query = query.Where(r => r.Weight >= filter.WeightRange.Min.Value);
                
            if (filter.WeightRange?.Max.HasValue == true)
                query = query.Where(r => r.Weight <= filter.WeightRange.Max.Value);
            
            query = query
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize);
            
            return await query.ToListAsync();
        }
        
      public async Task<StatisticsResponseDTO> GetStatisticsAsync(StatisticsRequestDTO request)
{
    var allRolls = await _context.Rolls.ToListAsync();
    
    var rollsInPeriod = allRolls
        .Where(r => r.AddedDate.Date >= request.StartDate.Date && 
                   r.AddedDate.Date <= request.EndDate.Date)
        .ToList();
    
    if (!rollsInPeriod.Any())
    {
        return new StatisticsResponseDTO();
    }
    
    var removedRolls = rollsInPeriod.Where(r => r.IsRemoved).ToList();
    
    // Время хранения как строки в формате "00:00:00"
    string? minStorageTime = null;
    string? maxStorageTime = null;
    
    if (removedRolls.Any())
    {
        var storageTimes = removedRolls
            .Select(r => (r.RemovedDate!.Value - r.AddedDate))
            .ToList();
        
        // Форматируем в "часы:минуты:секунды"
        var minTime = storageTimes.Min();
        var maxTime = storageTimes.Max();
        
        minStorageTime = $"{(int)minTime.TotalHours:D2}:{minTime.Minutes:D2}:{minTime.Seconds:D2}";
        maxStorageTime = $"{(int)maxTime.TotalHours:D2}:{maxTime.Minutes:D2}:{maxTime.Seconds:D2}";
    }
    
    // Статистика
    var statistics = new StatisticsResponseDTO
    {
        AddedRollsCount = rollsInPeriod.Count,
        RemovedRollsCount = removedRolls.Count,
        AverageLength = rollsInPeriod.Average(r => r.Length),
        AverageWeight = rollsInPeriod.Average(r => r.Weight),
        MinLength = rollsInPeriod.Min(r => r.Length),
        MaxLength = rollsInPeriod.Max(r => r.Length),
        MinWeight = rollsInPeriod.Min(r => r.Weight),
        MaxWeight = rollsInPeriod.Max(r => r.Weight),
        TotalWeight = rollsInPeriod.Sum(r => r.Weight),
        
        // Теперь строки в формате "00:00:00"
        MinStorageTime = minStorageTime,
        MaxStorageTime = maxStorageTime
    };
    
    return statistics;
}
    }
}